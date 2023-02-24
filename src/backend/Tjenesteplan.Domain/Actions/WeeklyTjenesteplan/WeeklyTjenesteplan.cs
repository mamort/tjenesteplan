using System.Collections.Generic;

namespace Tjenesteplan.Domain.Actions.WeeklyTjenesteplan
{
    public class WeeklyTjenesteplan
    {
        public int TjenesteplanId { get; }
        public int Year { get; }
        public IReadOnlyList<LøpendeTjenesteUke> Weeks { get; }

        public WeeklyTjenesteplan(
            int tjenesteplanId, 
            int year, 
            IReadOnlyList<LøpendeTjenesteUke> weeks
        )
        {
            TjenesteplanId = tjenesteplanId;
            Year = year;
            Weeks = weeks;
        }
    }
}