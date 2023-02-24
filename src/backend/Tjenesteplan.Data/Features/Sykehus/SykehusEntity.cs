using System.Collections.Generic;
using Tjenesteplan.Data.Features.Avdelinger;

namespace Tjenesteplan.Data.Features.Sykehus
{
    public class SykehusEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<AvdelingEntity> Avdelinger { get; set; }

        public bool IsDeleted { get; set; }
    }
}