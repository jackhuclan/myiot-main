// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Common.Models.Ack
{
    public class RegistAckBody : BaseBody
    {
        /// <summary>
        ///注册结果:200为成功，其它为失败
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 注册信息
        /// </summary>
        public string msg { get; set; } = string.Empty;
    }
}
