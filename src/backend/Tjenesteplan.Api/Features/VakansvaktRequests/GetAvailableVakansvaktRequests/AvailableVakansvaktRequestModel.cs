using System;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.VakansvaktRequests.GetAvailableVakansvaktRequests
{
    public class AvailableVakansvaktRequestModel
    {
        public int Id { get; }
        public DateTime Date { get; }
        public VakansvaktRequestStatus Status { get; }
        public string RequestedBy { get; }

        public AvailableVakansvaktRequestModel(int id, DateTime date, VakansvaktRequestStatus status, string requestedBy)
        {
            Id = id;
            Date = date;
            Status = status;
            RequestedBy = requestedBy;
        }
    }
}