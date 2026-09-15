// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Resources;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using Quartz.Util;
using VgAutoDrill.Fundation.Utils;

namespace VgEAPClient.Common;
public static class StringUtil
{

    static public bool IsNum(string s)
    {
        for (int k = 0; k < s.Length; k++)
        {
            var c = s[k];

            if (false == char.IsNumber(c) && c != '+' && c != '-')
                return false;
        }

        return true;
    }

    static public bool TryParseBool(string s, bool def_value)
    {
        try
        {
            var v = bool.Parse(s);
            return v;
        }
        catch
        {
        }
        return def_value;
    }

    static public int TryParseInt(string s, int def_value = -1)
    {
        try
        {
            int v = int.Parse(s);
            return v;
        }
        catch
        {
        }
        return def_value;
    }

    static public long TryParseLong(string s, long def_value = -1)
    {
        try
        {
            long v = long.Parse(s);
            return v;
        }
        catch
        {
        }
        return def_value;
    }

    static public double TryParseDouble(string s, double def_value = -1)
    {
        try
        {
            var v = double.Parse(s);
            return v;
        }
        catch
        {
        }

        return def_value;
    }


    public static string WordToHexString(ushort word)
    {
        return word.ToString("X4");
    }

    /// <summary>
    /// 如果insert_space为true， 返回的格式形如 "1B 05 A1 FE"；否则，形如“1B05A1FE”；
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="insert_space"></param>
    /// <returns></returns>
    public static string ByteArrayToHexString(byte[] bytes, bool insert_space = true)
    {
        var s = BitConverter.ToString(bytes);
        if (insert_space)
            return s.Replace("-", " ");
        else
            return s.Replace("-", string.Empty);
    }
    public static string ByteArrayToHexString(byte[] bytes, int start_index, int count, bool insert_space = true)
    {
        var des = new byte[count];
        Array.Copy(bytes, start_index, des, 0, count);
        var s = BitConverter.ToString(des);
        if (insert_space)
            return s.Replace("-", " ");
        else
            return s.Replace("-", string.Empty);
    }


    /// <summary>
    /// 输入字符串的格式形如 "1B 05 A1 FE" 或者 “1B05A1FE”；
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static byte[] HexStringToByteArray(string hex)
    {
        hex = hex.Replace(" ", "");
        if (hex.Length % 2 != 0)
            throw new ArgumentException("去除空格后，十六进制字符串的长度必须是偶数。");

        byte[] bytes = new byte[hex.Length / 2];
        for (int i = 0; i < hex.Length; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }

    public static string ToStringEx(this object? obj)
    {
        string str = string.Empty;
        if (obj != null)
        {
            str = obj.ToString() ?? "";
        }
        return str;
    }

    public static decimal ToDecimal(this object? obj)
    {
        decimal dec = 0;
        if (obj != null)
        {
            decimal.TryParse(obj.ToStringEx(), out dec);
        }
        return dec;
    }

    /// <summary>
    /// 判断字符串是否符合正则表达式
    /// </summary>
    /// <param name="str"></param>
    /// <param name="pattern">正则表达式</param>
    /// <returns></returns>
    public static bool IsMatch(this string str, string pattern)
    {
        bool IsMatch = false;
        try
        {
            if (!pattern.IsNullOrWhiteSpace())
            {
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                IsMatch = regex.IsMatch(str);
            }
        }
        catch
        {

        }
        return IsMatch;
    }

    /// <summary>
    /// Json序列化,忽略Null属性
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string ToJsonNull(this object obj)
    {
        return JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        });
    }

    public static string GetStringEx(this ResourceManager rm, string name)
    {
        return rm.GetString(name) ?? name;
    }
}


public class StringConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetDecimal().ToStringEx();
        }
        else
        {
            try
            {
                return reader.GetString();
            }
            catch
            {
                return "";
            }
        }
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToStringEx());

    }
}


public class IntConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        int reint = 0;
        try
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Number:
                    reint = reader.GetDecimal().ToInt();
                    break;
                case JsonTokenType.True:
                    reint = 1;
                    break;
                case JsonTokenType.False:
                    reint = 0;
                    break;
                default:
                    reint = reader.GetString().ToInt();
                    break;

            }
        }
        catch
        {

        }
        return reint;
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}
