// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Vegalot.External.XianjinIot.Common.Models.Ack;

namespace Vegalot.External.XianjinIot.Agv.Models.Command
{
    /// <summary>
    /// 用于MES向AGV下发装载料仓指令
    /// </summary>
    internal class AgvLoadSiloPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();

        /// <summary>
        /// 配置集合
        /// </summary>
        public AgvLoadSiloBody body { get; set; } = new();
    }

    internal class AgvLoadSiloBody
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        //public string timestamp { get; set; } = string.Empty;
        public long timestamp { get; set; } = 0;
        /// 任务编号
        /// </summary>
        public string taskCode { get; set; } = string.Empty;
    }
}
