using System;
using MediatR;

namespace Tjenesteplan.Domain.Events
{
    public class VakansvaktRequestApprovedEvent : INotification
    {
        public int TjenesteplanId { get; }
        public User Requestor { get; }
        public int VakansvaktRequestId { get; }
        public DateTime Date { get; }
        public DagsplanEnum Dagsplan { get; }

        public VakansvaktRequestApprovedEvent(
            int tjenesteplanId,
            User requestor, 
            int vakansvaktRequestId, 
            DateTime date,
            DagsplanEnum dagsplan)
        {
            TjenesteplanId = tjenesteplanId;
            Requestor = requestor;
            VakansvaktRequestId = vakansvaktRequestId;
            Date = date;
            Dagsplan = dagsplan;
        }
        
    }
}