using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Tjenesteplan.Api.Configuration;
using Tjenesteplan.Api.Filters;
using Tjenesteplan.Domain;

[assembly: InternalsVisibleTo("Tjenesteplan.Api.UnitTests")]

namespace Tjenesteplan.Api
{
    public class Startup
    {
        private string CurrentCulture => "nb-NO";

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(CurrentCulture);
            });

            services.AddApplicationInsightsTelemetry();

            services.AddCors();
            services.ConfigureAppsettings(Configuration);
            services.ConfigureEntityFramework();
            services.AddResponseCaching();
            services.AddMvc(options => options.Filters.Add(typeof(ApiExceptionFilter)));
	        services.AddSwaggerGen(c =>
	        {
		        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tjenesteplan API", Version = "v1" });
				c.CustomSchemaIds(type => type.FullName);
	        });
			services.ConfigureAuthentication(Configuration);
            services.ConfigureDependencies();
            services.ConfigureDomainDependencies();

            services.AddMediatR(
                typeof(Startup).Assembly,
                typeof(Domain.Tjenesteplan).Assembly
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var supportedCultures = new[]{
                new CultureInfo(CurrentCulture)
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(CurrentCulture),
                SupportedCultures = supportedCultures,
                FallBackToParentCultures = false
            });
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture(CurrentCulture);
            
	        if (env.IsDevelopment())
	        {
		        app.UseDeveloperExceptionPage();
	        }
	        else
	        {
		        app.UseExceptionHandler("/Spa/Error");
		        app.UseHsts();
	        }

			app.CreateOrMigrateDatabase();
            app.SeedDatabase();

            // global cors policy
            app.UseCors(x => x
                .WithOrigins(
                    "http://localhost:8081", 
                    "https://www.tjenesteplan-dev.me",
                    "https://tjenesteplandevsa.z6.web.core.windows.net",
                    "https://www.tjenesteplan.no"
                )
                .SetPreflightMaxAge(TimeSpan.FromHours(1))
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseDefaultFiles()
	            .UseStaticFiles()
	            .UseAuthentication()
	            .UseResponseCaching()
	            .UseSwagger()
	            .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tjenesteplan API V1"); })
	            .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
	            {
		            endpoints.MapControllers();
		            endpoints.MapFallbackToController("Index", "Spa");
	            });

            //.UseMvcWithDefaultRoute()
            // Følgende triks er godkjent av Steve Sanderson (https://github.com/aspnet/JavaScriptServices/issues/973)
            // Fallback til å servere index.html for alle ukjente URL-er som ikke er under /api
            //.MapWhen(ctx => !ctx.Request.Path.Value.StartsWith("/api"), builder =>
            //{
            // builder.UseMvc(routes =>
            // {
            //routes.MapFallbackToController("spa-fallback", new {controller = "Spa", action = "Index"});
            // });
            //});
        }
    }
}
