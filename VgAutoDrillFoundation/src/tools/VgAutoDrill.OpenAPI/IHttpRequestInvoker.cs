using System.Net.Http.Headers;
using System.Text.Json;

namespace VgAutoDrill.OpenAPI;

public interface IHttpRequestInvoker
{
    /// <summary>
    /// 配置HttpRequestHeaders
    /// </summary>
    /// <param name="configure"></param>
    void ConfigureHttpRequestHeaders(Action<HttpRequestHeaders> configure);
    Task DownloadAsync(string requestUri, string savePath);
    Task<TResult?> GetFromJsonAsync<TResult>(string requestUri, Dictionary<string, object> value, string token = "");
    Task<TResult?> GetFromJsonAsync<TResult>(string requestUri, string token = "");
    Task<TResult?> PostAsJsonAsync<TResult>(string requestUri, Dictionary<string, object> value);
    Task<TResult?> PostAsJsonAsync<TValue, TResult>(string requestUri, TValue value, string token = "");
    Task<TResult?> PostAsJsonAsync<TValue, TResult>(string requestUri, TValue value, JsonSerializerOptions jsonSerializerOptions);
    Task<TResult?> PostAsJsonAsync<TValue, TResult>(string requestUri, TValue value, JsonSerializerOptions jsonSerializerOptions, string token = "");
}
