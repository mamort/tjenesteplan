using System;
using System.Linq;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;
using WebApi.Features.Users.Data;

namespace Tjenesteplan.Api.Features.Users.ViewUser
{
    public class GetUserAction
    {
        private readonly IUserRepository _userRepository;
        private readonly IAvdelingRepository _avdelingRepository;

        public GetUserAction(IUserRepository userRepository, IAvdelingRepository avdelingRepository)
        {
            _userRepository = userRepository;
            _avdelingRepository = avdelingRepository;
        }

        public UserViewModel GetUser(string username, int id)
        {
            var currentUser = _userRepository.GetUserByUsername(username);
            var user = _userRepository.GetUserById(id);

            if (user == null)
            {
                return null;
            }

            if (currentUser.Role == Role.Admin)
            {
                return CreateUser(user);
            }

            if (user.Avdelinger.Any(a => currentUser.Avdelinger.Any(a2 => a2 == a)))
            {
                return CreateUser(user);
            }

            if (currentUser.Role == Role.Overlege)
            {
                var avdelinger = _avdelingRepository.GetAllAvdelinger()
                    .Where(a => user.Avdelinger.Any(aid => aid == a.Id))
                    .ToList();

                if (avdelinger.Any(a => a.ListeforerId == currentUser.Id))
                {
                    return CreateUser(user);
                }
            }

            throw new UnauthorizedAccessException();
        }

        public UserViewModel GetUser(string username)
        {
            var user = _userRepository.GetUserByUsername(username);
            return user != null
                ? CreateUser(user)
                : null;
        }

        private UserViewModel CreateUser(User user)
        {
            return new UserViewModel(
                id: user.Id,
                firstname: user.FirstName,
                lastname: user.LastName,
                spesialitetId: user.Spesialitet?.Id,
                username: user.Username,
                email: user.Email,
                role: user.Role,
                avdelinger: user.Avdelinger,
                tjenesteplaner: user.Tjenesteplaner
            );
        }
    }
}