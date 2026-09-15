

namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 上报接收退板指令接收ACK
    /// </summary>
    internal class DrillPushPanelAckPayload
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
