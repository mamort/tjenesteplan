using System;
using MediatR;

namespace Tjenesteplan.Domain.Events
{
    public class VaktChangeSuggestionsReceivedEvent : INotification
    {
        public int TjenesteplanId { get; }
        public int VaktChangeRequestId { get; }
        public User VaktChangeRequestor { get; }
        public User VaktChangeRespondent { get; }
        public DateTime Date { get; }

        public VaktChangeSuggestionsReceivedEvent(
            int tjenesteplanId,
            int vaktChangeRequestId,
            User vaktChangeRequestor, 
            User vaktChangeRespondent, 
            DateTime date
        )
        {
            TjenesteplanId = tjenesteplanId;
            VaktChangeRequestId = vaktChangeRequestId;
            VaktChangeRequestor = vaktChangeRequestor;
            VaktChangeRespondent = vaktChangeRespondent;
            Date = date;
        }
    }
}