// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Vegalot.External.XianjinIot.Common.Models;

namespace Vegalot.External.XianjinIot.Agv.Models.Ack
{
    /// <summary>
    /// 用于上报已接收到装载料仓指令
    /// </summary>
    internal class AgvLoadSiloAckPayload : BasePayload<BaseBody>
    {
        public new AgvLoadSiloAckBody body { get; set; }
    }
}
