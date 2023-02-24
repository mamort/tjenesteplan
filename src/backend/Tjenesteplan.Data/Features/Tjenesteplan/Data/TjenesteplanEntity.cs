using System;
using System.Collections.Generic;
using Tjenesteplan.Data.Features.Avdelinger;
using Tjenesteplan.Data.Features.Vakansvakter;
using Tjenesteplan.Data.Features.VaktChangeRequests;
using WebApi.Features.Users.Data;

namespace Tjenesteplan.Data.Features.Tjenesteplan.Data
{
    public class TjenesteplanEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfWeeks { get; set; }
        public DateTime StartDate { get; set; }

        public int AvdelingId { get; set; }
        public AvdelingEntity Avdeling { get; set; }

        public ICollection<TjenesteplanUkeEntity> Weeks { get; set; }
        public ICollection<UserTjenesteplanEntity> Leger { get; set; }

        public ICollection<VaktChangeRequestEntity> VaktChangeRequests { get; set; }
        public ICollection<VakansvaktRequestEntity> VakansvaktRequests { get; set; }
    }
}