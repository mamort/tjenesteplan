using System.Collections.Generic;

namespace Tjenesteplan.Domain
{
    public class Sykehus
    {
        public int Id { get; }
        public string Name { get; }
        public IReadOnlyList<Avdeling> Avdelinger { get; }

        public Sykehus(int id, string Name, IReadOnlyList<Avdeling> Avdelinger)
        {
            Id = id;
            this.Name = Name;
            this.Avdelinger = Avdelinger;
        }
    }
}