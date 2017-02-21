using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SalesForce
{
    public class OpportunitySAP
    {
        public SalesForceSVC.Opportunity ConvertXML(string text)
        {
            SalesForceSVC.Opportunity opportunity = new SalesForceSVC.Opportunity();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(text);

            var nsmgr = new XmlNamespaceManager(xml.NameTable);
            nsmgr.AddNamespace("ns1", "http://www.example.org/WestconCommon");

            XmlNodeList nodeList = xml.GetElementsByTagName("ns1:Header");
            XmlNode node = nodeList[0];

            opportunity.Name = node["ns1:SalesOrderNo"].InnerText;
            opportunity.CreatedDate = DateTime.Now;
            opportunity.CloseDate = DateTime.Now.AddDays(10);
            opportunity.CloseDateSpecified = true;
            opportunity.WC_Region__c = node["ns1:SalesOrg"].InnerText;
            opportunity.WC_End_User__c = node["ns1:EndUser"].InnerText;
            opportunity.StageName = "RabbitMQ";

            return opportunity;
        }
    }
}
