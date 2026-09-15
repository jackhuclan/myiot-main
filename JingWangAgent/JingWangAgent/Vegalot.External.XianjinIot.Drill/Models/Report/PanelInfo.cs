// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Drill.Models.Report
{
    internal class PanelInfo
    {
        /// <summary>
        /// 轴号
        /// </summary>
        public int axleNum { get; set; }

        /// <summary>
        /// 二维码信息
        /// </summary>
        public string QRCode { get; set; } = string.Empty;
    }
}
