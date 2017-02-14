﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Rabbit.Controllers
{
    public class PublishSubscribeController : Controller
    {
        private static Dictionary<string, Queue.ReceivePublishSubscribe> threads = new Dictionary<string, Queue.ReceivePublishSubscribe>();

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

        public void Listen(string exchange, string threadNumber)
        {
            Queue.ReceivePublishSubscribe receive = new Queue.ReceivePublishSubscribe();
            var thread = new Thread(() => receive.CreateListenerExchange(exchange));
            thread.Start();
            threads[threadNumber] = receive;
        }

        public void Unlisten(string exchange, string threadNumber)
        {
            Queue.ReceivePublishSubscribe receive = threads[threadNumber];
            receive.listen = false;
            threads.Remove(threadNumber);
        }

        public ActionResult Fetch(string threadNumber)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string message = string.Empty;
                    if (threadNumber != null && threads.ContainsKey(threadNumber))
                    {
                        Queue.ReceivePublishSubscribe receive = threads[threadNumber];
                        message = receive.message;
                        receive.message = string.Empty;
                    }
                    
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