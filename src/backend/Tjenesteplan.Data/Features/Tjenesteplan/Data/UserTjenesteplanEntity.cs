using WebApi.Features.Users.Data;

namespace Tjenesteplan.Data.Features.Tjenesteplan.Data
{
    public class UserTjenesteplanEntity
    {
        public int TjenesteplanId { get; set; }
        public int UserId { get; set; }
        
        public TjenesteplanEntity Tjenesteplan { get; set; }
        public UserEntity User { get; set; }
    }
}