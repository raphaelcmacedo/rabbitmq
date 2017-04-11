using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Net;

namespace Main.SalesForceIntegration
{
    public class SalesForceService
    {
        private String login = "andre.vellinha@westcon.com", senha = "Westcon123X1ffRYYPT62EhET1U0AjFCxio";


        private SalesForceSVC.SoapClient loginClient = new SalesForceSVC.SoapClient();
        private SalesForceSVC.SoapClient serviceClient;
        private SalesForceSVC.SessionHeader sessionHeader = new SalesForceSVC.SessionHeader();
        private SalesForceSVC.LoginScopeHeader loginHeader = new SalesForceSVC.LoginScopeHeader();
        private SalesForceSVC.AssignmentRuleHeader ruleHeader = new SalesForceSVC.AssignmentRuleHeader();
        private SalesForceSVC.MruHeader mruHeader = new SalesForceSVC.MruHeader();
        private SalesForceSVC.AllowFieldTruncationHeader truncateHeader = new SalesForceSVC.AllowFieldTruncationHeader();
        private SalesForceSVC.DisableFeedTrackingHeader trackingHeader = new SalesForceSVC.DisableFeedTrackingHeader();
        private SalesForceSVC.StreamingEnabledHeader streamingHeader = new SalesForceSVC.StreamingEnabledHeader();
        private SalesForceSVC.AllOrNoneHeader allOrNoneHeader = new SalesForceSVC.AllOrNoneHeader();
        private SalesForceSVC.DuplicateRuleHeader duplicateHeader = new SalesForceSVC.DuplicateRuleHeader();
        private SalesForceSVC.LocaleOptions localeOption = new SalesForceSVC.LocaleOptions();
        private SalesForceSVC.DebuggingHeader debugHeader = new SalesForceSVC.DebuggingHeader();
        private SalesForceSVC.PackageVersion version = new SalesForceSVC.PackageVersion();
        private SalesForceSVC.PackageVersion[] versionHeader = null;
        private SalesForceSVC.EmailHeader emailHeader = new SalesForceSVC.EmailHeader();
        private SalesForceSVC.OwnerChangeOption[] ownerChanges = null;

        /*public ActionResult Index()
        {

            SalesForceSVC.QueryResult queryResult = FindByName("Teste Prion");

            SalesForceSVC.SaveResult[] saveResult = CreateOpportunity();

            SalesForceSVC.sObject[] sObjs = RetrieveRecords(saveResult);

            return View();

        }*/


        public SalesForceSVC.QueryResult FindByName(string name)
        {
            SalesForceSVC.QueryResult queryResult = null;

            try
            {
                ConfigureHeaders();

                SalesForceSVC.QueryOptions queryOptions = new SalesForceSVC.QueryOptions();
                string queryString = "SELECT StageName FROM Opportunity where Name = '" + name + "'";
                serviceClient.query(sessionHeader, queryOptions, mruHeader, versionHeader, queryString, out queryResult);


            }
            catch (Exception)
            {

                throw;
            }

            return queryResult;
        }

        public SalesForceSVC.QueryResult FindUserByName(string name)
        {
            SalesForceSVC.QueryResult queryResult = null;

            try
            {
                ConfigureHeaders();
                SalesForceSVC.QueryOptions queryOptions = new SalesForceSVC.QueryOptions();
                string queryString = "SELECT WC_External_Username__c FROM User where WC_External_Username__c = '" + name + "'";
                serviceClient.query(sessionHeader, queryOptions, mruHeader, versionHeader, queryString, out queryResult);


            }
            catch (Exception)
            {

                throw;
            }
            return queryResult;
        }

        public SalesForceSVC.QueryResult FindUserByUsername(string userName)
        {
            SalesForceSVC.QueryResult queryResult = null;

            try
            {
                ConfigureHeaders();
                SalesForceSVC.QueryOptions queryOptions = new SalesForceSVC.QueryOptions();
                string queryString = "SELECT Id FROM User where UserName = '" + userName + "'";
                serviceClient.query(sessionHeader, queryOptions, mruHeader, versionHeader, queryString, out queryResult);

                //List<SalesForceSVC.User> users = new List<SalesForceSVC.User>();
                //for (int i = 0, len = queryResult.records.Length; i < len; i++)
                //{
                //    SalesForceSVC.User user = (SalesForceSVC.User)queryResult.records[i];
                //    if (user.WC_External_Username__c != null && (user.IsActive??false))
                //    {
                //        users.Add(user);
                //    }
                //}
            }
            catch (Exception e)
            {

                throw e;
            }
            return queryResult;
        }

        public SalesForceSVC.QueryResult FindAccountByExternalId(string externalId)
        {
            SalesForceSVC.QueryResult queryResult = null;

            ConfigureHeaders();

            SalesForceSVC.QueryOptions queryOptions = new SalesForceSVC.QueryOptions();
            string queryString = "SELECT WC_SAP_Cust_ID__c FROM Account where WC_SAP_Cust_ID__c = '" + externalId + "'";
            serviceClient.query(sessionHeader, queryOptions, mruHeader, versionHeader, queryString, out queryResult);
            
            return queryResult;
        }

        //public SalesForceSVC.QueryResult FindManufacturerByExternalId(string externalId)
        //{
        //    SalesForceSVC.QueryResult queryResult = null;

        //    ConfigureHeaders();

        //    SalesForceSVC.QueryOptions queryOptions = new SalesForceSVC.QueryOptions();
        //    string queryString = "SELECT WC_Authorized_Vendors__c FROM Account where WC_SAP_Cust_ID__c = '" + externalId + "'";
        //    serviceClient.query(sessionHeader, queryOptions, mruHeader, versionHeader, queryString, out queryResult);

