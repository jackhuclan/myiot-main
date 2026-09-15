// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Vegalot.External.XianjinIot.Common.Models.Ack;

namespace Vegalot.External.XianjinIot.Agv.Models.Command
{
    internal class AgvLockingUnit900Payload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();

        /// <summary>
        /// 配置集合
        /// </summary>
        public AgvLockingUnit900Body body { get; set; } = new();
    }

    internal class AgvLockingUnit900Body
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { get; set; } = string.Empty;

        /// <summary>
        /// 任务编号
        /// </summary>
        public string taskCode { get; set; } = string.Empty;
    }
}
