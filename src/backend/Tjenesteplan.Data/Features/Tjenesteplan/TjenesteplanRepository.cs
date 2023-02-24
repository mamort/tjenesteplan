using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tjenesteplan.Data.Contexts;
using Tjenesteplan.Data.Features.Avdelinger;
using Tjenesteplan.Data.Features.Tjenesteplan.Data;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;
using Tjenesteplan.Domain.Tjenesteplaner;

namespace Tjenesteplan.Data.Features.Tjenesteplan
{
    public class TjenesteplanRepository : ITjenesteplanRepository
    {
        private readonly DataContext _dbContext;

        public TjenesteplanRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IReadOnlyList<Domain.Tjenesteplan> GetTjenesteplanerCreatedByUser(string username)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                throw new Exception($"Could not find user with username {username}");
            }

            return _dbContext.Tjenesteplaner
                .Include(t => t.Avdeling)
                .Include(t => t.Leger)
                .Include(t => t.Weeks)
                .ThenInclude(w => w.Days)
                .Where(t => t.Avdeling.ListeforerId == user.Id || user.Role == Role.Admin)
                .Select(t => CreateTjenesteplan(t.Avdeling, t))
                .ToList();
        }

        public IReadOnlyList<Domain.Tjenesteplan> GetTjenesteplanerForUser(int userId)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception($"Could not find user with id {userId}");
            }

            return _dbContext.Tjenesteplaner
                .Include(t => t.Avdeling)
                .Include(t => t.Leger)
                .Include(t => t.Weeks)
                .ThenInclude(w => w.Days)
                .Where(t => t.Leger.Any(l => l.UserId == userId))
                .Select(t => CreateTjenesteplan(t.Avdeling, t))
                .ToList();
        }

        public Domain.Tjenesteplan GetTjenesteplanById(int id)
        {
            var tjenesteplan = _dbContext.Tjenesteplaner
                .Include(t => t.Avdeling)
                .Include(t => t.Leger)
                .Include(t => t.Weeks)
                .ThenInclude(w => w.Days)
                .FirstOrDefault(t => t.Id == id);

            if (tjenesteplan == null)
            {
                return null;
            }

            return CreateTjenesteplan(tjenesteplan.Avdeling, tjenesteplan);
        }

        public bool IsUserListeforerForTjenesteplan(int userId, int tjenesteplanId)
        {
            var tjenesteplan = _dbContext.Tjenesteplaner
                .Include(t => t.Avdeling)
                .FirstOrDefault(t => t.Id == tjenesteplanId);

            if (tjenesteplan == null)
            {
                throw new Exception($"Could not find tjenesteplan with id {tjenesteplanId}");
            }

            return tjenesteplan.Avdeling?.ListeforerId == userId;
        }

        public int CreateTjenesteplan(NewTjenesteplan tjenesteplan)
        {
            var tjenesteplanUkeId = 1;
            var tjenesteplanEntity = new TjenesteplanEntity
            {
                Name = tjenesteplan.Name,
                AvdelingId = tjenesteplan.AvdelingId,
                StartDate = tjenesteplan.StartDate,
                NumberOfWeeks = tjenesteplan.Weeks.Count,
                Weeks = tjenesteplan.Weeks.Select(w => new TjenesteplanUkeEntity
                {
                    TjenesteplanUkeId = tjenesteplanUkeId++,
                    Days =  w.Days.Select(d => new TjenesteplanUkedagEntity
                    {
                        Date = d.Date,
                        Dagsplan = d.Dagsplan
                    }).ToList()
                }).ToList()
            };

            _dbContext.Tjenesteplaner.Add(tjenesteplanEntity);
            _dbContext.SaveChanges();

            return tjenesteplanEntity.Id;
        }
        public void EditTjenesteplan(EditTjenesteplan editTjenesteplan)
        {
            var tjenesteplan = _dbContext.Tjenesteplaner
                .Include(t => t.Weeks)
                .ThenInclude(w => w.Days)
                .FirstOrDefault(t => t.Id == editTjenesteplan.Id);

            if (tjenesteplan != null)
            {
                tjenesteplan.Name = editTjenesteplan.Name;
                tjenesteplan.StartDate = editTjenesteplan.StartDate;
                tjenesteplan.NumberOfWeeks = editTjenesteplan.Weeks.Count;

                foreach (var week in tjenesteplan.Weeks)
                {
                    var editWeek = editTjenesteplan.Weeks.FirstOrDefault(w => w.Id == week.TjenesteplanUkeId);
                    if (editWeek != null)
                    {
                        week.UserId = editWeek.LegeId;
                        EditAndRemoveDays(week, editWeek);
                        AddNewDays(editWeek, week);
                    }
                }
            }

            _dbContext.SaveChanges();
        }

        private void EditAndRemoveDays(TjenesteplanUkeEntity week, EditTjenesteUke editWeek)
        {
            foreach (var day in week.Days)
            {
                var editDay = editWeek.Days.FirstOrDefault(d => d.Date.Date == day.Date.Date);
                if (editDay != null)
                {
                    day.Dagsplan = editDay.Dagsplan;
                }
                else
                {
                    _dbContext.Remove(day);
                }
            }
        }

        private static void AddNewDays(EditTjenesteUke editWeek, TjenesteplanUkeEntity week)
        {
            foreach (var day in editWeek.Days)
            {
                if (!week.Days.Any(d => d.Date.Date == day.Date.Date))
                {
                    week.Days.Add(new TjenesteplanUkedagEntity
                    {
                        Date = day.Date,
                        Dagsplan = day.Dagsplan,
                    });
                }
            }
        }

        public void AssignLegeToWeek(int tjenesteplanId, int weekId, int userId)
        {
            var tjenesteplan = _dbContext.Tjenesteplaner
                .Include(t => t.Weeks)
                .FirstOrDefault(t => t.Id == tjenesteplanId);

            if (tjenesteplan == null)
            {
                throw new Exception($"Could not find tjenesteplan with id {tjenesteplanId}");
            }

            var week = tjenesteplan.Weeks.FirstOrDefault(w => w.TjenesteplanUkeId == weekId);

            if (week == null)
            {
                throw new Exception($"Could not find week with id {weekId}");
            }

            week.UserId = userId;

            _dbContext.Update(week);
            _dbContext.SaveChanges();
        }

        public void RemoveLegeFromTjenesteplanWeeK(int tjenesteplanId, int userId)
        {
            var tjenesteplan = _dbContext.Tjenesteplaner
                .Include(t => t.Weeks)
                .FirstOrDefault(t => t.Id == tjenesteplanId);

            if (tjenesteplan == null)
            {
                throw new Exception($"Could not find tjenesteplan with id {tjenesteplanId}");
            }

            foreach (var week in tjenesteplan.Weeks)
            {
                if (week.UserId == userId)
                {
                    week.UserId = null;;
                    _dbContext.Update(week);
                }
            }

            _dbContext.SaveChanges();
        }

        public void RemoveUserFromTjenesteplaner(int userId)
        {
            var tjenesteplaner = _dbContext.Tjenesteplaner
                .Include(t => t.Weeks)
                .Where(t => t.Weeks.Any(w => w.UserId == userId));

            foreach (var tjenesteplan in tjenesteplaner)
            {
                foreach (var week in tjenesteplan.Weeks)
                {
                    if (week.UserId == userId)
                    {
                        week.UserId = null; ;
                        _dbContext.Update(week);
                    }
                }
            }

            var changesForUser = _dbContext.TjenesteplanChanges.Where(tc => tc.UserId == userId);
            foreach (var change in changesForUser)
            {
                _dbContext.TjenesteplanChanges.Remove(change);
            }

            _dbContext.SaveChanges();
        }

        private static Domain.Tjenesteplan CreateTjenesteplan(AvdelingEntity avdeling, TjenesteplanEntity entity)
        {
            return new Domain.Tjenesteplan
            {
                Id = entity.Id,
                Name = entity.Name,
                Start = entity.StartDate,
                AvdelingId = avdeling.Id,
                ListeførerId = avdeling.ListeforerId,
                Uker = entity.Weeks.Select(w => new TjenesteUke
                {
                    Id = w.TjenesteplanUkeId,
                    UserId = w.UserId,
                    Dager = w.Days.Select(d => new TjenesteDag(          
                        dagsplan: d.Dagsplan,
                        date: d.Date.Date
                    )).ToList()
                }).ToList()
            };
        }
    }
}