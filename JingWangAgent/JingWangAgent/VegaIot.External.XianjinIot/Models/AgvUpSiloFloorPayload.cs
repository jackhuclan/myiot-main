
namespace VegaIot.External.XianjinIot.Models
{

    /// <summary>
    /// 用于MES下发AGV上升到指定层
    /// </summary>
    internal class AgvUpSiloFloorPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();
        /// <summary>
        /// 内容
        /// </summary>
        public AgvUpSiloFloorBody body { get; set; } = new();
    }
    /// <summary>
    /// 内容
    /// </summary>
    internal class AgvUpSiloFloorBody
    {
        /// <summary>
        /// sn
        /// </summary>
        public string sn { get; set; } = string.Empty;
        /// <summary>
        /// 任务编号
        /// </summary>
        public string taskCode { get; set; } = string.Empty;
        /// <summary>
        /// 料仓层
        /// </summary>
        public int siloFloor { get; set; } = 0;
        /// <summary>
        /// 上升的高度,单位cm
        /// </summary>
        public int upHeight { get; set; } = 0;
    }

}
