using System;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.VakansvaktRequests.GetVakansvaktRequest
{
    public class VakansvaktRequestModel
    {
        public int Id { get; }
        public int AvdelingId { get; }
        public int TjenesteplanId { get; }
        public DateTime Date { get; }
        public Dagsplan CurrentDagsplan { get; }
        public Dagsplan RequestedDagsplan { get; }
        public VakansvaktRequestStatus Status { get; }
        public string RequestedBy { get; }
        public string AcceptedBy { get; }
        public string Reason { get; }

        public VakansvaktRequestModel(
            int id,
            int avdelingId,
            int tjenesteplanId,
            DateTime date, 
            Dagsplan currentDagsplan,
            Dagsplan requestedDagsplan,
            VakansvaktRequestStatus status, 
            string requestedBy,
            string acceptedBy,
            string reason
        )
        {
            Id = id;
            AvdelingId = avdelingId;
            TjenesteplanId = tjenesteplanId;
            Date = date;
            CurrentDagsplan = currentDagsplan;
            RequestedDagsplan = requestedDagsplan;
            Status = status;
            RequestedBy = requestedBy;
            AcceptedBy = acceptedBy;
            Reason = reason;
        }
    }
}