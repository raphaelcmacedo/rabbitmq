using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Models
{
    public class Discount
    {
        public string AgreementNo { get; set; }
        public string SBANo { get; set; }
        public string RoutingIndicator { get; set; }
        public decimal Value { get; set; }

    }
}
