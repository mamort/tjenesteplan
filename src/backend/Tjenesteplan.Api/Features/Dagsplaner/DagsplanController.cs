using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.Dagsplaner
{
	[Route("api/dagsplaner")]
	public class DagsplanController : BaseController
	{
		[HttpGet]
		public IActionResult Get()
		{
			var dagsplaner = Dagsplan.AllDagsplaner
				.Select(dagsplan => new
				{
					Id = (int) dagsplan.DagsplanId,
					Name = dagsplan.Name,
                    IsSystemDagsplan = dagsplan.IsSystemDagsplan,
                    IsRolling = dagsplan.IsRolling
				});

			return Ok(dagsplaner);
		}
	}
}
