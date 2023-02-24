using System.Collections.Generic;

namespace Tjenesteplan.Domain.Repositories
{
    public interface ISykehusRepository
    {
        int AddSykehus(string name);
        IReadOnlyList<Sykehus> GetSykehus();
        void DeleteSykehus(int id);
        void UpdateSykehus(int id, string name);
        Sykehus GetSykehus(int sykehusId);
    }
}