using System;

namespace Tjenesteplan.Domain
{
    public class VaktChangeRequestAlternative
    {
        public int Id { get; }
        public DateTime Date { get; }

        public VaktChangeRequestAlternative(int id, DateTime date)
        {
            Id = id;
            Date = date;
        }
    }
}