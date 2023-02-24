using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Tjenesteplan.Api.Configuration;
using Tjenesteplan.Api.Exceptions;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Actions.MinTjenesteplan;
using Tjenesteplan.Domain.Events;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.VaktChangeRequests.AddVaktChangeRequest
{
    public class AddVaktChangeRequestAction
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        private readonly CurrentTjenesteplanAction _currentTjenesteplanAction;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly ITjenesteplanChangesRepository _tjenesteplanChangesRepository;
        private readonly IVaktChangeRequestRepository _vaktChangeRequestRepository;
        private readonly IVaktChangeRequestRepliesRepository _vaktChangeRequestRepliesRepository;

        public AddVaktChangeRequestAction(
            IMediator mediator,
            IUserRepository userRepository,
            CurrentTjenesteplanAction currentTjenesteplanAction,
            ITjenesteplanRepository tjenesteplanRepository,
            ITjenesteplanChangesRepository tjenesteplanChangesRepository,
            IVaktChangeRequestRepository vaktChangeRequestRepository,
            IVaktChangeRequestRepliesRepository vaktChangeRequestRepliesRepository
        )
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _currentTjenesteplanAction = currentTjenesteplanAction;
            _tjenesteplanRepository = tjenesteplanRepository;
            _tjenesteplanChangesRepository = tjenesteplanChangesRepository;
            _vaktChangeRequestRepository = vaktChangeRequestRepository;
            _vaktChangeRequestRepliesRepository = vaktChangeRequestRepliesRepository;
        }

        public Task ExecuteAsync(string username, AddVaktChangeRequestModel model)
        {
            var user = _userRepository.GetUserByUsername(username);
            var tjenesteplaner = _tjenesteplanRepository.GetTjenesteplanerForUser(user.Id);

            if (!tjenesteplaner.Any())
            {
                throw new AppException($"User with id {user.Id} does not have a assigned tjenesteplan");
            }

            var currentTjenesteplan = _currentTjenesteplanAction.Execute(
                userId: user.Id, 
                tjenesteplanId: model.TjenesteplanId
            );

            foreach (var date in model.Dates)
            {
                ValidateDay(date, currentTjenesteplan, user);
            }

            return AddVaktChangeRequests(model, user, model.TjenesteplanId);
        }

        private async Task AddVaktChangeRequests(AddVaktChangeRequestModel model, User user, int tjenesteplanId)
        {
            var leger = _userRepository.GetUsersByTjenesteplan(tjenesteplanId)
                .Where(u => u.Role == Role.Lege || u.Role == Role.Overlege)
                .Where(u => u.Id != user.Id).ToList();

            foreach (var date in model.Dates)
            {
                var vaktChangeRequestId = _vaktChangeRequestRepository.AddVaktChangeRequest(
                    userId: user.Id,
                    tjenesteplanId: tjenesteplanId,
                    date: date,
                    dagsplan: model.Dagsplan
                );

                _tjenesteplanChangesRepository.AddTjenesteplanChange(
                    userId: user.Id,
                    tjenesteplanId: tjenesteplanId,
                    vaktChangeRequestId: vaktChangeRequestId,
                    date: date,
                    dagsplan: DagsplanEnum.ForespørselOmVaktbytte
                );

                await AddVaktChangeRequestRepliesAsync(tjenesteplanId, user, leger, date, model.Dagsplan, vaktChangeRequestId);
            }
        }

        private async Task AddVaktChangeRequestRepliesAsync(
            int tjenesteplanId, 
            User user, 
            List<User> leger, 
            DateTime date, 
            DagsplanEnum dagsplan, 
            int vaktChangeRequestId
        )
        {
            foreach (var lege in leger)
            {
                var legeCurrentTjenesteplan = _currentTjenesteplanAction.Execute(lege.Id, tjenesteplanId);
                if (legeCurrentTjenesteplan.IsDateAvailableForVakt(date, dagsplan))
                {
                    _vaktChangeRequestRepliesRepository.AddVaktChangeRequestReply(
                        userId: lege.Id,
                        vaktChangeRequestId: vaktChangeRequestId
                    );

                    await _mediator.Publish(new VaktChangeRequestReceivedEvent(
                        tjenesteplanId: tjenesteplanId,
                        vaktChangeRequestId: vaktChangeRequestId,
                        requestor: user,
                        receiver: lege,
                        date: date
                    ));
                }
            }
        }

        private static void ValidateDay(DateTime date, CurrentTjenesteplan currentTjenesteplan, User user)
        {
            var day = currentTjenesteplan.Dates.FirstOrDefault(d => d.Date.Date == date.Date);
            if (day == null)
            {
                throw new AppException($"The date {date.ToShortDateString()} is " +
                                       "not configured in user's tjenesteplan");
            }

            if (day.Dagsplan != DagsplanEnum.Døgnvakt && 
                day.Dagsplan != DagsplanEnum.Nattevakt && 
                day.Dagsplan != DagsplanEnum.Dagvakt)
            {
                throw new AppException($"Cannot add a vakt change request to a date which is " +
                                       $"registered with dagsplan {day.Dagsplan.ToString()}");
            }

            if (date.Date <= DateTime.UtcNow.Date)
            {
                throw new AppException("Request to change vakt must be performed on a date later than todays date");
            }
        }
    }
}