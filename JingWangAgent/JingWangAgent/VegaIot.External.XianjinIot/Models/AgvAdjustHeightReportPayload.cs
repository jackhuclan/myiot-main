namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 上报AGV调整高度已完成
    /// </summary>
    internal class AgvAdjustHeightReportPayload
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
