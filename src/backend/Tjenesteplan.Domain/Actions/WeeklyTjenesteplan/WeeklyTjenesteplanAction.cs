using System.Collections.Generic;
using System.Linq;
using Tjenesteplan.Domain.Repositories;
using Tjenesteplan.Domain.Services.Holiday;

namespace Tjenesteplan.Domain.Actions.WeeklyTjenesteplan
{
    public class WeeklyTjenesteplanAction
    {
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly ITjenesteplanChangesRepository _tjenesteplanChangesRepository;
        private readonly IHolidayService _holidayService;

        public WeeklyTjenesteplanAction(
            ITjenesteplanRepository tjenesteplanRepository,
            ITjenesteplanChangesRepository tjenesteplanChangesRepository,
            IHolidayService holidayService)
        {
            _tjenesteplanRepository = tjenesteplanRepository;
            _tjenesteplanChangesRepository = tjenesteplanChangesRepository;
            _holidayService = holidayService;
        }


        public WeeklyTjenesteplan GetWeeklyTjenesteplan(int tjenesteplanId)
        {
            var tjenesteplan = _tjenesteplanRepository.GetTjenesteplanById(tjenesteplanId);
            var holidays = _holidayService.GetHolidays(tjenesteplan.Start.Year);

            var allWeeks = new List<LøpendeTjenesteUke>();
            foreach(var week in tjenesteplan.Uker)
            {
                if (week.UserId.HasValue)
                {
                    var legeWeeks = tjenesteplan.GetWeeksForCurrentYear(week.UserId.Value).ToList();
                    AddTjenesteplanChanges(tjenesteplan, holidays, week, legeWeeks);
                    AddHolidays(tjenesteplan, holidays, legeWeeks);

                    allWeeks.AddRange(legeWeeks);
                }
            }

            return new WeeklyTjenesteplan(
                tjenesteplanId: tjenesteplanId,
                year: tjenesteplan.Start.Year,
                weeks: allWeeks
                    .Select(w => 
                            new LøpendeTjenesteUke(
                                weekNr: w.WeekNr, 
                                dager: w.Dager.OrderBy(d => d.Date),
                                userId: w.UserId
                            ))
                    .OrderBy(w => w.WeekNr)
                    .ToList()
            );
        }

        private void AddTjenesteplanChanges(
            Tjenesteplan tjenesteplan, 
            List<Holiday> holidays, 
            TjenesteUke week, 
            List<LøpendeTjenesteUke> legeWeeks
        )
        {
            var ignoreDagsplaner = new List<DagsplanEnum> { 
                Dagsplan.ForespørselOmVaktbytte.DagsplanId, 
                Dagsplan.ForslagTilVaktbytte.DagsplanId 
            };

            var changes = _tjenesteplanChangesRepository
                .GetTjenesteplanChanges(tjenesteplan.Id, week.UserId.Value)
                .Where(c => !ignoreDagsplaner.Contains(c.Dagsplan));

            foreach (var change in changes)
            {
                var weekNr = tjenesteplan.GetWeekNumber(change.Date);
                var legeWeek = legeWeeks.FirstOrDefault(w => w.WeekNr == weekNr);

                if (legeWeek != null)
                {
                    var day = legeWeek.Dager.FirstOrDefault(d => d.Date.Date == change.Date.Date);
                    if (day != null)
                    {
                        legeWeek.Dager.Remove(day);
                    }

                    var holiday = holidays.FirstOrDefault(h => h.Date.Date == change.Date.Date);
                    legeWeek.Dager.Add(
                        new TjenesteDag(
                            date: change.Date.Date, 
                            dagsplan: change.Dagsplan,
                            isHoliday: holiday != null,
                            holidayDescription: holiday?.Description
                        )
                    );
                } 
                else
                {
                    legeWeeks.Add(
                        new LøpendeTjenesteUke(
                            weekNr,
                            new List<TjenesteDag>
                            {
                                new TjenesteDag(
                                    change.Date.Date,
                                    change.Dagsplan
                                )
                            },
                            legeWeeks.First().UserId
                        )
                    );
                }
            }
        }

        private static void AddHolidays(Tjenesteplan tjenesteplan, List<Holiday> holidays, List<LøpendeTjenesteUke> legeWeeks)
        {
            foreach(var holiday in holidays.Where(h => h.Date >= tjenesteplan.Start))
            {
                var weekNr = tjenesteplan.GetWeekNumber(holiday.Date);
                var legeWeek = legeWeeks.FirstOrDefault(w => w.WeekNr == weekNr);
                if(legeWeek != null)
                {
                    var day = legeWeek.Dager.FirstOrDefault(d => d.Date.Date == holiday.Date.Date);
                    if (day != null)
                    {
                        legeWeek.Dager.Remove(day);
                        legeWeek.Dager.Add(
                            new TjenesteDag(
                                date: holiday.Date.Date,
                                dagsplan: day.Dagsplan,
                                isHoliday: true,
                                holidayDescription: holiday.Description
                            )
                        );
                    }
                    else
                    {
                        legeWeek.Dager.Add(
                            new TjenesteDag(
                                holiday.Date.Date,
                                DagsplanEnum.None,
                                isHoliday: true,
                                holidayDescription: holiday.Description
                            )
                        );
                    }
                }
                else
                {
                    legeWeeks.Add(
                        new LøpendeTjenesteUke(
                            weekNr,
                            new List<TjenesteDag>
                            {
                                new TjenesteDag(
                                    holiday.Date.Date,
                                    DagsplanEnum.None,
                                    isHoliday: true,
                                    holidayDescription: holiday.Description
                                )
                            },
                            legeWeeks.First().UserId
                        )
                    );
                }
            }
        }
    }
}