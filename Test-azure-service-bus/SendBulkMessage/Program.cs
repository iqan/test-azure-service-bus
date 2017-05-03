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
            var connectionString = "Endpoint=sb://iqanstest1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=kACru3WDbVWYmD5GEFF81sLIsgi+eyR9fjeO6+NWYpY=";
            var queueName = "MessagesQueue";
            Console.WriteLine("Enter count: ");
            var count = Console.ReadLine();
            try
            {
                int c = int.Parse(count);
                var client = QueueClient.CreateFromConnectionString(connectionString, queueName);
                for (int i = 0; i < c; i++)
                {
                    client.Send(new BrokeredMessage(string.Format("Hi there! [Message:{0}]", c)));   
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
