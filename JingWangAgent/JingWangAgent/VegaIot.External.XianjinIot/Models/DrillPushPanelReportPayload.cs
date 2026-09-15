

namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 机下发退板完成上报
    /// </summary>
    internal class DrillPushPanelReportPayload
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
