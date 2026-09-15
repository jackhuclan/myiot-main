namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于钻机上报打孔进度
    /// </summary>
    internal class DrillingScheduledPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public DrillingScheduledHeader header { get; set; } = new();
        /// <summary>
        /// 内容
        /// </summary>
        public DrillingScheduledBody body { get; set; } = new();
    }
    /// <summary>
    /// 请求头
    /// </summary>
    internal class DrillingScheduledHeader
    {
        /// <summary>
        /// 根据body生产的hashCode
        /// </summary>
        public string signCode { get; set; } = string.Empty;
        /// <summary>
        /// 用存储的公钥对hashCode进行加密
        /// </summary>
        public string signature { get; set; } = string.Empty;
    }
    /// <summary>
    /// 内容
    /// </summary>
    internal class DrillingScheduledBody
    {
        /// <summary>
        /// 设备码
        /// </summary>
        public string sn { get; set; } = string.Empty;
        /// <summary>
        /// 时间戳
        /// </summary>
        public long timestamp { get; set; } = 0;
        /// <summary>
        /// 任务编码
        /// </summary>
        public string taskCode { get; set; } = string.Empty;
        /// <summary>
        /// 是否板子已到熟料仓
        /// </summary>
        public string clinkerSiloFlag { get; set; } = string.Empty;
        /// <summary>
        /// 进度
        /// </summary>
        public string taskSchedule { get; set; } = string.Empty;

    }
}
