using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;
using WebApi.Features.Users.Data;

namespace Tjenesteplan.Api.Filters
{
    public class RoleAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IList<Role> _roles;

        public RoleAuthorizationFilter(IList<Role> roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var username = context.HttpContext.User.Identity.Name;

            var userRepository = context.HttpContext.RequestServices.GetService<IUserRepository>();
            var user = userRepository.GetUserByUsername(username);

            if (!_roles.Any(r => r == user.Role))
            {
                context.Result = new ForbidResult();
            }
        }
    }

}