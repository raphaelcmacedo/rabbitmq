using Main.Models;
using Main.Repositories;
using Main.SalesForceIntegration;
using SalesForce.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Main.Services
{
    public class OpportunityIntegration
    {
        public string CreateOpportunity(string message)
        {
            //Read xml
            OpportunitySAP sap = new OpportunitySAP();
            SalesData salesData = sap.ReadXML(message);
            AttachmentToFile attachmentService = new AttachmentToFile();

            //Save Sales Data
            using (SalesDataRepository repository = new SalesDataRepository())
            {
                repository.Add(salesData);
            }
            //Create sales data spreadsheet
            string sheetBase64 = attachmentService.CreateExcel(salesData);

            //Convert sales data to opportunity
            Opportunity opportunity = OpportunityConvertion.SalesDataToOpportunity(salesData);
            opportunity.RelatedAttachment_base64 = sheetBase64;

            //Save Opportunity
            using (OpportunityRepository repository = new OpportunityRepository())
            {
                repository.Add(opportunity);
            }

            //Create xml message
            string xml = Util.ToXml(opportunity, typeof(Opportunity));

            return xml;
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
