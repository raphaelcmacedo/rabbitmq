﻿using Main.Models;
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
            opportunity.Type = "Renewal";
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
            opportunity.SalesDataId = salesData.SalesDataId;

            SetCountingFields(salesData, opportunity);

            opportunity.SalesData = salesData;

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
                string sku = lineItem.SKU;

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

            opportunity.CloseDate = CalculateEndDate(salesData);



        }

        private static DateTime? CalculateEndDate(SalesData salesData)
        {
            //Try to fetch date from EndDate
            DateTime? closeDate = salesData.LineItems.Min(x => x.EndDate);
            if (closeDate != null)
            {// If we have the EndDate, we need to add 1 Month
                 closeDate = closeDate.Value.AddMonths(1);
            }
            else
            {//Try to fetch date from BillingDate
                closeDate = salesData.LineItems.Min(x => x.EarliestBillingPostDate);
                if (closeDate != null)
                {// If we have the Billing Date, we need to add 13 Month
                    closeDate = closeDate.Value.AddMonths(13);
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

