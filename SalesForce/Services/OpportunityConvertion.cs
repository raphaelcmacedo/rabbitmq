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
            opportunity.SalesDataId = salesData.SalesDataId;

            SetCountingFields(salesData, opportunity);

            opportunity.SalesData = salesData;

            return opportunity;
        }

        public static void SetCountingFields(SalesData salesData, Opportunity opportunity)
        {
            Dictionary<string, int> accountManagerIDs = new Dictionary<string, int>();
            Dictionary<string, int> salesPractices = new Dictionary<string, int>();
            Dictionary<string, DateTime?> minCloseDate = new Dictionary<string, DateTime?>();
            Dictionary<string, DateTime?> minEarliestBillingDate = new Dictionary<string, DateTime?>();

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

                if (lineItem.EndDate != null)
                {
                    minCloseDate.Add(sku, lineItem.EndDate);
                }
                if (lineItem.EarliestBillingPostDate != null)
                {
                    minEarliestBillingDate.Add(sku, lineItem.EarliestBillingPostDate);
                }

            }
            //Account Manager
            int maxAccountManagers = accountManagerIDs.Values.Max();
            string mainAccountManager = accountManagerIDs.FirstOrDefault(x => x.Value == maxAccountManagers).Key;
            opportunity.MainAccountManagerID = mainAccountManager;

            //Sales Practice
            int maxSalesPractice = salesPractices.Values.Max();
            string ownerID = salesPractices.FirstOrDefault(x => x.Value == maxSalesPractice).Key;
            opportunity.OwnerID = ownerID;

            DateTime oppCloseDate;

            if (minCloseDate.Values.Count > 0)
            {
                oppCloseDate = minCloseDate.Values.Min() ?? DateTime.Now;
                oppCloseDate = oppCloseDate.AddMonths(1);
                opportunity.CloseDate = oppCloseDate;
            }else if(minEarliestBillingDate.Values.Count > 0)
            {
                oppCloseDate = minEarliestBillingDate.Values.Min() ?? DateTime.Now;
                oppCloseDate = oppCloseDate.AddMonths(13);
                //if the year is lower than 2000, uses the min value of sql server
                opportunity.CloseDate = (oppCloseDate.Year < 2000) ? (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue : oppCloseDate;
            }
        }
    }
}

