using System.Collections.Generic;
using System.Linq;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.Users.ViewUser
{
    public class GetLegerAction
    {
        private readonly IUserRepository _userRepository;
        private readonly ISykehusRepository _sykehusRepository;

        public GetLegerAction(IUserRepository userRepository, ISykehusRepository sykehusRepository)
        {
            _userRepository = userRepository;
            _sykehusRepository = sykehusRepository;
        }

        public IReadOnlyList<UserViewModel> GetLegerInSameAvdeling(string username)
        {
            var currentUser = _userRepository.GetUserByUsername(username);

            if (currentUser.Role == Role.Admin)
            {
                return
                    _userRepository
                        .GetAllUsers()
                        .Where(u => (u.Role == Role.Lege || u.Role == Role.Overlege))
                        .Select(CreateUser)
                        .ToList();
            }

            if (currentUser.Role == Role.Overlege)
            {
                var listeforerAvdelinger = _sykehusRepository
                    .GetSykehus()
                    .SelectMany(s => s.Avdelinger)
                    .Where(a => a.ListeforerId == currentUser.Id)
                    .ToList();

                var avdelinger = listeforerAvdelinger
                    .Select(a => a.Id)
                    .Concat(currentUser.Avdelinger)
                    .Distinct()
                    .ToList();

                return
                    _userRepository
                        .GetUsersByAvdelinger(avdelinger)
                        .Where(u => (u.Role == Role.Lege || u.Role == Role.Overlege))
                        .Select(CreateUser)
                        .ToList();
            }

            return _userRepository
                .GetUsersByAvdelinger(currentUser.Avdelinger)
                .Where(u => u.Role == Role.Lege || u.Role == Role.Overlege)
                .Select(CreateUser)
                .ToList();
        }

        public IReadOnlyList<BasicUserInfoModel> GetAlleLeger(string identityName)
        {
            return
                _userRepository
                    .GetAllUsers()
                    .Where(u => (u.Role == Role.Lege || u.Role == Role.Overlege))
                    .Select(u => new BasicUserInfoModel { Id = u.Id, Fullname = u.Fullname, Avdelinger = u.Avdelinger })
                    .ToList();
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