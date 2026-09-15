namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于钻机上报已创建任务
    /// </summary>
    internal class DrillStartDrillingAckPayload
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
