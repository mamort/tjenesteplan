using System.ComponentModel.DataAnnotations;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.Tjenesteplaner.ChangeTjenesteplanDateForLege
{
    public class ChangeTjenesteplanDateForLegeModel
    {
        [Required]
        public DagsplanEnum NewDagsplan { get; set; }
    }
}