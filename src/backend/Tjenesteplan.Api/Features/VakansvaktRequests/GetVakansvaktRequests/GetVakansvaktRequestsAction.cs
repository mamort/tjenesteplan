using System;
using System.Collections.Generic;
using System.Linq;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.VakansvaktRequests.GetVakansvaktRequests
{
    public class GetVakansvaktRequestsAction
    {
        private readonly IUserRepository _userRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly IVakansvaktRequestRepository _vakansvaktRequestRepository;

        public GetVakansvaktRequestsAction(
            IUserRepository userRepository,
            ITjenesteplanRepository tjenesteplanRepository,
            IVakansvaktRequestRepository vakansvaktRequestRepository
        )
        {
            _userRepository = userRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
            _vakansvaktRequestRepository = vakansvaktRequestRepository;
        }


        public IReadOnlyList<VakansvaktRequestModel> GetVakansvaktRequests(string username, int tjenesteplanId)
        {
            var user = _userRepository.GetUserByUsername(username);
            return _vakansvaktRequestRepository
                .GetVakansvaktRequests(tjenesteplanId, user.Id)
                .Select(CreateVakansvaktRequestModel)
                .ToList();
        }

        public IReadOnlyList<VakansvaktRequestModel> GetUnapprovedVakansvaktRequests(string username, int tjenesteplanId)
        {
            return GetVakansvaktRequestsWithStatus(username, tjenesteplanId, VakansvaktRequestStatus.Received);
        }

        public IReadOnlyList<VakansvaktRequestModel> GetApprovedVakansvaktRequests(string username, int tjenesteplanId)
        {
            return GetVakansvaktRequestsWithStatus(username, tjenesteplanId, VakansvaktRequestStatus.Approved);
        }

        public IReadOnlyList<VakansvaktRequestModel> GetAcceptedVakansvaktRequests(string username, int tjenesteplanId)
        {
            return GetVakansvaktRequestsWithStatus(username, tjenesteplanId, VakansvaktRequestStatus.Accepted);
        }

        private IReadOnlyList<VakansvaktRequestModel> GetVakansvaktRequestsWithStatus(
            string username, 
            int tjenesteplanId, 
            VakansvaktRequestStatus status
        )
        {
            var user = _userRepository.GetUserByUsername(username);
            var tjenesteplan = _tjenesteplanRepository.GetTjenesteplanById(tjenesteplanId);

            if (user.Id != tjenesteplan.ListeførerId && user.Role != Role.Admin)
            {
                throw new UnauthorizedAccessException();
            }

            return _vakansvaktRequestRepository
                .GetVakansvaktRequests(tjenesteplanId, status)
                .Select(CreateVakansvaktRequestModel)
                .ToList();
        }

        private VakansvaktRequestModel CreateVakansvaktRequestModel(VakansvaktRequest req)
        {
            User acceptedBy = null;
            if (req.AcceptedByUserId.HasValue)
            {
                acceptedBy = _userRepository.GetUserById(req.AcceptedByUserId.Value);
            }

            return new VakansvaktRequestModel(
                id: req.Id,
                date: req.Date,
                status: req.Status,
                requestedBy: req.RequestedBy,
                acceptedBy: acceptedBy?.Fullname
            );
        }
    }
}