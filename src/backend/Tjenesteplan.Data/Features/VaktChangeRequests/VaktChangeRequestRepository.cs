using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tjenesteplan.Data.Contexts;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Data.Features.VaktChangeRequests
{
    public class VaktChangeRequestRepository : IVaktChangeRequestRepository
    {
        private readonly DataContext _dbContext;

        public VaktChangeRequestRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int AddVaktChangeRequest(int userId, int tjenesteplanId, DateTime date, DagsplanEnum dagsplan)
        {
            var entity = new VaktChangeRequestEntity
            {
                RequestRegisteredDate = DateTime.UtcNow,
                Status = VaktChangeRequestStatus.InProgress,
                Date = date,
                Dagsplan  = dagsplan,
                UserId = userId,
                TjenesteplanId = tjenesteplanId
            };

            _dbContext.VaktChangeRequests.Add(entity);

            _dbContext.SaveChanges();

            return entity.Id;
        }

        public IReadOnlyList<VaktChangeRequest> GetChangeRequestsCreatedByUser(int userId, int tjenesteplanId)
        {
            return _dbContext.VaktChangeRequests
                .Include(v => v.VaktChangeRequestsReplies)
                .ThenInclude(r => r.VaktChangeRequestAlternatives)
                .Where(v => v.UserId == userId && v.TjenesteplanId == tjenesteplanId)
                .Select(v => CreateVaktChangeRequest(v)).ToList();
        }

        public IReadOnlyList<VaktChangeRequest> GetChangeRequests(int tjenesteplanId)
        {
            return _dbContext.VaktChangeRequests
                .Include(v => v.VaktChangeRequestsReplies)
                .ThenInclude(r => r.VaktChangeRequestAlternatives)
                .Where(v => v.TjenesteplanId == tjenesteplanId)
                .Select(v => CreateVaktChangeRequest(v)).ToList();
        }

        public VaktChangeRequest GetRequestById(int id)
        {
            var req = _dbContext.VaktChangeRequests.FirstOrDefault(v => v.Id == id);
            if (req == null)
            {
                return null;
            }

            return CreateVaktChangeRequest(req);
        }

        public void UpdateVaktChangeRequest(int id, int chosenVaktChangeAlternativeId, VaktChangeRequestStatus status)
        {
            var req = _dbContext.VaktChangeRequests.FirstOrDefault(v => v.Id == id);
            if (req == null)
            {
                throw new Exception($"Could not find request with id {id}");
            }

            req.VaktChangeChosenAlternativeId = chosenVaktChangeAlternativeId;
            req.Status = status;

            _dbContext.Update(req);
            _dbContext.SaveChanges();
        }

        public void UpdateVaktChangeRequestStatus(int id, VaktChangeRequestStatus status)
        {
            var req = _dbContext.VaktChangeRequests.FirstOrDefault(v => v.Id == id);
            if (req == null)
            {
                throw new Exception($"Could not find request with id {id}");
            }

            req.Status = status;

            _dbContext.Update(req);
            _dbContext.SaveChanges();
        }

        public void DeleteRequest(int id)
        {
            var req = _dbContext.VaktChangeRequests.FirstOrDefault(v => v.Id == id);
            if (req != null)
            {
                _dbContext.VaktChangeRequests.Remove(req);
                _dbContext.SaveChanges();
            }
        }

        private static VaktChangeRequest CreateVaktChangeRequest(VaktChangeRequestEntity v)
        {
            return new VaktChangeRequest(
                id: v.Id,
                tjenesteplanId: v.TjenesteplanId,
                userId: v.UserId,
                registeredDate: v.RequestRegisteredDate,
                date: v.Date,
                chosenChangeDate: v.VaktChangeChosenAlternative?.Date,
                dagsplan: v.Dagsplan,
                status: v.Status,
                replies: v.VaktChangeRequestsReplies
                    ?.Select(reply => CreateVaktChangeRequestReply(reply, v.Status))
                    .ToList() ?? new List<VaktChangeRequestReply>()
            );
        }

        private static VaktChangeRequestReply CreateVaktChangeRequestReply(VaktChangeRequestReplyEntity r, VaktChangeRequestStatus status)
        {           
            return new VaktChangeRequestReply(
                id: r.Id,
                userId: r.UserId,
                vaktChangeRequestId: r.VaktChangeRequestId,
                requestStatus: status,
                status: r.Status,
                numberOfRemindersSent: r.NumberOfRemindersSent,
                alternatives: r.VaktChangeRequestAlternatives.Select(a => new VaktChangeRequestAlternative(a.Id, a.Date)).ToList()
            );
        }
    }
}