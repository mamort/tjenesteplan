using System.ComponentModel.DataAnnotations;

namespace Tjenesteplan.Api.Features.Sykehus
{
    public class UpdateSykehusModel
    {
        [Required]
        public string Name { get; set; }
    }
}