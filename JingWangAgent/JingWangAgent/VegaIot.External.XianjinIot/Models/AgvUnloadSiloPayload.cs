
namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于MES向AGV下发卸载料仓指令
    /// </summary>
    internal class AgvUnloadSiloPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();
        /// <summary>
        /// 配置集合
        /// </summary>
        public UnloadSiloBody body { get; set; } = new();
    }

    internal class UnloadSiloBody
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public long timestamp { get; set; } = 0;
        /// <summary>
        /// 任务编号
        /// </summary>
        public string taskCode { get; set; } = string.Empty;

    }
}
