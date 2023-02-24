using System;
using Tjenesteplan.Data.Features.Avdelinger;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Data.Features.Invitations
{
    public class InvitationEntity
    {
        public Guid Guid { get; set; }

        public int AvdelingId { get; set; }

        public AvdelingEntity Avdeling { get; set; }

        public string Email { get; set; }

        public Role Role { get; set; }
    }
}