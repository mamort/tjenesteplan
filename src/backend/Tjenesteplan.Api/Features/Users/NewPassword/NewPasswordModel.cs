using System;
using System.ComponentModel.DataAnnotations;

namespace Tjenesteplan.Api.Features.Users.NewPassword
{
    public class NewPasswordModel
    {
        [Required]
        public Guid Token { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}