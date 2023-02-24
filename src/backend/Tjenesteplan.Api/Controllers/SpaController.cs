using Microsoft.AspNetCore.Mvc;

namespace Tjenesteplan.Api.Controllers
{
    public class SpaController : Controller
    {
        public IActionResult Index()
        {
	        var request = HttpContext.Request;
	        var host = request.Host.ToUriComponent();
	        var pathBase = request.PathBase.ToUriComponent();
	        var swaggerEndpoint = $"{request.Scheme}://{host}{pathBase}/swagger";

	        return Ok(new { status = "ready", apiInfo = swaggerEndpoint});
		}

		public IActionResult Error() => Problem();
	}
}