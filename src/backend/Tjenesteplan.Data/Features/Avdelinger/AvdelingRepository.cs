using System.Collections.Generic;
using System.Linq;
using Tjenesteplan.Data.Contexts;
using Tjenesteplan.Domain.Repositories;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Data.Features.Avdelinger
{
    public class AvdelingRepository : IAvdelingRepository
    {
        private readonly DataContext _dataContext;

        public AvdelingRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public int AddAvdeling(int sykehusId, string name)
        {
            var entity = new AvdelingEntity { SykehusId = sykehusId, Name = name};
            _dataContext.Avdelinger.Add(entity);
            _dataContext.SaveChanges();
            return entity.Id;
        }

        public IReadOnlyList<Avdeling> GetAvdelinger(int sykehusId)
        {
            return _dataContext.Avdelinger
                .Where(a => a.SykehusId == sykehusId && !a.IsDeleted)
                .Select(s => new Avdeling(s.SykehusId, s.Id, s.Name, s.ListeforerId))
                .ToList();
        }

        public IReadOnlyList<Avdeling> GetAllAvdelinger()
        {
            return _dataContext.Avdelinger
                .Where(a => !a.IsDeleted)
                .Select(s => new Avdeling(s.SykehusId, s.Id, s.Name, s.ListeforerId))
                .ToList();
        }

        public Avdeling GetAvdeling(int id)
        {
            var avdeling = _dataContext.Avdelinger
                .FirstOrDefault(a => a.Id == id && !a.IsDeleted);

            return avdeling == null 
                ? null 
                : new Avdeling(avdeling.SykehusId, avdeling.Id, avdeling.Name, avdeling.ListeforerId);
        }

        public void UpdateAvdeling(int id, string name, int? listeforerId)
        {
            var avdeling = _dataContext.Avdelinger.FirstOrDefault(s => s.Id == id);
            if (avdeling != null)
            {
                avdeling.Name = name;

                if (listeforerId.HasValue)
                {
                    avdeling.ListeforerId = listeforerId;
                }

                _dataContext.Avdelinger.Update(avdeling);
                _dataContext.SaveChanges();
            }
        }

        public void DeleteAvdeling(int id)
        {
            var avdeling = _dataContext.Avdelinger.FirstOrDefault(s => s.Id == id);
            if (avdeling != null)
            {
                avdeling.IsDeleted = true;
                _dataContext.Update(avdeling);
                _dataContext.SaveChanges();
            }
            
        }
    }
}