// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Vegalot.External.XianjinIot.Common.Models;

namespace Vegalot.External.XianjinIot.Agv.Models.Report
{
    /// <summary>
    /// 上报料仓上升完成
    /// </summary>
    internal class AgvUpHeightNumberReportPayload : BasePayload<BaseBody>
    {
        public new AgvUpHeightNumberReportBody body {  get; set; }
    }
}
