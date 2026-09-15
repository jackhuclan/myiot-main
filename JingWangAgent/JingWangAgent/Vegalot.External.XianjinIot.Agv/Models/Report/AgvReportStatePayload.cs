// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Vegalot.External.XianjinIot.Common.Models;

namespace Vegalot.External.XianjinIot.Agv.Models.Report
{
    public class AgvReportStatePayload : BasePayload<BaseBody>
    {

        public new AgvReportStateBody body { get; set; }
    }
}
