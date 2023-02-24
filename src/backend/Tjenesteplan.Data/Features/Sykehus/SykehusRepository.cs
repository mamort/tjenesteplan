using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tjenesteplan.Data.Contexts;
using Tjenesteplan.Domain.Repositories;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Data.Features.Sykehus
{
    public class SykehusRepository : ISykehusRepository
    {
        private readonly DataContext _dataContext;

        public SykehusRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public int AddSykehus(string name)
        {
            var entity = new SykehusEntity {Name = name};
            _dataContext.Sykehus.Add(entity);
            _dataContext.SaveChanges();
            return entity.Id;
        }

        public IReadOnlyList<Domain.Sykehus> GetSykehus()
        {
            return _dataContext.Sykehus
                .Where(s => !s.IsDeleted)
                .Include(s => s.Avdelinger)
                .Select(s => CreateSykehus(s))
                .ToList();
        }

        public Domain.Sykehus GetSykehus(int id)
        {
            var sykehus = _dataContext.Sykehus
                .Where(s => !s.IsDeleted)
                .Include(s => s.Avdelinger)
                .FirstOrDefault(s => s.Id == id);
            return sykehus == null
                ? null
                : CreateSykehus(sykehus);
        }

        public void UpdateSykehus(int id, string name)
        {
            var sykehus = _dataContext.Sykehus.FirstOrDefault(s => s.Id == id);
            if (sykehus != null)
            {
                sykehus.Name = name;
                _dataContext.Sykehus.Update(sykehus);
                _dataContext.SaveChanges();
            }
        }

        public void DeleteSykehus(int id)
        {
            var sykehus = _dataContext.Sykehus.FirstOrDefault(s => s.Id == id);
            if (sykehus != null)
            {
                sykehus.IsDeleted = true;
                _dataContext.Update(sykehus);
                _dataContext.SaveChanges();
            }
        }

        private static Domain.Sykehus CreateSykehus(SykehusEntity s)
        {
            return new Domain.Sykehus(
                s.Id,
                s.Name,
                s.Avdelinger
                    .Where(a => !a.IsDeleted)
                    .Select(a => new Avdeling(a.SykehusId, a.Id, a.Name, a.ListeforerId)).ToList()
            );
        }
    }
}