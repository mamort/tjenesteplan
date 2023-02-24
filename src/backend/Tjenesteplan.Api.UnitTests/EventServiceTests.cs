using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Tjenesteplan.Events;
using Xunit;

namespace Tjenesteplan.Api.UnitTests
{
    public class EventServiceTests
    {
        [Fact]
        public void ScheduleDelayedEvent_ShouldBePublishedAfterDelay()
        {
            // Arrange
            var eventServiceTestSetup = new EventServiceTestSetup();
            eventServiceTestSetup.CurrentTime = DateTime.UtcNow;
            var eventService = eventServiceTestSetup.Create();

            var evt = new TestEvent();
            var receivedEvent = false;
            eventServiceTestSetup.Mediatr.Setup(m => m.Publish(
                    It.IsAny<object>(),
                    It.IsAny<CancellationToken>()
                ))
                .Callback((object theEvent, CancellationToken token) =>
                {
                    if (theEvent is TestEvent)
                    {
                        receivedEvent = true;
                    }
                }).Returns(() => Task.CompletedTask);


            // Act
            eventService.ScheduleDelayedEvent(TimeSpan.FromDays(10), evt);
            eventService.ProcessEvents();

            Assert.False(receivedEvent);

            // Fastforward 10 days
            eventServiceTestSetup.CurrentTime = eventServiceTestSetup.CurrentTime.AddDays(10);
            eventService.ProcessEvents();

            // Assert
            Assert.True(receivedEvent);
        }
    }
}