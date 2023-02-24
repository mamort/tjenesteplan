using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Tjenesteplan.Api.Exceptions;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Actions.MinTjenesteplan;
using Tjenesteplan.Domain.Events;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.VakansvaktRequests.AddVakansvaktRequest
{
    public class AddVakansvaktRequestAction
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        private readonly CurrentTjenesteplanAction _currentTjenesteplanAction;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly ITjenesteplanChangesRepository _tjenesteplanChangesRepository;
        private readonly IVakansvaktRequestRepository _vakansvaktRequestRepository;

        public AddVakansvaktRequestAction(
            IMediator mediator,
            IUserRepository userRepository,
            CurrentTjenesteplanAction currentTjenesteplanAction,
            ITjenesteplanRepository tjenesteplanRepository,
            ITjenesteplanChangesRepository tjenesteplanChangesRepository,
            IVakansvaktRequestRepository vakansvaktRequestRepository
        )
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _currentTjenesteplanAction = currentTjenesteplanAction;
            _tjenesteplanRepository = tjenesteplanRepository;
            _tjenesteplanChangesRepository = tjenesteplanChangesRepository;
            _vakansvaktRequestRepository = vakansvaktRequestRepository;
        }


        public Task ExecuteAsync(string username, AddVakansvaktRequestModel model)
        {
            var user = _userRepository.GetUserByUsername(username);
            var tjenesteplaner = _tjenesteplanRepository.GetTjenesteplanerForUser(user.Id);

            if (!tjenesteplaner.Any())
            {
                throw new AppException($"User with id {user.Id} does not have a assigned tjenesteplan");
            }

            var currentTjenesteplan = _currentTjenesteplanAction.Execute(user.Id, model.TjenesteplanId);

            var tjenestedag = currentTjenesteplan.Dates.FirstOrDefault(d => d.Date.Date == model.Date.Date);
            if (tjenestedag == null)
            {
                throw new Exception($"Could not find date {model.Date} in user {user.Id} tjenesteplan");
            }

            var vakansvaktRequestId = _vakansvaktRequestRepository.AddVakansvaktRequest(
                userId: user.Id,
                tjenesteplanId: model.TjenesteplanId,
                date: model.Date,
                currentDagsplan: tjenestedag.Dagsplan,
                requestedDagsplan: model.Dagsplan,
                reason: model.Reason
            );

            _tjenesteplanChangesRepository.AddTjenesteplanVakansvaktChange(
                userId: user.Id,
                tjenesteplanId: model.TjenesteplanId,
                date: model.Date,
                vakansvaktRequestId: vakansvaktRequestId,
                dagsplan: DagsplanEnum.ForespørselOmVakansvakt
            );

            _mediator.Publish(
                new VakansvaktRequestReceivedEvent(
                    tjenesteplanId: model.TjenesteplanId,
                    requestor: user,
                    vakansvaktRequestId: vakansvaktRequestId,
                    date: model.Date
                )
            );

            return Task.CompletedTask;
        }
    }
}