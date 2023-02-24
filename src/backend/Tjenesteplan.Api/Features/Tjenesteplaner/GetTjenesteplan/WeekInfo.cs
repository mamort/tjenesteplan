using System.Collections.Generic;

namespace Tjenesteplan.Api.Features.Tjenesteplaner.GetTjenesteplan
{
    public class WeekInfo
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public List<DayInfo> Days { get; set; }
        public LegeInfo Lege { get; set; }
    }
}