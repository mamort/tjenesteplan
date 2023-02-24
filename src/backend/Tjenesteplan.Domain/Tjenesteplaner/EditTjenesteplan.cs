using System;
using System.Collections.Generic;

namespace Tjenesteplan.Domain.Tjenesteplaner
{
    public class EditTjenesteplan
    {
        public int Id { get; }
        public string Name { get; }
        public int UserId { get; }
        public DateTime StartDate { get; }
        public IReadOnlyList<EditTjenesteUke> Weeks { get; }

        public EditTjenesteplan(
            int id,
            int userId,
            string name,
            DateTime startDate,
            IReadOnlyList<EditTjenesteUke> weeks
        )
        {
            Id = id;
            UserId = userId;
            Name = name;
            StartDate = startDate;
            Weeks = weeks;
        }
    }

    public class EditTjenesteUke
    {
        public int Id { get; }
        public int? LegeId { get; }
        public IReadOnlyList<EditTjenesteDag> Days { get; }

        public EditTjenesteUke(int id, int? legeId, IReadOnlyList<EditTjenesteDag> days)
        {
            Id = id;
            LegeId = legeId;
            Days = days;
        }
    }

    public class EditTjenesteDag
    {
        public DateTime Date { get; }
        public DagsplanEnum Dagsplan { get; }

        public EditTjenesteDag(DateTime date, DagsplanEnum dagsplan)
        {
            Date = date;
            Dagsplan = dagsplan;
        }
    }
}