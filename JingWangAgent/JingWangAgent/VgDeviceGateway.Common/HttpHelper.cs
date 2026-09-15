// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using System.Text;

namespace VgDeviceGateway.Devices.Common
{
    public class HttpHelper
    {
        public string HttpMethod(string method, string url, string body, string contentType = "application/json", Dictionary<string, string> headers = null)
        {
            return HttpMethod((stream) =>
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }, method, url, body, contentType, headers);
        }

        public T HttpMethod<T>(Func<Stream, T> func, string method, string url, string body, string contentType = "application/json", Dictionary<string, string> headers = null)
        {
            if (string.IsNullOrEmpty(contentType)) contentType = "application/json";

            string responseContent = string.Empty;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = method;
            //httpWebRequest.Accept = "text/html, application/xhtml+xml, */*";
            httpWebRequest.Timeout = 600000;       // 10分钟,10*60*1000=600000
            //httpWebRequest.KeepAlive = true;     // 获取或设置一个值，该值指示是否与 Internet 资源建立持久性连接默认为true。
            httpWebRequest.ReadWriteTimeout = 600000; // 10分钟,10*60*1000=600000
            httpWebRequest.ContentType = contentType; // 内容类型
            httpWebRequest.MaximumResponseHeadersLength = 40000;
            httpWebRequest.ContentLength = 0;

            if (headers != null)
            {
                FormatRequestHeaders(headers, httpWebRequest);
            }

            if (!string.IsNullOrEmpty(body))
            {
                byte[] btBodys = Encoding.UTF8.GetBytes(body);
                httpWebRequest.ContentLength = btBodys.Length;

                using (Stream writeStream = httpWebRequest.GetRequestStream())
                {
                    writeStream.Write(btBodys, 0, btBodys.Length);
                    writeStream.Flush();
                }
            }

            T t;
            using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using (Stream stream = httpWebResponse.GetResponseStream())
                {
                    t = func(stream);
                }
            }

            return t;
        }

        /// <summary>
        /// 格式化请求头信息
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="request"></param>
        public void FormatRequestHeaders(Dictionary<string, string> headers, HttpWebRequest request)
        {
            foreach (var hd in headers)
            {
                //因为HttpWebRequest中很多标准标头都被封装成只能通过属性设置，添加的话会抛出异常
                switch (hd.Key.ToLower())
                {
                    case "connection":
                        request.KeepAlive = false;
                        break;

                    case "content-type":
                        request.ContentType = hd.Value;
                        break;

                    case "transfer-enconding":
                        request.TransferEncoding = hd.Value;
                        break;

                    default:
                        request.Headers.Add(hd.Key, hd.Value);
                        break;
                }
            }
        }
    }
}
