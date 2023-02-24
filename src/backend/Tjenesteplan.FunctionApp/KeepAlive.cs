using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Tjenesteplan.FunctionApp
{
    public class KeepAlive
    {
	    private HttpClient _client;

	    public KeepAlive(IHttpClientFactory httpClientFactory)
	    {
		    _client = httpClientFactory.CreateClient();
		    var baseUrl = Environment.GetEnvironmentVariable("WebApiBaseUrl", EnvironmentVariableTarget.Process);
		    _client.BaseAddress = new Uri(baseUrl);
	    }

	    [FunctionName("KeepAliveFunc")]
	    public Task Run([TimerTrigger("0 */15 * * * *")] TimerInfo myTimer, ILogger log)
	    {
		    return _client.GetAsync("api/keepalive");
	    }
    }
}
