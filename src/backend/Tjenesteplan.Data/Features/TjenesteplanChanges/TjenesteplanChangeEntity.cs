using System;
using Tjenesteplan.Data.Features.Tjenesteplan.Data;
using Tjenesteplan.Data.Features.VaktChangeRequests;
using Tjenesteplan.Domain;
using WebApi.Features.Users.Data;

namespace Tjenesteplan.Data.Features.TjenesteplanChanges
{
    public class TjenesteplanChangeEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime ChangeDate { get; set; }
        public DagsplanEnum Dagsplan { get; set; }

        public int TjenesteplanId { get; set; }
        public int UserId { get; set; }

        public int? VaktChangeRequestId { get; set; }
        public int? VakansvaktRequestId { get; set; }
        public UserEntity User { get; set; }
        public TjenesteplanEntity Tjenesteplan { get; set; }
        public VaktChangeRequestEntity VaktChangeRequest { get; set; }
    }
}