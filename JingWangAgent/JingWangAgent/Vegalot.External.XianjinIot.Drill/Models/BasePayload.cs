// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;

namespace Vegalot.External.XianjinIot.Drill.Models
{
    internal class BasePayload<T> where T : BaseBody
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();

        /// <summary>
        /// 内容
        /// </summary>

        public T body { get; set; }

        public void ModifyHeader(bool isNeedBodySign = true)
        {
            header.signCode = body.GetHashCode();
            if (isNeedBodySign && body != null && !string.IsNullOrWhiteSpace(CommonModel.Key))
            {
                header.signature = HMACSigner.Sign(JsonSerializer.Serialize(body), CommonModel.Key);
            }

        }
    }
}
