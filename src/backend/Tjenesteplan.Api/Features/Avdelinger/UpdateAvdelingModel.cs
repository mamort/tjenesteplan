using System.ComponentModel.DataAnnotations;

namespace Tjenesteplan.Api.Features.Avdelinger
{
    public class UpdateAvdelingModel
    {
        [Required]
        public string Name { get; set; }

        public int? ListeforerId { get; set; }
    }
}