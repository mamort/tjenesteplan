using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tjenesteplan.Data.Contexts;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Data.Features.VaktChangeRequests
{
    public class VaktChangeRequestRepliesRepository : IVaktChangeRequestRepliesRepository
    {
        private readonly DataContext _dbContext;

        public VaktChangeRequestRepliesRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddVaktChangeRequestReply(int userId, int vaktChangeRequestId)
        {
            _dbContext.VaktChangeRequestReplies.Add(new VaktChangeRequestReplyEntity
            {
               NumberOfRemindersSent = 0,
               Status = VaktChangeRequestReplyStatus.None,
               UserId = userId,
               VaktChangeRequestId = vaktChangeRequestId
            });

            _dbContext.SaveChanges();
        }

        public IReadOnlyList<VaktChangeRequestReply> GetRequestRepliesForUser(int userId, int tjenesteplanId)
        {
            return _dbContext.VaktChangeRequestReplies
                .Include(v => v.VaktChangeRequest)
                .Include(v => v.VaktChangeRequestAlternatives)
                .Where(v => v.UserId == userId && v.VaktChangeRequest.TjenesteplanId == tjenesteplanId)
                .Select(v => CreateVaktChangeRequestReply(v)).ToList();
        }

        public void AddVaktChangeSuggestions(int replyId, List<DateTime> dates)
        {
            var reply = _dbContext.VaktChangeRequestReplies.FirstOrDefault(r => r.Id == replyId);
            if (reply == null)
            {
                throw new Exception($"Could not find reply with id: {replyId}");
            }

            reply.VaktChangeRequestAlternatives.Clear();
            _dbContext.SaveChanges();

            foreach (var date in dates)
            {
                reply.VaktChangeRequestAlternatives.Add(new VaktChangeAlternativeEntity
                {
                    Date = date,
                    VaktChangeRequestReplyId = replyId

                });
            }

            _dbContext.SaveChanges();
        }
        public void ChangeVaktChangeRequestStatus(int replyId, VaktChangeRequestReplyStatus status)
        {
            var reply = _dbContext.VaktChangeRequestReplies.FirstOrDefault(r => r.Id == replyId);
            if (reply == null)
            {
                throw new Exception($"Could not find reply with id: {replyId}");
            }

            reply.Status = status;
            _dbContext.SaveChanges();
        }

        public void RejectVaktChangeRequest(int replyId)
        {
            var reply = _dbContext.VaktChangeRequestReplies.FirstOrDefault(r => r.Id == replyId);
            if (reply == null)
            {
                throw new Exception($"Could not find reply with id: {replyId}");
            }

            reply.VaktChangeRequestAlternatives.Clear();
            reply.Status = VaktChangeRequestReplyStatus.Rejected;
            _dbContext.SaveChanges();
        }

        public VaktChangeRequestReply GetVaktChangeReply(int replyId)
        {
            var reply = _dbContext.VaktChangeRequestReplies
                .Include(v => v.VaktChangeRequest)
                .Include(r => r.VaktChangeRequestAlternatives)
                .FirstOrDefault(r => r.Id == replyId);
            if (reply == null)
            {
                throw new Exception($"Could not find reply with id: {replyId}");
            }

            return CreateVaktChangeRequestReply(reply);
        }

        public void DeleteVaktChangeReplies(int vaktChangeRequestId)
        {
            var replies = _dbContext.VaktChangeRequestReplies
                .Where(v => v.VaktChangeRequest.Id == vaktChangeRequestId);

            if (replies.Any())
            {
                foreach (var reply in replies)
                {
                    _dbContext.VaktChangeRequestReplies.Remove(reply);
                }
                
                _dbContext.SaveChanges();
            }
        }

        public void DeleteVaktChangeRepliesForUser(int userId)
        {
            var replies = _dbContext.VaktChangeRequestReplies
                .Where(v => v.UserId == userId);

            if (replies.Any())
            {
                foreach (var reply in replies)
                {
                    _dbContext.VaktChangeRequestReplies.Remove(reply);
                }

                _dbContext.SaveChanges();
            }
        }

        private static VaktChangeRequestReply CreateVaktChangeRequestReply(VaktChangeRequestReplyEntity v)
        {
            return new VaktChangeRequestReply(
                id: v.Id,
                userId: v.UserId,
                vaktChangeRequestId: v.VaktChangeRequestId,
                requestStatus: v.VaktChangeRequest.Status,
                status: v.Status,
                numberOfRemindersSent: v.NumberOfRemindersSent,
                alternatives: v.VaktChangeRequestAlternatives.Select(a => new VaktChangeRequestAlternative(a.Id, a.Date)).ToList()
            );
        }
    }
}