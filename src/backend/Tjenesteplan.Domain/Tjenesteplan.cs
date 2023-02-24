using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Tjenesteplan.Domain
{
	public class Tjenesteplan
	{
        public int Id { get; set; }
        public string Name { get; set; }
		public DateTime Start { get; set; }
		public List<TjenesteUke> Uker { get; set; }
	    public int? ListeførerId { get; set; }
	    public int AvdelingId { get; set; }

	    public IReadOnlyList<TjenesteDag> GetDatesForCurrentYear()
	    {
	        var dates = new List<TjenesteDag>();
	        foreach (var uke in Uker)
	        {
	            if (uke.UserId.HasValue)
	            {
	                dates.AddRange(GetDatesForCurrentYear(uke.UserId.Value));
                }
	        }

	        return dates;
	    }

	    public IReadOnlyList<TjenesteDag> GetDatesForCurrentYear(int userId)
	    {
	        var dates = new List<TjenesteDag>();
	        var uke = Uker.FirstOrDefault(u => u.UserId == userId);
	        if (uke == null)
	        {
	            throw new Exception($"UserId {userId} is not assigned to any weeks in tjenesteplan with id {Id}");
	        }

	        var currentDate = DateTime.SpecifyKind(Start, DateTimeKind.Utc);

            while(currentDate.Year == Start.Year)
	        {
	            var weekNr = GetWeekNumber(currentDate);
                var weekForDay = weekNr;
	            while (weekNr == weekForDay)
                {
	                var dag = uke.Dager.FirstOrDefault(d => d.Date.DayOfWeek == currentDate.DayOfWeek);
                    dates.Add(dag != null
                        ? new TjenesteDag(currentDate, dag.Dagsplan)
                        : new TjenesteDag(currentDate, DagsplanEnum.None)
                    );

                    currentDate = currentDate.AddDays(1);
                    weekForDay = GetWeekNumber(currentDate);
                }

	            uke = GetNextTjenesteUke(uke.Id);
	        }

	        return dates;
	    }

	    public IReadOnlyList<LøpendeTjenesteUke> GetWeeksForCurrentYear(int userId)
	    {
	        var tjenesteuke = Uker.FirstOrDefault(u => u.UserId == userId);
	        if (tjenesteuke == null)
	        {
	            throw new Exception($"UserId {userId} is not assigned to any weeks in tjenesteplan with id {Id}");
	        }

	        var currentDate = DateTime.SpecifyKind(Start.Date, DateTimeKind.Utc);

	        var weeks = new Dictionary<int, LøpendeTjenesteUke>();
            while (currentDate.Year == Start.Year)
	        {
	            var weekNr  = GetWeekNumber(currentDate);
	            weeks[weekNr] = GetLøpendeTjenesteuke(weeks, weekNr, userId);
	            var week = weeks[weekNr];

	            var weekForDay = weekNr;
	            while (weekNr == weekForDay)
	            {
                    var dag = tjenesteuke.Dager.FirstOrDefault(d => d.Date.DayOfWeek == currentDate.DayOfWeek);
	                week.Dager.Add(dag != null
	                    ? new TjenesteDag(currentDate, dag.Dagsplan)
	                    : new TjenesteDag(currentDate, DagsplanEnum.None)
                    );

	                currentDate = currentDate.AddDays(1);
	                weekForDay = GetWeekNumber(currentDate);
                }

	            tjenesteuke = GetNextTjenesteUke(tjenesteuke.Id);
	        }

	        return weeks.Values.ToList();
	    }

	    private TjenesteUke GetNextTjenesteUke(int tjenesteUkeNr)
	    {
	        var ukeId = tjenesteUkeNr - 1;
	        if (ukeId == 0)
	        {
	            ukeId = Uker.Max(u => u.Id);
            }

	        var tjenesteuke = Uker.FirstOrDefault(u => u.Id == ukeId);
	        if (tjenesteuke == null)
	        {
	            throw new Exception($"Could not find ukeid: {ukeId}");
	        }
	        return tjenesteuke;
	    }

	    private LøpendeTjenesteUke GetLøpendeTjenesteuke(
            IReadOnlyDictionary<int, LøpendeTjenesteUke> weeks, 
            int weekNr, 
            int userId
        )
	    {
	        if (!weeks.ContainsKey(weekNr))
	        {
	            return new LøpendeTjenesteUke(
	                weekNr: weekNr,
	                dager: new List<TjenesteDag>(),
	                userId: userId
	            );
	        }

	        return weeks[weekNr];
	    }

	    public int GetWeekNumber(DateTime date)
	    {
	        return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                date, 
                CalendarWeekRule.FirstFourDayWeek, 
                DayOfWeek.Monday
            );
	    }
    }

	public class TjenesteUke
	{
        public int Id { get; set; }
	    public DateTime FirstDayOfWeek => Dager.Min(d => d.Date);
	    public DateTime LastDayOfWeek => Dager.Max(d => d.Date);
		public List<TjenesteDag> Dager { get; set; }
        public int? UserId { get; set; }
        public int WeekNr { get; internal set; }
    }

    public class LøpendeTjenesteUke
    {
        public int? UserId { get; }
        public int WeekNr { get; }
        public List<TjenesteDag> Dager { get; }

        public LøpendeTjenesteUke(int weekNr, IEnumerable<TjenesteDag> dager, int? userId = null)
        {
            UserId = userId;
            WeekNr = weekNr;
            Dager = dager.ToList();
        }
    }

	public class TjenesteDag
	{
		public DateTime Date { get; }
		public DagsplanEnum Dagsplan { get; }
	    public bool IsHoliday { get; }
	    public string HolidayDescription { get; }

	    public TjenesteDag(DateTime date, DagsplanEnum dagsplan, bool isHoliday = false, string holidayDescription = null)
	    {
	        Date = date;
	        Dagsplan = dagsplan;
	        IsHoliday = isHoliday;
	        HolidayDescription = holidayDescription;

	        if ((isHoliday || IsWeekend) && dagsplan == DagsplanEnum.FriEtterVakt)
	        {
	            Dagsplan = DagsplanEnum.None;
	        }
	    }

	    private bool IsWeekend => Date.DayOfWeek == DayOfWeek.Saturday ||
	                              Date.DayOfWeek == DayOfWeek.Sunday;
    }
}
