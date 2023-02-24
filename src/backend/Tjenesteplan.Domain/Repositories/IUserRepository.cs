using System;
using System.Collections.Generic;

namespace Tjenesteplan.Domain.Repositories
{
    public interface INonCachedUserRepository : IUserRepository
    {
    }

    public interface IUserRepository : IRepository
    {
        bool UserExists(string username);
        User GetUserByUsername(string username);
        User GetUserById(int id);
        IReadOnlyList<User> GetAllUsers();
        IReadOnlyList<User> GetUsersByAvdelinger(IReadOnlyList<int> avdelinger);

        void UpdateUser(
            int id,
            string firstname,
            string lastname,
            LegeSpesialitet spesialitet,
            string username,
            byte[] passwordHash,
            byte[] passwordSalt
        );
        void DeleteUser(int id);
        int CreateUser(
            string firstName,
            string lastName,
            string username,
            byte[] passwordHash,
            byte[] passwordSalt,
            Role role
        );

        IReadOnlyList<User> GetUsersByTjenesteplan(int tjenesteplanId);
        void AssignLegeToTjenesteplan(int userId, int tjenesteplanId);
        void RemoveLegeFromTjenesteplan(int tjenesteplanId, int userId);
        void SetResetPasswordToken(string email, string token);
        User GetUserByNewPasswordToken(Guid token);

        void UpdateUserRole(int userId, Role role);
        void AddUserToAvdeling(int userId, int avdelingId);
        void RemoveUserFromAvdeling(int avdelingId, int userId);
    }
}