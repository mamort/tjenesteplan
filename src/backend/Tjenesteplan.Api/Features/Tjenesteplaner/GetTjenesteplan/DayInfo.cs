using System;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.Tjenesteplaner.GetTjenesteplan
{
    public class DayInfo
    {
        public DateTime Date { get; set; }
        public DagsplanEnum Dagsplan { get; set; }
    }
}