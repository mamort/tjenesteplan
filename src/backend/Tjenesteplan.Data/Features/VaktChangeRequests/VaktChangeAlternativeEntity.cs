using System;

namespace Tjenesteplan.Data.Features.VaktChangeRequests
{
    public class VaktChangeAlternativeEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public int VaktChangeRequestReplyId { get; set; }
        public VaktChangeRequestReplyEntity VaktChangeRequestReply { get; set; }
    }
}