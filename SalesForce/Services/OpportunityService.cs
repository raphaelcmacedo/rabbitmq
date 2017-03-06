﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace SalesForce.Services
{
    public class OpportunityService
    {
        private String login = "andre.vellinha@westcon.com", senha = "1234WestconuYoID1asym58ULuQUXNxF2NwJ";


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

            if (opp == null)
            {
                opp = this.FillOpportunityObj();
            }
                
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
        public SalesForceSVC.SaveResult[] UpdateOpportunity()
        {
            SalesForceSVC.SaveResult[] saveResult = null;

            try
            {
                ConfigureHeaders();

                SalesForceSVC.Opportunity opp = this.FillOpportunityObj();
                SalesForceSVC.sObject[] objs = new List<SalesForceSVC.sObject> { opp }.ToArray();

                SalesForceSVC.LimitInfo[] infoHeader = getInfoHeader();

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
        private SalesForceSVC.LimitInfo[] getInfoHeader()
        {
            SalesForceSVC.LimitInfo info = new SalesForceSVC.LimitInfo();
            info.type = "API REQUESTS";
            info.current = 20;
            info.limit = 250;
            SalesForceSVC.LimitInfo[] infoHeader = new List<SalesForceSVC.LimitInfo>() { info }.ToArray();
            return infoHeader;

        }
        public SalesForceSVC.sObject[] RetrieveRecords(SalesForceSVC.SaveResult[] saveResult)
        {
            SalesForceSVC.sObject[] objs = null;

            String[] ids = new string[saveResult.Length];

            for (int i = 0; i < saveResult.Length; i++)
            {
                ids[i] = saveResult[i].id;
            }
            try
            {

                SalesForceSVC.QueryOptions queryOptions = new SalesForceSVC.QueryOptions();

                string fieldList = "AccountId, SyncedQuoteId, Name, Amount, CreatedDate, StageName";
                string sObjectsType = "Opportunity";

                SalesForceSVC.Opportunity opp = new SalesForceSVC.Opportunity();
                opp = FillOpportunityObj();
                objs = new List<SalesForceSVC.sObject> { opp }.ToArray();


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
        private SalesForceSVC.Opportunity FillOpportunityObj()
        {
            SalesForceSVC.Opportunity op = new SalesForceSVC.Opportunity();
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
