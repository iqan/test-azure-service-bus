using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace SendBulkMessage
{
    class Program
    {
        public static void Main()
        {
            Console.WriteLine("Initializing QueueClient...");
            QueueConnector.Initialize();
            Console.WriteLine("Done!");

            Console.WriteLine("Enter count: ");
            var count = Console.ReadLine();
            try
            {
                int c = int.Parse(count);
                
                
                for (int i = 0; i < c; i++)
                {
                    var msg = new BrokeredMessage(string.Format("Hi there! [Message]: {0}", i));
                    QueueConnector.MessagesQueueClient.Send(msg);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            Console.WriteLine("Done!");
            Console.ReadLine();   
        }
    }
}
