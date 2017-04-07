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
            SqlParameter salesPracticeParam = new SqlParameter("@SalesPractice", (!string.IsNullOrEmpty(salesPractice)) ? salesPractice : null);
            SqlParameter manufacturerParam = new SqlParameter("@Manufacturer", (!string.IsNullOrEmpty(manufacturer)) ? manufacturer : null);

            List<object> parameters = new List<object>();

            parameters.Add(appName);
            parameters.Add(salesPracticeParam);
            parameters.Add(manufacturerParam);
            parameters.Add(salesOrgParam);

            String sql = @"SELECT sm.SalesMappingId as SalesMappingId, sm.ApplicationId as ApplicationId, sm.SalesOrg as SalesOrg,sm.SalesPractice as SalesPractice, sm.Manufacturer as Manufacturer, 
                            sm.MappedUserId as MappedUserId, sm.MappedSFUserName  as MappedSFUserName FROM [sap].[SalesMapping] as sm INNER JOIN[sap].[Application] as app ON app.ApplicationId = sm.ApplicationId 
                            WHERE app.Name = @ApplicationName AND  ((sm.Manufacturer = @Manufacturer or sm.Manufacturer is null) AND (sm.SalesPractice = @SalesPractice OR sm.SalesPractice is null) AND (sm.SalesOrg = @SalesOrg or sm.SalesOrg is null))
                        Order by
	                            sm.Manufacturer desc
	                            ,sm.SalesPractice desc 
	                            ,sm.SalesOrg desc";        

          
           
           
                List<SalesMapping> dataModels = dataModels = base.DbContext.Database.SqlQuery<SalesMapping>(sql, parameters.ToArray()).ToList();
                if (dataModels.Count > 0)
                {
                    return dataModels[0].MappedSFUserName;
                }          
          

            return Main.Helpers.Settings.DefaultOwner;
        }
    }
}
