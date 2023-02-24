namespace Tjenesteplan.Domain
{
    public class Avdeling
    {
        public int SykehusId { get; }
        public int Id { get; }
        public string Name { get; }
        public int? ListeforerId { get; }

        public Avdeling(int sykehusId, int id, string name, int? listeforerId)
        {
            SykehusId = sykehusId;
            Id = id;
            Name = name;
            ListeforerId = listeforerId;
        }
    }
}