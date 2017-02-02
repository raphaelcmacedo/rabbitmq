using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rabbit.Controllers
{
    public class PublishSubscribeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Add(string text, string exchange)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Queue.Send.SendToExchange(text, exchange);
                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { Success = false, Message = e.Message });
                }

            }

            return View();
        }

        public void Listen(string exchange)
        {
            Queue.Receive.CreateListenerExchange(exchange);
        }

        public void Unlisten(string exchange)
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

        public ActionResult FetchOneMessage(bool simulateError, bool simulateRejection, bool durable)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string message = Queue.Receive.GetOneMessage(durable, simulateError, simulateRejection);
                    return Json(new { Success = true, data = message }, JsonRequestBehavior.AllowGet);
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