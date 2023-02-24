using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Tjenesteplan.Api.Services.PasswordHash;
using Tjenesteplan.Data.Contexts;
using Tjenesteplan.Data.Features.Users.Data;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Configuration
{
    public static class EntityFrameworkConfiguration
    {
        public static void ConfigureEntityFramework(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var databaseOptions = serviceProvider
                .GetService<IOptions<DatabaseOptions>>()
                .Value;

            services.AddDbContext<DataContext>(x => x.UseSqlServer(databaseOptions.ConnectionString));
        }

        public static void CreateOrMigrateDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<DataContext>();
                //TODO: where is this?
                if (!dbContext.Database.IsInMemory())
                {
                    using (dbContext)
                    {
                        dbContext.Database.Migrate();
                    }
                }
            }
        }

        public static void SeedDatabase(this IApplicationBuilder app)
        {
            const string adminUsername = "admin";

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var configuration = serviceScope.ServiceProvider.GetService<IConfiguration>();
                var dbContext = serviceScope.ServiceProvider.GetService<DataContext>();
                using (dbContext)
                {
                    var userRepository = new UserRepository(dbContext);
                    var passwordHasService = new PasswordHashService();
                    var adminUserExists = userRepository.UserExists(adminUsername);
                    var adminPassword = configuration["Tjenesteplan-Administrator-Password"];
                    var hashSalt = passwordHasService.CreatePasswordHash(adminPassword);

                    if(string.IsNullOrEmpty(adminPassword))
                    {
                        throw new Exception("Tjenesteplan administrator password is empty");
                    }

                    if (!adminUserExists)
                    {                 
                        userRepository.CreateUser(
                            firstName: "Administrator",
                            lastName: "Administrator",
                            username: adminUsername,
                            passwordHash: hashSalt.PasswordHash,
                            passwordSalt: hashSalt.PasswordSalt,
                            role: Role.Admin
                        );
                    }
                }
            }
        }
    }
}