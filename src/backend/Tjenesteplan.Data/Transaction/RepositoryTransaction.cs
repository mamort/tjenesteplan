using Microsoft.EntityFrameworkCore.Storage;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Data.Transaction
{
    internal class RepositoryTransaction : IRepositoryTransaction
    {
        private readonly IDbContextTransaction _transaction;

        public RepositoryTransaction(IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public void Commit()
        {
            _transaction?.Commit();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
        }
    }
}