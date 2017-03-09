using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForce.Models
{
    [Table("Contact", Schema = "sap")]
    public class Contact
    {
        [Key]
        public Int64 ContactId { get; set; }
        public String WestconId { get; set; }
        public String WorkPhone { get; set; }
        public String FaxNumber { get; set; }
        public String EmailAddress { get; set; }
        public String Extension { get; set; }
        public String MobilePhone { get; set; }
        
        
    }
}
