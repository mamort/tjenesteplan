using System;

namespace Tjenesteplan.Domain.Framework
{
    public class InMemoryQueueMessage : IPersistentQueueMessage
    {
        public DateTime CreationDate { get; }
        public string MessageId { get; }
        public string MessageText { get; }
        public string PopReceipt { get; }

        public TimeSpan VisibilityTimeout { get; }

        public InMemoryQueueMessage(DateTime creationDate, string messageId, string messageText, TimeSpan visibilityTimeout)
        {
            CreationDate = creationDate;
            MessageId = messageId;
            MessageText = messageText;
            VisibilityTimeout = visibilityTimeout;
            PopReceipt = Guid.NewGuid().ToString();
        }
    }
}