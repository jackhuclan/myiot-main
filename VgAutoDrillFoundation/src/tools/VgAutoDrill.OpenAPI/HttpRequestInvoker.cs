using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace VgAutoDrill.OpenAPI;

public class HttpRequestInvoker : IHttpRequestInvoker
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<HttpRequestInvoker> _logger;
    private Action<HttpRequestHeaders>? _configureHttpRequestHeadersAction;

    public HttpRequestInvoker(IHttpClientFactory httpClientFactory,
        ILoggerFactory loggerFactory)
    {
        _httpClientFactory = httpClientFactory;
        _logger = loggerFactory.CreateLogger<HttpRequestInvoker>();
    }

    public void ConfigureHttpRequestHeaders(Action<HttpRequestHeaders> configure)
    {
        _configureHttpRequestHeadersAction = configure;
    }

    public async Task DownloadAsync(string requestUri, string savePath)
    {
        if (string.IsNullOrWhiteSpace(requestUri)) throw new ArgumentNullException(nameof(requestUri));

        using HttpClient client = _httpClientFactory.CreateClient();
        _configureHttpRequestHeadersAction?.Invoke(client.DefaultRequestHeaders);

        var responseStream = await client.GetStreamAsync(requestUri);

        using (FileStream fileStream = new FileStream(savePath, FileMode.Create))
        {
            await responseStream.CopyToAsync(fileStream);
            await fileStream.FlushAsync();
            fileStream.Close();
        }
    }

    public async Task<TResult?> GetFromJsonAsync<TResult>(string requestUri, Dictionary<string, object> value, string token = "")
    {
        if (string.IsNullOrWhiteSpace(requestUri)) throw new ArgumentNullException(nameof(requestUri));
        using HttpClient client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Accept.TryParseAdd(MediaTypeNames.Application.Json);
        _configureHttpRequestHeadersAction?.Invoke(client.DefaultRequestHeaders);

        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Add("token", token);
        }

        return await GetFromJsonAsync<TResult?>(requestUri.ToUrlParams(value));
    }

    public async Task<TResult?> GetFromJsonAsync<TResult>(string requestUri, string token = "")
    {
        if (string.IsNullOrWhiteSpace(requestUri)) throw new ArgumentNullException(nameof(requestUri));

        try
        {
            using HttpClient client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.TryParseAdd(MediaTypeNames.Application.Json);
            _configureHttpRequestHeadersAction?.Invoke(client.DefaultRequestHeaders);

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Add("token", token);
            }

            return await client.GetFromJsonAsync<TResult?>(requestUri);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"requestUri:{requestUri},detail:{ex.Message}");
            return await Task.FromResult(default(TResult));
        }
    }

    public async Task<TResult?> PostAsJsonAsync<TResult>(string requestUri, Dictionary<string, object> value)
    {
        if (string.IsNullOrWhiteSpace(requestUri)) throw new ArgumentNullException(nameof(requestUri));

        try
        {
            using HttpClient client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.TryParseAdd(MediaTypeNames.Application.Json);
            _configureHttpRequestHeadersAction?.Invoke(client.DefaultRequestHeaders);

            var httpResponseMessage = await client.PostAsJsonAsync(requestUri, value);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return await httpResponseMessage.Content.ReadFromJsonAsync<TResult>();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"requestUri:{requestUri},detail:{ex.Message}");
        }

        return await Task.FromResult(default(TResult));
    }

    public async Task<TResult?> PostAsJsonAsync<TValue, TResult>(string requestUri, TValue value, string token = "")
    {
        return await PostAsJsonAsync<TValue, TResult>(requestUri, value, JsonSerializerOptions.Default, token);
    }

    public async Task<TResult?> PostAsJsonAsync<TValue, TResult>(string requestUri, TValue value, JsonSerializerOptions jsonSerializerOptions)
    {
        return await PostAsJsonAsync<TValue, TResult>(requestUri, value, jsonSerializerOptions, string.Empty);
    }

    public async Task<TResult?> PostAsJsonAsync<TValue, TResult>(string requestUri, TValue value, JsonSerializerOptions jsonSerializerOptions, string token = "")
    {
        if (string.IsNullOrWhiteSpace(requestUri)) throw new ArgumentNullException(nameof(requestUri));

        try
        {
            using HttpClient client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.TryParseAdd(MediaTypeNames.Application.Json);
            _configureHttpRequestHeadersAction?.Invoke(client.DefaultRequestHeaders);

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Add("token", token);
            }

            var httpResponseMessage = await client.PostAsJsonAsync(requestUri, value, jsonSerializerOptions);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                _logger.LogDebug($"PostAsJsonAsync {requestUri}, successfull!");
                return await httpResponseMessage.Content.ReadFromJsonAsync<TResult>();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"requestUri:{requestUri},detail:{ex.Message}");
        }

        return await Task.FromResult(default(TResult));
    }
}
