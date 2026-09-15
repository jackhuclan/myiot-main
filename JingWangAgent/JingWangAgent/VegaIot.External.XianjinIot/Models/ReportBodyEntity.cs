
namespace VegaIot.External.XianjinIot.Models
{
    internal class ReportBodyEntity
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
        /// 0-未知异常，1-完成，2-AGV故障,3-电量不够
        /// </summary>
        public int status { get; set; } = 0;
        /// <summary>
        /// 信息
        /// </summary>
        public string msg { get; set; } = string.Empty;
    }
}
