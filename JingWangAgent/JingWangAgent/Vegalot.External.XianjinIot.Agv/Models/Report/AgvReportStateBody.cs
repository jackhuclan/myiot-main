// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Vegalot.External.XianjinIot.Common.Models.Report;

namespace Vegalot.External.XianjinIot.Agv.Models.Report
{
    public class AgvReportStateBody : ReportStateBodyEntity
    {
        public int receiveUnitState { get; set; } = 0;
    }
}
