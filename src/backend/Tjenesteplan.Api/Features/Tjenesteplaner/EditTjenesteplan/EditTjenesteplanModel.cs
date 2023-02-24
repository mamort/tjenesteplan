using System;
using System.Collections.Generic;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.Tjenesteplaner.EditTjenesteplan
{
    public class EditTjenesteplanModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; } 
        public int NumberOfLeger { get; set; }

        public List<TjenesteplanWeekModel> Weeks { get; set; }
    }

    public class TjenesteplanWeekModel
    {
        public int Id { get; set; }
        public int? LegeId { get; set; }
        public List<TjenesteplanDayModel> Days { get; set; }
    }

    public class TjenesteplanDayModel
    {
        public DagsplanEnum Dagsplan { get; set; }
        public DateTime Date { get; set; }
    }
}