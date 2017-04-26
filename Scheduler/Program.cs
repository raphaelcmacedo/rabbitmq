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
            Queue.Opportunity.ReceiveOpportunity service = new Queue.Opportunity.ReceiveOpportunity();
            switch (args[0])
            {
                case "SalesData":
                    Console.WriteLine("SalesData");
                    service.CreateSalesDataListener();
                    break;
                case "Opportunity":
                    Console.WriteLine("Opportunity");
                    service.CreateOpportunityListener();
                    break;
                case "TestSalesForce":
                    service.TesteSalesForce();
                    break;
            }
        }
    }
}
