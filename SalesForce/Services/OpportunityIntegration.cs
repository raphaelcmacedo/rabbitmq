using SalesForce.Models;
using SalesForce.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForce.Services
{
    public class OpportunityIntegration
    {
        public SalesForceSVC.Opportunity CreateOpportunity(string message)
        {
            OpportunitySAP sap = new OpportunitySAP();
            SalesData salesData = sap.ReadXML(message);
            SalesForceSVC.Opportunity opportunity = sap.ConvertOpportunity(salesData);
            OpportunityService service = new OpportunityService();
            SalesForceSVC.SaveResult[] result = service.CreateOpportunity(opportunity);
            salesData.SalesForceId = result[0].id;

            //Grava Sales Data
            using (SalesDataRepository repository = new SalesDataRepository())
            {
                repository.Add(salesData);
                
            }

            return opportunity;
        }

        public SalesForceSVC.sObject[] FindAllSalesForce()
        {
            OpportunityService service = new OpportunityService();
            SalesForceSVC.QueryResult result = service.FindAllRabbitMQ();
            return result.records;
        }
    }
}
