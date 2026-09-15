namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于MES向AGV下发调整高度指令
    /// </summary>
    internal class AgvAdjustHeightPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();
        /// <summary>
        /// 内容
        /// </summary>
        public AdjustHeightBody body { get; set; } = new();
    }

    internal class AdjustHeightBody
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { get; set; } = string.Empty;
        /// <summary>
        /// 任务编号
        /// </summary>
        public string taskCode { get; set; } = string.Empty;

        /// <summary>
        /// 调整到指定高度（单位
        /// </summary>
        public int heigh { get; set; } = 0;
    }
}
