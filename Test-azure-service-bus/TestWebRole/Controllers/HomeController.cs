using System;
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
                ViewBag.Result = "Message sent successfully.";
            }
            catch (Exception e)
            {
                ViewBag.Result = "Error: " + e.Message;
            }

            return RedirectToAction("Index");

        }
    }
}