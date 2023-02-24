using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Tjenesteplan.Api.Features.Tjenesteplaner;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Actions.MinTjenesteplan;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.MinTjenesteplan.GetMinTjenesteplan
{
    public class GetMinTjenesteplanAction
    {
        private readonly IAvdelingRepository _avdelingRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly CurrentTjenesteplanAction _currentTjenesteplanAction;
        public GetMinTjenesteplanAction(
            IAvdelingRepository avdelingRepository,
            ITjenesteplanRepository tjenesteplanRepository,
            CurrentTjenesteplanAction currentTjenesteplanAction
        )
        {
            _avdelingRepository = avdelingRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
            _currentTjenesteplanAction = currentTjenesteplanAction;
        }

        public MinTjenesteplanModel Execute(int tjenesteplanId, string username)
        {
            
            var minTjenesteplan = _currentTjenesteplanAction.Execute(tjenesteplanId, username);

            if (minTjenesteplan == null)
            {
                return null;
            }

            var tjenesteplan = _tjenesteplanRepository.GetTjenesteplanById(tjenesteplanId);
            var avdeling = _avdelingRepository.GetAvdeling(tjenesteplan.AvdelingId);

            return new MinTjenesteplanModel
            {
                SykehusId = avdeling.SykehusId,
                AvdelingId = avdeling.Id,
                Name = tjenesteplan.Name,
                Dates = minTjenesteplan.Dates.Select(d => new TjenesteplanDate
                {
                    Dagsplan = d.Dagsplan,
                    Date = d.Date,
                    IsHoliday = d.IsHoliday,
                    Description = d.HolidayDescription
                }).ToList()
            };
        }
        
    }
}