using System;
using MediatR;

namespace Tjenesteplan.Domain.Events
{
    public class VakansvaktRequestAcceptedEvent : INotification
    {
        public int TjenesteplanId { get; }
        public User Requestor { get; }
        public User Acceptor { get; }
        public int VakansvaktRequestId { get; }
        public DateTime Date { get; }

        public VakansvaktRequestAcceptedEvent(int tjenesteplanId, User requestor, User acceptor, int vakansvaktRequestId, DateTime date)
        {
            TjenesteplanId = tjenesteplanId;
            Requestor = requestor;
            Acceptor = acceptor;
            VakansvaktRequestId = vakansvaktRequestId;
            Date = date;
        }
        
    }
}