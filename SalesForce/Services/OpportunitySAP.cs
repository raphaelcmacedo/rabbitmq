using Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Main.Services
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

            salesData.SalesOrderNo = Util.GetValue(node, "ns0:SalesOrderNo");
            salesData.SalesOrg = Util.GetValue(node, "ns0:SalesOrg");
            salesData.SourceSystem = Util.GetValue(node, "ns0:SourceSystem");
            salesData.ExtractionRuleType = Util.GetValue(node, "ns0:ExtractionRuleType");

            //Sold To
            XmlNode soldToNode = node["ns0:SoldTo"];
            XmlNode soldToContactNode = node["ns0:SoldToContact"];
            salesData.SoldTo = this.ReadCompany(soldToNode, soldToContactNode);

            //Items
            nodeList = xml.GetElementsByTagName("ns0:LineItem");
            salesData.LineItems = this.ReadLineItems(nodeList);

            //Message
            string message = Util.Base64Encode(text);
            salesData.Message = message;

            return salesData;
        }

        private List<LineItem> ReadLineItems(XmlNodeList nodeList)
        {
            List<LineItem> items = new List<LineItem>();
            foreach (XmlNode node in nodeList)
            {
                LineItem item = new LineItem();
                item.LineNumber = Util.GetValue(node, "ns0:LineNo");
                item.SKU = Util.GetValue(node, "ns0:SKUNo");
                item.SKUDescription = Util.GetValue(node, "ns0:SKUDescription");
                item.ProductType = Util.GetValue(node, "ns0:ProductType");
                item.SalesDistrict = Util.GetValue(node, "ns0:SalesDistrict");
                item.SalesPractice = Util.GetValue(node, "ns0:SalesPractice");
                item.CreatedBy = Util.GetValue(node, "ns0:CreatedBy");
                item.AccountManagerId = Util.GetValue(node, "ns0:AccountManagerID");
                item.AccountManagerName = Util.GetValue(node, "ns0:AccountManagerName");
                item.SalesOrderQty = Util.ToInt(node, "ns0:SalesOrderQty");
                item.SalesUnit = Util.GetValue(node, "ns0:SalesUnit");
                item.BillingCost = Util.ToDecimal(node, "ns0:BillingCost");
                item.BillingValue = Util.ToDecimal(node, "ns0:BillingValue");
                item.DocumentCurrency = Util.GetValue(node, "ns0:DocumentCurrency");
                item.ContractNo = Util.GetValue(node, "ns0:ContractNo");
                item.StartDate = Util.ToDate(node, "ns0:StartDate");
                item.EndDate = Util.ToDate(node, "ns0:EndDate");
                item.ManufacturerQuoteNo = Util.GetValue(node, "ns0:ManufacturerQuoteNo");
                item.ModelNo = Util.GetValue(node, "ns0:ModelNo");
                item.NSP = Util.GetValue(node, "ns0:NSP");
                item.IsEarliestInvoicedItem = Util.GetValue(node, "ns0:IsEarliestInvoicedItem");
                item.EarliestBillingPostDate = Util.ToDate(node, "ns0:EarliestBillingPostDate");
                item.ManufacturerID = Util.GetValue(node, "ns0:ManufacturerID");
                item.ManufacturerName = Util.GetValue(node, "ns0:ManufacturerName");
                item.ManufacturerAccreditationLevelForSoldTo = Util.GetValue(node, "ns0:ManufacturerAccreditationLevelForSoldTo");
                //item.Discount = Util.ToDecimal(node["ns0:DealID"]["ns0:Discount"].InnerText);
                item.PromoID = Util.GetValue(node, "ns0:PromoID");
                item.Promo2ID = Util.GetValue(node, "ns0:Promo2ID");
                item.AccreditationID = Util.GetValue(node, "ns0:AccreditationID");
                item.LineItemSerialNo = Util.GetValue(node, "ns0:LineItemSerialNo");

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
                if (contractNode != null)
                {
                    Contract contract = new Contract();
                    item.Contract = contract;
                    contract.ContractNo = Util.GetValue(contractNode, "ns0:ContractNo");
                    contract.SalesOrderNo = Util.GetValue(contractNode, "ns0:SalesOrderNo");
                    contract.ManufacturerInvoiceNo = Util.GetValue(contractNode, "ns0:ManufacturerInvoiceNo");
                    contract.ManufacturerQuoteNo = Util.GetValue(contractNode, "ns0:ManufacturerQuoteNo");
                    contract.ModelNo = Util.GetValue(contractNode, "ns0:ModelNo");
                    contract.NSP = Util.GetValue(contractNode, "ns0:NSP");
                    contract.StartDate = Util.ToDate(contractNode, "ns0:StartDate");
                    contract.EndDate = Util.ToDate(contractNode, "ns0:EndDate");
                }
                

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
            company.WestconId = Util.GetValue(companyNode, "ns0:WestconID");
            company.Name = Util.GetValue(companyNode, "ns0:Name");
            company.CountryPrefix = Util.GetValue(companyNode, "ns0:CountryPrefix");
            company.WorkPhone = Util.GetValue(companyNode, "ns0:WorkPhone");
            company.FaxNumber = Util.GetValue(companyNode, "ns0:FaxNumber");

            //Address Info
            XmlNode addressNode = companyNode["ns0:Address"];
            address.Addr1 = Util.GetValue(addressNode, "ns0:Addr1");
            address.Addr2 = Util.GetValue(addressNode, "ns0:Addr2");
            address.Addr3 = Util.GetValue(addressNode, "ns0:Addr3");
            address.Addr4 = Util.GetValue(addressNode, "ns0:Addr4");
            address.City = Util.GetValue(addressNode, "ns0:City");
            address.State = Util.GetValue(addressNode, "ns0:State");
            address.PostalCode = Util.GetValue(addressNode, "ns0:PostalCode");
            address.Country = Util.GetValue(addressNode, "ns0:Country");

            if (contactNode != null)
            {
                Contact contact = new Contact();
                company.Contact = contact;
                contact.WestconId = Util.GetValue(contactNode, "ns0:WestconID");
                contact.WorkPhone = Util.GetValue(contactNode, "ns0:WorkPhone");
                contact.FaxNumber = Util.GetValue(contactNode, "ns0:FaxNumber");
                contact.EmailAddress = Util.GetValue(contactNode, "ns0:EmailAddress");
                contact.Extension = Util.GetValue(contactNode, "ns0:Extension");
                contact.MobilePhone = Util.GetValue(contactNode, "ns0:MobilePhone");
            }

            return company;
        }

        public SalesForceSVC.Opportunity ConvertOpportunity(Opportunity opp)
        {
            SalesForceSVC.Opportunity opportunity = new SalesForceSVC.Opportunity();
            SalesForceSVC.QueryResult result;
            SalesForceIntegration.SalesForceService service = new SalesForceIntegration.SalesForceService();
            //Required fields
            opportunity.CreatedDate = DateTime.Now;
            opportunity.CloseDate = opp.CloseDate;
            opportunity.CloseDateSpecified = true;

            //Opportunity fields
            opportunity.Name = opp.Name;
            /*opp.OwnerID = "Linda.Cardellini@westcon.com";
            result = service.FindUserByEmail(opp.OwnerID);
            opportunity.Owner = (result != null && result.records != null && result.records.Length > 0) ? (SalesForceSVC.User)result.records[0] : null;
            */

            opportunity.Owner = null;

            result = service.FindAccountByExternalId(opp.AccountID);
            if (result != null && result.records != null && result.records.Length > 0)
            {
                opportunity.Account = (SalesForceSVC.Account)result.records[0];
            }
            else
            {
                throw new Exception("The account with ID " + opp.AccountID + " has not be found in SalesForce, so the Opportunity " + opp.Name + " has not been created");
            }

            result = service.FindAccountByExternalId(opp.ManufacturerID);
            if (result != null && result.records != null && result.records.Length > 0)
            {
                opportunity.WC_Authorized_Vendors__r = (SalesForceSVC.Account)result.records[0];
                opportunity.WC_Authorized_Vendors__c = opportunity.WC_Authorized_Vendors__r.Id;
            }
            else
            {
                throw new Exception("The manufacturer account with ID " + opp.ManufacturerID + " has not be found in SalesForce, so the Opportunity " + opp.Name + " has not been created");

            }

            //opportunity.WC_Authorized_Vendors__c = opp.ManufacturerName;
            opportunity.WC_Westcon_Opportunity_Type__c = opp.WCType;

            result = service.FindUserByName(opp.MainAccountManagerID);
            opportunity.WC_Account_Manager_Name__r = (result != null && result.records != null && result.records.Length > 0) ? (SalesForceSVC.User)result.records[0] : null;

            opportunity.StageName = opp.StageName;
            opportunity.CurrencyIsoCode = opp.CurrencyCode;
            //opportunity.Amount = 0;
            opportunity.WC_Forecast_Revenue__c = (double)opp.TotalBillingValue;
            opportunity.WC_Gross_Margin_Amount__c = (double)(opp.TotalBillingValue - opp.TotalBillingCost);
            opportunity.WC_Gross_Margin_Amount__cSpecified = true;
            if (opp.TotalBillingValue > 0)
            {
                opportunity.WC_Gross_Margin_Percent__c = (double)((opp.TotalBillingValue - opp.TotalBillingCost) * 100 / opp.TotalBillingValue);
            }
            opportunity.Amount = (double)opp.TotalBillingValue;
            opportunity.Type = opp.Type;

            //INNO-217
            opportunity.WC_End_User__c = opp.EndUserName;

            return opportunity;

        }
    }
}
