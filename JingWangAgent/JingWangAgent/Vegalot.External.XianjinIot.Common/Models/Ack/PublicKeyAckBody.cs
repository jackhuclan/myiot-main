// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Common.Models.Ack
{
    public class PublicKeyAckBody : BaseBody
    {
        /// <summary>
        /// 公钥
        /// </summary>
        public string publicKey { get; set; } = string.Empty;

        /// <summary>
        /// 公钥，剔除首尾格式
        /// </summary>
        /// <returns></returns>
        public string PublicKeyTrim()
        {
            return publicKey.Replace("-----BEGIN PUBLIC KEY-----\n", "").Replace("\n-----END PUBLIC KEY-----\n", "").Trim();
        }
    }
}
