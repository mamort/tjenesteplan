using System;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Tjenesteplan.Api.Configuration;
using Tjenesteplan.Api.Services.Events;
using Tjenesteplan.Domain.Framework;

namespace Tjenesteplan.Api.UnitTests
{
    public class EventServiceTestSetup
    {
        public DateTime CurrentTime { get; set; }
        public Mock<IMediator> Mediatr { get; private set; }

        public Mock<IDateTimeService> DateTimeService { get; private set; }

        public EventService Create()
        {
            var options = new Mock<IOptions<CommonOptions>>();
            options.Setup(o => o.Value).Returns(new CommonOptions() { StorageConnectionString = "" });
            Mediatr = new Mock<IMediator>();
            DateTimeService = new Mock<IDateTimeService>();
            DateTimeService.Setup(s => s.UtcNow()).Returns(() => CurrentTime);

            return new EventService(
                DateTimeService.Object,
                new NullLogger<EventService>(),
                Mediatr.Object,
                options.Object,
                new InMemoryQueueFactory(DateTimeService.Object)
            );
        }
    }
}