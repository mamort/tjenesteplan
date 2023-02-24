using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tjenesteplan.Api.Configuration
{
    public static class AppsettingsConfiguration
    {
        public static void ConfigureAppsettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));
            services.Configure<DatabaseOptions>(configuration.GetSection("Database"));
            services.Configure<SendGridOptions>(configuration.GetSection("sendGrid"));
            services.Configure<CommonOptions>(configuration);
        }
    }
}