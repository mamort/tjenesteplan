using System.Collections.Generic;

namespace Tjenesteplan.Api.Features.Users.ViewUser
{
    public class BasicUserInfoModel
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public IReadOnlyList<int> Avdelinger { get; set; }
    }
}