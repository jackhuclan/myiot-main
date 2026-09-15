
namespace VgAutoDrill.OpenAPI;

public interface IConsulServiceConsumer
{
    Task DownloadAsync(string requestUri, string savePath);
    Task<TResult?> GetFromJsonAsync<TResult>(string requestUri, string token = "");
    Task<TResult?> GetFromJsonAsync<TResult>(string requestUri, Dictionary<string, object> value, string token = "");
    Task<TResult?> PostAsJsonAsync<TResult>(string requestUri, Dictionary<string, object> value);
    Task<TResult?> PostAsJsonAsync<TValue, TResult>(string requestUri, TValue value, string token = "");
}
