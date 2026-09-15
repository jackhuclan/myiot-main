namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于钻机上报已完成调取资料
    /// </summary>
    internal class DrillLoadDrillingFileReportPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();
        /// <summary>
        /// 内容
        /// </summary>
        public ReportBodyEntity body { get; set; } = new();
    }

}
