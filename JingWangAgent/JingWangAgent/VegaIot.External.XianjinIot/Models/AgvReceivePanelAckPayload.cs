
namespace VegaIot.External.XianjinIot.Models
{

    /// <summary>
    /// 上报收到 收料指令
    /// </summary>
    internal class AgvReceivePanelAckPayload
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
