using System.Text;

namespace VgAutoDrill.OpenAPI;

public static class StringExtensions
{
    public static string ToUrlParams(this string url, Dictionary<string, object> value)
    {
        if (value == null)
        {
            return url;
        }

        StringBuilder res = new StringBuilder(url);
        var enumerator = value.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var item = enumerator.Current;
            res.Append(url.Contains("=") ? "&" : "?");
            res.AppendFormat("{0}={1}", item.Key, item.Value);
        }

        return res.ToString();
    }
}
