using System.Collections.Generic;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.Users.ViewUser
{
    public class UserViewModel
    {
        public int Id { get; }
        public string Firstname { get; }
        public string Lastname { get; }
        public string Fullname => $"{Firstname} {Lastname}";
        public string Username { get; }
        public string Email { get; }
        public int? SpesialitetId { get; }
        public Role Role { get; }
        public IReadOnlyList<int> Avdelinger { get; }
        public IReadOnlyList<int> Tjenesteplaner { get; }

        public UserViewModel(
            int id,
            string firstname,
            string lastname,
            string username,
            string email,
            int? spesialitetId,
            Role role,
            IReadOnlyList<int> avdelinger,
            IReadOnlyList<int> tjenesteplaner
        )
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Username = username;
            Email = email;
            SpesialitetId = spesialitetId;
            Role = role;
            Avdelinger = avdelinger;
            Tjenesteplaner = tjenesteplaner;
        }
    }
}