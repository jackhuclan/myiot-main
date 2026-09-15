// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Common.Models.Report
{
    public class ReportBaseBodyEntity : BaseBody
    {
        /// sn
        /// </summary>
        public string sn { get; set; } = string.Empty;

        /// <summary>
        /// 时间戳
        /// </summary>
        public long timestamp { get; } = (long)(((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0))).TotalMilliseconds);
    }
}
