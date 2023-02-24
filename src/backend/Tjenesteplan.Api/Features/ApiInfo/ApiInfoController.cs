using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tjenesteplan.Api.Configuration;

namespace Tjenesteplan.Api.Features.ApiInfo
{
    [Route("api")]
    public class ApiInfoController : BaseController
    {
        private readonly CommonOptions _commonOptions;

        public ApiInfoController(IOptions<CommonOptions> commonOptions)
        {
            _commonOptions = commonOptions.Value;
        }

        [AllowAnonymous]
        [HttpGet] 
        public IActionResult GetInfo()
        {
            return Ok(new { Environment = _commonOptions.Environment});
        }
    }
}