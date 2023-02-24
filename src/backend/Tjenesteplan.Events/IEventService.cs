using System;

namespace Tjenesteplan.Events
{
    public interface IEventService
    {
        void ScheduleDelayedEvent(TimeSpan delay, ITjenesteplanEvent evt);
        void ProcessEvents();
    }
}