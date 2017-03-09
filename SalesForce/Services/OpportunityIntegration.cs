using Main.Models;
using Main.Repositories;
using Main.SalesForceIntegration;
using SalesForce.Services;
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
        public string CreateOpportunity(string message)
        {
            //Read xml
            OpportunitySAP sap = new OpportunitySAP();
            SalesData salesData = sap.ReadXML(message);
            //Save Sales Data
            using (SalesDataRepository repository = new SalesDataRepository())
            {
                repository.Add(salesData);

            }
            //Create sales data spreadsheet


            //Convert sales data to opportunity
            Opportunity opportunity = OpportunityConvertion.SalesDataToOpportunity(salesData);







            return "";
        }

        public void CreateSalesForceOpportunity(string messsage)
        {
            /*SalesForce.SalesForceSVC.Opportunity opportunity = sap.ConvertOpportunity(salesData);
            SalesForceService service = new SalesForceService();
            SalesForce.SalesForceSVC.SaveResult[] result = service.CreateOpportunity(opportunity);*/

        }

        public SalesForce.SalesForceSVC.sObject[] FindAllSalesForce()
        {
            SalesForceService service = new SalesForceService();
            SalesForce.SalesForceSVC.QueryResult result = service.FindAllRabbitMQ();
            return result.records;
        }
    }
}
