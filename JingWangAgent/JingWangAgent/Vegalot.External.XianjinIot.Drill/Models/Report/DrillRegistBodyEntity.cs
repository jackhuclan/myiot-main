// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Vegalot.External.XianjinIot.Drill.Models.Report;

namespace Vegalot.External.XianjinIot.Drill.Models.Models.Report
{
    internal class DrillRegistBodyEntity : ReportBaseBodyEntity
    {
        /// <summary>
        /// 设备类型
        /// </summary>
        public string deviceType { get; set; } = string.Empty;

        /// <summary>
        /// 任务编号
        /// </summary>
        public string deviceCode { get; set; } = string.Empty;

        /// <summary>
        /// 设备供应商
        /// </summary>
        public string deviceProvider { get; set; } = "vega";

        /// <summary>
        /// 会话密钥
        /// </summary>
        public string sessionKey { get; set; } = string.Empty;

        /// <summary>
        /// ip
        /// </summary>
        public string deviceIp { get; set; } = string.Empty;

        /// <summary>
        /// port
        /// </summary>
        public string devicePort { get; set; } = string.Empty;

        /// <summary>
        /// 入料口高度，针对钻机参数，其它设备不传,单位cm
        /// </summary>
        public string feedHeight { get; set; } = string.Empty;

        /// <summary>
        ///出料口高度  钻机参数，其它设备不传,单位cm
        /// </summary>
        public int dischargeHeight { get; set; }

        /// <summary>
        /// 应用信息集合
        /// </summary>
        public List<ApplicationInfo> applicationInfoList { get; set; } = new();
        /// <summary>
        /// 轴数
        /// </summary>
        public int axlesNumber { get; set; }
    }

    internal class ApplicationInfo
    {
        /// <summary>
        /// 应用名
        /// </summary>
        public string name { get; set; } = string.Empty;

        /// <应用版本>
        /// 任务编号
        /// </summary>
        public string version { get; set; } = string.Empty;


    }
}
