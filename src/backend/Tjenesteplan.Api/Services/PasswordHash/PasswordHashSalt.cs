namespace Tjenesteplan.Api.Services.PasswordHash
{
    public class PasswordHashSalt
    {
        public byte[] PasswordHash { get; }
        public byte[] PasswordSalt { get; }

        public PasswordHashSalt(byte[] passwordHash, byte[] passwordSalt)
        {
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }
    }
}