using System.ComponentModel.DataAnnotations;

namespace Tjenesteplan.Api.Features.Users.AddUserToAvdeling
{
    public class AddUserToAvdelingModel
    {
        [Required]
        public int UserId { get; set; }
    }
}