// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Vegalot.External.XianjinIot.Common.Models;

namespace Vegalot.External.XianjinIot.Agv.Models.Ack
{
    /// <summary>
    /// 用于上报AGV已接收到调整高度指令
    /// </summary>
    internal class AgvLockingUnit900AckPayload : BasePayload<BaseBody>
    {
        public new AgvLockingUnit900AckBody body { get; set; }
    }
}
