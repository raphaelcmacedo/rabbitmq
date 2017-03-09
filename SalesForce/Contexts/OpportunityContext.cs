using SalesForce.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForce.Contexts
{
    public class OpportunityContext:PrionContext
    {
        //Sets
        public DbSet<Opportunity> Opportunities { get; set; }
        

    }
}
