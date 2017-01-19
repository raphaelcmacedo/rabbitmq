using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rabbit.Controllers
{
    public class RabbitController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Add(String text)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Queue.Send.Main(text);
                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { Success = false, Message = e.Message });
                }

            }

            return View();
        }

        public void Listen()
        {
            Queue.Receive.Main();
        }

        public void Unlisten()
        {
            Queue.Receive.listen = false;
        }

        public ActionResult Fetch()
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

        public ActionResult FetchOneMessage(bool simulateError, bool simulateRejection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string message = Queue.Receive.GetOneMessage(simulateError, simulateRejection);
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