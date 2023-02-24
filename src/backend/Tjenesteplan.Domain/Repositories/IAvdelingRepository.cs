using System.Collections.Generic;

namespace Tjenesteplan.Domain.Repositories
{
    public interface IAvdelingRepository
    {
        int AddAvdeling(int sykehusId, string name);
        IReadOnlyList<Avdeling> GetAvdelinger(int sykehusId);
        void DeleteAvdeling(int id);
        void UpdateAvdeling(int id, string name, int? listeforerId);
        Avdeling GetAvdeling(int id);
        IReadOnlyList<Avdeling> GetAllAvdelinger();
    }
}