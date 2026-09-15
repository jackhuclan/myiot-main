// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Vegalot.External.XianjinIot.Common.Models;

namespace Vegalot.External.XianjinIot.Agv.Models.Ack
{
    /// <summary>
    /// 上报收到 收料指令
    /// </summary>
    internal class AgvReceivePanelAckPayload : BasePayload<BaseBody>
    {
        public new AgvReceivePanelAckBody body { get; set; }
    }
}
