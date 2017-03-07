using SalesForce.Models;
using System;
using System.Collections.Generic;
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
            service.CreateOpportunity(opportunity);

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
