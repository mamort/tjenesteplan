using System;
using System.Collections.Generic;
using System.Linq;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.VakansvaktRequests.GetVakansvaktRequest
{
    public class GetVakansvaktRequestAction
    {
        private readonly IUserRepository _userRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly IVakansvaktRequestRepository _vakansvaktRequestRepository;

        public GetVakansvaktRequestAction(
            IUserRepository userRepository,
            ITjenesteplanRepository tjenesteplanRepository,
            IVakansvaktRequestRepository vakansvaktRequestRepository
        )
        {
            _userRepository = userRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
            _vakansvaktRequestRepository = vakansvaktRequestRepository;
        }
    
        public VakansvaktRequestModel Execute(string username, int vakansvaktRequestId)
        {
            var user = _userRepository.GetUserByUsername(username);
            var vakansvaktRequest = _vakansvaktRequestRepository.GetVakansvaktRequest(vakansvaktRequestId);
            if (vakansvaktRequest == null)
            {
                return null;
            }

            var tjenesteplan = _tjenesteplanRepository.GetTjenesteplanById(vakansvaktRequest.TjenesteplanId);
            var users = _userRepository.GetUsersByTjenesteplan(vakansvaktRequest.TjenesteplanId);

            // Do not allow users that do not belong to the tjenesteplan
            return !users.Any(u => u.Id == user.Id) && user.Role != Role.Admin && user.Id != tjenesteplan.ListeførerId
                ? null // Results in 404
                : CreateVakansvaktRequestModel(user, vakansvaktRequest);
        }

        private VakansvaktRequestModel CreateVakansvaktRequestModel(User user, VakansvaktRequest req)
        {
            var tjenesteplan = _tjenesteplanRepository.GetTjenesteplanById(req.TjenesteplanId);

            var message = req.Reason;
            if (user.Id != tjenesteplan.ListeførerId && user.Role != Role.Admin)
            {
                // Only admin and listefører should be able to see the reason/message the user gave
                // for the vakansvakt request
                message = null;
            }

            var acceptedBy = "";
            if (req.AcceptedByUserId.HasValue)
            {
                var acceptedByUser = _userRepository.GetUserById(req.AcceptedByUserId.Value);
                acceptedBy = acceptedByUser.Fullname;
            }

            return new VakansvaktRequestModel(
                id: req.Id,
                avdelingId: tjenesteplan.AvdelingId,
                tjenesteplanId: tjenesteplan.Id,
                date: req.Date,
                currentDagsplan: Dagsplan.AllDagsplaner.FirstOrDefault(d => d.DagsplanId == req.CurrentDagsplan),
                requestedDagsplan: Dagsplan.AllDagsplaner.FirstOrDefault(d => d.DagsplanId == req.RequestedDagsplan),
                status: req.Status,
                requestedBy: req.RequestedBy,
                acceptedBy: acceptedBy,
                reason: message
            );
        }
    }
}