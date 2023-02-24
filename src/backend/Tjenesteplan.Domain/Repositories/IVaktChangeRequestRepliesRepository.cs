using System;
using System.Collections.Generic;

namespace Tjenesteplan.Domain.Repositories
{
    public interface IVaktChangeRequestRepliesRepository
    {
        void AddVaktChangeRequestReply(int userId, int vaktChangeRequestId);
        IReadOnlyList<VaktChangeRequestReply> GetRequestRepliesForUser(int userId, int tjenesteplan);
        void AddVaktChangeSuggestions(int replyId, List<DateTime> dates);
        VaktChangeRequestReply GetVaktChangeReply(int replyId);

        void DeleteVaktChangeReplies(int vaktChangeRequestId);
        void DeleteVaktChangeRepliesForUser(int userId);
        void RejectVaktChangeRequest(int replyId);
        void ChangeVaktChangeRequestStatus(int replyId, VaktChangeRequestReplyStatus status);
    }
}