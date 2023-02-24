using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Events;
using Tjenesteplan.Domain.Notifications.VakansvaktReminderNotification;
using Tjenesteplan.Domain.Repositories;
using Tjenesteplan.Events;
using Xunit;

namespace Tjenesteplan.Api.UnitTests
{
    public class ScheduleVakansvaktRemindersTests
    {
        [Fact]
        public void ProcessEvents_OneAndTwoDaysBeforeUncoveredVakansvakt_ShouldPublishVakansvaktScheduledSoonEvent()
        {
            var tjenesteplanid = 1;
            var avdelingId = 1;
            var eventServiceTestSetup = new EventServiceTestSetup();
            eventServiceTestSetup.CurrentTime = DateTime.UtcNow;
            var vakansvaktDate = eventServiceTestSetup.CurrentTime.AddDays(10);

            var tjenesteplanRepository = new Mock<ITjenesteplanRepository>();
            tjenesteplanRepository
                .Setup(t => t.GetTjenesteplanById(tjenesteplanid))
                .Returns(() => new Domain.Tjenesteplan()
                {
                    Id = tjenesteplanid,
                    AvdelingId = avdelingId,
                    ListeførerId = 1
                });

            var eventService = eventServiceTestSetup.Create();
            var scheduleVakansvaktReminders = new ScheduleVakansvaktReminders(
                eventServiceTestSetup.DateTimeService.Object,
                eventService,
                tjenesteplanRepository.Object
            );

            scheduleVakansvaktReminders.Handle(
                new VakansvaktRequestApprovedEvent(
                    tjenesteplanId: tjenesteplanid,
                    requestor: new User( 
                        1, 
                        "", 
                        "",
                        LegeSpesialitet.AkuttOgMottaksmedisin, 
                        "", 
                        new byte[0], 
                        new byte[0], 
                        Role.Overlege,
                        new [] { 1 },
                        new []{ 1 }
                    ),
                    vakansvaktRequestId: 1,
                    date: vakansvaktDate,
                    dagsplan: DagsplanEnum.Døgnvakt
                ), CancellationToken.None
            );

            var receivedEvent = false;
            eventServiceTestSetup.Mediatr.Setup(m => m.Publish(
                    It.IsAny<object>(),
                    It.IsAny<CancellationToken>()
                ))
                .Callback((object theEvent, CancellationToken token) =>
                {
                    if (theEvent is VakansvaktScheduledSoonEvent)
                    {
                        receivedEvent = true;
                    }
                }).Returns(() => Task.CompletedTask);

            eventService.ProcessEvents();
            Assert.False(receivedEvent);

            foreach (var timeBeforeVakansvaktToSendReminder in ScheduleVakansvaktReminders
                .TimesBeforeVakansvaktToSendReminder)
            {
                // Fast forward to reminder time(s)
                eventServiceTestSetup.CurrentTime = vakansvaktDate - timeBeforeVakansvaktToSendReminder;

                // Call process events that will trigger the event 
                eventService.ProcessEvents();
                Assert.True(receivedEvent);
            }
        }
    }
}