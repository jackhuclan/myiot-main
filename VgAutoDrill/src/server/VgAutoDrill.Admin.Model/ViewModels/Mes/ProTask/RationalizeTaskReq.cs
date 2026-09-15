namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask
{
    /// <summary>
    /// 理顺任务
    /// </summary>
    public class RationalizeTaskReq
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 生产工单编号
        /// </summary>
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 工作站编号
        /// </summary>
        public virtual string? WorkStationCode { get; set; }

        /// <summary>
        /// 工序编号
        /// </summary>
        public virtual string? ProcessCode { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public virtual long? ItemId { get; set; }

        /// <summary>
        /// 工作站编号集合
        /// </summary>
        public virtual List<string>? WorkStationList { get; set; }
    }
}
