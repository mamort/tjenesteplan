using System;
using System.Collections.Generic;
using System.Linq;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Actions.MinTjenesteplan;
using Tjenesteplan.Domain.Repositories;
using VaktChangeRequest = Tjenesteplan.Domain.VaktChangeRequest;

namespace Tjenesteplan.Api.Features.VaktChangeRequests.AllRequests
{
    public class GetAllRequestsAction
    {
        private readonly IUserRepository _userRepository;
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly IVaktChangeRequestRepository _vaktChangeRequestRepository;
        private readonly CurrentTjenesteplanAction _getCurrentTjenesteplanAction;

        public GetAllRequestsAction(
            IUserRepository userRepository,
            ITjenesteplanRepository tjenesteplanRepository,
            IVaktChangeRequestRepository vaktChangeRequestRepository,
            CurrentTjenesteplanAction getCurrentTjenesteplanAction)
        {
            _userRepository = userRepository;
            _tjenesteplanRepository = tjenesteplanRepository;
            _vaktChangeRequestRepository = vaktChangeRequestRepository;
            _getCurrentTjenesteplanAction = getCurrentTjenesteplanAction;
        }

        public AllRequestsModel Execute(string username, int tjenesteplanId)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (!_tjenesteplanRepository.IsUserListeforerForTjenesteplan(user.Id, tjenesteplanId) &&
                user.Role != Role.Admin
            )
            {
                throw new UnauthorizedAccessException();
            }

            var allUsers = _userRepository.GetAllUsers();
            var requests = _vaktChangeRequestRepository.GetChangeRequests(tjenesteplanId)
                .Where(r => r.Status != VaktChangeRequestStatus.Cancelled &&
                            r.Status != VaktChangeRequestStatus.CanceledByUser);

            var currentTjenesteplan = _getCurrentTjenesteplanAction.Execute(tjenesteplanId, username);

            if (currentTjenesteplan == null)
            {
                return new AllRequestsModel
                {
                    Requests = new List<RequestModel>()
                };
            }

            return new AllRequestsModel
            {
                Requests = requests.Select(r => CreateRequestModel(allUsers, currentTjenesteplan, r)).ToList()
            };
        }

        private RequestModel CreateRequestModel(
            IReadOnlyList<User> allUsers, 
            CurrentTjenesteplan currentTjenesteplan, 
            VaktChangeRequest r
        )
        {
            var chosenReply = r.Replies.FirstOrDefault(r => r.Status == VaktChangeRequestReplyStatus.Accepted);
            RequestReplyModel chosenReplyModel = null;
            if (chosenReply != null)
            {
                chosenReplyModel = CreateReplyModel(allUsers, currentTjenesteplan, r, chosenReply);
            }

            var user = allUsers.FirstOrDefault(u => u.Id == r.UserId);

            return new RequestModel
            {
                Id = r.Id,
                Fullname = user?.Fullname,
                RegisteredDate = r.RegisteredDate,
                Date = r.Date,
                Status = r.Status,
                ChosenChangeDate = r.ChosenChangeDate,
                ChosenReply = chosenReplyModel,
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