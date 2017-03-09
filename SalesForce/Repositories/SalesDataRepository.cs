using Main.Contexts;
using Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Repositories
{
    public class SalesDataRepository : GenericRepository<SalesData>
    {
        public SalesDataRepository() : base(new SalesDataContext())
        {

        }
     
    }
}
