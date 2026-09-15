using VgAutoDrill.Admin.Model.ViewModels;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.SiloPanelTrace
{
    /// <summary>
    /// 获取料仓板料追溯列表请求参数
    /// </summary>
    public class GetSiloPanelTraceListReq : Page
    {
        /// <summary>
        /// ID
        /// </summary>
        public long? ID { get; set; }

        /// <summary>
        /// 料仓号
        /// </summary>
        public string? SiloCode { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// 生料
        /// </summary>
        public string? UndrilledItem { get; set; }

        /// <summary>
        /// 熟料
        /// </summary>
        public string? DrilledItem { get; set; }

        /// <summary>
        /// 是否存在多个熟料
        /// </summary>
        public bool? HasMultipleDrilled { get; set; }

        /// <summary>
        /// 操作主题描述
        /// </summary>
        public string? Subject { get; set; }

        /// <summary>
        /// 调度记录表ID
        /// </summary>
        public long? ScheduleId { get; set; }

        /// <summary>
        /// 运输任务表ID
        /// </summary>
        public long? TransportationTaskId { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string? CreatorName { get; set; }

        /// <summary>
        /// 创建时间-开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 创建时间-结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 修改时间-开始时间
        /// </summary>
        public DateTime? ModifyStartTime { get; set; }

        /// <summary>
        /// 修改时间-结束时间
        /// </summary>
        public DateTime? ModifyEndTime { get; set; }

        /// <summary>
        /// 数据排序方式
        /// 1: ID正序, 2: ID倒序
        /// </summary>
        public int? QueryOrderBy { get; set; }

        /// <summary>
        /// 告警标识(0:无告警,1:有告警)
        /// </summary>
        public bool? IsWarning { get; set; }

    }
}
