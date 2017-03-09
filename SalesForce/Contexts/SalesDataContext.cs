using Main.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Contexts
{
    public class SalesDataContext:PrionContext
    {
        //Sets
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<SalesData> SalesDatas { get; set; }

    }
}
