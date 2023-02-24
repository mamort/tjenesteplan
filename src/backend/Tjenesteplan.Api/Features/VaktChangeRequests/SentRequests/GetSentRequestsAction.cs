using System;
using System.Collections.Generic;
using System.Linq;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Actions.MinTjenesteplan;
using Tjenesteplan.Domain.Repositories;
using VaktChangeRequest = Tjenesteplan.Domain.VaktChangeRequest;

namespace Tjenesteplan.Api.Features.VaktChangeRequests.SentRequests
{
    public class GetSentRequestsAction
    {
        private readonly IUserRepository _userRepository;
        private readonly IVaktChangeRequestRepository _vaktChangeRequestRepository;
        private readonly CurrentTjenesteplanAction _getCurrentTjenesteplanAction;

        public GetSentRequestsAction(
            IUserRepository userRepository,
            IVaktChangeRequestRepository vaktChangeRequestRepository,
            CurrentTjenesteplanAction getCurrentTjenesteplanAction)
        {
            _userRepository = userRepository;
            _vaktChangeRequestRepository = vaktChangeRequestRepository;
            _getCurrentTjenesteplanAction = getCurrentTjenesteplanAction;
        }

        public SentRequestsModel Execute(string username, int tjenesteplanId)
        {
            var user = _userRepository.GetUserByUsername(username);

            var allUsers = _userRepository.GetAllUsers();
            var requests = _vaktChangeRequestRepository.GetChangeRequestsCreatedByUser(user.Id, tjenesteplanId)
                .Where(r => r.Status != VaktChangeRequestStatus.Cancelled &&
                            r.Status != VaktChangeRequestStatus.CanceledByUser);
            var currentTjenesteplan = _getCurrentTjenesteplanAction.Execute(tjenesteplanId, username);

            if (currentTjenesteplan == null)
            {
                return new SentRequestsModel
                {
                    Requests = new List<SentRequestModel>()
                };
            }

            return new SentRequestsModel
            {
                Requests = requests.Select(r => CreateRequestModel(allUsers, currentTjenesteplan, r)).ToList()
            };
        }

        private SentRequestModel CreateRequestModel(
            IReadOnlyList<User> allUsers, 
            CurrentTjenesteplan currentTjenesteplan, 
            VaktChangeRequest r
        )
        {
            return new SentRequestModel
            {
                Id = r.Id,
                RegisteredDate = r.RegisteredDate,
                Date = r.Date,
                Status = r.Status,
                ChosenChangeDate = r.ChosenChangeDate,
                Replies = r.Replies.Select(reply => CreateReplyModel(allUsers, currentTjenesteplan, r, reply)).ToList()
            };
        }

        private RequestReplyModel CreateReplyModel(
            IReadOnlyList<User> allUsers, 
            CurrentTjenesteplan currentTjenesteplan, 
            VaktChangeRequest request, 
            VaktChangeRequestReply reply
        )
        {
            var user = allUsers.FirstOrDefault(u => u.Id == reply.UserId);
            if (user == null)
            {
                throw new Exception($"Could not find user with id {reply.UserId} connected to vaktchangerequest reply with id {reply.Id}");
            }

            var dateAlternatives = reply.Alternatives.Select(a => a.Date).ToList();
            var vaktbytteDateAlternatives = request.FindPossibleVaktbytter(currentTjenesteplan, dateAlternatives);

            return new RequestReplyModel
            {
                Id = reply.Id,
                UserId = reply.UserId,
                Fullname = user.Fullname,
                Status = reply.Status,
                Alternatives = vaktbytteDateAlternatives.ToList()
            };
        }
    }
}