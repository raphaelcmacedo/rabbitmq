using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Main.SalesForceIntegration
{
    public class SalesForceService
    {
        private String login = "andre.vellinha@westcon.com", senha = "1234WestconuYoID1asym58ULuQUXNxF2NwJ";


        private SalesForce.SalesForceSVC.SoapClient loginClient = new SalesForce.SalesForceSVC.SoapClient();
        private SalesForce.SalesForceSVC.SoapClient serviceClient;
        private SalesForce.SalesForceSVC.SessionHeader sessionHeader = new SalesForce.SalesForceSVC.SessionHeader();
        private SalesForce.SalesForceSVC.LoginScopeHeader loginHeader = new SalesForce.SalesForceSVC.LoginScopeHeader();
        private SalesForce.SalesForceSVC.AssignmentRuleHeader ruleHeader = new SalesForce.SalesForceSVC.AssignmentRuleHeader();
        private SalesForce.SalesForceSVC.MruHeader mruHeader = new SalesForce.SalesForceSVC.MruHeader();
        private SalesForce.SalesForceSVC.AllowFieldTruncationHeader truncateHeader = new SalesForce.SalesForceSVC.AllowFieldTruncationHeader();
        private SalesForce.SalesForceSVC.DisableFeedTrackingHeader trackingHeader = new SalesForce.SalesForceSVC.DisableFeedTrackingHeader();
        private SalesForce.SalesForceSVC.StreamingEnabledHeader streamingHeader = new SalesForce.SalesForceSVC.StreamingEnabledHeader();
        private SalesForce.SalesForceSVC.AllOrNoneHeader allOrNoneHeader = new SalesForce.SalesForceSVC.AllOrNoneHeader();
        private SalesForce.SalesForceSVC.DuplicateRuleHeader duplicateHeader = new SalesForce.SalesForceSVC.DuplicateRuleHeader();
        private SalesForce.SalesForceSVC.LocaleOptions localeOption = new SalesForce.SalesForceSVC.LocaleOptions();
        private SalesForce.SalesForceSVC.DebuggingHeader debugHeader = new SalesForce.SalesForceSVC.DebuggingHeader();
        private SalesForce.SalesForceSVC.PackageVersion version = new SalesForce.SalesForceSVC.PackageVersion();
        private SalesForce.SalesForceSVC.PackageVersion[] versionHeader = null;
        private SalesForce.SalesForceSVC.EmailHeader emailHeader = new SalesForce.SalesForceSVC.EmailHeader();

        /*public ActionResult Index()
        {

            SalesForceSVC.QueryResult queryResult = FindByName("Teste Prion");

            SalesForceSVC.SaveResult[] saveResult = CreateOpportunity();

            SalesForceSVC.sObject[] sObjs = RetrieveRecords(saveResult);

            return View();

        }*/


        public SalesForce.SalesForceSVC.QueryResult FindByName(string name)
        {
            SalesForce.SalesForceSVC.QueryResult queryResult = null;

            try
            {
                ConfigureHeaders();

                SalesForce.SalesForceSVC.QueryOptions queryOptions = new SalesForce.SalesForceSVC.QueryOptions();
                string queryString = "SELECT StageName FROM Opportunity where Name = '" + name + "'";
                serviceClient.query(sessionHeader, queryOptions, mruHeader, versionHeader, queryString, out queryResult);


            }
            catch (Exception)
            {

                throw;
            }

            return queryResult;
        }

        public SalesForce.SalesForceSVC.QueryResult FindAllRabbitMQ()
        {
            SalesForce.SalesForceSVC.QueryResult queryResult = null;

            ConfigureHeaders();

            SalesForce.SalesForceSVC.QueryOptions queryOptions = new SalesForce.SalesForceSVC.QueryOptions();
            string queryString = "SELECT Name FROM Opportunity where StageName = 'RabbitMQ' ";
            serviceClient.query(sessionHeader, queryOptions, mruHeader, versionHeader, queryString, out queryResult);
            
            return queryResult;
        }

        public SalesForce.SalesForceSVC.SaveResult[] CreateOpportunity(SalesForce.SalesForceSVC.Opportunity opp = null)
        {

            SalesForce.SalesForceSVC.SaveResult[] saveResult = null;
            ConfigureHeaders();

            if (opp == null)
            {
                opp = this.FillOpportunityObj();
            }


            SalesForce.SalesForceSVC.sObject[] objs = new List<SalesForce.SalesForceSVC.sObject> { opp }.ToArray();

            SalesForce.SalesForceSVC.LimitInfo[] infoHeader = getInfoHeader();

            serviceClient.create(sessionHeader, ruleHeader, mruHeader, truncateHeader, trackingHeader,
                streamingHeader, allOrNoneHeader, duplicateHeader, localeOption, debugHeader, versionHeader,
                    emailHeader, objs, out infoHeader, out saveResult);
            if (!saveResult[0].success)
            {
                string message = "";
                foreach (SalesForce.SalesForceSVC.Error error in saveResult[0].errors)
                {
                    message += error.message + "\r\n";
                }
                throw new Exception(message);
            }

               
                     
            return saveResult;
        }
        public SalesForce.SalesForceSVC.SaveResult[] UpdateOpportunity()
        {
            SalesForce.SalesForceSVC.SaveResult[] saveResult = null;

            try
            {
                ConfigureHeaders();

                SalesForce.SalesForceSVC.Opportunity opp = this.FillOpportunityObj();
                SalesForce.SalesForceSVC.sObject[] objs = new List<SalesForce.SalesForceSVC.sObject> { opp }.ToArray();

                SalesForce.SalesForceSVC.LimitInfo[] infoHeader = getInfoHeader();

                serviceClient.create(sessionHeader, ruleHeader, mruHeader, truncateHeader, trackingHeader,
                    streamingHeader, allOrNoneHeader, duplicateHeader, localeOption, debugHeader, versionHeader,
                        emailHeader, objs, out infoHeader, out saveResult);
                
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }

            return saveResult;


        }
        private SalesForce.SalesForceSVC.LimitInfo[] getInfoHeader()
        {
            SalesForce.SalesForceSVC.LimitInfo info = new SalesForce.SalesForceSVC.LimitInfo();
            info.type = "API REQUESTS";
            info.current = 20;
            info.limit = 250;
            SalesForce.SalesForceSVC.LimitInfo[] infoHeader = new List<SalesForce.SalesForceSVC.LimitInfo>() { info }.ToArray();
            return infoHeader;

        }
        public SalesForce.SalesForceSVC.sObject[] RetrieveRecords(SalesForce.SalesForceSVC.SaveResult[] saveResult)
        {
            SalesForce.SalesForceSVC.sObject[] objs = null;

            String[] ids = new string[saveResult.Length];

            for (int i = 0; i < saveResult.Length; i++)
            {
                ids[i] = saveResult[i].id;
            }
            try
            {

                SalesForce.SalesForceSVC.QueryOptions queryOptions = new SalesForce.SalesForceSVC.QueryOptions();

                string fieldList = "AccountId, SyncedQuoteId, Name, Amount, CreatedDate, StageName";
                string sObjectsType = "Opportunity";

                SalesForce.SalesForceSVC.Opportunity opp = new SalesForce.SalesForceSVC.Opportunity();
                opp = FillOpportunityObj();
                objs = new List<SalesForce.SalesForceSVC.sObject> { opp }.ToArray();


                serviceClient.retrieve(sessionHeader, queryOptions, mruHeader, versionHeader, fieldList, sObjectsType, ids, out objs);

                return objs;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }

            return objs;
        }
        private void ConfigureHeaders()
        {
            SalesForce.SalesForceSVC.LoginResult lr = loginClient.login(loginHeader, login, senha);
            if (lr.passwordExpired)
            {
                throw new Exception("Password expired");
            }
            EndpointAddress endpoint = new EndpointAddress(lr.serverUrl);
            sessionHeader.sessionId = lr.sessionId;
            serviceClient = new SalesForce.SalesForceSVC.SoapClient("Soap", endpoint);

            ruleHeader.useDefaultRule = true;

            mruHeader.updateMru = true;

            truncateHeader.allowFieldTruncation = true;

            trackingHeader.disableFeedTracking = false;

            versionHeader = new List<SalesForce.SalesForceSVC.PackageVersion>() { version }.ToArray();

            allOrNoneHeader.allOrNone = false;

            //https://developer.salesforce.com/docs/atlas.en-us.api.meta/api/sforce_api_header_duplicateruleheader.htm
            duplicateHeader.allowSave = false;
            duplicateHeader.includeRecordDetails = true;
            duplicateHeader.runAsCurrentUser = true;

            localeOption.language = "en_US";

            debugHeader.debugLevel = SalesForce.SalesForceSVC.DebugLevel.None;

            //https://developer.salesforce.com/docs/atlas.en-us.api.meta/api/sforce_api_header_emailheader.htm
            emailHeader.triggerAutoResponseEmail = false;
            emailHeader.triggerOtherEmail = false;
            emailHeader.triggerUserEmail = false;

        }
        private SalesForce.SalesForceSVC.Opportunity FillOpportunityObj()
        {
            SalesForce.SalesForceSVC.Opportunity op = new SalesForce.SalesForceSVC.Opportunity();
            op.AccountId = "";
            op.SyncedQuoteId = "";
            op.Name = "Teste Prion";
            op.StageName = "Testing";
            op.CreatedDate = DateTime.Now;
            op.CloseDate = DateTime.Today.AddDays(30);
            op.CloseDateSpecified = true;

            op.Amount = 10;
            op.AmountSpecified = true;

            return op;

        }
    }
}
