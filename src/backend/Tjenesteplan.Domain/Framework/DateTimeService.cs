using System;

namespace Tjenesteplan.Domain.Framework
{
    public interface IDateTimeService
    {
        DateTime UtcNow();
    }
    public class DateTimeService : IDateTimeService
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}