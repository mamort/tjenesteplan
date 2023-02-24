using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Tjenesteplan.Api.Exceptions;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Actions.MinTjenesteplan;
using Tjenesteplan.Domain.Events;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.VaktChangeRequests.AcceptVaktChange
{
    public class AcceptVaktChangeAction
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly ITjenesteplanChangesRepository _tjenesteplanChangesRepository;
        private readonly IVaktChangeRequestRepository _vaktChangeRequestRepository;
        private readonly IVaktChangeRequestRepliesRepository _vaktChangeRequestRepliesRepository;
        private readonly CurrentTjenesteplanAction _currentTjenesteplanAction;

        public AcceptVaktChangeAction(
            IMediator mediator,
            IUserRepository userRepository,
            ITjenesteplanRepository tjenesteplanRepository,
            ITjenesteplanChangesRepository tjenesteplanChangesRepository,
            IVaktChangeRequestRepository vaktChangeRequestRepository,
            IVaktChangeRequestRepliesRepository vaktChangeRequestRepliesRepository,
            CurrentTjenesteplanAction currentTjenesteplanAction
        )        
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
            _tjenesteplanChangesRepository = tjenesteplanChangesRepository;
            _vaktChangeRequestRepository = vaktChangeRequestRepository;
            _vaktChangeRequestRepliesRepository = vaktChangeRequestRepliesRepository;
            _currentTjenesteplanAction = currentTjenesteplanAction;
        }

        public Task ExecuteAsync(string username, AcceptVaktChangeModel model)
        {
            var user = _userRepository.GetUserByUsername(username);

            var vaktChangeReply = _vaktChangeRequestRepliesRepository.GetVaktChangeReply(model.ReplyId);

            if (vaktChangeReply == null)
            {
                throw new Exception($"Could not find vaktchangereply with id {model.ReplyId}");
            }

            var request = _vaktChangeRequestRepository.GetRequestById(vaktChangeReply.VaktChangeRequestId);
            
            // Change dates for requestor
            ChangeDatesForRequestor(
                tjenesteplanId: request.TjenesteplanId,
                requestDate: request.Date, 
                changeDate: model.Date, 
                user: user, 
                vaktChangeReply: vaktChangeReply,
                dagsplan: request.Dagsplan
            );

            var respondent = _userRepository.GetUserById(vaktChangeReply.UserId);
            // Change dates for respondent
            ChangeDatesForRespondent(
                tjenesteplanId: request.TjenesteplanId,
                requestDate: request.Date,
                changeDate: model.Date,
                user: respondent,
                vaktChangeReply: vaktChangeReply,
                dagsplan: request.Dagsplan
            );

            var chosenVaktChangeAlternative =
                vaktChangeReply.Alternatives.FirstOrDefault(a => a.Date.Date == model.Date.Date);

            if (chosenVaktChangeAlternative == null)
            {
                throw new Exception(
                    $"Could not find vaktchange alternative with date {model.Date} " +
                    $"for reply with id {vaktChangeReply.Id}"
                );
            }

            _vaktChangeRequestRepository.UpdateVaktChangeRequest(request.Id, chosenVaktChangeAlternative.Id, VaktChangeRequestStatus.Completed);
            _vaktChangeRequestRepliesRepository.ChangeVaktChangeRequestStatus(vaktChangeReply.Id, VaktChangeRequestReplyStatus.Accepted);

            return _mediator.Publish(new VaktChangeCompletedEvent(
                requestor: user,
                respondent: respondent,
                requestDate: request.Date,
                changeDate: model.Date
            ));
        }

        private void ChangeDatesForRequestor(
            DateTime requestDate, 
            DateTime changeDate,
            User user, 
            VaktChangeRequestReply vaktChangeReply,
            DagsplanEnum dagsplan,
            int tjenesteplanId)
        {
            var currentTjenesteplan = _currentTjenesteplanAction.Execute(user.Id, tjenesteplanId);
            RemoveVakt(currentTjenesteplan, tjenesteplanId, requestDate, user, vaktChangeReply);
            AddVakt(currentTjenesteplan, tjenesteplanId, changeDate, user, vaktChangeReply, dagsplan);
        }

        private void ChangeDatesForRespondent(
            int tjenesteplanId,
            DateTime requestDate,
            DateTime changeDate,
            User user,
            VaktChangeRequestReply vaktChangeReply,
            DagsplanEnum dagsplan
        )
        {
            var currentTjenesteplan = _currentTjenesteplanAction.Execute(user.Id, tjenesteplanId);
            AddVakt(currentTjenesteplan, tjenesteplanId, requestDate, user, vaktChangeReply, dagsplan);
            RemoveVakt(currentTjenesteplan, tjenesteplanId, changeDate, user, vaktChangeReply);
        }

        private void AddVakt(
            CurrentTjenesteplan currentTjenesteplan, 
            int tjenesteplanId,
            DateTime date,
            User user,
            VaktChangeRequestReply vaktChangeReply,
            DagsplanEnum dagsplan)
        {
            _tjenesteplanChangesRepository.AddTjenesteplanChange(
                userId: user.Id,
                tjenesteplanId: tjenesteplanId,
                vaktChangeRequestId: vaktChangeReply.VaktChangeRequestId,
                date: date,
                dagsplan: dagsplan
            );

            var dayAfterVakt = currentTjenesteplan.Dates.FirstOrDefault(d => d.Date.Date == date.Date.Date.AddDays(1));
            if (dayAfterVakt?.Dagsplan == DagsplanEnum.None &&
                (dagsplan == DagsplanEnum.Døgnvakt || dagsplan == DagsplanEnum.Nattevakt))
            {
                _tjenesteplanChangesRepository.AddTjenesteplanChange(
                    userId: user.Id,
                    tjenesteplanId: tjenesteplanId,
                    vaktChangeRequestId: vaktChangeReply.VaktChangeRequestId,
                    date: date.AddDays(1),
                    dagsplan: DagsplanEnum.FriEtterVakt
                );
            }
        }

        private void RemoveVakt(
            CurrentTjenesteplan currentTjenesteplan, 
            int tjenesteplanId,
            DateTime date,
            User user,
            VaktChangeRequestReply vaktChangeReply)
        {
            _tjenesteplanChangesRepository.AddTjenesteplanChange(
                userId: user.Id,
                tjenesteplanId: tjenesteplanId,
                vaktChangeRequestId: vaktChangeReply.VaktChangeRequestId,
                date: date,
                dagsplan: DagsplanEnum.None
            );

            // If date after vakt is Fri Etter Vakt => change it to no plan
            var nextDay = currentTjenesteplan.Dates.FirstOrDefault(d => d.Date.Date == date.Date.Date.AddDays(1));
            if (nextDay != null && nextDay.Dagsplan == DagsplanEnum.FriEtterVakt)
            {
                _tjenesteplanChangesRepository.AddTjenesteplanChange(
                    userId: user.Id,
                    tjenesteplanId: tjenesteplanId,
                    vaktChangeRequestId: vaktChangeReply.VaktChangeRequestId,
                    date: date.AddDays(1),
                    dagsplan: DagsplanEnum.None
                );
            }
        }
    }
}