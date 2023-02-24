using System;

namespace Tjenesteplan.Events
{
    public class VakansvaktScheduledSoonEvent : ITjenesteplanEvent
    {
        public int TjenesteplanId { get; }
        public DateTime Date { get; }
        public int VakansvaktRequestId { get; }

        public VakansvaktScheduledSoonEvent(
            int tjenesteplanId, 
            DateTime date,
            int vakansvaktRequestId
        )
        {
            TjenesteplanId = tjenesteplanId;
            Date = date;
            VakansvaktRequestId = vakansvaktRequestId;
        }
    }
}