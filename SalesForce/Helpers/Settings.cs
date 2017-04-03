using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Helpers
{
    public class Settings
    {
        private static string GetValue(string chave)
        {
            System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();
            return appReader.GetValue(chave, typeof(string)).ToString();
        }

        public static string ApplicationName
        {
            get { return GetValue("AppName"); }
        }

        public static string QueueHost
        {
            get { return GetValue("QueueHost"); }
        }

        public static string QueueUserName
        {
            get { return GetValue("QueueUserName"); }
        }

        public static string QueuePassword
        {
            get { return GetValue("QueuePassword"); }
        }

        public static string QueueVirtualHostDev
        {
            get { return GetValue("QueueVirtualHostDev"); }
        }

        public static string QueueVirtualHostQA
        {
            get { return GetValue("QueueVirtualHostQA"); }
        }

        public static string SalesDataQueue
        {
            get { return GetValue("SalesDataQueue"); }
        }

        public static string OpportunityQueue
        {
            get { return GetValue("OpportunityQueue"); }
        }

        public static string DefaultOwner
        {
            get { return GetValue("DefaultOwnerId"); }
        }
        public static string RoutingKey
        {
            get { return GetValue("RoutingKey"); }
        }
        public static string QueueExchange
        {
            get { return GetValue("QueueExchange"); }
        }
        public static string BwOpportunityQueue
        {
            get { return GetValue("BwOpportunityQueue"); }
        }
    }
}
