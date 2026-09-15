
namespace VegaIot.External.XianjinIot.Models
{

    /// <summary>
    /// 用于上升料仓层指令已接收
    /// </summary>
    internal class AgvUpSiloFloorAckPayload
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
