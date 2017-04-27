using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TestWebRole.Models;
using Microsoft.ServiceBus.Messaging;
using TestWebRole.Connector;

namespace TestWebRole.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                // Get a NamespaceManager which allows you to perform management and
                // diagnostic operations on your Service Bus queues.
                var namespaceManager = QueueConnector.CreateNamespaceManager();

                // Get the queue, and obtain the message count.
                var queue = namespaceManager.GetQueue(QueueConnector.QueueName);
                ViewBag.MessageCount = queue.MessageCount;
            }
            catch (Exception e)
            {
                ViewBag.Result = "Error: " + e.Message;
                ViewBag.MessageCount = 0;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(CustomMessage custMessage)
        {
            if (!ModelState.IsValid)
                return View(custMessage);

            // Create a message from the custMessage.
            var message = new BrokeredMessage(custMessage);

            try
            {
                // Submit the custMessage.
                QueueConnector.MessagesQueueClient.Send(message);
                TempData["Result"] = "Message sent successfully.";
            }
            catch (Exception e)
            {
                TempData["Result"] = "Error: " + e.Message;
            }

            return RedirectToAction("Index");

        }

        public ActionResult Process()
        {
            var connectionString = "Endpoint=sb://iqanstest1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=kACru3WDbVWYmD5GEFF81sLIsgi+eyR9fjeO6+NWYpY=";
            var queueName = "MessagesQueue";
            var list = new List<string>();

            try
            {
                var client = QueueClient.CreateFromConnectionString(connectionString, queueName);

                client.OnMessage(message =>
                {
                    list.Add(string.Format("Message id: {0}", message.MessageId));
                    var msg = message.GetBody<CustomMessage>();
                    list.Add("Message: ");
                    list.Add("CustomerId: " + msg.CustomerId);
                    list.Add("Name: " + msg.Name);
                    list.Add("Message: " + msg.Message);

                    message.Complete();
                });
                TempData["Output"] = list;
            }
            catch (Exception e)
            {
                TempData["Err"] = "Error: " + e.Message;
            }
            
            return RedirectToAction("Index");
        }
    }
}