// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Vegalot.External.XianjinIot.Common.Models;

namespace Vegalot.External.XianjinIot.Agv.Models.Report
{
    /// <summary>
    /// 上报完成收料
    /// </summary>
    internal class AgvReceivePanelReportPayload : BasePayload<BaseBody>
    {
        public new AgvReceivePanelReportBody body {  get; set; }
    }
}
