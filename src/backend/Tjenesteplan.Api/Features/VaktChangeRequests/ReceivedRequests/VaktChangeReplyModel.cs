using System;
using System.Collections.Generic;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.VaktChangeRequests.ReceivedRequests
{
    public class VaktChangeReplyModel
    {
        public int Id { get; set; }
        public VaktChangeRequestReplyStatus Status { get; set; }

        public List<DateTime> Alternatives { get; set; }
    }
}