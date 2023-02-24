using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.VaktChangeRequests.AddVaktChangeRequest
{
    public class AddVaktChangeRequestModel
    {
        [Required]
        public int TjenesteplanId { get; set; }
        [Required]
        public DagsplanEnum Dagsplan { get; set; }

        [Required]
        public List<DateTime> Dates { get; set; }
    }
}