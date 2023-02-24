using Tjenesteplan.Api.Exceptions;
using Tjenesteplan.Api.Services.PasswordHash;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.Users.NewPassword
{
    public class NewPasswordAction
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashService _passwordHashService;

        public NewPasswordAction(
            IUserRepository userRepository, 
            IPasswordHashService passwordHashService
        )
        {
            _userRepository = userRepository;
            _passwordHashService = passwordHashService;
        }

        public void Execute(NewPasswordModel model)
        {
            var user = _userRepository.GetUserByNewPasswordToken(model.Token);
            if (user == null)
            {
                throw new AppException("Invalid token");
            }

            var hashSalt = _passwordHashService.CreatePasswordHash(model.Password);

            _userRepository.UpdateUser(
                id: user.Id,
                firstname: user.FirstName,
                lastname: user.LastName,
                username: user.Username,
                spesialitet: user.Spesialitet,
                passwordHash: hashSalt.PasswordHash,
                passwordSalt: hashSalt.PasswordSalt
            );
        }
    }
}