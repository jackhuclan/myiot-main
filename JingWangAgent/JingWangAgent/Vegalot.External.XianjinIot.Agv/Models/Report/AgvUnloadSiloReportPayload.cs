// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Vegalot.External.XianjinIot.Common.Models;

namespace Vegalot.External.XianjinIot.Agv.Models.Report
{
    /// <summary>
    /// 用于AGV上报卸载料仓完成
    /// </summary>
    internal class AgvUnloadSiloReportPayload : BasePayload<BaseBody>
    {
        public new AgvUnloadSiloReportBody body { get; set; }
    }
}
