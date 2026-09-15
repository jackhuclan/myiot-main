
namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 上报料仓上升完成
    /// </summary>
    internal class AgvUpSiloFloorReportPayload
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
