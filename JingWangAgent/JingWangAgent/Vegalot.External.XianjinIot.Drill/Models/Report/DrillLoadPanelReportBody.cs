// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Drill.Models.Report
{
    internal class DrillLoadPanelReportBody : ReportStateBodyEntity
    {
        /// <summary>
        ///叠板信息
        /// </summary>
        public List<PanelInfo> panelInfos { get; set; } = new();
    }
}
