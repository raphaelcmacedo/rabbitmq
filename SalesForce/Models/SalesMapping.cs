using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Models
{
    [Table("SalesMapping", Schema = "sap")]
    public class SalesMapping
    {
        [Key]
        public Int64 SalesMappingId { get; set; }
        public Int64 ApplicationId { get; set; }
        public string SalesOrg { get; set; }
        public string SalesPractice { get; set; }
        public string Manufacturer { get; set; }
        public string MappedUserId { get; set; }
        public string MappedUserEmail { get; set; }
    }
}
