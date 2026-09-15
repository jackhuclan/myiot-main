
namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于AGV上报卸载料仓完成
    /// </summary>
    internal class AgvUnloadSiloReportPayload
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
