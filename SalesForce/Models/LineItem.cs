using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Models
{
    [Table("LineItem", Schema = "sap")]
    public class LineItem
    {
        [Key]
        public Int64 LineItemId { get; set; }
        public Int64? SalesDataId { get; set; }
        public Int64? ShipToId { get; set; }
        public Int64? EndUserId { get; set; }
        public Int64? ContractId { get; set; }

        public String LineNumber { get; set; }
        public String SKU { get; set; }
        public String SKUDescription { get; set; }
        public String ProductType { get; set; }
        public String SalesDistrict { get; set; }
        public String SalesPractice { get; set; }
        public String CreatedBy { get; set; }
        public String AccountManagerId { get; set; }
        public String AccountManagerName { get; set; }
        public int SalesOrderQty { get; set; }
        public String SalesUnit { get; set; }
        public decimal BillingCost { get; set; }
        public decimal BillingValue { get; set; }
        public String DocumentCurrency { get; set; }
        public String ContractNo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public String ManufacturerQuoteNo { get; set; }
        public String ModelNo { get; set; }
        public String NSP { get; set; }
        public String IsEarliestInvoicedItem { get; set; }
        public DateTime? EarliestBillingPostDate { get; set; }
        public String ManufacturerID { get; set; }
        public String ManufacturerName { get; set; }
        public String ManufacturerAccreditationLevelForSoldTo { get; set; }
        public decimal Discount { get; set; }
        public String PromoID { get; set; }
        public String Promo2ID { get; set; }
        public String AccreditationID { get; set; }
        public String LineItemSerialNo { get; set; }

        [ForeignKey("SalesDataId")]
        public virtual SalesData SalesData { get; set; }
        [ForeignKey("ShipToId")]
        public virtual Company ShipTo { get; set; }
        [ForeignKey("EndUserId")]
        public virtual Company EndUser { get; set; }
        [ForeignKey("ContractId")]
        public virtual Contract Contract { get; set; }
    }
}


