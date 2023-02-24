using System;
using System.Security.Cryptography;
using System.Text;

namespace Tjenesteplan.Api.Services.PasswordHash
{
    public class PasswordHashService : IPasswordHashService
    {
        public PasswordHashSalt CreatePasswordHash(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(
                    "Value cannot be empty or whitespace only string.", 
                    nameof(password)
                );
            }

            using (var hmac = new HMACSHA512())
            {
                var salt = hmac.Key;
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                return new PasswordHashSalt (
                    passwordHash: hash,
                    passwordSalt: salt
                );
            }
        }
    }
}