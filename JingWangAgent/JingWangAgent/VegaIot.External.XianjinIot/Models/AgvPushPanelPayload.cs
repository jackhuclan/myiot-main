

namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于MES下发AGV上料
    /// </summary>
    internal class AgvPushPanelPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();
        /// <summary>
        /// 内容
        /// </summary>
        public AgvPushPanelBody body { get; set; } = new();
    }
    /// <summary>
    /// 内容
    /// </summary>
    internal class AgvPushPanelBody
    {

        /// <summary>
        /// 任务编号
        /// </summary>
        public string taskCode { get; set; } = string.Empty;
    }
}
