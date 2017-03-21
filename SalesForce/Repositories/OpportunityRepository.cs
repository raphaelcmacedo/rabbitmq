using Main.Contexts;
using Main.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Repositories
{
    public class OpportunityRepository : GenericRepository<Opportunity>
    {
        public OpportunityRepository() : base(new OpportunityContext())
        {

        }

        public void SaveMessageEntries(SalesData salesData, Opportunity opportunity)
        {
            using (DbContextTransaction t = base.DbContext.Database.BeginTransaction())
            {
                DbContext.Set<SalesData>().Add(salesData);
                opportunity.SalesData = salesData;
                this.Add(opportunity);

                t.Commit();
            }
        }

    }
}
