// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Nancy.Json;

namespace VgEAPClient.Common;

internal class EncodeUtil
{
    /// <summary>
    /// 字符串转字节数组
    /// </summary>
    /// <param name="str">待转换的字符串</param>
    /// <returns>转换完成的字节数组</returns>
    public static byte[] StrToBytes(string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    /// <summary>
    /// 字节数组转字符串
    /// </summary>
    /// <param name="bytes">待转换的字节数组</param>
    /// <returns>转换完成的字符串</returns>
    public static string BytesToStr(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }
}

public class EntityUtil<T>//通过泛型减少重复代码量
{
    /// <summary>
    /// json转实体类
    /// </summary>
    /// <param name="json">json字符串</param>
    /// <returns>实体类</returns>
    public static T JsonToEntity(string json)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();
        return js.Deserialize<T>(json);
    }

    /// <summary>
    /// 实体类转JSON
    /// </summary>
    /// <param name="t">泛型实体类</param>
    /// <returns>JSON字符串</returns>
    public static string EntityToJson(T t)
    {
        return JsonSerializer.Serialize<T>(t, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
    }

    /// <summary>
    /// Http工具 将操作后的实体类返回
    /// </summary>
    /// <param name="context"></param>
    /// <param name="t"></param>
    public static void ResponseMsg(HttpListenerContext context, T t)
    {
        //构造Response响应
        HttpListenerResponse response = context.Response;
        response.StatusCode = 200;
        response.ContentType = "application/json;charset=UTF-8";
        response.ContentEncoding = Encoding.UTF8;
        response.AppendHeader("Content-Type", "application/json;charset=UTF-8");

        using (StreamWriter writer = new StreamWriter(response.OutputStream, Encoding.UTF8))
        {
            writer.Write(EntityToJson(t));
            writer.Close();
            response.Close();
        }
    }
}

#region SysTimeUtil

public class SysTimeUtil
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemTime
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMiliseconds;
    }

    public class SetSystemDateTime
    {
        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime(ref SystemTime sysTime);

        public static bool SetLocalTimeByStr(string timestr, string dtFormat = "yyyyMMddHHmmss")
        {
            bool flag = false;
            SystemTime sysTime = new SystemTime();
            DateTime dt = DateTime.ParseExact(timestr, dtFormat, System.Globalization.CultureInfo.CurrentCulture);

            sysTime.wYear = Convert.ToUInt16(dt.Year);
            sysTime.wMonth = Convert.ToUInt16(dt.Month);
            sysTime.wDay = Convert.ToUInt16(dt.Day);
            sysTime.wHour = Convert.ToUInt16(dt.Hour);
            sysTime.wMinute = Convert.ToUInt16(dt.Minute);
            sysTime.wSecond = Convert.ToUInt16(dt.Second);
            try
            {
                flag = SetLocalTime(ref sysTime);
            }
            catch (Exception e)
            {
                Console.WriteLine("SetSystemDateTime函数执行异常" + e.Message);
            }
            return flag;
        }
    }
}

#endregion SysTimeUtil
