using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForce.Models
{
    [Table("Company", Schema = "sap")]
    public class Company
    {
        [Key]
        public Int64 CompanyId { get; set; }
        public Int64? AddressId { get; set; }
        public Int64? ContactId { get; set; }
        public String WestconId { get; set; }
        public String Name { get; set; }
        public String CountryPrefix { get; set; }
        public String WorkPhone { get; set; }
        public String FaxNumber { get; set; }
        
        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }
        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }
    }
}
