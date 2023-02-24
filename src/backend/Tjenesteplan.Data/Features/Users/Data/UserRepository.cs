using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tjenesteplan.Data.Contexts;
using Tjenesteplan.Data.Features.Avdelinger;
using Tjenesteplan.Data.Features.Tjenesteplan.Data;
using Tjenesteplan.Data.Transaction;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;
using WebApi.Features.Users.Data;

namespace Tjenesteplan.Data.Features.Users.Data
{
    public class UserRepository : AbstractRepository, INonCachedUserRepository
    {
        private readonly DataContext _dbContext;

        public UserRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public bool UserExists(string username)
        {
            return _dbContext.Users.Any(u => u.Username == username);
        }

        public User GetUserByUsername(string username)
        {
            var userEntity = _dbContext.Users
                .Include(u => u.Avdelinger)
                .Include(u => u.Tjenesteplaner)
                .SingleOrDefault(x => x.Username == username);

            return userEntity == null 
                ? null 
                : CreateUser(userEntity);
        }

        public User GetUserById(int id)
        {
            var userEntity = _dbContext.Users
                .Include(u => u.Avdelinger)
                .Include(u => u.Tjenesteplaner)
                .FirstOrDefault(u => u.Id == id);

            return userEntity == null 
                ? null
                : CreateUser(userEntity);
        }

        public IReadOnlyList<User> GetAllUsers()
        {
            return _dbContext.Users
                .Include(u => u.Avdelinger)
                .Include(u => u.Tjenesteplaner)
                .ToList()
                .Select(CreateUser)
                .ToList();
        }

        public IReadOnlyList<User> GetUsersByAvdelinger(IReadOnlyList<int> avdelinger)
        {
            return _dbContext.Users
                .Include(u => u.Avdelinger)
                .Include(u => u.Tjenesteplaner)
                .Where(u => u.Avdelinger.Any(a => avdelinger.Contains(a.AvdelingId)))
                .ToList()
                .Select(CreateUser)
                .ToList();
        }

        public void UpdateUser(
            int id,
            string firstName,
            string lastName,
            LegeSpesialitet spesialitet,
            string username,
            byte[] passwordHash,
            byte[] passwordSalt
            )
        {
            var userEntity = _dbContext.Users.FirstOrDefault(u => u.Id == id);

            if (userEntity == null)
            {
                throw new InvalidOperationException($"Cannot find user with id {id}");
            }

            userEntity.FirstName = firstName;
            userEntity.LastName = lastName;
            userEntity.SpesialitetId = spesialitet?.Id;
            userEntity.Username = username;
            userEntity.PasswordHash = passwordHash;
            userEntity.PasswordSalt = passwordSalt;

            _dbContext.Users.Update(userEntity);
            _dbContext.SaveChanges();
        }

        public void UpdateUserRole(int id, Role role)
        {
            var userEntity = _dbContext.Users.FirstOrDefault(u => u.Id == id);

            if (userEntity == null)
            {
                throw new InvalidOperationException($"Cannot find user with id {id}");
            }

            userEntity.Role = role;

            _dbContext.Users.Update(userEntity);
            _dbContext.SaveChanges();
        }

        public void AddUserToAvdeling(int userId, int avdelingId)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new InvalidOperationException($"Cannot find user with id {userId}");
            }

            var avdeling = _dbContext.Avdelinger.FirstOrDefault(u => u.Id == avdelingId);

            if (avdeling == null)
            {
                throw new InvalidOperationException($"Cannot find avdeling with id {avdelingId}");
            }

            if (user.Avdelinger == null)
            {
                user.Avdelinger = new List<UserAvdelingEntity>();
            }
            user.Avdelinger.Add(new UserAvdelingEntity { AvdelingId = avdelingId, UserId = userId });

