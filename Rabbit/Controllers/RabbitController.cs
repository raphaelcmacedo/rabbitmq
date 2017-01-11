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

        public ActionResult Listen()
        {
            Queue.Receive.Main();
            return View();
        }

        public ActionResult Fetch()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    string message = Queue.Receive.message;
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