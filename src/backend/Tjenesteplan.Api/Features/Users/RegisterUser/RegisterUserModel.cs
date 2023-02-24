using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.Users.RegisterUser
{
    public class RegisterUserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public int LegeSpesialitetId { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}