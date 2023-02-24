using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Events;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.VakansvaktRequests.AcceptVakansvaktRequest
{
    public class AcceptVakansvaktRequestAction
    {
        private readonly IUserRepository _userRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly IVakansvaktRequestRepository _vakansvaktRequestRepository;
        private readonly ITjenesteplanChangesRepository _tjenesteplanChangesRepository;
        private readonly IMediator _mediator;

        public AcceptVakansvaktRequestAction(
            IUserRepository userRepository,
            ITjenesteplanRepository tjenesteplanRepository,
            IVakansvaktRequestRepository vakansvaktRequestRepository,
            ITjenesteplanChangesRepository tjenesteplanChangesRepository,
            IMediator mediator
        )
        {
            _userRepository = userRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
            _vakansvaktRequestRepository = vakansvaktRequestRepository;
            _tjenesteplanChangesRepository = tjenesteplanChangesRepository;
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

            var tjenesteplaner = _tjenesteplanRepository.GetTjenesteplanerForUser(user.Id);
            if (!tjenesteplaner.Any(t => t.Id == vakansvaktRequest.TjenesteplanId))
            {
                throw new UnauthorizedAccessException();
            }

            /* A user cannot accept a vakansvakt request that he requested */
            if (vakansvaktRequest.RequestedByUserId == user.Id)
            {
                throw new UnauthorizedAccessException();
            }

            _vakansvaktRequestRepository.AcceptVakansvakt(
               vakansvaktRequestId: vakansvaktRequestId,
               userId: user.Id
            );

            _tjenesteplanChangesRepository.AddTjenesteplanVakansvaktChange(
                vakansvaktRequestId: vakansvaktRequestId,
                userId: user.Id,
                tjenesteplanId: vakansvaktRequest.TjenesteplanId,
                date: vakansvaktRequest.Date,
                dagsplan: vakansvaktRequest.CurrentDagsplan
            );

            _tjenesteplanChangesRepository.AddTjenesteplanVakansvaktChange(
                vakansvaktRequestId: vakansvaktRequestId,
                userId: vakansvaktRequest.RequestedByUserId,
                tjenesteplanId: vakansvaktRequest.TjenesteplanId,
                date: vakansvaktRequest.Date,
                dagsplan: vakansvaktRequest.RequestedDagsplan
            );

            var requestor = _userRepository.GetUserById(vakansvaktRequest.RequestedByUserId);

            _mediator.Publish(
                new VakansvaktRequestAcceptedEvent(
                    tjenesteplanId: vakansvaktRequest.TjenesteplanId,
                    requestor: requestor,
                    acceptor: user,
                    vakansvaktRequestId: vakansvaktRequestId,
                    date: vakansvaktRequest.Date
                )
            );

            return Task.CompletedTask;
        }
    }
}