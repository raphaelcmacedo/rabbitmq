using Main.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForce.Services
{
    public static class OpportunityConvertion
    {
        public static Dictionary<string, string> Settings { get; set; }
        private static CultureInfo cultureInfo = new CultureInfo("en-US");

        public static Opportunity SalesDataToOpportunity(SalesData salesData)
        {
            if(Settings == null)
            {
                Main.Repositories.SettingsRepository settingsRepository = new Main.Repositories.SettingsRepository();
                Settings = settingsRepository.GetConfig(Main.Helpers.Settings.ApplicationName);
            }

            Main.Repositories.OpportunityRepository opportunityRepository = new Main.Repositories.OpportunityRepository();
            Opportunity opportunity = new Opportunity();
           
            if (salesData.SoldTo != null)
            {
                opportunity.AccountID = salesData.SoldTo.WestconId;
            }

          
            //opportunity.CloseDate = 
            opportunity.StageName = Settings["Stage Name"];// "Qualification";
            opportunity.Type = Settings["Type"];//"Renewal";
            opportunity.WCType = Settings["Westcon Type"]; //"Renewals";
            opportunity.GeneratedBy = Settings["Created By"]; //"Renewals";
           

            decimal totalBillingValue = 0;
            decimal totalBillingCost = 0;
            foreach (LineItem lineItem in salesData.LineItems)
            {
                totalBillingValue += lineItem.BillingValue;
                totalBillingCost += lineItem.BillingCost;
            }
            opportunity.TotalBillingValue = totalBillingValue;
            opportunity.TotalBillingCost = totalBillingCost;
            opportunity.SalesDataId = salesData.SalesDataId;

            SetCountingFields(salesData, opportunity);
            opportunity.SalesForceID = opportunityRepository.SearchForSalesForceId(opportunity.Name);
            salesData.CreationTimestamp = DateTime.Now;
            opportunity.SalesData = salesData;

            opportunity.CreationTimestamp = DateTime.Now;

            #region "INNO-275"
            string oppFormatedName = String.Format(Main.Helpers.Settings.OpportunityNameFormat.ToString(), salesData.SalesOrderNo, totalBillingValue.ToString(cultureInfo));
            opportunity.Name = oppFormatedName;
            #endregion "INNO-275"

            return opportunity;
        }

        public static void SetCountingFields(SalesData salesData, Opportunity opportunity)
        {
            Dictionary<string, int> accountManagerIDs = new Dictionary<string, int>();
            Dictionary<string, int> salesPractices = new Dictionary<string, int>();
            Dictionary<string, int> manufacturerIDs = new Dictionary<string, int>();
            Dictionary<string, int> endUsers = new Dictionary<string, int>();

            foreach (LineItem lineItem in salesData.LineItems)
            {
                string accountManager = lineItem.AccountManagerId;
                string salesPractice = lineItem.SalesPractice;
                string manufacturer = lineItem.ManufacturerID + ";" + lineItem.ManufacturerName;
                //concatenates the ID and the name of enduser
                string endUser = lineItem.EndUser.WestconId + ";" + lineItem.EndUser.Name;

                string sku = lineItem.SKU;

                int totalAccountManager = 0;
                int totalSalesPractice = 0;
                int totalManufacturer = 0;
                int totalEndUser = 0;

                //Count account manager
                if (accountManager != null)
                {
                    if (accountManagerIDs.ContainsKey(accountManager))
                    {
                        totalAccountManager = accountManagerIDs[accountManager];
                    }
                    totalAccountManager++;
                    accountManagerIDs[accountManager] = totalAccountManager;
                }
                //Count sales practices
                if (salesPractice != null)
                {
                    if (salesPractices.ContainsKey(salesPractice))
                    {
                        totalSalesPractice = salesPractices[salesPractice];
                    }
                    totalSalesPractice++;
                    salesPractices[salesPractice] = totalSalesPractice;
                }

                //Count ManufacturerIds
                if(manufacturer != null)
                {
                    if (manufacturerIDs.ContainsKey(manufacturer))
                    {
                        totalManufacturer = manufacturerIDs[manufacturer];
                    }
                    totalManufacturer++;
                    manufacturerIDs[manufacturer] = totalManufacturer++;
                }

                //Count EndUser
                if (endUser != null)
                {
                    if (endUsers.ContainsKey(endUser))
                    {
                        totalEndUser = endUsers[endUser];
                    }
                    totalEndUser++;
                    endUsers[endUser] = totalEndUser++;
                }
            }
            //Account Manager
            int maxAccountManagers = (accountManagerIDs.Count > 0) ? accountManagerIDs.Values.Max() : 0;
            string mainAccountManager = (accountManagerIDs.Count > 0) 
                ? accountManagerIDs.FirstOrDefault(x => x.Value == maxAccountManagers).Key
                : null;
            opportunity.MainAccountManagerID = mainAccountManager;           

            //Manufacturer
            int maxManufacturer = (manufacturerIDs.Count > 0) ? manufacturerIDs.Values.Max() : 0;
            string manufacturerID = (manufacturerIDs.Count > 0)
                ? manufacturerIDs.FirstOrDefault(x => x.Value == maxManufacturer).Key
                : null;
            if (!string.IsNullOrEmpty(manufacturerID))
            {
                opportunity.ManufacturerID = manufacturerID.Split(';')[0];
                opportunity.ManufacturerName = manufacturerID.Split(';')[1];
            }
            else
            {
                opportunity.ManufacturerID = null;
                opportunity.ManufacturerName = null;
            }

            //Sales Practice
            int maxSalesPractice = (salesPractices.Count > 0) ? salesPractices.Values.Max() : 0;
            string salesPracticeName = (salesPractices.Count > 0)
                ? salesPractices.FirstOrDefault(x => x.Value == maxSalesPractice).Key
                : null;
           
            //Owner
            opportunity.OwnerID = salesPracticeName;            
            #region "INNO-182"
            Main.Repositories.SalesMappingRepository repository = new Main.Repositories.SalesMappingRepository();
            opportunity.OwnerSFUserName = repository.GetMappedUserID(salesData.SalesOrg, salesPracticeName, opportunity.ManufacturerName);
            #endregion "INNO-182"

            //End User
            int maxEndUser = (endUsers.Count > 0) ? endUsers.Values.Max() : 0;
            string endUserStr = (endUsers.Count > 0)
                ? endUsers.FirstOrDefault(x => x.Value == maxEndUser).Key
                : null;

            if (!string.IsNullOrEmpty(endUserStr))
            {
                opportunity.EndUserID = endUserStr.Split(';')[0];
                opportunity.EndUserName = endUserStr.Split(';')[1];
            }
            else
            {
                opportunity.EndUserID = null;
                opportunity.EndUserName = null;
            }


            opportunity.CloseDate = CalculateEndDate(salesData);
        }

        private static DateTime? CalculateEndDate(SalesData salesData)
        {
            //Try to fetch date from EndDate
            DateTime? closeDate = salesData.LineItems.Min(x => x.EndDate);
            if (closeDate != null)
            {// If we have the EndDate, we need to add 1 Month
                 closeDate = closeDate.Value.AddMonths(Convert.ToInt32(Settings["End Date Additional Months"]));
            }
            else
            {//Try to fetch date from BillingDate
                closeDate = salesData.LineItems.Min(x => x.EarliestBillingPostDate);
                if (closeDate != null)
                {// If we have the Billing Date, we need to add 13 Month
                    closeDate = closeDate.Value.AddMonths(Convert.ToInt32(Settings["Billing Date Additional Months"]));
                }
            }

            if (closeDate == null || (closeDate ?? DateTime.MinValue).Year < 2000)
            {
                throw new Exception("The sales data " + salesData.SalesOrderNo + " does not contain any valid Close Date.");
            }

            return closeDate;
        }
    }
}

