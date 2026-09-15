// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Common.Models.Report
{
    public class ReportCodeBodyEntity : ReportBodyEntity
    {
        /// <summary>
        ///200为成功 其他为失败
        /// </summary>
        public int code { get; set; } = 0;
    }
}
