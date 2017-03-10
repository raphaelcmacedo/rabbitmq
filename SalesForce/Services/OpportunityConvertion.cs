using Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForce.Services
{
    public static class OpportunityConvertion
    {
        public static Opportunity SalesDataToOpportunity(SalesData salesData)
        {
            Opportunity opportunity = new Opportunity();
            opportunity.Name = salesData.SalesOrderNo;
            if (salesData.SoldTo != null)
            {
                opportunity.AccountID = salesData.SoldTo.WestconId;
            }

            /* Hardcoded */
            //opportunity.CloseDate = 
            opportunity.StageName = "Qualification";
            opportunity.Type = "Sales Opportunity";
            opportunity.WCType = "Renewals";
            opportunity.GeneratedBy = "Renewals";
            
            decimal totalBillingValue = 0;
            decimal totalBillingCost = 0;
            foreach (LineItem lineItem in salesData.LineItems)
            {
                totalBillingValue += lineItem.BillingValue;
                totalBillingCost += lineItem.BillingCost;
            }
            opportunity.TotalBillingValue = totalBillingValue;
            opportunity.TotalBillingCost = totalBillingCost;

            SetCountingFields(salesData, opportunity);

            return opportunity;
        }

        public static void SetCountingFields(SalesData salesData, Opportunity opportunity)
        {
            Dictionary<string, int> accountManagerIDs = new Dictionary<string, int>();
            Dictionary<string, int> salesPractices = new Dictionary<string, int>();
            foreach (LineItem lineItem in salesData.LineItems)
            {
                string accountManager = lineItem.AccountManagerId;
                string salesPractice = lineItem.SalesPractice;

                int totalAccountManager = 0;
                int totalSalesPractice = 0;

                //Count account manager
                if (accountManagerIDs.ContainsKey(accountManager))
                {
                    totalAccountManager = accountManagerIDs[accountManager];
                }
                totalAccountManager++;
                accountManagerIDs[accountManager] = totalAccountManager;

                //Count sales practices
                if (salesPractices.ContainsKey(salesPractice))
                {
                    totalSalesPractice = salesPractices[salesPractice];
                }
                totalSalesPractice++;
                salesPractices[salesPractice] = totalSalesPractice;

            }
            //Account Manager
            int maxAccountManagers = accountManagerIDs.Values.Max();
            string mainAccountManager = accountManagerIDs.FirstOrDefault(x => x.Value == maxAccountManagers).Key;
            opportunity.MainAccountManagerID = mainAccountManager;

            //Sales Practice
            int maxSalesPractice = salesPractices.Values.Max();
            string ownerID = salesPractices.FirstOrDefault(x => x.Value == maxSalesPractice).Key;
            opportunity.OwnerID = ownerID;
        }
    }
}

