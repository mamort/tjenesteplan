using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tjenesteplan.Domain.Events;
using Tjenesteplan.Domain.Framework;
using Tjenesteplan.Domain.Repositories;
using Tjenesteplan.Events;

namespace Tjenesteplan.Domain.Notifications.VakansvaktReminderNotification
{
    public class ScheduleVakansvaktReminders : INotificationHandler<VakansvaktRequestApprovedEvent>
    {
        public static TimeSpan[] TimesBeforeVakansvaktToSendReminder = new TimeSpan[]
        {
            TimeSpan.FromDays(1),
            TimeSpan.FromDays(2)
        };

        private readonly IDateTimeService _dateTimeService;
        private readonly IEventService _eventService;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;

        public ScheduleVakansvaktReminders(
            IDateTimeService dateTimeService,
            IEventService eventService,
            ITjenesteplanRepository tjenesteplanRepository
        )
        {
            _dateTimeService = dateTimeService;
            _eventService = eventService;
            _tjenesteplanRepository = tjenesteplanRepository;
        }

        public Task Handle(VakansvaktRequestApprovedEvent vakansvaktRequestApprovedEvt, CancellationToken cancellationToken)
        {
            var tjenesteplan = _tjenesteplanRepository.GetTjenesteplanById(vakansvaktRequestApprovedEvt.TjenesteplanId);

            if (!tjenesteplan.ListeførerId.HasValue)
            {
                throw new Exception($"Tjenesteplan with id {tjenesteplan.Id} is missing listefører");
            }

            var timeToEvent = vakansvaktRequestApprovedEvt.Date - _dateTimeService.UtcNow();

            foreach (var timeBeforeEventToSendReminder in TimesBeforeVakansvaktToSendReminder)
            {
                ScheduleVakansvaktReminderEvent(
                    vakansvaktRequestApprovedEvt: vakansvaktRequestApprovedEvt,
                    timeToEvent: timeToEvent,
                    timeBeforeEventToSendReminder: timeBeforeEventToSendReminder
                );
            }

            return Task.CompletedTask;
        }

        private void ScheduleVakansvaktReminderEvent(
            VakansvaktRequestApprovedEvent vakansvaktRequestApprovedEvt,
            TimeSpan timeToEvent,
            TimeSpan timeBeforeEventToSendReminder
        )
        {
            _eventService.ScheduleDelayedEvent(
                delay: timeToEvent.Subtract(timeBeforeEventToSendReminder),
                evt: new VakansvaktScheduledSoonEvent(
                    tjenesteplanId: vakansvaktRequestApprovedEvt.TjenesteplanId,
                    date: vakansvaktRequestApprovedEvt.Date,
                    vakansvaktRequestId: vakansvaktRequestApprovedEvt.VakansvaktRequestId
                )
            );
        }
    }
}