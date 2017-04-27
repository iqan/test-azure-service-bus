using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.ServiceBus.Messaging;

namespace ConsoleWorkerRole
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Endpoint=sb://iqanstest1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=kACru3WDbVWYmD5GEFF81sLIsgi+eyR9fjeO6+NWYpY=";
            var queueName = "MessagesQueue";

            try
            {
                var client = QueueClient.CreateFromConnectionString(connectionString, queueName);

                client.OnMessage(message =>
                {
                    Console.WriteLine("Processing message:");
                    Console.WriteLine(string.Format("Message id: {0}", message.MessageId));
                    //var msg = message.GetBody<CustomMessage>();
                    //Console.WriteLine("CustomerId: " + msg.CustomerId);
                    //Console.WriteLine("Name: " + msg.Name);
                    //Console.WriteLine("Message: " + msg.Message);
                    Console.WriteLine("Processed at " + DateTime.Now);
                    Console.WriteLine("-------------------------------------------");
                    message.Complete();
                });
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: "+ e.Message);
            }
        }
    }
}
