using System.Collections.Generic;

namespace Tjenesteplan.Domain.Framework
{
    public interface IPersistentQueueFactory
    {
        IPersistentQueue GetQueue(string connectionString, string name);
    }
    public class InMemoryQueueFactory : IPersistentQueueFactory
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly Dictionary<string, InMemoryQueue> _queues = new Dictionary<string, InMemoryQueue>();

        public InMemoryQueueFactory(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
        }
        public IPersistentQueue GetQueue(string connectionString, string name)
        {
            if (_queues.ContainsKey(name))
            {
                return _queues[name];
            }

            var queue = new InMemoryQueue(_dateTimeService);
            _queues[name] = queue;

            return queue;
        }
    }
}