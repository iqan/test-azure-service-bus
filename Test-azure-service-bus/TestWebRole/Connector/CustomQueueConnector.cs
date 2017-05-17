using System;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace TestWebRole.Connector
{
    public class CustomQueueConnector
    {
        public CustomQueueConnector(string queueName, string topicName, string subName)
        {
            QueueName = queueName;
            TopicName = topicName;
            SubName = subName;
        }
        // Thread-safe. Recommended that you cache rather than recreating it
        // on every request.
        public QueueClient MessagesQueueClient;
        public TopicClient TopicClient;
        public SubscriptionClient SubClient;

        // Obtain these values from the portal.
        public const string Namespace = "iqanstest1";

        // The name of your queue.
        public string QueueName;
        public string TopicName;
        public string SubName;

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

            //create Topic
            if (!namespaceManager.TopicExists(TopicName))
            {
                namespaceManager.CreateTopic(TopicName);
            }

            // Create subs
            if (!namespaceManager.SubscriptionExists(TopicName, SubName))
                namespaceManager.CreateSubscription(TopicName, SubName);


            // Get a client to the queue.
            var messagingFactory = MessagingFactory.Create(
                namespaceManager.Address,
                namespaceManager.Settings.TokenProvider);
            MessagesQueueClient = messagingFactory.CreateQueueClient(QueueName);

            TopicClient = messagingFactory.CreateTopicClient(TopicName);
            SubClient = messagingFactory.CreateSubscriptionClient(TopicName, SubName);
        }
    }
}