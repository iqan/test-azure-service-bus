using System;
using System.Web.Mvc;
using Microsoft.ServiceBus.Messaging;
using TestWebRole.Connector;
using TestWebRole.Models;
using Newtonsoft.Json;

namespace TestWebRole.Controllers
{
    public class ReturnsController : Controller
    {
        private CustomQueueConnector _connector;
        public ReturnsController()
        {
            _connector = new CustomQueueConnector("ReturnsQueue");
            _connector.Initialize();
        }

        public ActionResult Send()
        {
            try
            {
                var namespaceManager = _connector.CreateNamespaceManager();

                var queue = namespaceManager.GetQueue(_connector.QueueName);
                ViewBag.MessageCount = queue.MessageCount;
                ViewBag.QueueName = _connector.QueueName;
            }
            catch (Exception e)
            {
                ViewBag.Error = "Error connecting to the Service Bus";
            }
            return View();
        }

        [HttpPost]
        public ActionResult Send(ReturnCreated msg)
        {
            if (!ModelState.IsValid)
                return View(msg);

            msg.Timestamp = DateTime.Now;
            var jsonMsg = JsonConvert.SerializeObject(msg,Formatting.Indented);
            var message = new BrokeredMessage(jsonMsg);

            try
            {
                // Submit the custMessage.
                _connector.MessagesQueueClient.Send(message);
                TempData["Result"] = "Message sent successfully. (" + message.MessageId +")";
                TempData["Out"] = jsonMsg;
            }
            catch (Exception e)
            {
                TempData["Result"] = "Error: " + e.Message;
            }

            return RedirectToAction("Send");

        }
    }
}