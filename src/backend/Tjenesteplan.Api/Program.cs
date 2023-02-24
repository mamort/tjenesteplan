using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Tjenesteplan.Domain;
using WebApi;

namespace Tjenesteplan.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((ctx, builder) =>
                {
                    var config = builder.Build();
                    var environment = config["Environment"];
                    var keyVaultUrl = config["AppSecretsKeyVaultUrl"];

                    if (!string.IsNullOrEmpty(keyVaultUrl) && environment != TjenesteplanEnvironment.Local)
                    {
                        var azureServiceTokenProvider = new AzureServiceTokenProvider();
                        var keyVaultClient = new KeyVaultClient(
                            new KeyVaultClient.AuthenticationCallback(
                                azureServiceTokenProvider.KeyVaultTokenCallback));
                        builder.AddAzureKeyVault(
                            keyVaultUrl, keyVaultClient, new DefaultKeyVaultSecretManager());
                    }
                })
                .UseStartup<Startup>()
                .ConfigureLogging((hostingContext, logging) => {
                    if (hostingContext.Configuration["Environment"] != TjenesteplanEnvironment.Local)
                    {
                        var appInsightKey = hostingContext.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
                        logging.AddApplicationInsights(appInsightKey);
                    }
                })
                .Build();
    }
}
