using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.Models;
using Main.Contexts;
using System.Data.SqlClient;

namespace Main.Repositories
{
    public class SettingsRepository : GenericRepository<Settings>
    {
        public SettingsRepository() : base(new SalesDataContext()) { }

        public Dictionary<string,string> GetConfig(string configName)
        {
            Dictionary<string, string> appAllSettings = new Dictionary<string, string>();

            SqlParameter appName = new SqlParameter("@ApplicationName", configName);            
            object[] parameters = new object[] { appName };

            string sql = @"SELECT config.ApplicationId as ApplicationId, config.ApplicationConfigId as ApplicationConfigId, config.ConfigurationCode as ConfigurationCode, config.ConfigurationValue as ConfigurationValue 
                    FROM [sap].[ApplicationConfig] as config 
                    INNER JOIN [sap].[Application] as app ON app.ApplicationId = config.ApplicationId 
                    WHERE app.Name = @ApplicationName";
               
            List<Settings> dataModels = dataModels = base.DbContext.Database.SqlQuery<Settings>(sql, parameters).ToList();

            foreach(Settings setting in dataModels)
            {
                appAllSettings.Add(setting.ConfigurationCode, setting.ConfigurationValue);
            }

            return appAllSettings;
        }
    }
}
