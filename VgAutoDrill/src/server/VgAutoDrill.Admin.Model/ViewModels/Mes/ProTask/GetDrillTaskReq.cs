namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask
{
    public class GetDrillTaskReq
    {
        /// <summary>
        /// 生产工单编号
        /// </summary>
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public virtual string? ProcessCode { get; set; }

        /// <summary>
        /// 展示的任务天数
        /// </summary>
        public virtual int? TaskNumber { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 工艺路线编码集合
        /// </summary>
        public virtual List<string>? RouteCodeList { get; set; }

        /// <summary>
        /// 是否紧急插单
        /// </summary>
        public virtual int? IsUrgent { get; set; }

        /// <summary>
        /// 是否查询历史为完成工单
        /// </summary>
        public virtual bool? IsHistory { get; set; } = false;

        /// <summary>
        /// 自动机器还是手动机器
        /// </summary>
        public virtual bool? IsAuto { get; set; }
        /// <summary>
        /// 任务状态列表
        /// </summary>
        public virtual List<string>? TaskStatusList { get; set; }
    }
}
