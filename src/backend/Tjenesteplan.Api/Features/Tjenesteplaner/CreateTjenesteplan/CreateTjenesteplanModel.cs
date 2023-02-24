using System;
using System.Collections.Generic;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.Tjenesteplaner
{
    public class CreateTjenesteplanModel
    {
        public int AvdelingId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; } 
        public int NumberOfLeger { get; set; }

        public List<TjenesteplanWeekModel> Weeks { get; set; }
    }

    public class TjenesteplanWeekModel
    {
        public int? LegeId { get; set; }
        public List<TjenesteplanDayModel> Days { get; set; }
    }

    public class TjenesteplanDayModel
    {
        public DagsplanEnum Dagsplan { get; set; }
        public DateTime Date { get; set; }
    }
}