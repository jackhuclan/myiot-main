namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于MES给钻机下发退板指令
    /// </summary>
    internal class DrillPushPanelPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();
        /// <summary>
        /// 内容
        /// </summary>
        public DrillPushPanelBody body { get; set; } = new();
    }
    /// <summary>
    /// 内容
    /// </summary>
    internal class DrillPushPanelBody
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public string taskCode { get; set; } = string.Empty;
    }
}
