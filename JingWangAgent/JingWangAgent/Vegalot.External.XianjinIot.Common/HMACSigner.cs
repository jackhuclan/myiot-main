// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Vegalot.External.XianjinIot.Common;
public class HMACSigner
{
    public static string Sign(string data, string key)
    {
        using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
        {
            data = SortJson(data);//json排序
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(hash); 
        }
    }

    public static string SortJson(string json)
    {
        var token = JToken.Parse(json);
        var sortedToken = SortToken(token);
        return sortedToken.ToString(Formatting.None);
    }

    private static JToken SortToken(JToken token)
    {
        switch (token.Type)
        {
            case JTokenType.Object:
                var sortedObj = new JObject();
                foreach (var property in ((JObject)token).Properties()
                    .OrderBy(p => p.Name, StringComparer.Ordinal))
                {
                    sortedObj.Add(property.Name, SortToken(property.Value));
                }
                return sortedObj;

            case JTokenType.Array:
                var sortedArray = new JArray();
                foreach (var item in token)
                {
                    sortedArray.Add(SortToken(item));
                }
                return sortedArray;

            default:
                return token;
        }
    }
}
