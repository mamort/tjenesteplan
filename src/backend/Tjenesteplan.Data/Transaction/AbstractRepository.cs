using System.Data;
using Tjenesteplan.Data.Contexts;
using Tjenesteplan.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tjenesteplan.Data.Transaction
{
    public abstract class AbstractRepository : IRepository
    {
        private readonly DataContext _context;

        protected AbstractRepository(DataContext context)
        {
            _context = context;
        }
        public IRepositoryTransaction BeginTransaction(IsolationLevel isolation = IsolationLevel.RepeatableRead)
        {
            var transaction = _context.Database.BeginTransaction(isolation);
            return new RepositoryTransaction(transaction);
        }
    }
}