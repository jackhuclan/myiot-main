// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Vegalot.External.XianjinIot.Common.Models.Ack;

namespace Vegalot.External.XianjinIot.Agv.Models.Command
{
    /// <summary>
    /// 用于MES下发AGV上升到指定层
    /// </summary>
    internal class AgvUpHeightNumberPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();

        /// <summary>
        /// 内容
        /// </summary>
        public AgvUpSiloFloorBody body { get; set; } = new();
    }

    /// <summary>
    /// 内容
    /// </summary>
    internal class AgvUpSiloFloorBody
    {
        /// <summary>
        /// sn
        /// </summary>
        public string sn { get; set; } = string.Empty;

        /// <summary>
        /// 任务编号
        /// </summary>
        public string taskCode { get; set; } = string.Empty;

        /// <summary>
        /// 料仓层
        /// </summary>
        public int siloFloor { get; set; } = 0;

        /// <summary>
        /// 上升的高度,单位cm
        /// </summary>
        public int upHeight { get; set; } = 0;
    }
}
