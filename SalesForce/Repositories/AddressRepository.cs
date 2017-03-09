using SalesForce.Contexts;
using SalesForce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForce.Repositories
{
    public class AddressRepository : GenericRepository<Address>
    {
        public AddressRepository() : base(new SalesDataContext())
        {

        }
     
    }
}
