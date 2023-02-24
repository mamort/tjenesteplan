using System.Linq;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.VaktChangeRequests.ReceivedRequests
{
    public class GetVaktChangeRequestsReceivedAction
    {
        private readonly IUserRepository _userRepository;
        private readonly IVaktChangeRequestRepository _vaktChangeRequestRepository;
        private readonly IVaktChangeRequestRepliesRepository _vaktChangeRequestRepliesRepository;

        public GetVaktChangeRequestsReceivedAction(
            IUserRepository userRepository,
            IVaktChangeRequestRepository vaktChangeRequestRepository,
            IVaktChangeRequestRepliesRepository vaktChangeRequestRepliesRepository)
        {
            _userRepository = userRepository;
            _vaktChangeRequestRepository = vaktChangeRequestRepository;
            _vaktChangeRequestRepliesRepository = vaktChangeRequestRepliesRepository;
        }

        public RequestRepliesModel Execute(string username, int tjenesteplanId)
        {
            var user = _userRepository.GetUserByUsername(username);
            var replies = _vaktChangeRequestRepliesRepository.GetRequestRepliesForUser(user.Id, tjenesteplanId)
                .Where(r => r.RequestStatus != VaktChangeRequestStatus.Cancelled &&
                            r.RequestStatus != VaktChangeRequestStatus.CanceledByUser);

            return new RequestRepliesModel
            {
                Requests = replies
                    .Select(CreateReplyModel)
                    .ToList()
            };
        }
        private VaktChangeRequestReceivedModel CreateReplyModel(VaktChangeRequestReply reply)
        {
            var request = _vaktChangeRequestRepository.GetRequestById(reply.VaktChangeRequestId);
            var user = _userRepository.GetUserById(request.UserId);
 
            return new VaktChangeRequestReceivedModel
            {
                VaktChangeRequestId = request.Id,
                Date = request.Date,
                ChosenChangeDate = request.ChosenChangeDate,
                Dagsplan = request.Dagsplan,
                RequestedByUserId = user.Id,
                RequestedBy = user.Fullname,
                RequestedBySpesialisering = user.Spesialitet?.Name,
                Status = request.Status,
                Reply = new VaktChangeReplyModel
                {
                    Id = reply.Id,
                    Status = reply.Status,
                    Alternatives = reply.Alternatives.Select(a => a.Date).ToList()
                }
            };
        }
    }
}