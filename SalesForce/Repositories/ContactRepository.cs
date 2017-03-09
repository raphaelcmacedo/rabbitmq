using Main.Contexts;
using Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Repositories
{
    public class ContactRepository : GenericRepository<Contact>
    {
        public ContactRepository() : base(new SalesDataContext())
        {

        }
     
    }
}
