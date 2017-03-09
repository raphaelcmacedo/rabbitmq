using SalesForce.SalesForceSVC;
using Main.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rabbit.Controllers
{
    public class SalesForceController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        

        public ActionResult IntegrateOneMessage(string queue, bool durable)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(queue))
                    {
                        queue = "ha.bwsalesopportunity.queue";
                    }

                    //Busca a msg no Rabbit
                    string message = Queue.Receive.GetOneMessage(durable, queue, "qa", false, false);

                    //Manda a msg para o salesforce
                    OpportunityIntegration opportunityIntegration = new OpportunityIntegration();
                    Opportunity opportunity = opportunityIntegration.CreateOpportunity(message);

                    return Json(new { Success = true, data = opportunity.Name }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { Success = false, Message = e.Message });
                }

            }

            return View();
        }

        public ActionResult FindAllSalesForce()
        {
            OpportunityIntegration opportunityIntegration = new OpportunityIntegration();
            var opportunities = opportunityIntegration.FindAllSalesForce();

            return Json(new { Success = true, data = opportunities }, JsonRequestBehavior.AllowGet);
        }
    }
}