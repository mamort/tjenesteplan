using System.ComponentModel.DataAnnotations;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.UserInvitations.RegisterInvitation
{
    public class RegisterInvitationModel
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public int AvdelingId { get; set; }
    }
}