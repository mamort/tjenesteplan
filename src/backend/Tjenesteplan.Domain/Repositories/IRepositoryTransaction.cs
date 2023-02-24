using System;

namespace Tjenesteplan.Domain.Repositories
{
    public interface IRepositoryTransaction : IDisposable
    {
        void Commit();
    }
}