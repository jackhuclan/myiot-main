namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于上报AGV已接收到调整高度指令
    /// </summary>
    internal class AgvAdjustHeightAckPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();
        /// <summary>
        /// 配置集合
        /// </summary>
        public AckBodyEntity body { get; set; } = new();
    }
}
