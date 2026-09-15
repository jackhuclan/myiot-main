using System.Text;
using System.Text.Json;

namespace VgAutoDrill.Fundation.Utils;

public static class StringExtensions
{
    public static string? ToUrlParams(this string url, Dictionary<string, object> value)
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

    public static byte[] GetBytes(this string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    public static byte[] GetBytes(this string str, Encoding encoding)
    {
        return encoding.GetBytes(str);
    }

    public static string SafePadLeft(this int? input, int totalWidth)
    {
        if (!input.HasValue || input == 0) return "0".PadLeft(totalWidth, '0');
        else return input.Value.ToString().PadLeft(totalWidth, '0');
    }

    public static string SafePadLeft(this int? input, int totalWidth, char paddingChar)
    {
        if (!input.HasValue || input == 0) return "0".PadLeft(totalWidth, paddingChar);
        else return input.Value.ToString().PadLeft(totalWidth, paddingChar);
    }

    /// <summary>
    /// 截取左边字符
    /// </summary>
    /// <param name="sSource"></param>
    /// <param name="iLength"></param>
    /// <returns></returns>
    public static string Left(this string sSource, int iLength)
    {
        return sSource.Substring(0, iLength > sSource.Length ? sSource.Length : iLength);
    }

    /// <summary>
    /// 截取右边字符
    /// </summary>
    /// <param name="sSource"></param>
    /// <param name="iLength"></param>
    /// <returns></returns>
    public static string Right(this string sSource, int iLength)
    {
        return sSource.Substring(iLength > sSource.Length ? 0 : sSource.Length - iLength);
    }

    /// <summary>
    /// 截取中间字符
    /// </summary>
    /// <param name="sSource"></param>
    /// <param name="iStart"></param>
    /// <param name="iLength"></param>
    /// <returns></returns>
    public static string Mid(this string sSource, int iStart, int iLength)
    {
        int iStartPoint = iStart > sSource.Length ? sSource.Length : iStart;
        return sSource.Substring(iStartPoint, iStartPoint + iLength > sSource.Length ? sSource.Length - iStartPoint : iLength);
    }

    /// <summary>
    /// 将<paramref name="source"/>重复<paramref name="count"/>次数
    /// </summary>
    /// <param name="source">源字符</param>
    /// <param name="count">重复次数</param>
    /// <returns></returns>
    public static string Repeat(this string source, int count)
    {
        return string.Join(string.Empty, Enumerable.Repeat(source, count));
    }

    /// <summary>
    /// 将json反序列化为一个对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="jsonSerializer"></param>
    /// <returns></returns>
    public static T? FromJson<T>(this string json, JsonSerializerOptions? jsonSerializer = null)
    {
        return JsonSerializer.Deserialize<T>(json, jsonSerializer);
    }

    /// <summary>
    /// 修剪字符串开头
    /// </summary>
    /// <param name="source"></param>
    /// <param name="trimStr"></param>
    /// <param name="recursive">循环删除trimstr直到条件不满足为止</param>
    /// <returns></returns>
    public static string TrimStart(this string source, string trimStr, bool recursive = false)
    {
        if (source.StartsWith(trimStr, StringComparison.Ordinal))
        {
            if (recursive)
            {
                return source.Substring(trimStr.Length).TrimStart(trimStr, true);
            }

            return source.Substring(trimStr.Length);
        }

        return source;
    }

    /// <summary>
    /// 修剪字符串末尾
    /// </summary>
    /// <param name="source"></param>
    /// <param name="trimStr"></param>
    /// <param name="recursive">循环删除trimstr直到条件不满足为止</param>
    /// <returns></returns>
    public static string TrimEnd(this string source, string trimStr, bool recursive = false)
    {
        if (source.EndsWith(trimStr, StringComparison.Ordinal))
        {
            if (recursive)
            {
                return source.Substring(0, source.Length - trimStr.Length).TrimEnd(trimStr, true);
            }

            return source.Substring(0, source.Length - trimStr.Length);
        }

        return source;
    }

    /// <summary>
    /// compare two strings with StringComparison.OrdinalIgnoreCase
    /// </summary>
    /// <param name="source"></param>
    /// <param name="expect"></param>
    /// <returns></returns>
    public static bool OrdinalIgnoreCaseEquals(this string? source, string? expect)
    {
        return string.Equals(source, expect, StringComparison.OrdinalIgnoreCase);
    }
}
