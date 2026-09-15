
namespace VegaIot.External.XianjinIot.Models
{

    /// <summary>
    /// 下发准备接料指令
    /// </summary>
    internal class DrillReceivePanelPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();
        /// <summary>
        /// 内容
        /// </summary>
        public DrillReceivePanelBody body { get; set; } = new();
    }

    /// <summary>
    /// 内容
    /// </summary>
    internal class DrillReceivePanelBody
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public string taskCode { get; set; } = string.Empty;
    }
}
