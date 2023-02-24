using System;
using System.Collections.Generic;
using Tjenesteplan.Data.Features.Tjenesteplan.Data;
using Tjenesteplan.Domain;
using WebApi.Features.Users.Data;

namespace Tjenesteplan.Data.Features.VaktChangeRequests
{
    public class VaktChangeRequestEntity
    {
        public int Id { get; set; }

        public DateTime RequestRegisteredDate { get; set; }
        public DateTime Date { get; set; }
        public DagsplanEnum Dagsplan { get; set; }
        public VaktChangeRequestStatus Status { get; set; }
        public int TjenesteplanId { get; set; }

        public int UserId { get; set; }
        public TjenesteplanEntity Tjenesteplan { get; set; }
        public UserEntity User { get; set; }
        public ICollection<VaktChangeRequestReplyEntity> VaktChangeRequestsReplies { get; set; }

        public int? VaktChangeChosenAlternativeId { get; set; }
        public VaktChangeAlternativeEntity VaktChangeChosenAlternative { get; set; }
    }
}