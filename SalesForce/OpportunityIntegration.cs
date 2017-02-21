using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForce
{
    public class OpportunityIntegration
    {
        public SalesForceSVC.Opportunity CreateOpportunity(string message)
        {
            OpportunitySAP sap = new OpportunitySAP();
            SalesForceSVC.Opportunity opportunity = sap.ConvertXML(message);
            OpportunityService service = new OpportunityService();
            service.CreateOpportunity(opportunity);

            return opportunity;
        }
    }
}
