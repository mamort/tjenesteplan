using WebApi.Features.Users.Data;

namespace Tjenesteplan.Data.Features.Avdelinger
{
    public class UserAvdelingEntity
    {
        public int UserId { get; set; }
        public int AvdelingId { get; set; }

        public UserEntity User { get; set; }
        public AvdelingEntity Avdeling { get; set; }
    }
}