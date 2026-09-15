// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Vegalot.External.XianjinIot.Common.Models.Ack;

namespace Vegalot.External.XianjinIot.Agv.Models.Command
{
    /// <summary>
    /// 用于MES下发AGV上料
    /// </summary>
    internal class AgvPushPanelPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();

        /// <summary>
        /// 内容
        /// </summary>
        public AgvPushPanelBody body { get; set; } = new();
    }

    /// <summary>
    /// 内容
    /// </summary>
    internal class AgvPushPanelBody
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public string taskCode { get; set; } = string.Empty;
    }
}
