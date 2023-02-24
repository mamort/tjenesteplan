using System;

namespace Tjenesteplan.Domain
{
    public class VakansvaktRequest
    {
        public int Id { get; }
        public string RequestedBy { get; }
        public int TjenesteplanId { get; }
        public string Reason { get; }
        public int RequestedByUserId { get; }
        public int? AcceptedByUserId { get; }
        public DateTime Date { get; }
        public DagsplanEnum CurrentDagsplan { get; }
        public DagsplanEnum RequestedDagsplan { get; }
        public VakansvaktRequestStatus Status { get; }

        public VakansvaktRequest(
            int id, 
            DateTime date, 
            DagsplanEnum currentDagsplan,
            DagsplanEnum requestedDagsplan,
            VakansvaktRequestStatus status, 
            string requestedBy, 
            int tjenesteplanId, 
            string reason,
            int requestedByUserId,
            int? acceptedByUserId
        )
        {
            Id = id;
            RequestedBy = requestedBy;
            TjenesteplanId = tjenesteplanId;
            Reason = reason;
            RequestedByUserId = requestedByUserId;
            AcceptedByUserId = acceptedByUserId;
            Date = date;
            CurrentDagsplan = currentDagsplan;
            RequestedDagsplan = requestedDagsplan;
            Status = status;
        }
    }
}