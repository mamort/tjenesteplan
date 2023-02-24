using System;
using System.Collections.Generic;
using System.Linq;
using Tjenesteplan.Api.Features.MinTjenesteplan.GetMinTjenesteplan;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Actions.MinTjenesteplan;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.VakansvaktRequests.GetAvailableVakansvaktRequests
{
    public class GetAvailableVakansvaktRequestsAction
    {
        private readonly IUserRepository _userRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly IVakansvaktRequestRepository _vakansvaktRequestRepository;
        private readonly CurrentTjenesteplanAction _currentTjenesteplanAction;

        public GetAvailableVakansvaktRequestsAction(
            IUserRepository userRepository,
            ITjenesteplanRepository tjenesteplanRepository,
            IVakansvaktRequestRepository vakansvaktRequestRepository,
            CurrentTjenesteplanAction currentTjenesteplanAction
        )
        {
            _userRepository = userRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
            _vakansvaktRequestRepository = vakansvaktRequestRepository;
            _currentTjenesteplanAction = currentTjenesteplanAction;
        }


        public IReadOnlyList<AvailableVakansvaktRequestModel> Execute(string username, int tjenesteplanId)
        {
            var user = _userRepository.GetUserByUsername(username);

            var requests = _vakansvaktRequestRepository
                .GetAvailableVakansvaktRequests(tjenesteplanId)
                .Where(r => r.RequestedByUserId != user.Id)
                .Select(CreateVakansvaktRequestModel)
                .ToList();

            if (!requests.Any())
            {
                return new List<AvailableVakansvaktRequestModel>();
            }

            var currentTjenesteplan = _currentTjenesteplanAction.Execute(tjenesteplanId, username);

            var validVakansvaktRequests = new List<AvailableVakansvaktRequestModel>();
            foreach (var request in requests)
            {
                var day = currentTjenesteplan.Dates.FirstOrDefault(d => d.Date.Date == request.Date.Date);

                if (day == null || currentTjenesteplan.IsDateAvailableForVakt(day.Date, day.Dagsplan))
                {
                    validVakansvaktRequests.Add(request);
                }
            }

            return validVakansvaktRequests;
        }

        private AvailableVakansvaktRequestModel CreateVakansvaktRequestModel(VakansvaktRequest req)
        {
            return new AvailableVakansvaktRequestModel(
                id: req.Id,
                date: req.Date,
                status: req.Status,
                requestedBy: req.RequestedBy
            );
        }
    }
}