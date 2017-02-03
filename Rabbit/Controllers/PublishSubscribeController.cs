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
        private static Dictionary<string, Thread> threads = new Dictionary<string, Thread>();

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
            var thread = new Thread(() => Queue.Receive.CreateListenerExchange(exchange));
            thread.Start();
            var id = thread.ManagedThreadId;
            threads.Add(threadNumber, thread);
            
        }

        public void Unlisten(string exchange, string threadNumber)
        {
            Thread thread = threads[threadNumber];
            thread.Abort();
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