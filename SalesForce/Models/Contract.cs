using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForce.Models
{
    [Table("Contract", Schema = "sap")]
    public class Contract
    {
        [Key]
        public Int64 ContractId { get; set; }
        public String ContractNo { get; set; }
        public String SalesOrderNo { get; set; }
        public String ManufacturerInvoiceNo { get; set; }
        public String WestconPONo { get; set; }
        public String ManufacturerQuoteNo { get; set; }
        public String ModelNo { get; set; }
        public String NSP { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


    }
}
