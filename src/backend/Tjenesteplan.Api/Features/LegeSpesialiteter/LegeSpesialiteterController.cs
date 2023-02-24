using Microsoft.AspNetCore.Mvc;
using Tjenesteplan.Api.Features;
using Tjenesteplan.Api.Filters;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.LegeSpesialiteter
{
    [Route("api/lege-spesialiteter")]
    public class LegeSpesialiteterController : Controller
    {

        [ResponseCache(Duration = 60 * 10)]
        [Route("")]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(LegeSpesialitet.Spesialiteter);
        }

    }
}