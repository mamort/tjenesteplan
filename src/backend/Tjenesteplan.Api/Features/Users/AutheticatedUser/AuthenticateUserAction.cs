using System;
using System.Linq;
using Tjenesteplan.Api.Services.JwtToken;
using Tjenesteplan.Domain.Repositories;
using WebApi.Features.Users.Data;

namespace Tjenesteplan.Api.Features.Users.AutheticatedUser
{
    public class AuthenticateUserAction
    {
        private readonly IUserRepository userRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthenticateUserAction(
            IUserRepository userRepository,
            IJwtTokenService jwtTokenService)
        {
            this.userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        public AuthenticatedUserViewModel Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = userRepository.GetUserByUsername(username);

            if (user == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return new AuthenticatedUserViewModel(
                user: user,
                authToken: _jwtTokenService.CreateToken(user.Username)
            );
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            }

            if (storedHash.Length != 64)
            {
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(password));
            }

            if (storedSalt.Length != 128)
            {
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", nameof(storedSalt));
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                if (computedHash.Where((t, i) => t != storedHash[i]).Any())
                {
                    return false;
                }
            }

            return true;
        }
    }
}