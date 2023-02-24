using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tjenesteplan.Data.Contexts;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Data.Features.TjenesteplanChanges
{
    public class TjenesteplanChangesRepository : ITjenesteplanChangesRepository
    {
        private readonly DataContext _dbContext;

        public TjenesteplanChangesRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IReadOnlyList<TjenesteplanChange> GetTjenesteplanChanges(int tjenesteplanId, int userId)
        {
            var tjenesteplanChanges = _dbContext.TjenesteplanChanges
                .Where(t => t.TjenesteplanId == tjenesteplanId && t.UserId == userId)
                .ToList();

            return tjenesteplanChanges
                .OrderBy(c => c.ChangeDate)
                .Select(CreateTjenesteplanChange)
                .ToList();
        }

        public IReadOnlyList<TjenesteplanUserChange> GetTjenesteplanChanges(int tjenesteplanId)
        {
            var tjenesteplanChanges = _dbContext.TjenesteplanChanges
                .Include(t => t.User)
                .Where(t => t.TjenesteplanId == tjenesteplanId)
                .ToList();

            return tjenesteplanChanges
                .OrderByDescending(c => c.ChangeDate)
                .Select(CreateTjenesteplanUserChange)
                .ToList();
        }

        public int AddTjenesteplanChange(
            int userId, 
            int tjenesteplanId, 
            DateTime date, 
            DagsplanEnum dagsplan)
        {
            var change = new TjenesteplanChangeEntity
            {
                TjenesteplanId = tjenesteplanId,
                UserId = userId,
                Date = date,
                Dagsplan = dagsplan
            };

            _dbContext.TjenesteplanChanges.Add(change);
            _dbContext.SaveChanges();

            return change.Id;
        }

        public int AddTjenesteplanChange(
            int userId,
            int tjenesteplanId,
            int vaktChangeRequestId,
            DateTime date,
            DagsplanEnum dagsplan)
        {
            var change = new TjenesteplanChangeEntity
            {
                TjenesteplanId = tjenesteplanId,
                UserId = userId,
                VaktChangeRequestId = vaktChangeRequestId,
                Date = date,
                Dagsplan = dagsplan
            };

            _dbContext.TjenesteplanChanges.Add(change);
            _dbContext.SaveChanges();

            return change.Id;
        }

        public int AddTjenesteplanVakansvaktChange(
            int userId,
            int tjenesteplanId,
            int vakansvaktRequestId,
            DateTime date,
            DagsplanEnum dagsplan
        )
        {
            var change = new TjenesteplanChangeEntity
            {
                TjenesteplanId = tjenesteplanId,
                UserId = userId,
                VakansvaktRequestId = vakansvaktRequestId,
                Date = date,
                Dagsplan = dagsplan
            };

            _dbContext.TjenesteplanChanges.Add(change);
            _dbContext.SaveChanges();

            return change.Id;
        }

        public void UndoTjenesteplanVakansvaktChange(int tjenesteplanId, int vakansvaktRequestId)
        {
            var changes = _dbContext.TjenesteplanChanges
                .Where(c => c.TjenesteplanId == tjenesteplanId && c.VakansvaktRequestId == vakansvaktRequestId)
                .ToList();

            _dbContext.TjenesteplanChanges.RemoveRange(changes);
            _dbContext.SaveChanges();
        }

        public void UndoTjenesteplanVaktChange(int tjenesteplanId, int vaktChangeRequestId)
        {
            var changes = _dbContext.TjenesteplanChanges
                .Where(c => c.TjenesteplanId == tjenesteplanId && c.VaktChangeRequestId == vaktChangeRequestId)
                .ToList();

            _dbContext.TjenesteplanChanges.RemoveRange(changes);
            _dbContext.SaveChanges();
        }

        private static TjenesteplanUserChange CreateTjenesteplanUserChange(TjenesteplanChangeEntity entity)
        {
            return new TjenesteplanUserChange(
                userId: entity.User.Id, 
                fullname: entity.User.FirstName + " " + entity.User.LastName,
                change: CreateTjenesteplanChange(entity)
            );
        }

        private static TjenesteplanChange CreateTjenesteplanChange(TjenesteplanChangeEntity entity)
        {
            return new TjenesteplanChange(
                id: entity.Id,
                tjenesteplanId: entity.TjenesteplanId,
                changeDate: entity.ChangeDate,
                date: entity.Date,
                dagsplan: entity.Dagsplan,
                vakansvaktRequestId: entity.VakansvaktRequestId,
                vaktChangeRequestId: entity.VaktChangeRequestId

            );
        }
    }
}