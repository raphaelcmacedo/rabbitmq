using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Models
{
    [Table("ApplicationConfig", Schema = "sap")]
    public class Settings
    {
        [Key]
        public Int64 ApplicationConfigId { get; set; }
        public Int64 ApplicationId { get; set; }
        public string ConfigurationCode { get; set; }
        public string ConfigurationValue { get; set; }
    }
       
}
