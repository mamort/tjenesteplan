using System;

namespace Tjenesteplan.Domain
{
    public class Invitation
    {
        public Guid Guid { get; }
        public string Email { get; }
        public Role Role { get; }

        public Invitation(Guid guid, string email, Role role)
        {
            Guid = guid;
            Email = email;
            Role = role;
        }
    }
}