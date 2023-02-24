using System;
using System.Collections.Generic;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.VaktChangeRequests.SentRequests
{
    public class RequestReplyModel
    {
        public int Id { get; set; }
        public VaktChangeRequestReplyStatus Status { get; set; }
        public List<DateTime> Alternatives { get; set; }
        public int UserId { get; set; }
        public string Fullname { get; set; }
    }
}