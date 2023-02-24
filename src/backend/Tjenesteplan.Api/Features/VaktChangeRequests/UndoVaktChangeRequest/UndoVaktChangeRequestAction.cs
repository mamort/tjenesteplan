using System;
using System.Threading.Tasks;
using Tjenesteplan.Api.Exceptions;
using Tjenesteplan.Data.Features.Tjenesteplan;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.VaktChangeRequests.UndoVaktChangeRequest
{
    public class UndoVaktChangeRequestAction
    {
        private readonly IUserRepository _userRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly ITjenesteplanChangesRepository _tjenesteplanChangesRepository;
        private readonly IVaktChangeRequestRepository _vaktChangeRequestRepository;

        public UndoVaktChangeRequestAction(
            IUserRepository userRepository,
            ITjenesteplanRepository tjenesteplanRepository,
            ITjenesteplanChangesRepository tjenesteplanChangesRepository,
            IVaktChangeRequestRepository vaktChangeRequestRepository
        )
        {
            _userRepository = userRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
            _tjenesteplanChangesRepository = tjenesteplanChangesRepository;
            _vaktChangeRequestRepository = vaktChangeRequestRepository;
        }

        public Task Execute(string username, int vaktChangeRequestId)
        {
            var user = _userRepository.GetUserByUsername(username);
            var vaktChangeRequest = _vaktChangeRequestRepository.GetRequestById(vaktChangeRequestId);
            if (vaktChangeRequest == null)
            {
                throw new AppException($"VaktChangeRequest with id {vaktChangeRequestId} does not exist");
            }

            var tjenesteplan = _tjenesteplanRepository.GetTjenesteplanById(vaktChangeRequest.TjenesteplanId);
            if (user.Id != vaktChangeRequest.UserId && user.Id != tjenesteplan.ListeførerId && user.Role != Role.Admin)
            {
                throw new UnauthorizedAccessException();
            }

            // If cancelled by user who requested the change it has to be in progress (and not completed)
            if (user.Id == vaktChangeRequest.UserId && vaktChangeRequest.Status != VaktChangeRequestStatus.InProgress)
            {
                throw new UnauthorizedAccessException();
            }

            _tjenesteplanChangesRepository.UndoTjenesteplanVaktChange(vaktChangeRequest.TjenesteplanId, vaktChangeRequestId);

            var newStatus = user.Id == vaktChangeRequest.UserId
                ? VaktChangeRequestStatus.CanceledByUser
                : VaktChangeRequestStatus.Cancelled;

            _vaktChangeRequestRepository.UpdateVaktChangeRequestStatus(vaktChangeRequestId, newStatus);

            return Task.CompletedTask;
        }
    }
}