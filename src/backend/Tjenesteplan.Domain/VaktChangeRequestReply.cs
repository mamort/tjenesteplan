using System;
using System.Collections.Generic;

namespace Tjenesteplan.Domain
{
    public class VaktChangeRequestReply
    {
        public int Id { get; }
        public int UserId { get; }
        public int VaktChangeRequestId { get; }
        public VaktChangeRequestStatus RequestStatus { get; }
        public VaktChangeRequestReplyStatus Status { get; }
        public int NumberOfRemindersSent { get; }
        public IReadOnlyList<VaktChangeRequestAlternative> Alternatives { get; }

        public VaktChangeRequestReply(int id,
            int userId,
            int vaktChangeRequestId,
            VaktChangeRequestStatus requestStatus,
            VaktChangeRequestReplyStatus status,
            int numberOfRemindersSent,
            IReadOnlyList<VaktChangeRequestAlternative> alternatives)
        {
            Id = id;
            UserId = userId;
            VaktChangeRequestId = vaktChangeRequestId;
            RequestStatus = requestStatus;
            Status = status;
            NumberOfRemindersSent = numberOfRemindersSent;
            Alternatives = alternatives;
        }
    }
}