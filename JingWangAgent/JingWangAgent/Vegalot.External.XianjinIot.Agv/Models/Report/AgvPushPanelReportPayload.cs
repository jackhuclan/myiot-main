// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Vegalot.External.XianjinIot.Common.Models;

namespace Vegalot.External.XianjinIot.Agv.Models.Report
{
    /// <summary>
    /// 上料完成上报
    /// </summary>
    internal class AgvPushPanelReportPayload : BasePayload<BaseBody>
    {
        public new AgvPushPanelReportBody body { get; set; }
    }
}
