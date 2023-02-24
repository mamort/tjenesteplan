using Azure.Storage.Queues.Models;
using Tjenesteplan.Domain.Framework;

namespace Tjenesteplan.Api.Services.Events
{
    public class AzureQueueMessage : IPersistentQueueMessage
    {
        private readonly QueueMessage _queueMessage;

        public AzureQueueMessage(QueueMessage queueMessage)
        {
            _queueMessage = queueMessage;
        }

        public string MessageId => _queueMessage.MessageId;
        public string MessageText => _queueMessage.MessageText;
        public string PopReceipt => _queueMessage.PopReceipt;
    }
}