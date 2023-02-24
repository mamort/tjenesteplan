using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Data.Features.Users
{
    public class CachedUserRepository : IUserRepository
    {
        private const string AllUsersCacheKey = "all-users";
        private readonly INonCachedUserRepository _userRepository;
        private readonly IMemoryCache _memoryCache;

        public CachedUserRepository(INonCachedUserRepository userRepository, IMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            _memoryCache = memoryCache;
        }

        public bool UserExists(string username)
        {
            return _userRepository.UserExists(username);
        }

        public User GetUserByUsername(string username)
        {
            return GetAllUsers().FirstOrDefault(u => u.Username == username);
        }

        public User GetUserById(int id)
        {
            return GetAllUsers().FirstOrDefault(u => u.Id == id);
        }

        public IReadOnlyList<User> GetAllUsers()
        {
            return _memoryCache.GetOrCreate(AllUsersCacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return _userRepository.GetAllUsers();
            });
        }

        public IReadOnlyList<User> GetUsersByAvdelinger(IReadOnlyList<int> avdelinger)
        {
            return GetAllUsers().Where(u => u.Avdelinger.Any(avdelinger.Contains)).ToList();
        }

        public void UpdateUser(int id, string firstname, string lastname, LegeSpesialitet spesialitet, string username,
            byte[] passwordHash, byte[] passwordSalt)
        {
            _userRepository.UpdateUser(id, firstname, lastname, spesialitet, username, passwordHash, passwordSalt);
            _memoryCache.Remove(AllUsersCacheKey);
        }

        public void DeleteUser(int id)
        {
            _userRepository.DeleteUser(id);
            _memoryCache.Remove(AllUsersCacheKey);
        }

        public int CreateUser(string firstName, string lastName, string username, byte[] passwordHash, byte[] passwordSalt, Role role)
        {
            var user = _userRepository.CreateUser(firstName, lastName, username, passwordHash, passwordSalt, role);
            _memoryCache.Remove(AllUsersCacheKey);
            return user;
        }

        public IReadOnlyList<User> GetUsersByTjenesteplan(int tjenesteplanId)
        {
            return GetAllUsers()
                .Where(u => u.Tjenesteplaner.Any(id => id == tjenesteplanId))
                .ToList();
        }

        public void AssignLegeToTjenesteplan(int userId, int tjenesteplanId)
        {
            _userRepository.AssignLegeToTjenesteplan(userId, tjenesteplanId);
            _memoryCache.Remove(AllUsersCacheKey);
        }

        public void RemoveLegeFromTjenesteplan(int tjenesteplanId, int userId)
        {
            _userRepository.RemoveLegeFromTjenesteplan(tjenesteplanId, userId);
            _memoryCache.Remove(AllUsersCacheKey);
        }

        public void SetResetPasswordToken(string email, string token)
        {
            _userRepository.SetResetPasswordToken(email, token);
            _memoryCache.Remove(AllUsersCacheKey);
        }

        public User GetUserByNewPasswordToken(Guid token)
        {
            return _userRepository.GetUserByNewPasswordToken(token);
        }

        public void UpdateUserRole(int userId, Role role)
        {
            _userRepository.UpdateUserRole(userId, role);
            _memoryCache.Remove(AllUsersCacheKey);
        }

        public void AddUserToAvdeling(int userId, int avdelingId)
        {
            _userRepository.AddUserToAvdeling(userId, avdelingId);
            _memoryCache.Remove(AllUsersCacheKey);
        }

        public void RemoveUserFromAvdeling(int avdelingId, int userId)
        {
            _userRepository.RemoveUserFromAvdeling(avdelingId, userId);
            _memoryCache.Remove(AllUsersCacheKey);
        }

        public IRepositoryTransaction BeginTransaction(IsolationLevel isolation = IsolationLevel.RepeatableRead)
        {
            return _userRepository.BeginTransaction(isolation);
        }
    }
}