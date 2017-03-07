using SalesForce.Models;
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
        public SalesData ReadXML(string text)
        {
            SalesData salesData = new SalesData();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(text);
            
            XmlNodeList nodeList = xml.GetElementsByTagName("ns0:Header");
            XmlNode node = nodeList[0];

            salesData.SalesOrderNo = node["ns0:SalesOrderNo"].InnerText;
            salesData.SalesOrg = node["ns0:SalesOrg"].InnerText;
            salesData.SourceSystem = node["ns0:SourceSystem"].InnerText;
            salesData.ExtractionRuleType = node["ns0:ExtractionRuleType"].InnerText;

            //Sold To
            XmlNode soldToNode = node["ns0:SoldTo"];
            XmlNode soldToContactNode = node["ns0:SoldToContact"];
            salesData.SoldTo = this.ReadCompany(soldToNode, soldToContactNode);

            //Items
            nodeList = xml.GetElementsByTagName("ns0:LineItem");
            salesData.LineItems = this.ReadLineItems(nodeList);

            return salesData;
        }

        private List<LineItem> ReadLineItems(XmlNodeList nodeList)
        {
            List<LineItem> items = new List<LineItem>();
            foreach (XmlNode node in nodeList)
            {
                LineItem item = new LineItem();
                item.LineNumber = node["ns0:LineNo"].InnerText;
                item.SKU = node["ns0:SKUNo"].InnerText;
                item.SKUDescription = node["ns0:SKUDescription"].InnerText;
                item.ProductType = node["ns0:ProductType"].InnerText;
                item.SalesDistrict = node["ns0:SalesDistrict"].InnerText;
                item.SalesPractice = node["ns0:SalesPractice"].InnerText;
                item.CreatedBy = node["ns0:CreatedBy"].InnerText;
                item.AccountManagerId = node["ns0:AccountManagerID"].InnerText;
                item.AccountManagerName = node["ns0:AccountManagerName"].InnerText;
                item.SalesOrderQty = Util.ToInt(node["ns0:SalesOrderQty"].InnerText);
                item.SalesUnit = node["ns0:SalesUnit"].InnerText;
                item.BillingCost = Util.ToDecimal(node["ns0:BillingCost"].InnerText);
                item.BillingValue = Util.ToDecimal(node["ns0:BillingValue"].InnerText);
                item.DocumentCurrency = node["ns0:DocumentCurrency"].InnerText;
                item.ContractNo = node["ns0:ContractNo"].InnerText;
                item.StartDate = Util.ToDate(node["ns0:StartDate"].InnerText);
                item.EndDate = Util.ToDate(node["ns0:EndDate"].InnerText);
                item.ManufacturerQuoteNo = node["ns0:ManufacturerQuoteNo"].InnerText;
                item.ModelNo = node["ns0:ModelNo"].InnerText;
                item.NSP = node["ns0:NSP"].InnerText;
                item.IsEarliestInvoicedItem = node["ns0:IsEarliestInvoicedItem"].InnerText;
                item.EarliestBillingPostDate = Util.ToDate(node["ns0:EarliestBillingPostDate"].InnerText);
                item.ManufacturerID = node["ns0:ManufacturerID"].InnerText;
                item.ManufacturerName = node["ns0:ManufacturerName"].InnerText;
                item.ManufacturerAccreditationLevelForSoldTo = node["ns0:ManufacturerAccreditationLevelForSoldTo"].InnerText;
                //item.Discount = Util.ToDecimal(node["ns0:DealID"]["ns0:Discount"].InnerText);
                item.PromoID = node["ns0:PromoID"].InnerText;
                item.Promo2ID = node["ns0:Promo2ID"].InnerText;
                item.AccreditationID = node["ns0:AccreditationID"].InnerText;
                item.LineItemSerialNo = node["ns0:LineItemSerialNo"].InnerText;

                //Ship To
                XmlNode shipToNode = node["ns0:ShipTo"];
                XmlNode shipToContactNode = node["ns0:ShipToContact"];
                item.ShipTo = this.ReadCompany(shipToNode, shipToContactNode);

                //Ship To
                XmlNode endUserNode = node["ns0:EndUser"];
                XmlNode endUserContactNode = node["ns0:EndUserContact"];
                item.EndUser = this.ReadCompany(endUserNode, endUserContactNode);

                //Contract
                XmlNode contractNode = node["ns0:Contract"];
                Contract contract = new Contract();
                item.Contract = contract;
                contract.ContractNo = contractNode["ns0:ContractNo"].InnerText;
                contract.SalesOrderNo = contractNode["ns0:SalesOrderNo"].InnerText;
                contract.ManufacturerInvoiceNo = contractNode["ns0:ManufacturerInvoiceNo"].InnerText;
                contract.ManufacturerQuoteNo = contractNode["ns0:ManufacturerQuoteNo"].InnerText;
                contract.ModelNo = contractNode["ns0:ModelNo"].InnerText;
                contract.NSP = contractNode["ns0:NSP"].InnerText;
                contract.StartDate = Util.ToDate(contractNode["ns0:StartDate"].InnerText);
                contract.EndDate = Util.ToDate(contractNode["ns0:EndDate"].InnerText);

                items.Add(item);
            }

            return items;
        }

        private Company ReadCompany(XmlNode companyNode, XmlNode contactNode)
        {
            Company company = new Company();
            Address address = new Address();
            company.Address = address;

            //Company Info
            company.WestconId = companyNode["ns0:WestconID"].InnerText;
            company.Name = companyNode["ns0:Name"].InnerText;
            company.CountryPrefix = companyNode["ns0:CountryPrefix"].InnerText;
            company.WorkPhone = companyNode["ns0:WorkPhone"].InnerText;
            company.FaxNumber = companyNode["ns0:FaxNumber"].InnerText;

            //Address Info
            XmlNode addressNode = companyNode["ns0:Address"];
            address.Addr1 = addressNode["ns0:Addr1"].InnerText;
            address.Addr2 = addressNode["ns0:Addr2"].InnerText;
            address.Addr3 = addressNode["ns0:Addr3"].InnerText;
            address.Addr4 = addressNode["ns0:Addr4"].InnerText;
            address.City = addressNode["ns0:City"].InnerText;
            address.State = addressNode["ns0:State"].InnerText;
            address.PostalCode = addressNode["ns0:PostalCode"].InnerText;
            address.Country = addressNode["ns0:Country"].InnerText;

            if (contactNode != null)
            {
                Contact contact = new Contact();
                company.Contact = contact;
                contact.WestconId = contactNode["ns0:WestconID"].InnerText;
                contact.WorkPhone = contactNode["ns0:WorkPhone"].InnerText;
                contact.FaxNumber = contactNode["ns0:FaxNumber"].InnerText;
                contact.EmailAddress = contactNode["ns0:EmailAddress"].InnerText;
                contact.Extension = contactNode["ns0:Extension"].InnerText;
                contact.MobilePhone = contactNode["ns0:MobilePhone"].InnerText;
            }

            return company;
        }

        public SalesForceSVC.Opportunity ConvertOpportunity(SalesData salesData)
        {
            SalesForceSVC.Opportunity opportunity = new SalesForceSVC.Opportunity();

            //Required fields
            opportunity.CreatedDate = DateTime.Now;
            opportunity.CloseDate = DateTime.Now.AddDays(10);
            opportunity.CloseDateSpecified = true;

            //SalesOrder fields
            opportunity.Name = salesData.SalesOrderNo;
            opportunity.WC_Region__c = salesData.SalesOrg;

            //Identify RabbitMQ
            opportunity.StageName = "RabbitMQ";

            return opportunity;

        }
    }
}
