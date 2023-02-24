using System;
using System.Collections.Generic;

namespace Tjenesteplan.Domain.Tjenesteplaner
{
    public class NewTjenesteplan
    {
        public int AvdelingId { get;  }
        public string Name { get; }
        public int UserId { get; }
        public DateTime StartDate { get; }
        public IReadOnlyList<NewTjenesteUke> Weeks { get; }

        public NewTjenesteplan(
            int avdelingId,
            int userId,
            string name,
            DateTime startDate,
            IReadOnlyList<NewTjenesteUke> weeks
        )
        {
            AvdelingId = avdelingId;
            UserId = userId;
            Name = name;
            StartDate = startDate;
            Weeks = weeks;
        }
    }

    public class NewTjenesteUke
    {
        public IReadOnlyList<NewTjenesteDag> Days { get; }

        public NewTjenesteUke(IReadOnlyList<NewTjenesteDag> days)
        {
            Days = days;
        }
    }

    public class NewTjenesteDag
    {
        public DateTime Date { get; }
        public DagsplanEnum Dagsplan { get; }

        public NewTjenesteDag(DateTime date, DagsplanEnum dagsplan)
        {
            Date = date;
            Dagsplan = dagsplan;
        }
    }
}