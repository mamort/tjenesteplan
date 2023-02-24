using System;
using System.Collections.Generic;
using System.Linq;
using Azure.Storage.Queues;
using Tjenesteplan.Domain.Framework;

namespace Tjenesteplan.Api.Services.Events
{
    public class AzureQueue : IPersistentQueue
    {
        private readonly QueueClient _queueClient;

        public AzureQueue(QueueClient queueClient)
        {
            _queueClient = queueClient;
        }

        public void CreateIfNotExists()
        {
            _queueClient.CreateIfNotExists();
        }

        public bool Exists()
        {
            return _queueClient.Exists();
        }

        public IReadOnlyList<IPersistentQueueMessage> ReceiveMessages(int maxCount)
        {
            return _queueClient
                .ReceiveMessages(maxCount).Value
                .Select(m => new AzureQueueMessage(m))
                .ToList();
        }

        public void DeleteMessage(string messageId, string popReceipt)
        {
            _queueClient.DeleteMessage(messageId, popReceipt);
        }

        public void SendMessage(string messageText, in TimeSpan visibilityTimeout)
        {
            _queueClient.SendMessage(messageText, visibilityTimeout);
        }
    }
}