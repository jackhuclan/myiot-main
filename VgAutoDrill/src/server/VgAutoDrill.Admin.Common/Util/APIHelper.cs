using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace VgAutoDrill.Admin.Common.Util
{
    /// <summary>
    /// 调用API接口
    /// </summary>
    public class APIHelper : IAPIHelper
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<APIHelper> _logger;
        private readonly JsonSerializerOptions option;

        public APIHelper(IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory)
        {
            _httpClientFactory = httpClientFactory;
            _logger = loggerFactory.CreateLogger<APIHelper>();
            option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public string RequestData(string url, string method = "Get", string postData = "", int timeoutMilliseconds = 10000)
        {
            try
            {
                method = method.ToUpper();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                request.ContentType = "application/json";
                request.Timeout = timeoutMilliseconds;

                if (method == "POST" && !string.IsNullOrEmpty(postData))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = bytes.Length;

                    Stream stream = request.GetRequestStream();
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Close();
                }
                request.ServerCertificateValidationCallback = (message, certificate2, arg3, arg4) => true;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string responseText = streamReader.ReadToEnd();
                streamReader.Close();
                _logger.LogDebug($"{url}请求成功,{(string.IsNullOrWhiteSpace(postData) ? string.Empty : $"请求参数:{postData}")}，返回结果:{responseText}");
                return responseText;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{url},请求参数：{postData}请求异常:{ex.Message},堆栈信息:{ex.StackTrace}");
                throw;
            }
        }

        public T? RequestData<T>(string url, string method = "Get", string postData = null)
        {
            try
            {
                method = method.ToUpper();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                request.ContentType = "application/json";

                if (method.ToLower() == "post" && !string.IsNullOrWhiteSpace(postData))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = bytes.Length;

                    Stream stream = request.GetRequestStream();
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Close();
                }
                request.ServerCertificateValidationCallback = (message, certificate2, arg3, arg4) => true;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string responseText = streamReader.ReadToEnd();
                streamReader.Close();
                _logger.LogDebug($"{url}请求成功,{(string.IsNullOrWhiteSpace(postData) ? string.Empty : $"请求参数:{postData}")}，返回结果:{responseText}");
                responseText = string.IsNullOrWhiteSpace(responseText) ? string.Empty : responseText;
                var data = JsonSerializer.Deserialize<T>(responseText, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{url}请求异常:{ex.Message},堆栈信息:{ex.StackTrace}");
                throw;
            }
        }

        public async Task<TResult?> PostFromJsonAsync<TResult>(string? requestUri, Dictionary<string, object> value)
        {
            if (string.IsNullOrWhiteSpace(requestUri)) throw new ArgumentNullException(nameof(requestUri));

            try
            {
                using HttpClient client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Accept.TryParseAdd(MediaTypeNames.Application.Json);
                var httpResponseMessage = await client.PostAsJsonAsync(requestUri, value);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return await httpResponseMessage.Content.ReadFromJsonAsync<TResult>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return await Task.FromResult(default(TResult));
        }
    }
}