using Main.Models;
using Main.Repositories;
using Main.SalesForceIntegration;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Services
{
    public class OpportunityIntegration
    {
        public SalesForce.SalesForceSVC.Opportunity CreateOpportunity(string message)
        {
            OpportunitySAP sap = new OpportunitySAP();
            SalesData salesData = sap.ReadXML(message);
            SalesForce.SalesForceSVC.Opportunity opportunity = sap.ConvertOpportunity(salesData);
            SalesForceService service = new SalesForceService();
            SalesForce.SalesForceSVC.SaveResult[] result = service.CreateOpportunity(opportunity);
            

            //Grava Sales Data
            using (SalesDataRepository repository = new SalesDataRepository())
            {
                repository.Add(salesData);
                
            }

            return opportunity;
        }

        public SalesForce.SalesForceSVC.sObject[] FindAllSalesForce()
        {
            SalesForceService service = new SalesForceService();
            SalesForce.SalesForceSVC.QueryResult result = service.FindAllRabbitMQ();
            return result.records;
        }
    }
}
