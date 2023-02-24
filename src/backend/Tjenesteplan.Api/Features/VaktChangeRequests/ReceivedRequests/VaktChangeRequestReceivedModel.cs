using System;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.VaktChangeRequests.ReceivedRequests
{
    public class VaktChangeRequestReceivedModel
    {
        public int VaktChangeRequestId { get; set; }
        public VaktChangeReplyModel Reply { get; set; }
        public int RequestedByUserId { get; set; }
        public string RequestedBy { get; set; }
        public string RequestedBySpesialisering { get; set; }
        public DateTime Date { get; set; }
        public VaktChangeRequestStatus Status { get; set; }
        public DagsplanEnum Dagsplan { get; set; }
        public DateTime? ChosenChangeDate { get; set; }
    }
}