        //    return queryResult;
        //}

        public SalesForceSVC.QueryResult FindAllRabbitMQ()
        {
            SalesForceSVC.QueryResult queryResult = null;

            ConfigureHeaders();

            SalesForceSVC.QueryOptions queryOptions = new SalesForceSVC.QueryOptions();
            string queryString = "SELECT Name FROM Opportunity where StageName = 'RabbitMQ' ";
            serviceClient.query(sessionHeader, queryOptions, mruHeader, versionHeader, queryString, out queryResult);
            
            return queryResult;
        }

        public SalesForceSVC.SaveResult[] CreateOpportunity(SalesForceSVC.Opportunity opp = null)
        {

            SalesForceSVC.SaveResult[] saveResult = null;
            ConfigureHeaders();

            //Set timeout
            serviceClient.InnerChannel.OperationTimeout = new TimeSpan(0, 1, 0);
            
            SalesForceSVC.sObject[] objs = new List<SalesForceSVC.sObject> { opp }.ToArray();
            SalesForceSVC.LimitInfo[] infoHeader = getInfoHeader();

            serviceClient.create(sessionHeader, ruleHeader, mruHeader, truncateHeader, trackingHeader,
                streamingHeader, allOrNoneHeader, duplicateHeader, localeOption, debugHeader, versionHeader,
                    emailHeader, objs, out infoHeader, out saveResult);
            if (!saveResult[0].success)
            {
                string message = "";
                foreach (SalesForceSVC.Error error in saveResult[0].errors)
                {
                    message += error.message + "\r\n";
                }
                throw new Exception(message);
            }

               
                     
            return saveResult;
        }

        public SalesForceSVC.SaveResult[] UpdateOpportunity(SalesForceSVC.Opportunity opp = null)
        {

            SalesForceSVC.SaveResult[] saveResult = null;
            ConfigureHeaders();

            //Set timeout
            serviceClient.InnerChannel.OperationTimeout = new TimeSpan(0, 1, 0);

            SalesForceSVC.sObject[] objs = new List<SalesForceSVC.sObject> { opp }.ToArray();
            SalesForceSVC.LimitInfo[] infoHeader = getInfoHeader();

            serviceClient.update(sessionHeader, ruleHeader, mruHeader, truncateHeader, trackingHeader,
                streamingHeader, allOrNoneHeader, duplicateHeader, localeOption, debugHeader, versionHeader,
                    emailHeader,ownerChanges, objs, out infoHeader, out saveResult);
            if (!saveResult[0].success)
            {
                string message = "";
                foreach (SalesForceSVC.Error error in saveResult[0].errors)
                {
                    message += error.message + "\r\n";
                }
                throw new Exception(message);
            }



            return saveResult;
        }

        public SalesForceSVC.SaveResult[] SaveAttachment(SalesForceSVC.Attachment attachment)
        {

            SalesForceSVC.SaveResult[] saveResult = null;
            ConfigureHeaders();           

            SalesForceSVC.sObject[] objs = new List<SalesForceSVC.sObject> { attachment }.ToArray();

            SalesForceSVC.LimitInfo[] infoHeader = getInfoHeader();

            serviceClient.create(sessionHeader, ruleHeader, mruHeader, truncateHeader, trackingHeader,
                streamingHeader, allOrNoneHeader, duplicateHeader, localeOption, debugHeader, versionHeader,
                    emailHeader, objs, out infoHeader, out saveResult);
            if (!saveResult[0].success)
            {
                string message = "";
                foreach (SalesForceSVC.Error error in saveResult[0].errors)
                {
                    message += error.message + "\r\n";
                }
                throw new Exception(message);
            }



            return saveResult;
        }


        
        private SalesForceSVC.LimitInfo[] getInfoHeader()
        {
            SalesForceSVC.LimitInfo info = new SalesForceSVC.LimitInfo();
            info.type = "API REQUESTS";
            info.current = 20;
            info.limit = 250;
            SalesForceSVC.LimitInfo[] infoHeader = new List<SalesForceSVC.LimitInfo>() { info }.ToArray();
            return infoHeader;

        }
        
        private void ConfigureHeaders()
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            SalesForceSVC.LoginResult lr = loginClient.login(loginHeader, login, senha);
            if (lr.passwordExpired)
            {
                throw new Exception("Password expired");
            }
            EndpointAddress endpoint = new EndpointAddress(lr.serverUrl);
            sessionHeader.sessionId = lr.sessionId;
            serviceClient = new SalesForceSVC.SoapClient("Soap", endpoint);

            ruleHeader.useDefaultRule = true;

            mruHeader.updateMru = true;

            truncateHeader.allowFieldTruncation = true;

            trackingHeader.disableFeedTracking = false;

            versionHeader = new List<SalesForceSVC.PackageVersion>() { version }.ToArray();

            allOrNoneHeader.allOrNone = false;

            //https://developer.salesforce.com/docs/atlas.en-us.api.meta/api/sforce_api_header_duplicateruleheader.htm
            duplicateHeader.allowSave = false;
            duplicateHeader.includeRecordDetails = true;
            duplicateHeader.runAsCurrentUser = true;

            localeOption.language = "en_US";

            debugHeader.debugLevel = SalesForceSVC.DebugLevel.None;

            //https://developer.salesforce.com/docs/atlas.en-us.api.meta/api/sforce_api_header_emailheader.htm
            emailHeader.triggerAutoResponseEmail = false;
            emailHeader.triggerOtherEmail = false;
            emailHeader.triggerUserEmail = false;

        }
        
    }
}
