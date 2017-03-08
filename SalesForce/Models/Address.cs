using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForce.Models
{
    [Table("Address", Schema = "sap")]
    public class Address
    {
        [Key]
        public Int64 AddressId { get; set; }
        public String Addr1 { get; set; }
        public String Addr2 { get; set; }
        public String Addr3 { get; set; }
        public String Addr4 { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String PostalCode { get; set; }
        public String Country { get; set; }
        
        
    }
}
