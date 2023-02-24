using System.Collections.Generic;
using Tjenesteplan.Data.Features.Sykehus;
using Tjenesteplan.Data.Features.Tjenesteplan.Data;
using WebApi.Features.Users.Data;

namespace Tjenesteplan.Data.Features.Avdelinger
{
    public class AvdelingEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int SykehusId { get; set; }
        public SykehusEntity Sykehus { get; set; }

        public int? ListeforerId { get; set; }
        public UserEntity Listefører { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<UserAvdelingEntity> Users { get; set; } 
        public ICollection<TjenesteplanEntity> Tjenesteplaner { get; set; }
    }
}