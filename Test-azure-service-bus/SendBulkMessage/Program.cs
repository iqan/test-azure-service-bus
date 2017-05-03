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
                    var msg = new BrokeredMessage(string.Format("Hi there! [Message No.]: {0}", i+1));
                    Console.Write(i+1+"...");
                    QueueConnector.MessagesQueueClient.Send(msg);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" SENT !\r\n");
                    Console.ResetColor();
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: " + e.Message);
                Console.ResetColor();
            }
            Console.WriteLine("Done!");
            Console.ReadLine();   
        }
    }
}
