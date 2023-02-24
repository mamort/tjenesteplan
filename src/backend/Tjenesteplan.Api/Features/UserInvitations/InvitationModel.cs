using System;

namespace Tjenesteplan.Api.Features.UserInvitations
{
    public class InvitationModel
    {
        public string Email { get; set; }
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int? LegeSpesialitetId { get; set; }
    }
}