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
        private static int i = 0;

        public string CreateOpportunity(string message)
        {
            try
            {
                //Read xml
                OpportunitySAP sap = new OpportunitySAP();
                SalesData salesData = sap.ReadXML(message);
                AttachmentToFile attachmentService = new AttachmentToFile();

                //Create sales data spreadsheet
                string sheetBase64 = attachmentService.CreateExcel(salesData);

                //Convert sales data to opportunity
                Opportunity opportunity = OpportunityConvertion.SalesDataToOpportunity(salesData);
                opportunity.RelatedAttachment_base64 = sheetBase64;

                using (OpportunityRepository repository = new OpportunityRepository())
                {
                    repository.SaveMessageEntries(salesData, opportunity);
                }

                //Create xml message
                string xml = Util.ToXml(opportunity, typeof(Opportunity));
                Console.WriteLine(++i);
                return xml;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
        }

        public void CreateSalesForceOpportunity(string messsage)
        {
            
            Opportunity opportunity;
            OpportunitySAP conversor = new OpportunitySAP();
            XmlSerializer serializer = new XmlSerializer(typeof(Opportunity));

            using (TextReader reader = new StringReader(messsage))
            {
                opportunity = (Opportunity)serializer.Deserialize(reader);
            }            

            
            SalesForceSVC.Opportunity OpportunitySalesForce = conversor.ConvertOpportunity(opportunity);
            SalesForceService service = new SalesForceService();
            AttachmentToFile fileService = new AttachmentToFile();
            SalesForceSVC.SaveResult[] result = null;

            if (string.IsNullOrEmpty(opportunity.SalesForceID))
            {
               result = service.CreateOpportunity(OpportunitySalesForce);
            }else
            {
                result = service.UpdateOpportunity(OpportunitySalesForce);
            }

            if (result != null && result.Length > 0)
            {
                string parentId = result[0].id;
                SalesForceSVC.Attachment attachment = fileService.Base64ToSalesForceAttachment(opportunity.RelatedAttachment_base64, parentId);
                service.SaveAttachment(attachment);

                using (OpportunityRepository repository = new OpportunityRepository())
                {
                    opportunity = repository.Find((int) opportunity.OpportunityId);
                    opportunity.SalesForceID = parentId;
                    repository.Update(opportunity);
                }

            }

        }

        public SalesForceSVC.sObject[] FindAllSalesForce()
        {
            SalesForceService service = new SalesForceService();
            SalesForceSVC.QueryResult result = service.FindAllRabbitMQ();
            return result.records;
        }
    }
}
