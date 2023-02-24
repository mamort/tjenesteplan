namespace Tjenesteplan.Api.Services.JwtToken
{
    public interface IJwtTokenService
    {
        string CreateToken(string userId);
    }
}