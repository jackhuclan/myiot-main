namespace VgAutoDrill.Admin.Model.ViewModels.Mes.FeedBack
{
    public class FeedBackDto : BaseDto
    {
        /// <summary>
        /// 报工类型
        /// </summary>
        public virtual string? FeedBackType { get; set; }

        /// <summary>
        /// 工作站ID
        /// </summary>
        public virtual long? WorkStationId { get; set; }

        /// <summary>
        /// 工作站编号
        /// </summary>
        public virtual string? WorkStationCode { get; set; }

        /// <summary>
        /// 工作站名称
        /// </summary>
        public virtual string? WorkStationName { get; set; }

        /// <summary>
        /// 生产工单ID
        /// </summary>
        public virtual long? WorkOrderId { get; set; }

        /// <summary>
        /// 生产工单编号
        /// </summary>
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 生产工单名称
        /// </summary>
        public virtual string? WorkOrderName { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public virtual long? ProcessId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public virtual string? ProcessCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public virtual string? ProcessName { get; set; }

        /// <summary>
        /// 生产任务ID
        /// </summary>
        public virtual long? TaskId { get; set; }

        /// <summary>
        /// 生产任务编号
        /// </summary>
        public virtual string? TaskCode { get; set; }

        /// <summary>
        /// 产品物料ID
        /// </summary>
        public virtual long? ItemId { get; set; }

        /// <summary>
        /// 产品物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 产品物料名称
        /// </summary>
        public virtual string? ItemName { get; set; }

        /// <summary>
        /// 物料产品类型Id
        /// </summary>
        public virtual long? ItemTypeId { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public virtual string? UnitOfMeasure { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public virtual string? Specification { get; set; }

        /// <summary>
        /// 排产数量
        /// </summary>
        public virtual decimal? Quantity { get; set; }

        /// <summary>
        /// 本次报工数量
        /// </summary>
        public virtual decimal? QuantityFeedBack { get; set; }

        /// <summary>
        /// 良品数量
        /// </summary>
        public virtual decimal? QuantityQualified { get; set; }

        /// <summary>
        /// 不良品数量
        /// </summary>
        public virtual decimal? QuantityUnQuanlified { get; set; }

        /// <summary>
        /// 报工用户名
        /// </summary>
        public virtual string? UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string? NickName { get; set; }

        /// <summary>
        /// 报工途径
        /// </summary>
        public virtual string? FeedBackChannel { get; set; }

        /// <summary>
        /// 报工时间 
        ///</summary>
        public virtual DateTime? FeedBackTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }

        /// <summary>
        /// 完工状态(DRAFT/COMMITED)
        /// </summary>
        public virtual string? FeedBackStatus { get; set; }

        /// <summary>
        /// 是否关键工序(0/1)
        /// </summary>
        public virtual string? KeyFlag { get; set; }
    }
}
