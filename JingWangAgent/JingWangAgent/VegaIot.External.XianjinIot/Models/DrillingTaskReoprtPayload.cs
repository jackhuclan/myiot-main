namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于钻机上报已完成接料，包括二维码读取信息
    /// </summary>
    internal class DrillingTaskReoprtPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();
        /// <summary>
        /// 内容
        /// </summary>
        public DrillingTaskReoprtbody body { get; set; } = new();
    }
    internal class DrillingTaskReoprtbody
    {
        /// <summary>
        /// sn
        /// </summary>
        public string sn { get; set; } = string.Empty;
        /// <summary>
        /// 时间戳
        /// </summary>
        public long timestamp { get; set; } = 0;
        /// <summary>
        /// 任务编号
        /// </summary>
        public string taskCode { get; set; } = string.Empty;
        /// <summary>
        /// 0-异常,1-进行中,2-暂停,3-换刀,4-完成
        /// </summary>
        public int status { get; set; } = 0;
        /// <summary>
        /// 信息
        /// </summary>
        public string msg { get; set; } = string.Empty;
        /// <summary>
        /// 进度
        /// </summary>
        public int schedule { get; set; } = 0;
    }
    }
