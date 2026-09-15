namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于MES下发钻孔任务给钻机
    /// </summary>
    internal class DrillStartDrillingPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();
        /// <summary>
        /// 配置集合
        /// </summary>
        public DrillStartDrillingBody body { get; set; } = new();
    }

    internal class DrillStartDrillingBody
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public string taskCode { get; set; } = string.Empty;
    }
}
