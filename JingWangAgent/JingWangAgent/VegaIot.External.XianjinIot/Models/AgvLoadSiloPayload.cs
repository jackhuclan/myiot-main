
namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于MES向AGV下发装载料仓指令
    /// </summary>
    internal class AgvLoadSiloPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();
        /// <summary>
        /// 配置集合
        /// </summary>
        public AgvLoadSiloBody body { get; set; } = new();
    }

    internal class AgvLoadSiloBody
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { get; set; } = string.Empty;
        /// <summary>
        /// 任务编号
        /// </summary>
        public string taskCode { get; set; } = string.Empty;
    }
}