            _dbContext.Update(user);
            _dbContext.SaveChanges();
        }

        public void RemoveUserFromAvdeling(int avdelingId, int userId)
        {
            var tjenesteplaner =
                _dbContext.Tjenesteplaner.Where(t =>
                    t.AvdelingId == avdelingId && t.Leger.Any(l => l.UserId == userId)
                ).ToList();

            foreach (var tjenesteplan in tjenesteplaner)
            {
                RemoveLegeFromTjenesteplan(tjenesteplan.Id, userId);
            }

            var user = _dbContext.Users
                .Include(u => u.Avdelinger)
                .FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                var avdeling = user.Avdelinger.FirstOrDefault(a => a.AvdelingId == avdelingId);
                if (avdeling != null)
                {
                    user.Avdelinger.Remove(avdeling);
                    _dbContext.Update(user);
                    _dbContext.SaveChanges();
                }
            }
        }

        public int CreateUser(
            string firstName,
            string lastName,
            string username,
            byte[] passwordHash,
            byte[] passwordSalt,
            Role role
        )
        {
            var userEntity = new UserEntity
            {
                FirstName = firstName,
                LastName = lastName,
                Username = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = role
            };

            _dbContext.Users.Add(userEntity);
            _dbContext.SaveChanges();

            return userEntity.Id;
        }

        public IReadOnlyList<User> GetUsersByTjenesteplan(int tjenesteplanId)
        {
            return _dbContext.Users
                .Include(u => u.Avdelinger)
                .Include(u => u.Tjenesteplaner)
                .Where(u => u.Tjenesteplaner.Any(t => t.TjenesteplanId == tjenesteplanId))
                .ToList()
                .Select(CreateUser)
                .ToList();
        }

        public void DeleteUser(int id)
        {
            var user = _dbContext.Users.Find(id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
            }
        }

        public void AssignLegeToTjenesteplan(int userId, int tjenesteplanId)
        {
            var user = _dbContext.Users
                .Include(t => t.Tjenesteplaner)
                .FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception($"Could not find user with id {userId}");
            }

            user.Tjenesteplaner.Add(new UserTjenesteplanEntity {TjenesteplanId = tjenesteplanId, UserId = userId});

            _dbContext.Update(user);
            _dbContext.SaveChanges();
        }

        public void RemoveLegeFromTjenesteplan(int tjenesteplanId, int userId)
        {
            var user = _dbContext.Users
                .Include(u => u.Tjenesteplaner)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new Exception($"Could not find user with id {userId}");
            }

            var tjenesteplan = user.Tjenesteplaner.FirstOrDefault(t => t.TjenesteplanId == tjenesteplanId);

            if (tjenesteplan == null)
            {
                throw new Exception($"User does not belong to tjenesteplan with id {tjenesteplanId}");
            }

            user.Tjenesteplaner.Remove(tjenesteplan);
            
            _dbContext.Update(user);
            _dbContext.SaveChanges();
        }

        public void SetResetPasswordToken(string email, string token)
        {
            var userEntity = _dbContext.Users.SingleOrDefault(x => x.Username == email);
            if (userEntity == null)
            {
                throw new Exception($"Could not find user with email: {email}");
            }
            userEntity.ResetPasswordToken = token;
            _dbContext.SaveChanges();
        }

        public User GetUserByNewPasswordToken(Guid token)
        {
            var userEntity = _dbContext.Users
                .Include(u => u.Avdelinger)
                .Include(u => u.Tjenesteplaner)
                .SingleOrDefault(u => u.ResetPasswordToken == token.ToString());

            if (userEntity == null)
            {
                return null;
            }

            return CreateUser(userEntity);
        }

        private static User CreateUser(UserEntity userEntity)
        {
            var legeSpesialitet = LegeSpesialitet.Spesialiteter.FirstOrDefault(s => s.Id == userEntity.SpesialitetId);
            return new User(
                id: userEntity.Id,
                firstName: userEntity.FirstName,
                lastName: userEntity.LastName,
                spesialitet: legeSpesialitet,
                username: userEntity.Username,
                passwordHash: userEntity.PasswordHash,
                passwordSalt: userEntity.PasswordSalt,
                role: userEntity.Role,
                avdelinger: userEntity.Avdelinger.Select(a => a.AvdelingId).ToList(),
                tjenesteplaner: userEntity.Tjenesteplaner.Select(t => t.TjenesteplanId).ToList()
            );
        }

    }
}