using Microsoft.AspNetCore.Mvc;
using Tjenesteplan.Api.Features.MinTjenesteplan.GetMineTjenesteplaner;
using Tjenesteplan.Api.Features.MinTjenesteplan.GetMinTjenesteplan;
using Tjenesteplan.Api.Filters;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.MinTjenesteplan
{
    [Route("api")]
    public class MinTjenesteplanController : BaseController
    {
        private readonly GetMineTjenesteplanerAction _getMineTjenesteplanerAction;
        private readonly GetMinTjenesteplanAction _getMinTjenesteplanAction;

        public MinTjenesteplanController(
            GetMineTjenesteplanerAction getMineTjenesteplanerAction,
            GetMinTjenesteplanAction getMinTjenesteplanAction)
        {
            _getMineTjenesteplanerAction = getMineTjenesteplanerAction;
            _getMinTjenesteplanAction = getMinTjenesteplanAction;
        }

        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [Route("minetjenesteplaner")]
        [HttpGet]
        public IActionResult MineTjenesteplaner()
        {
            var username = HttpContext.User.Identity.Name;
            var tjenesteplaner = _getMineTjenesteplanerAction.Execute(username);
            return Ok(tjenesteplaner);
        }

        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [Route("minetjenesteplaner/{tjenesteplanid:int}")]
        [HttpGet]
        public IActionResult MinTjenesteplan(int tjenesteplanId)
        {
            var username = HttpContext.User.Identity.Name;
            var tjenesteplan = _getMinTjenesteplanAction.Execute(tjenesteplanId, username);
            if (tjenesteplan == null)
            {
                return NotFound();
            }

            return Ok(tjenesteplan);
        }
    }
}