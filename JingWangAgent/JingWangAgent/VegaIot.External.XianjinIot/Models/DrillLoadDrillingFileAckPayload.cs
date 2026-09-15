namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于钻机上报已接收到调资料指令
    /// </summary>
    internal class DrillLoadDrillingFileAckPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();
        /// <summary>
        /// 内容
        /// </summary>
        public AckBodyEntity body { get; set; } = new();
    }

}
