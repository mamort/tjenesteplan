using System;
using System.Collections.Generic;

namespace Tjenesteplan.Domain.Framework
{
    public interface IPersistentQueueMessage
    {
        string MessageId { get; }
        string MessageText { get; }
        string PopReceipt { get; }

    }
    public interface IPersistentQueue
    {
        void CreateIfNotExists();
        bool Exists();

        IReadOnlyList<IPersistentQueueMessage> ReceiveMessages(int maxCount);
        void DeleteMessage(string messageId, string popReceipt);
        void SendMessage(string messageText, in TimeSpan visibilityTimeout);
    }
}