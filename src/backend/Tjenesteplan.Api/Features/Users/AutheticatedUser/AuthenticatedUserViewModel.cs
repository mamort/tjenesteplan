using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.Users.AutheticatedUser
{
    public class AuthenticatedUserViewModel
    {
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Username { get; }
        public Role Role { get; }
        public string AuthToken { get; }

        public AuthenticatedUserViewModel(User user, string authToken)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
            Role = user.Role;
            AuthToken = authToken;
        }
    }
}