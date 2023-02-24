using Microsoft.AspNetCore.Mvc;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Filters
{
    public class RoleAuthorizationAttribute : TypeFilterAttribute
    {
        public RoleAuthorizationAttribute(params Role[] roles) : base(typeof(RoleAuthorizationFilter))
        {
            Arguments = new object[] { roles };
        }
    }
}