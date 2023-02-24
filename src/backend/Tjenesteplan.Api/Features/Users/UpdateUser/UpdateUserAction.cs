using Tjenesteplan.Api.Exceptions;
using Tjenesteplan.Api.Services.PasswordHash;
using Tjenesteplan.Domain.Repositories;
using WebApi.Features.Users.UpdateUser;

namespace Tjenesteplan.Api.Features.Users.UpdateUser
{
    public class UpdateUserAction
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashService _passwordHashService;

        public UpdateUserAction(
            IUserRepository userRepository,
            IPasswordHashService passwordHashService)
        {
            _userRepository = userRepository;
            _passwordHashService = passwordHashService;
        }

        public void Update(UpdatedUser updatedUser)
        {
            var user = _userRepository.GetUserById(updatedUser.Id);

            if (user == null)
            {
                throw new AppException($"User with id {updatedUser.Id} not found");
            }
                
            if (updatedUser.Username != user.Username)
            {
                // username has changed so check if the new username is already taken
                var existingUser = _userRepository.GetUserByUsername(updatedUser.Username);
                if (existingUser != null)
                {
                    throw new AppException($"Username {updatedUser.Username} is already taken");
                }                
            }

            if (!string.IsNullOrWhiteSpace(updatedUser.Password))
            {
                var hashSalt = _passwordHashService.CreatePasswordHash(updatedUser.Password);

                _userRepository.UpdateUser(
                    id: updatedUser.Id,
                    firstname: updatedUser.FirstName ?? user.FirstName,
                    lastname: updatedUser.LastName ?? user.LastName,
                    spesialitet: user.Spesialitet,
                    username: updatedUser.Username ?? user.Username,
                    passwordHash: hashSalt.PasswordHash,
                    passwordSalt: hashSalt.PasswordSalt
                );
            }
            else
            {
                _userRepository.UpdateUser(
                    id: updatedUser.Id,
                    firstname: updatedUser.FirstName ?? user.FirstName,
                    lastname: updatedUser.LastName ?? user.LastName,
                    spesialitet: user.Spesialitet,
                    username: updatedUser.Username ?? user.Username,
                    passwordHash: user.PasswordHash,
                    passwordSalt: user.PasswordSalt
                );
            }


        }
    }
}