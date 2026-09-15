// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Common.Models.Report
{
    public class ReportStateBodyEntity : ReportBodyEntity
    {
        /// <summary>
        ///0 未知异常 1 完成 2  3
        /// </summary>
        public int status { get; set; } = 0;
    }
}
