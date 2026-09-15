

namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于MES下发钻孔任务给钻机
    /// </summary>
    internal class DrillCreateTaskPayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();
        /// <summary>
        /// 内容
        /// </summary>
        public DrillCreateTaskBody body { get; set; } = new();
    }

    /// <summary>
    /// 内容
    /// </summary>
    internal class DrillCreateTaskBody
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public string taskCode { get; set; } = string.Empty;

        /// <summary>
        /// 料号编码
        /// </summary>
        public string incodeNumber { get; set; } = string.Empty;

        /// <summary>
        /// 最小孔径
        /// </summary>
        public decimal minPoreSize { get; set; } = 0;
        /// <summary>
        /// 板长
        /// </summary>
        public decimal panelLength { get; set; } = 0;
        /// <summary>
        /// 板宽
        /// </summary>
        public decimal panelWidth { get; set; } = 0;
        /// <summary>
        /// 板厚
        /// </summary>
        public decimal panelThickness { get; set; } = 0;
        /// <summary>
        /// 铜厚
        /// </summary>
        public decimal copperThickness { get; set; } = 0;
        /// <summary>
        /// 执行钻孔任务的轴，根据任务轴进板
        /// </summary>
        public List<int> axleNums { get; set; } =new();
        /// <summary>
        /// 铜厚
        /// </summary>
        public decimal targetHoleSpacing { get; set; } = 0;
        /// <summary>
        /// 钻机编码
        /// </summary>
        public string drillingCode { get; set; } = string.Empty;
        /// <summary>
        /// 资料路径
        /// </summary>
        public string filePath { get; set; } = string.Empty;
    }
}
