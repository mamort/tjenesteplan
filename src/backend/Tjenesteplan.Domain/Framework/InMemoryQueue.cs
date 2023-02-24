using System;
using System.Collections.Generic;
using System.Linq;

namespace Tjenesteplan.Domain.Framework
{
    public class InMemoryQueue : IPersistentQueue
    {
        private readonly IDateTimeService _dateTimeService;
        private bool _isCreated = false;
        private List<InMemoryQueueMessage> _queueMessages;

        public InMemoryQueue(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
        }

        public void CreateIfNotExists()
        {
            if (!_isCreated)
            {
                _isCreated = true;
                _queueMessages = new List<InMemoryQueueMessage>();
            }
        }

        public bool Exists()
        {
            return _isCreated;
        }

        public IReadOnlyList<IPersistentQueueMessage> ReceiveMessages(int maxCount)
        {
            if (!_isCreated)
            {
                throw new Exception("Queue not created");
            }

            return _queueMessages.Where(IsVisible).ToList();
        }

        private bool IsVisible(InMemoryQueueMessage message)
        {
            var date = message.CreationDate + message.VisibilityTimeout;
            return date <= _dateTimeService.UtcNow();
        }

        public void DeleteMessage(string messageId, string popReceipt)
        {
            var message = _queueMessages.FirstOrDefault(m => m.MessageId == messageId && m.PopReceipt == popReceipt);
            if (message != null)
            {
                _queueMessages.Remove(message);
            }
        }

        public void SendMessage(string messageText, in TimeSpan visibilityTimeout)
        {
            _queueMessages.Add(new InMemoryQueueMessage(
                creationDate: _dateTimeService.UtcNow(),
                messageId: Guid.NewGuid().ToString(), 
                messageText: messageText, 
                visibilityTimeout: visibilityTimeout
            ));
        }
    }
}