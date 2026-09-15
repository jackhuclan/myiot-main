// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using StackExchange.Redis;
using Vegalot.External.XianjinIot.Common.Models.Ack;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace Vegalot.External.XianjinIot.Common.Models
{
    public class BasePayload<T> where T : BaseBody
    {

        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();

        /// <summary>
        /// 内容
        /// </summary>
        public T body { get; set; }
    }
}
