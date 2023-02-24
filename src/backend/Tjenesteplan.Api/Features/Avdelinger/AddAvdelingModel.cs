using System.ComponentModel.DataAnnotations;

namespace Tjenesteplan.Api.Features.Avdelinger
{
    public class AddAvdelingModel
    {
        [Required]
        public string Name { get; set; }
    }
}