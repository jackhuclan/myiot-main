
namespace VegaIot.External.XianjinIot.Models
{

    /// <summary>
    /// 上报完成收料
    /// </summary>
    internal class AgvReceivePanelReportPayload
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
