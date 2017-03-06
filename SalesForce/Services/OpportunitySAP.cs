using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SalesForce.Services
{
    public class OpportunitySAP
    {
        public SalesForceSVC.Opportunity ConvertXML(string text)
        {
            SalesForceSVC.Opportunity opportunity = new SalesForceSVC.Opportunity();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(text);

            //var nsmgr = new XmlNamespaceManager(xml.NameTable);
            //nsmgr.AddNamespace("ns1", "http://www.example.org/WestconCommon");

            XmlNodeList nodeList = xml.GetElementsByTagName("ns0:Header");
            XmlNode node = nodeList[0];

            //Required fields
            opportunity.CreatedDate = DateTime.Now;
            opportunity.CloseDate = DateTime.Now.AddDays(10);
            opportunity.CloseDateSpecified = true;

            //SalesOrder fields
            opportunity.Name = node["ns0:SalesOrderNo"].InnerText;
            opportunity.WC_Region__c = node["ns0:SalesOrg"].InnerText;
            
            //Sold To
            XmlNode soldToNode = node["ns0:SoldTo"];
            //opportunity.WC_Sold_To_Customer_Number__c = soldToNode["ns0:WestconID"].InnerText; Não tive permissão para setar
            
            opportunity.StageName = "RabbitMQ";

            return opportunity;
        }
    }
}
