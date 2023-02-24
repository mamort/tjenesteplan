using System;
using System.Threading.Tasks;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.VakansvaktRequests.RejectVakansvaktRequest
{
    public class RejectVakansvaktRequestAction
    {
        private readonly IUserRepository _userRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly IVakansvaktRequestRepository _vakansvaktRequestRepository;
        private readonly ITjenesteplanChangesRepository _tjenesteplanChangesRepository;

        public RejectVakansvaktRequestAction(
            IUserRepository userRepository,
            ITjenesteplanRepository tjenesteplanRepository,
            IVakansvaktRequestRepository vakansvaktRequestRepository,
            ITjenesteplanChangesRepository tjenesteplanChangesRepository
        )
        {
            _userRepository = userRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
            _vakansvaktRequestRepository = vakansvaktRequestRepository;
            _tjenesteplanChangesRepository = tjenesteplanChangesRepository;
        }


        public Task ExecuteAsync(string username, int vakansvaktRequestId)
        {
            var user = _userRepository.GetUserByUsername(username);
            var vakansvaktRequest = _vakansvaktRequestRepository.GetVakansvaktRequest(vakansvaktRequestId);
            if (vakansvaktRequest == null)
            {
                return null;
            }

            var tjenesteplan = _tjenesteplanRepository.GetTjenesteplanById(vakansvaktRequest.TjenesteplanId);

            if (user.Id != tjenesteplan.ListeførerId && user.Role != Role.Admin)
            {
                throw new UnauthorizedAccessException();
            }

            _tjenesteplanChangesRepository.AddTjenesteplanChange(
                userId: vakansvaktRequest.RequestedByUserId,
                tjenesteplanId: tjenesteplan.Id,
                date: vakansvaktRequest.Date,
                dagsplan: vakansvaktRequest.CurrentDagsplan
            );

            _vakansvaktRequestRepository.ChangeVakansvaktRequestStatus(
               vakansvaktRequestId: vakansvaktRequestId,
                status: VakansvaktRequestStatus.Rejected
            );

            return Task.CompletedTask;
        }
    }
}