using System;
using System.Threading.Tasks;
using MediatR;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.VakansvaktRequests.UndoVakansvaktRequest
{
    public class UndoVakansvaktRequestAction
    {
        private readonly IUserRepository _userRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly ITjenesteplanChangesRepository _tjenesteplanChangesRepository;
        private readonly IVakansvaktRequestRepository _vakansvaktRequestRepository;
        private readonly IMediator _mediator;

        public UndoVakansvaktRequestAction(
            IUserRepository userRepository,
            ITjenesteplanRepository tjenesteplanRepository,
            ITjenesteplanChangesRepository tjenesteplanChangesRepository,
            IVakansvaktRequestRepository vakansvaktRequestRepository,
            IMediator mediator
        )
        {
            _userRepository = userRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
            _tjenesteplanChangesRepository = tjenesteplanChangesRepository;
            _vakansvaktRequestRepository = vakansvaktRequestRepository;
            _mediator = mediator;
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

            _tjenesteplanChangesRepository.UndoTjenesteplanVakansvaktChange(tjenesteplan.Id, vakansvaktRequestId);
            _vakansvaktRequestRepository.ChangeVakansvaktRequestStatus(vakansvaktRequestId, VakansvaktRequestStatus.Cancelled);

            return Task.CompletedTask;
        }
    }
}