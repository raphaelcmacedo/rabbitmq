using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Models
{
    public class Log
    {
        public Int64 LogId { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public bool Success { get; set; }
        public string Details { get; set; }
        public string Message { get; set; }
        public string Operation { get; set; }

        public override string ToString()
        {
            return string.Format(@" Sucess: {0}  Start: {1}   Finish: {2}  Document Type: {3}  Document Id: {4}  Vendor: {5}  Region: {6}  Details: {7}  User: {8}  Email: {9}",
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
