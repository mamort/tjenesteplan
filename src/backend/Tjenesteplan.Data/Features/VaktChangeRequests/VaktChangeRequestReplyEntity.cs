using System.Collections.Generic;
using Tjenesteplan.Domain;
using WebApi.Features.Users.Data;

namespace Tjenesteplan.Data.Features.VaktChangeRequests
{
    public class VaktChangeRequestReplyEntity
    {
        public int Id { get; set; }

        public VaktChangeRequestReplyStatus Status { get; set; }

        public int NumberOfRemindersSent { get; set; }

        public int VaktChangeRequestId { get; set; }
        public VaktChangeRequestEntity VaktChangeRequest { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public ICollection<VaktChangeAlternativeEntity> VaktChangeRequestAlternatives { get; set; }
    }
}