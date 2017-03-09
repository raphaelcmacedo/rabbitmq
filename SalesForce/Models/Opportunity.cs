﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Models
{
    [Table("Opportunity", Schema = "sap")]
    public class Opportunity
    {
        [Key]
        public Int64 OpportunityId { get; set; }

        public String Name { get; set; }
        public String AccountID { get; set; }
        public DateTime CloseDate { get; set; }
        public String StageName { get; set; }
        public String Type { get; set; }
        public String WCType { get; set; }
        public String OwnerID { get; set; }
        public String OwnerName { get; set; }
        public String MainAccountManagerID { get; set; }
        public String MainAccountManagerName { get; set; }
        public String CurrencyCode { get; set; }
        public decimal TotalBillingValue { get; set; }
        public decimal TotalBillingCost { get; set; }
        public String RelatedAttachment_base64 { get; set; }
        public String GeneratedBy { get; set; }

    }
}
