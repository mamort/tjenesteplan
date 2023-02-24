using System;
using Tjenesteplan.Data.Features.Tjenesteplan.Data;
using Tjenesteplan.Domain;
using WebApi.Features.Users.Data;

namespace Tjenesteplan.Data.Features.Vakansvakter
{
    public class VakansvaktRequestEntity
    {
        public int Id { get; set; }
        public DateTime RegisteredDate { get; set; }
        public int TjenesteplanId { get; set; }
        public TjenesteplanEntity Tjenesteplan { get; set; }
        public DateTime Date { get; set; }

        public DagsplanEnum CurrentDagsplan { get; set; }
        public DagsplanEnum RequestedDagsplan { get; set; }
        public int OriginalLegeId { get; set; }
        public UserEntity OriginalLege { get; set; }
        public int? CoveredByLegeId { get; set; }
        public UserEntity CoveredByLege { get; set; }
        public string Message { get; set; }

        public VakansvaktRequestStatus Status { get; set; }
    }
}