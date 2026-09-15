// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Vegalot.External.XianjinIot.Common.Models;

namespace Vegalot.External.XianjinIot.Agv.Models.Ack
{
    internal class AgvStopWorkingAckPayload : BasePayload<BaseBody>
    {
        public new AgvStopWorkingAckBody body { get; set; }
    }
}
