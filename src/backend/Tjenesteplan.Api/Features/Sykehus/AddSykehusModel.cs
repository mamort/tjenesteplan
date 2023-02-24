using System.ComponentModel.DataAnnotations;

namespace Tjenesteplan.Api.Features.Sykehus
{
    public class AddSykehusModel
    {
        [Required]
        public string Name { get; set; }
    }
}