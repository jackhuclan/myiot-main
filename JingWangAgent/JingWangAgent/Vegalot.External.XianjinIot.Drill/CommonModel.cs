// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Drill
{
    internal class CommonModel
    {

        /// <summary>
        /// 公钥
        /// </summary>
        public volatile static string publicKey = string.Empty;

        /// <summary>
        /// sn
        /// </summary>
        public volatile static string sn = string.Empty;

        /// <summary>
        /// 密钥
        /// </summary>
        public volatile static string Key = string.Empty;

        /// <summary>
        /// 钻机上面的任务号
        /// </summary>

        public volatile static string DrillTaskCode = string.Empty;
        /// <summary>
        /// buffer 熟料层任务号
        /// </summary>

        public volatile static string BufferClinkerTaskCode = string.Empty;
        /// <summary>
        /// buffer 生料层任务号
        /// </summary>


        public volatile static string TempDrillTaskCode = string.Empty;


        public volatile static string UpPanelReportTaskCode = string.Empty;

        /// <summary>
        /// 上报设备状态5秒
        /// </summary>
        public static volatile int ReportDeviceFrequency = 5;

    }
}
