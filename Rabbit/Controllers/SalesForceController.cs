using SalesForce;
using SalesForce.SalesForceSVC;
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
        
        
        public void Listen(string queue, bool durable)
        {
            Queue.Receive.CreateListener(queue, durable);
        }

        public void Unlisten(bool durable)
        {
            Queue.Receive.listen = false;
        }

        public ActionResult Fetch(bool durable)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    string message = Queue.Receive.message;
                    Queue.Receive.message = string.Empty;
                    return Json(new { Success = true, data = message }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { Success = false, Message = e.Message });
                }

            }

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
                        queue = "ha.salesorder.queue";
                    }

                    //Busca a msg no Rabbit
                    string message = Queue.Receive.GetOneMessage(durable, queue, false, false);

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
    }
}