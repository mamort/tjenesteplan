using System;
using System.Collections.Generic;
using Tjenesteplan.Domain;
using WebApi.Features.Users.Data;

namespace Tjenesteplan.Data.Features.Tjenesteplan.Data
{
    public class TjenesteplanUkeEntity
    {
        public int TjenesteplanId { get; set; }
        public int TjenesteplanUkeId { get; set; }

        public int? UserId { get; set; }

        public TjenesteplanEntity Tjenesteplan { get; set; }

        public UserEntity User { get; set; }

        public ICollection<TjenesteplanUkedagEntity> Days { get; set; }
    }
}