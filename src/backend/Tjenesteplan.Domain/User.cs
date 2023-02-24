using System.Collections.Generic;

namespace Tjenesteplan.Domain
{
    public class User
    {
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Fullname => $"{FirstName} {LastName}";
        public string Username { get; }
        public string Email => Username;
        public byte[] PasswordHash { get; }
        public byte[] PasswordSalt { get; }

        public Role Role { get; }
        public IReadOnlyList<int> Avdelinger { get; }
        public IReadOnlyList<int> Tjenesteplaner { get; }
        public LegeSpesialitet Spesialitet { get;}

        public User(
            int id, 
            string firstName, 
            string lastName,
            LegeSpesialitet spesialitet,
            string username, 
            byte[] passwordHash, 
            byte[] passwordSalt, 
            Role role,
            IReadOnlyList<int> avdelinger,
            IReadOnlyList<int> tjenesteplaner)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            Role = role;
            Avdelinger = avdelinger;
            Tjenesteplaner = tjenesteplaner;
            Spesialitet = spesialitet;
        }
    }
}