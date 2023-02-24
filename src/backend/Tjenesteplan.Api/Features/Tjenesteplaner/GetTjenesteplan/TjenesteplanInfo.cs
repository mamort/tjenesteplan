using System;
using System.Collections.Generic;

namespace Tjenesteplan.Api.Features.Tjenesteplaner.GetTjenesteplan
{
    public class TjenesteplanInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public List<LegeInfo> Leger { get; set; }
        public int NumberOfWeeks { get; set; }
        public bool IsFull { get; set; }
        public List<WeekInfo> Weeks { get; set; }
    }
}