using System.Collections.Generic;
using Tjenesteplan.Data.Features.Avdelinger;
using Tjenesteplan.Data.Features.Tjenesteplan.Data;
using Tjenesteplan.Data.Features.VaktChangeRequests;
using Tjenesteplan.Domain;

namespace WebApi.Features.Users.Data
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? SpesialitetId { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public Role Role { get; set; }
        public string ResetPasswordToken { get; set; }

        public ICollection<UserAvdelingEntity> Avdelinger { get; set; }
        
        public ICollection<UserTjenesteplanEntity> Tjenesteplaner { get; set; }

        public ICollection<VaktChangeRequestEntity> VaktChangeRequests { get; set; }
    }
}