using Tjenesteplan.Domain;

namespace WebApi.Features.Users.RegisterUser
{
    public class NewUser
    {
        public string FirstName { get;}
        public string LastName { get;}
        public string Username { get; }
        public LegeSpesialitet LegeSpesialitet { get; }
        public string Password { get; }

        public Role Role { get; }

        public NewUser(
            string firstName, 
            string lastName, 
            string username, 
            string password,
            LegeSpesialitet legeSpesialitet,
            Role role
        )
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username.ToLower();
            Password = password;
            Role = role;
            LegeSpesialitet = legeSpesialitet;
        }
    }
}