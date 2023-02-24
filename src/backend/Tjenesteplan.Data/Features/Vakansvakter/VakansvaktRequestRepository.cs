using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tjenesteplan.Data.Contexts;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;
using WebApi.Features.Users.Data;

namespace Tjenesteplan.Data.Features.Vakansvakter
{
    public class VakansvaktRequestRepository : IVakansvaktRequestRepository
    {
        private readonly DataContext _dbContext;

        public VakansvaktRequestRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int AddVakansvaktRequest(
            int userId, 
            int tjenesteplanId, 
            DateTime date, 
            DagsplanEnum currentDagsplan,
            DagsplanEnum requestedDagsplan,
            string reason
        )
        {
            var entity = new VakansvaktRequestEntity
            {
                RegisteredDate = DateTime.UtcNow,
                OriginalLegeId = userId,
                TjenesteplanId = tjenesteplanId,
                Date = date,
                CurrentDagsplan = currentDagsplan,
                RequestedDagsplan = requestedDagsplan,
                Message = reason
            };

            _dbContext.VakansvaktRequests.Add(entity);

            _dbContext.SaveChanges();

            return entity.Id;
        }

        public IReadOnlyList<VakansvaktRequest> GetVakansvaktRequests(int tjenesteplanId, VakansvaktRequestStatus status)
        {
            return _dbContext.VakansvaktRequests
                .Where(v => v.TjenesteplanId == tjenesteplanId && v.Status == status)
                .Include(v => v.OriginalLege)
                .Select(v => CreateVakansvaktRequest(v))
                .ToList();
        }

        public IReadOnlyList<VakansvaktRequest> GetVakansvaktRequests(int tjenesteplanId, int userId)
        {
            return _dbContext.VakansvaktRequests
                .Where(v => v.TjenesteplanId == tjenesteplanId && v.OriginalLegeId == userId)
                .Include(v => v.OriginalLege)
                .Select(v => CreateVakansvaktRequest(v))
                .ToList();
        }

        public IReadOnlyList<VakansvaktRequest> GetAvailableVakansvaktRequests(int tjenesteplanId)
        {
            return _dbContext.VakansvaktRequests
                .Where(v => v.TjenesteplanId == tjenesteplanId && 
                    v.Date >= DateTime.UtcNow.Date &&
                    v.Status == VakansvaktRequestStatus.Approved)
                .Include(v => v.OriginalLege)
                .Select(v => CreateVakansvaktRequest(v))
                .ToList();
        }

        public VakansvaktRequest GetVakansvaktRequest(int vakansvaktRequestId)
        {
            return _dbContext.VakansvaktRequests
                .Where(v => v.Id == vakansvaktRequestId)
                .Include(v => v.OriginalLege)
                .Select(v => CreateVakansvaktRequest(v))
                .FirstOrDefault();
        }

        public void ChangeVakansvaktRequestStatus(int vakansvaktRequestId, VakansvaktRequestStatus status)
        {
            var request = _dbContext.VakansvaktRequests.FirstOrDefault(v => v.Id == vakansvaktRequestId);
            if (request != null)
            {
                request.Status = status;
                _dbContext.Update(request);
                _dbContext.SaveChanges();
            }            
        }

        public void AcceptVakansvakt(int vakansvaktRequestId, int userId)
        {
            var request = _dbContext.VakansvaktRequests.FirstOrDefault(v => v.Id == vakansvaktRequestId);
            if (request != null)
            {
                request.CoveredByLegeId = userId;
                request.Status = VakansvaktRequestStatus.Accepted;
                _dbContext.Update(request);
                _dbContext.SaveChanges();
            }
        }

        private static VakansvaktRequest CreateVakansvaktRequest(VakansvaktRequestEntity vakansvaktRequestEntity)
        {
            return new VakansvaktRequest(
                id: vakansvaktRequestEntity.Id,
                tjenesteplanId: vakansvaktRequestEntity.TjenesteplanId,
                requestedBy: $"{vakansvaktRequestEntity.OriginalLege.FirstName} {vakansvaktRequestEntity.OriginalLege.LastName}",
                requestedByUserId: vakansvaktRequestEntity.OriginalLege.Id,
                acceptedByUserId: vakansvaktRequestEntity.CoveredByLegeId,
                status: vakansvaktRequestEntity.Status,
                date: vakansvaktRequestEntity.Date,
                currentDagsplan: vakansvaktRequestEntity.CurrentDagsplan,
                requestedDagsplan: vakansvaktRequestEntity.RequestedDagsplan,
                reason: vakansvaktRequestEntity.Message
            );
        }
    }
}