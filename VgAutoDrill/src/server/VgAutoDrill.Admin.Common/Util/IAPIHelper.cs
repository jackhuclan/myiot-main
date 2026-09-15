namespace VgAutoDrill.Admin.Common.Util
{
    public interface IAPIHelper
    {
        Task<TResult?> PostFromJsonAsync<TResult>(string? requestUri, Dictionary<string, object> value);

        string RequestData(string url, string method = "Get", string postData = "", int timeOut = 10000);

        T? RequestData<T>(string url, string method = "Get", string postData = null);
    }
}