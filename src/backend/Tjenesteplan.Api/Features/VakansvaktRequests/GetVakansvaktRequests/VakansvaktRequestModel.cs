using System;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.VakansvaktRequests.GetVakansvaktRequests
{
    public class VakansvaktRequestModel
    {
        public int Id { get; }
        public DateTime Date { get; }
        public VakansvaktRequestStatus Status { get; }
        public string RequestedBy { get; }
        public string AcceptedBy { get; }

        public VakansvaktRequestModel(
            int id, 
            DateTime date, 
            VakansvaktRequestStatus status, 
            string requestedBy,
            string acceptedBy
        )
        {
            Id = id;
            Date = date;
            Status = status;
            RequestedBy = requestedBy;
            AcceptedBy = acceptedBy;
        }
    }
}