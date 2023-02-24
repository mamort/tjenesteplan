using System;
using System.Collections.Generic;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.Tjenesteplaner
{
    public class MinTjenesteplanModel
    {
        public List<TjenesteplanDate> Dates { get; set; }
        public int SykehusId { get; set; }
        public int AvdelingId { get; set; }

        public string Name { get; set; }
    }

    public class TjenesteplanDate
    {
        public DateTime Date { get; set; }
        public DagsplanEnum Dagsplan { get; set; }
        public bool IsHoliday { get; set; }
        public string Description { get; set; }
    }
}