
namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于AGV上报任务进度
    /// </summary>
    internal class AgvScheduledPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();
        /// <summary>
        /// 内容
        /// </summary>
        public AgvScheduledBody body { get; set; } = new();
    }
    /// <summary>
    /// 内容
    /// </summary>
    internal class AgvScheduledBody
    {
        /// <summary>
        /// 设备码
        /// </summary>
        public string sn { get; set; } = string.Empty;
        /// <summary>
        /// 任务编号
        /// </summary>
        public string taskCode { get; set; } = string.Empty;
        /// <summary>
        /// 进度
        /// </summary>
        public int schedule { get; set; } = 0;

    }
}
