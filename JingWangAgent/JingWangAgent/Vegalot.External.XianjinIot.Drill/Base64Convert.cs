// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;

namespace Vegalot.External.XianjinIot.Drill;
internal static class Base64Convert
{
    //TODO
    public static string FromStringToBase64String(string str)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        // 将字节数组转换为 Base64 字符串
        string base64String = Convert.ToBase64String(bytes);
        return base64String;
    }
    public static string FromBase64StringToString(string base64String)
    {
        byte[] dateByte = Convert.FromBase64String(base64String);
        var content = Encoding.UTF8.GetString(dateByte);
        return content;
    }
    //public static string FromStringToBase64String(string str)
    //{
    //    return str;
    //}
    //public static string FromBase64StringToString(string base64String)
    //{

    //    return base64String;
    //}


}
