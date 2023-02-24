using System.Collections.Generic;

namespace Tjenesteplan.Api.Features.Sykehus
{
    public class SykehusModel
    {
        public int Id { get; set;  }
        public string Name { get; set; }
        public List<AvdelingModel> Avdelinger { get; set; }
    }
}