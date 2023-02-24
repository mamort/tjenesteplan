using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Tjenesteplan.Api.Exceptions;
using Tjenesteplan.Domain.Actions.MinTjenesteplan;
using Tjenesteplan.Domain.Events;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.VaktChangeRequests.VaktChangeSuggestions
{
    public class AddVaktChangeSuggestionsAction
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        private readonly CurrentTjenesteplanAction _currentTjenesteplanAction;
        private readonly IVaktChangeRequestRepository _vaktChangeRequestRepository;
        private readonly IVaktChangeRequestRepliesRepository _vaktChangeRequestRepliesRepository;

        public AddVaktChangeSuggestionsAction(
            IMediator mediator,
            IUserRepository userRepository,
            CurrentTjenesteplanAction currentTjenesteplanAction,
            IVaktChangeRequestRepository vaktChangeRequestRepository,
            IVaktChangeRequestRepliesRepository vaktChangeRequestRepliesRepository
        )
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _currentTjenesteplanAction = currentTjenesteplanAction;
            _vaktChangeRequestRepository = vaktChangeRequestRepository;
            _vaktChangeRequestRepliesRepository = vaktChangeRequestRepliesRepository;        }

        public async Task ExecuteAsync(string username, int replyId, AddVaktChangeSugguestionsModel model)
        {
            var respondent = _userRepository.GetUserByUsername(username);
            var reply = _vaktChangeRequestRepliesRepository.GetVaktChangeReply(replyId);

            if (reply == null)
            {
                throw new AppException($"Could not find reply with id: {replyId}");
            }

            if (model.Dates.Any(d => d.Date < DateTime.UtcNow.Date))
            {
                throw new AppException("Cannot register a suggested vakt change date that is earlier" +
                                       "than tomorrows date");
            }

            _vaktChangeRequestRepliesRepository.AddVaktChangeSuggestions(replyId, model.Dates);

            var request = _vaktChangeRequestRepository.GetRequestById(reply.VaktChangeRequestId);
            var vaktChangeRequestor = _userRepository.GetUserById(request.UserId);

            var currentTjenesteplan = _currentTjenesteplanAction.Execute(request.UserId, request.TjenesteplanId);
            if (currentTjenesteplan == null)
            {
                throw new Exception($"User with id {respondent.Id} does not have a tjenesteplan assigned.");
            }

            var possibleVaktbytter = request.FindPossibleVaktbytter(currentTjenesteplan, model.Dates);

            if (possibleVaktbytter.Any())
            {
                await _mediator.Publish(new VaktChangeSuggestionsReceivedEvent(
                    tjenesteplanId: request.TjenesteplanId,
                    vaktChangeRequestId: request.Id,
                    vaktChangeRequestor: vaktChangeRequestor,
                    vaktChangeRespondent: respondent,
                    date: request.Date

                ));
            }
        }
    }
}