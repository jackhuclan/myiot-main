namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于钻机上报已开始打板
    /// </summary>
    internal class DrillStartDrillingReportPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();
        /// <summary>
        /// 配置集合
        /// </summary>
        public ReportBodyEntity body { get; set; } = new();
    }
}
