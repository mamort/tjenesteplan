using Microsoft.AspNetCore.Mvc;

namespace Tjenesteplan.Api.Features.KeepAlive
{
    [Route("api/keepalive")]
    public class KeepAliveController : Controller
    {
        [Route("")]
        [HttpGet]
        public IActionResult KeepAlive()
        {
            return Ok();
        }

    }
}