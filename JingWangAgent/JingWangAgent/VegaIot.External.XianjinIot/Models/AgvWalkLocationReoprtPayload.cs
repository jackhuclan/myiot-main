

namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于AGV上报已行走到指定位置
    /// </summary>
    internal class AgvWalkLocationReoprtPayload
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
