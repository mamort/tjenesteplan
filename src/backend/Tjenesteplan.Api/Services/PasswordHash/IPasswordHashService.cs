namespace Tjenesteplan.Api.Services.PasswordHash
{
    public interface IPasswordHashService
    {
        PasswordHashSalt CreatePasswordHash(string password);
    }
}