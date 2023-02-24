using System;
using System.Collections.Generic;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.VaktChangeRequests.AllRequests
{
    public class RequestModel
    {
        public int Id { get; set; }

        public string Fullname { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime Date { get; set; }
        public VaktChangeRequestStatus Status { get; set; }

        public List<RequestReplyModel> Replies { get; set; }
        public DateTime? ChosenChangeDate { get; set; }
        public RequestReplyModel ChosenReply { get; set; }
    }
}