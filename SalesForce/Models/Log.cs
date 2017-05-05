using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Models
{
    [Table("Log", Schema = "dbo")]
    public class Log
    {
        [Key]
        public Int64 LogId { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public bool Success { get; set; }
        public string Details { get; set; }
        public string Message { get; set; }
        public string Operation { get; set; }

      
        public override string ToString()
        {
            return string.Format(@" Sucess: {0}  Start: {1}   Finish: {2}  Operation: {3}  Details: {4}  Message: {5}",
                                Success,
                                Start,
                                Finish,
                                Operation,
                                Details,
                                Message
                            );
        }
    }
        
}
