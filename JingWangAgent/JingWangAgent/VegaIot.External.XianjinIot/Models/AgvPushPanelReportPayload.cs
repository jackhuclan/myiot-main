

namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 上料完成上报
    /// </summary>
    internal class AgvPushPanelReportPayload
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
