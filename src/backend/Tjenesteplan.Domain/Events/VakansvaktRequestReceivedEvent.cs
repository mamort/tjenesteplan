using System;
using MediatR;

namespace Tjenesteplan.Domain.Events
{
    public class VakansvaktRequestReceivedEvent : INotification
    {
        public int TjenesteplanId { get; }
        public User Requestor { get; }
        public int VakansvaktRequestId { get; }
        public DateTime Date { get; }

        public VakansvaktRequestReceivedEvent(int tjenesteplanId, User requestor, int vakansvaktRequestId, DateTime date)
        {
            TjenesteplanId = tjenesteplanId;
            Requestor = requestor;
            VakansvaktRequestId = vakansvaktRequestId;
            Date = date;
        }
    }
}