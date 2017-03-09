using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            switch (args[0])
            {
                case "ReceiveOpp":
                    Queue.Opportunity.ReceiveOpportunity service = new Queue.Opportunity.ReceiveOpportunity();
                    service.CreateSalesDataListener();
                    break;
                case "SendOpp":
                    break;
            }
        }
    }
}
