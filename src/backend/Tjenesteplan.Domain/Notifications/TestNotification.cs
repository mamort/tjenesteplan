using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Tjenesteplan.Events;

namespace Tjenesteplan.Domain.Notifications
{
    public class TestNotification : INotificationHandler<TestEvent>
    {
        private readonly ILogger<TestNotification> _logger;

        public TestNotification(ILogger<TestNotification> logger)
        {
            _logger = logger;
        }
        public Task Handle(TestEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling test event with name {notification.Name}");
            return Task.CompletedTask;
        }
    }
}