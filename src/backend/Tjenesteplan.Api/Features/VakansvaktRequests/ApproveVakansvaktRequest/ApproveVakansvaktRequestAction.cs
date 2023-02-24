using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Actions.MinTjenesteplan;
using Tjenesteplan.Domain.Events;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.VakansvaktRequests.ApproveVakansvaktRequest
{
    public class ApproveVakansvaktRequestAction
    {
        private readonly IUserRepository _userRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly ITjenesteplanChangesRepository _tjenesteplanChangesRepository;
        private readonly IVakansvaktRequestRepository _vakansvaktRequestRepository;
        private readonly CurrentTjenesteplanAction _currentTjenesteplanAction;
        private readonly IMediator _mediator;

        public ApproveVakansvaktRequestAction(
            IUserRepository userRepository,
            ITjenesteplanRepository tjenesteplanRepository,
            ITjenesteplanChangesRepository tjenesteplanChangesRepository,
            IVakansvaktRequestRepository vakansvaktRequestRepository,
            CurrentTjenesteplanAction currentTjenesteplanAction,
            IMediator mediator
        )
        {
            _userRepository = userRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
            _tjenesteplanChangesRepository = tjenesteplanChangesRepository;
            _vakansvaktRequestRepository = vakansvaktRequestRepository;
            _currentTjenesteplanAction = currentTjenesteplanAction;
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

            _vakansvaktRequestRepository.ChangeVakansvaktRequestStatus(
                vakansvaktRequestId: vakansvaktRequestId,
                status: VakansvaktRequestStatus.Approved
            );

            _tjenesteplanChangesRepository.AddTjenesteplanVakansvaktChange(
                userId: vakansvaktRequest.RequestedByUserId,
                tjenesteplanId: vakansvaktRequest.TjenesteplanId,
                date: vakansvaktRequest.Date,
                vakansvaktRequestId:vakansvaktRequestId,
                dagsplan: vakansvaktRequest.RequestedDagsplan
            );

            var currentTjenesteplan = _currentTjenesteplanAction.Execute(
                vakansvaktRequest.RequestedByUserId,
                vakansvaktRequest.TjenesteplanId
            );

            // If date after vakt is Fri Etter Vakt => change it to no plan
            var nextDay = currentTjenesteplan.Dates.FirstOrDefault(d => d.Date.Date == vakansvaktRequest.Date.Date.AddDays(1));
            if (nextDay != null && nextDay.Dagsplan == DagsplanEnum.FriEtterVakt)
            {
                _tjenesteplanChangesRepository.AddTjenesteplanVakansvaktChange(
                    userId: vakansvaktRequest.RequestedByUserId,
                    tjenesteplanId: vakansvaktRequest.TjenesteplanId,
                    date: vakansvaktRequest.Date.AddDays(1),
                    vakansvaktRequestId: vakansvaktRequestId,
                    dagsplan: DagsplanEnum.None
                );
            }

            var requestor = _userRepository.GetUserById(vakansvaktRequest.RequestedByUserId);

            _mediator.Publish(
                new VakansvaktRequestApprovedEvent(
                    tjenesteplanId: vakansvaktRequest.TjenesteplanId,
                    requestor: requestor,
                    vakansvaktRequestId: vakansvaktRequestId,
                    date: vakansvaktRequest.Date,
                    dagsplan: vakansvaktRequest.CurrentDagsplan
                )
            );

            return Task.CompletedTask;
        }
    }
}