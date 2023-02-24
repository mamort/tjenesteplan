using Azure.Storage.Queues;
using Tjenesteplan.Domain.Framework;

namespace Tjenesteplan.Api.Services.Events
{
    public class AzureQueueFactory : IPersistentQueueFactory
    {
        public IPersistentQueue GetQueue(string connectionString, string queueName)
        {
            return new AzureQueue(new QueueClient(connectionString, queueName));
        }
    }
}