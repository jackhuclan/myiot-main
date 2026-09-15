using Consul;
using Microsoft.Extensions.Logging;

namespace VgAutoDrill.OpenAPI;

public class ConsulServiceConsumer : IConsulServiceConsumer
{
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly ConsulClient _consulClient;
    private readonly ILogger<ConsulServiceConsumer> _logger;
    public ConsulServiceConsumer(IHttpRequestInvoker httpRequestInvoker,
        ILoggerFactory loggerFactory,
        ConsulClient consulClient)
    {
        _httpRequestInvoker = httpRequestInvoker;
        _consulClient = consulClient;
        _logger = loggerFactory.CreateLogger<ConsulServiceConsumer>();
    }

    public async Task DownloadAsync(string requestUri, string savePath)
    {
        var realUrl = await ConvertToAgentUrl(requestUri);
        _logger.LogDebug($"ConvertToAgentUrl from {requestUri} to {realUrl}");
        await _httpRequestInvoker.DownloadAsync(realUrl, savePath);
    }

    public async Task<TResult?> GetFromJsonAsync<TResult>(string requestUri, Dictionary<string, object> value, string token = "")
    {
        var realUrl = await ConvertToAgentUrl(requestUri);
        _logger.LogDebug($"ConvertToAgentUrl from {requestUri} to {realUrl}");
        return await _httpRequestInvoker.GetFromJsonAsync<TResult>(realUrl, value, token);
    }

    public async Task<TResult?> GetFromJsonAsync<TResult>(string requestUri, string token = "")
    {
        var realUrl = await ConvertToAgentUrl(requestUri);
        _logger.LogDebug($"ConvertToAgentUrl from {requestUri} to {realUrl}");
        return await _httpRequestInvoker.GetFromJsonAsync<TResult>(realUrl, token);
    }

    public async Task<TResult?> PostAsJsonAsync<TResult>(string requestUri, Dictionary<string, object> value)
    {
        var realUrl = await ConvertToAgentUrl(requestUri);
        _logger.LogDebug($"ConvertToAgentUrl from {requestUri} to {realUrl}");
        return await _httpRequestInvoker.PostAsJsonAsync<TResult>(realUrl, value);
    }

    public async Task<TResult?> PostAsJsonAsync<TValue, TResult>(string requestUri, TValue value, string token = "")
    {
        var realUrl = await ConvertToAgentUrl(requestUri);
        _logger.LogDebug($"ConvertToAgentUrl from {requestUri} to {realUrl}");
        return await _httpRequestInvoker.PostAsJsonAsync<TValue, TResult>(realUrl, value, token);
    }

    private async Task<string> ConvertToAgentUrl(string serviceUrl)
    {
        var uri = new Uri(serviceUrl);
        var serviceName = uri.Host;
        var scheme = uri.Scheme;
        var PathAndQuery = uri.PathAndQuery;

        var healthyServices = await _consulClient.Health.Service(serviceName);
        if (healthyServices == null
            || !healthyServices.Response.Any())
        {
            return serviceUrl;
        }

        var agentService = healthyServices.Response[0].Service;
        return $"{scheme}://{agentService.Address}:{agentService.Port}{PathAndQuery}";
    }
}
