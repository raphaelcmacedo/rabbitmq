using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForce.Models
{
    [Table("SalesData", Schema = "sap")]
    public class SalesData
    {
        [Key]
        public Int64 SalesDataId { get; set; }
        public Int64? SoldToId { get; set; }
        public String SalesOrderNo { get; set; }
        public String SalesOrg { get; set; }
        public String SourceSystem { get; set; }
        public String ExtractionRuleType { get; set; }
        public String Message { get; set; }

        [ForeignKey("SoldToId")]
        public virtual Company SoldTo { get; set; }

        public ICollection<LineItem> LineItems;
    }
}
