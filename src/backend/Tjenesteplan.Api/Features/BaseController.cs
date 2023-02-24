using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tjenesteplan.Api.Features
{
	[Authorize]
	public abstract class BaseController : Controller
	{
	}
}
