using System;
using System.Collections.Generic;
using System.Linq;
using Tjenesteplan.Domain.Repositories;
using Tjenesteplan.Domain.Services.Holiday;

namespace Tjenesteplan.Domain.Actions.MinTjenesteplan
{
    public class CurrentTjenesteplanAction
    {

        private readonly IUserRepository _userRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly ITjenesteplanChangesRepository _tjenesteplanChangesRepository;
        private readonly IHolidayService _holidayService;

        public CurrentTjenesteplanAction(
            IUserRepository userRepository,
            ITjenesteplanRepository tjenesteplanRepository,
            ITjenesteplanChangesRepository tjenesteplanChangesRepository,
            IHolidayService holidayService)
        {
            _userRepository = userRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
            _tjenesteplanChangesRepository = tjenesteplanChangesRepository;
            _holidayService = holidayService;
        }

        public CurrentTjenesteplan Execute(int userId, int tjenesteplanId)
        {
            var user = _userRepository.GetUserById(userId);
            return GetMinTjenesteplan(tjenesteplanId, user);
        }

        public CurrentTjenesteplan Execute(int tjenesteplanId, string username)
        {
            var user = _userRepository.GetUserByUsername(username);
            return GetMinTjenesteplan(tjenesteplanId, user);
        }

        private CurrentTjenesteplan GetMinTjenesteplan(int tjenesteplanId, User user)
        {
            var tjenesteplaner = _tjenesteplanRepository.GetTjenesteplanerForUser(user.Id);
            if (!tjenesteplaner.Any())
            {
                return null;
            }

            var tjenesteplan = tjenesteplaner.FirstOrDefault(t => t.Id == tjenesteplanId);

            if (tjenesteplan == null)
            {
                return null;
            }
  
            var tjenesteplanChanges = _tjenesteplanChangesRepository
                .GetTjenesteplanChanges(tjenesteplan.Id, user.Id);
            var holidays = _holidayService.GetHolidays(DateTime.UtcNow.Year);

            var dates = tjenesteplan.GetDatesForCurrentYear(user.Id)
                .Select(d => CreateTjenesteplanDate(d, holidays))
                .ToList();

            AddHolidays(dates, holidays);
            AddTjenesteplanChanges(holidays, tjenesteplanChanges, dates);

            return new CurrentTjenesteplan(dates);
        }

        private bool IsHoliday(DateTime date, List<Holiday> holidays)
        {
            return holidays.Any(h => h.Date.Date == date);
        }

        private static void AddTjenesteplanChanges(
                List<Holiday> holidays,
                IReadOnlyList<TjenesteplanChange> tjenesteplanChanges,
                List<TjenesteDag> dates
            )
        {
            foreach (var dateChange in tjenesteplanChanges)
            {
                var date = dates.FirstOrDefault(d => d.Date == dateChange.Date.Date);
                if (date != null)
                {
                    dates.Remove(date);
                }

                var holiday = holidays.FirstOrDefault(h => h.Date.Date == dateChange.Date.Date);
                dates.Add(new TjenesteDag(
                    date: dateChange.Date.Date, 
                    dagsplan: dateChange.Dagsplan, 
                    isHoliday: holiday != null,
                    holidayDescription: holiday?.Description
                ));
            }
        }

        private static void AddHolidays(List<TjenesteDag> dates, List<Holiday> holidays)
        {
            dates.AddRange(holidays.Where(h => !dates.Any(d => d.Date.Date == h.Date.Date))
                .Select(d => new TjenesteDag(
                    date: d.Date.Date, 
                    dagsplan: DagsplanEnum.None, 
                    isHoliday: true, 
                    holidayDescription: d.Description))
                .ToList()
            );
        }

        private static TjenesteDag CreateTjenesteplanDate(TjenesteDag d, List<Holiday> holidays)
        {
            var holiday = holidays.FirstOrDefault(h => h.Date.Date == d.Date.Date);
            return new TjenesteDag(
                date: d.Date.Date,
                dagsplan: d.Dagsplan,
                isHoliday: holiday != null,
                holidayDescription: holiday?.Description
            );
        }
    }
}