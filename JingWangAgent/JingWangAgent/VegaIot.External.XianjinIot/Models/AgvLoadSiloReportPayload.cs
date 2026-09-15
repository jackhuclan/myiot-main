
namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于上报调整高度已完成
    /// </summary>
    internal class AgvLoadSiloReportPayload
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
