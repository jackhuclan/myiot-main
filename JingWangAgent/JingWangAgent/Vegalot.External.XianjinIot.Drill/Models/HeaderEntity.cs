// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Drill.Models
{
    internal class HeaderEntity
    {
        /// <summary>
        /// 根据body生产的hashCode
        /// </summary>
        public int signCode { get; set; } = 0;

        /// <summary>
        /// 用存储的公钥对hashCode进行加密
        /// </summary>
        public string signature { get; set; } = string.Empty;
    }
}
