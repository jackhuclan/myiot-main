
namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于钻机上报已完成接料，包括二维码读取信息
    /// </summary>
    internal class DrillReceivePanelReportPayload
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
