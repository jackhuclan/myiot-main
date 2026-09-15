namespace VgAutoDrill.Admin.Model.ViewModels.Mes.FeedBack
{
    public class GetFeedBackListReq : Page
    {
        /// <summary>
        /// 报工类型
        /// </summary>
        public virtual string? FeedBackType { get; set; }

        /// <summary>
        /// 工作站编号
        /// </summary>
        public virtual string? WorkStationCode { get; set; }

        /// <summary>
        /// 工作站名称
        /// </summary>
        public virtual string? WorkStationName { get; set; }

        /// <summary>
        /// 生产工单编号
        /// </summary>
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 生产工单名称
        /// </summary>
        public virtual string? WorkOrderName { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public virtual string? ProcessCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public virtual string? ProcessName { get; set; }

        /// <summary>
        /// 生产任务编号
        /// </summary>
        public virtual string? TaskCode { get; set; }

        /// <summary>
        /// 物料产品类型Id
        /// </summary>
        public virtual long? ItemTypeId { get; set; }

        /// <summary>
        /// 产品物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 产品物料名称
        /// </summary>
        public virtual string? ItemName { get; set; }

        /// <summary>
        /// 完工状态(DRAFT/COMMITED)
        /// </summary>
        public virtual string? FeedBackStatus { get; set; }
    }
}
