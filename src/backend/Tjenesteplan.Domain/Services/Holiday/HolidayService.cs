using System;
using System.Collections.Generic;

namespace Tjenesteplan.Domain.Services.Holiday
{
    public class HolidayService : IHolidayService
    {
        public List<Holiday> GetHolidays(int year)
        {
            var list = new List<Holiday>();
           
            list.Add(CreateHoliday(
                 date: 1,
                 month: Month.January, 
                 year: year,
                 description: "Første nyttårsdag"
            ));

            list.Add(CreateHoliday(
                date: 1,
                month: Month.May,
                year: year,
                description: "Første mai"
            ));

            list.Add(CreateHoliday(
                date: 17,
                month: Month.May,
                year: year,
                description: "Syttende mai (Grunnlovsdagen)"
            ));

            list.Add(CreateHoliday(
                date: 25,
                month: Month.December,
                year: year,
                description: "Første juledag"
            ));

            list.Add(CreateHoliday(
                date: 26,
                month: Month.December,
                year: year,
                description: "Andre juledag"
            ));

            // Add movable holidays - based on easter day.
            var easterDay = GetEasterDay(year);

            // Sunday before easter.
            list.Add(CreateHoliday(
                date: easterDay, 
                dayOffset: -7,
                description: "Palmesøndag"
            ));

            // Thurday before easter.
            list.Add(CreateHoliday(
                date: easterDay,
                dayOffset: -3,
                description: "Skjærtorsdag"
            ));

            // Friday before easter.
            list.Add(CreateHoliday(
                date: easterDay,
                dayOffset: -2,
                description: "Langfredag"
            ));

            // Easter day.
            list.Add(new Holiday
            {
                Date = easterDay,
                Description = "Påskeaften"
            });

            // Second easter day.
            list.Add(CreateHoliday(
                date: easterDay,
                dayOffset: 1,
                description: "Første påskedag"
            ));

            // "Kristi himmelfart" day.
            list.Add(CreateHoliday(
                date: easterDay,
                dayOffset: 39,
                description: "Kristi himmelfartsdag"
            ));

            // "Pinse" day.
            list.Add(CreateHoliday(
                date: easterDay,
                dayOffset: 49,
                description: "Første pinsedag"
            ));

            // Second "Pinse" day.
            list.Add(CreateHoliday(
                date: easterDay,
                dayOffset: 50,
                description: "Andre pinsedag"
            ));

            return list;
        }

        private Holiday CreateHoliday(DateTime date, int dayOffset, string description)
        {
            return new Holiday
            {
                Date = date.AddDays(dayOffset),
                Description = description
            };
        }

        private Holiday CreateHoliday(int date, Month month, int year, string description)
        {
            return new Holiday
            {
                Date = new DateTime(year, (int) month, date),
                Description = description
            };
        }

        /**
	 * Calculates easter day (sunday) by using Spencer Jones formula found here:
	 * <a href="http://no.wikipedia.org/wiki/P%C3%A5skeformelen">Wikipedia -
	 * Påskeformelen</a>
	 *
	 * @param year
	 *            The year to calculate from.
	 * @return The Calendar object representing easter day for the given year.
	 */
        private static DateTime GetEasterDay(int year)
        {
            int a = year % 19;
            int b = year / 100;
            int c = year % 100;
            int d = b / 4;
            int e = b % 4;
            int f = (b + 8) / 25;
            int g = (b - f + 1) / 3;
            int h = ((19 * a) + b - d - g + 15) % 30;
            int i = c / 4;
            int k = c % 4;
            int l = (32 + (2 * e) + (2 * i) - h - k) % 7;
            int m = (a + (11 * h) + (22 * l)) / 451;
            int n = (h + l - (7 * m) + 114) / 31; // This is the month number.
            int p = (h + l - (7 * m) + 114) % 31; // This is the date minus one.

            return DateTime.SpecifyKind(new DateTime(year, n, p+1), DateTimeKind.Utc);
        }


        private enum Month
        {
            NotSet = 0,
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }
    }
}