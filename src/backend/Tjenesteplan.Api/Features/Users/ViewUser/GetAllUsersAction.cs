using System.Collections.Generic;
using System.Linq;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;
using WebApi.Features.Users.Data;

namespace Tjenesteplan.Api.Features.Users.ViewUser
{
    public class GetAllUsersAction
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersAction(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IReadOnlyList<UserViewModel> GetAllUsers()
        {
            var users = _userRepository.GetAllUsers();
            return users.Select(CreateUser).ToList();
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