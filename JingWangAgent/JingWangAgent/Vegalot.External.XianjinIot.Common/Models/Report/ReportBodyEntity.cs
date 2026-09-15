// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Common.Models.Report
{
    public class ReportBodyEntity : ReportBaseBodyEntity
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public string taskCode { get; set; } = string.Empty;

        /// <summary>
        /// 信息
        /// </summary>
        public string msg { get; set; } = string.Empty;
    }
}
