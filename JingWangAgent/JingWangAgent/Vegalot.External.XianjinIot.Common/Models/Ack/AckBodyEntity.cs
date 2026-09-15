// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Common.Models.Ack
{
    public class AckBodyEntity
    {
        /// <summary>
        /// sn
        /// </summary>
        public string sn { get; set; } = string.Empty;

        /// <summary>
        /// 时间戳
        /// </summary>
        public long timestamp { get; set; } = 0;

        /// <summary>
        /// 任务编号
        /// </summary>
        public string taskCode { get; set; } = string.Empty;

        /// <summary>
        /// 结果:200为成功，其它为失败
        /// </summary>
        public int code { get; set; } = 0;

        /// <summary>
        /// 信息
        /// </summary>
        public string msg { get; set; } = string.Empty;
    }
}
