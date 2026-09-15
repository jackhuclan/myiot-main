

namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 钻孔任务指令接收ACK
    /// </summary>
    internal class DrillCreateTaskAckPayload
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
