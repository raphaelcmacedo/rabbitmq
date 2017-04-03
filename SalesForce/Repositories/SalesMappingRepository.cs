using Main.Contexts;
using Main.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Repositories
{
    public class SalesMappingRepository: GenericRepository<SalesMapping>
    {
        public SalesMappingRepository() : base(new SalesDataContext())
        {

        }

        public string GetMappedUserID(string salesOrg,string salesPractice,string manufacturer)
        {
            SqlParameter appName = new SqlParameter("@ApplicationName", Main.Helpers.Settings.ApplicationName);
            SqlParameter salesOrgParam = new SqlParameter("@SalesOrg", salesOrg);
            List<object> parameters = new List<object>();
            parameters.Add(appName);
            parameters.Add(salesOrgParam);

            String sql = @"SELECT sm.SalesMappingId as SalesMappingId, sm.ApplicationId as ApplicationId, sm.SalesOrg as SalesOrg,sm.SalesPractice as SalesPractice, sm.Manufacturer as Manufacturer, 
                            sm.MappedUserId as MappedUserId FROM [sap].[SalesMapping] as sm INNER JOIN[sap].[Application] as app ON app.ApplicationId = sm.ApplicationId ";


            StringBuilder where = new StringBuilder();
            where.Append(" WHERE app.Name = @ApplicationName AND ");
            where.Append(" ((sm.Manufacturer = @Manufacturer) AND (sm.SalesPractice = @SalesPractice) AND (sm.SalesOrg = @SalesOrg))");          

          
            SqlParameter salesPracticeParam = new SqlParameter("@SalesPractice", (!string.IsNullOrEmpty(salesPractice)) ? salesPractice : null);
            parameters.Add(salesPracticeParam);
            SqlParameter manufacturerParam = new SqlParameter("@Manufacturer", (!string.IsNullOrEmpty(manufacturer)) ? manufacturer : null);
            parameters.Add(manufacturer);

            List<SalesMapping> dataModels = dataModels = base.DbContext.Database.SqlQuery<SalesMapping>(sql, parameters).ToList();

            if(dataModels.Count > 0)
            {
                return dataModels[0].MappedUserEmail;
            }

            return Main.Helpers.Settings.DefaultOwner;
        }
    }
}
