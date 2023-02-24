using System.Data;

namespace Tjenesteplan.Domain.Repositories
{
    public interface IRepository
    {
        IRepositoryTransaction BeginTransaction(IsolationLevel isolation = IsolationLevel.RepeatableRead);
    }
}