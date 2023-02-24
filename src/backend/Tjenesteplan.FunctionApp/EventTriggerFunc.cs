using System;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tjenesteplan.Events;

namespace Tjenesteplan.FunctionApp
{
    public class EventTriggerFunc
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public EventTriggerFunc(
            IConfiguration configuration, 
            IHttpClientFactory httpClientFactory
        )
        {
            _configuration = configuration;
            _client = httpClientFactory.CreateClient();
            var baseUrl = Environment.GetEnvironmentVariable("WebApiBaseUrl", EnvironmentVariableTarget.Process);
            _client.BaseAddress = new Uri(baseUrl);
        }

        [FunctionName("EventTriggerFunc")]
        public async Task Run(
            [QueueTrigger(QueueConstants.EventsTriggerQueue)]
            string message, 
            ILogger log
        )
        {
            // Add retry
            var response = await _client.PostAsync("api/events/process", new StringContent(""));
        }
    }
}
