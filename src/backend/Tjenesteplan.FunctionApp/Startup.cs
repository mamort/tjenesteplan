using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Tjenesteplan.Domain;

[assembly: FunctionsStartup(typeof(Tjenesteplan.FunctionApp.Startup))]

namespace Tjenesteplan.FunctionApp
{
	public class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			builder.Services.AddHttpClient();
        }
	}
}
