using System;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace TestWebRole.Connector
{
    public class CustomQueueConnector
    {
        public CustomQueueConnector(string queueName)
        {
            QueueName = queueName;
        }
        // Thread-safe. Recommended that you cache rather than recreating it
        // on every request.
        public QueueClient MessagesQueueClient;

        // Obtain these values from the portal.
        public const string Namespace = "iqanstest1";

        // The name of your queue.
        public string QueueName;

        public NamespaceManager CreateNamespaceManager()
        {
            // Create the namespace manager which gives you access to
            // management operations.
            var uri = ServiceBusEnvironment.CreateServiceUri(
                "sb", Namespace, String.Empty);
            var tP = TokenProvider.CreateSharedAccessSignatureTokenProvider(
                "RootManageSharedAccessKey", "kACru3WDbVWYmD5GEFF81sLIsgi+eyR9fjeO6+NWYpY=");
            return new NamespaceManager(uri, tP);
        }

        public void Initialize()
        {
            // Using Http to be friendly with outbound firewalls.
            ServiceBusEnvironment.SystemConnectivity.Mode =
                ConnectivityMode.Http;

            // Create the namespace manager which gives you access to
            // management operations.
            var namespaceManager = CreateNamespaceManager();

            // Create the queue if it does not exist already.
            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }

            // Get a client to the queue.
            var messagingFactory = MessagingFactory.Create(
                namespaceManager.Address,
                namespaceManager.Settings.TokenProvider);
            MessagesQueueClient = messagingFactory.CreateQueueClient(QueueName);
        }
    }
}