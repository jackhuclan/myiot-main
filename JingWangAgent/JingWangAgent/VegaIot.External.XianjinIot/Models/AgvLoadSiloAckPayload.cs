
namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于上报已接收到装载料仓指令
    /// </summary>
    internal class AgvLoadSiloAckPayload
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
