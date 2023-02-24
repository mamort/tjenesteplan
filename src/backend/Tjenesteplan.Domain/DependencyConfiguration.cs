using Microsoft.Extensions.DependencyInjection;
using Tjenesteplan.Domain.Actions.MinTjenesteplan;
using Tjenesteplan.Domain.Actions.WeeklyTjenesteplan;
using Tjenesteplan.Domain.Framework;

namespace Tjenesteplan.Domain
{
    public static class DependencyConfiguration
    {
        public static void ConfigureDomainDependencies(this IServiceCollection services)
        {
            services.AddScoped<IDateTimeService, DateTimeService>();
            services.AddScoped<CurrentTjenesteplanAction, CurrentTjenesteplanAction>();
            services.AddScoped<WeeklyTjenesteplanAction, WeeklyTjenesteplanAction>();
        }
    }
}