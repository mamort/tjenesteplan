using System.Collections.Generic;

namespace Tjenesteplan.Domain.Services.Holiday
{
    public interface IHolidayService
    {
        List<Holiday> GetHolidays(int year);
    }
}