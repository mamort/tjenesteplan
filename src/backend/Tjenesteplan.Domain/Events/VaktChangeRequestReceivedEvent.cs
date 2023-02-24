using System;
using MediatR;

namespace Tjenesteplan.Domain.Events
{
    public class VaktChangeRequestReceivedEvent : INotification
    {
        public int TjenesteplanId { get; }
        public int VaktChangeRequestId { get; }
        public User Requestor { get; }
        public User Receiver { get; }
        public DateTime Date { get; }

        public VaktChangeRequestReceivedEvent(
            int tjenesteplanId,
            int vaktChangeRequestId, 
            User requestor, 
            User receiver, 
            DateTime date
        )
        {
            TjenesteplanId = tjenesteplanId;
            VaktChangeRequestId = vaktChangeRequestId;
            Requestor = requestor;
            Receiver = receiver;
            Date = date;
        }
    }
}