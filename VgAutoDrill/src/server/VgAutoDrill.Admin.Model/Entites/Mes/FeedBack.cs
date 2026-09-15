using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 生产报工记录
    ///</summary>
    [SugarTable("t_feedback")]
    public class FeedBack : BaseEntity
    {
        /// <summary>
        /// 报工类型
        /// </summary>
        [SugarColumn(ColumnName = "feedback_type")]
        public virtual string? FeedBackType { get; set; }

        /// <summary>
        /// 工作站ID
        /// </summary>
        [SugarColumn(ColumnName = "workstation_id")]
        public virtual long? WorkStationId { get; set; }

        /// <summary>
        /// 工作站编号
        /// </summary>
        [SugarColumn(ColumnName = "workstation_code")]
        public virtual string? WorkStationCode { get; set; }

        /// <summary>
        /// 工作站名称
        /// </summary>
        [SugarColumn(ColumnName = "workstation_name")]
        public virtual string? WorkStationName { get; set; }

        /// <summary>
        /// 生产工单ID
        /// </summary>
        [SugarColumn(ColumnName = "workorder_id")]
        public virtual long? WorkOrderId { get; set; }

        /// <summary>
        /// 生产工单编号
        /// </summary>
        [SugarColumn(ColumnName = "workorder_code")]
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 生产工单名称
        /// </summary>
        [SugarColumn(ColumnName = "workorder_name")]
        public virtual string? WorkOrderName { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        [SugarColumn(ColumnName = "process_id")]
        public virtual long? ProcessId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        [SugarColumn(ColumnName = "process_code")]
        public virtual string? ProcessCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        [SugarColumn(ColumnName = "process_name")]
        public virtual string? ProcessName { get; set; }

        /// <summary>
        /// 生产任务ID
        /// </summary>
        [SugarColumn(ColumnName = "task_id")]
        public virtual long? TaskId { get; set; }

        /// <summary>
        /// 生产任务编号
        /// </summary>
        [SugarColumn(ColumnName = "task_code")]
        public virtual string? TaskCode { get; set; }

        /// <summary>
        /// 产品物料ID
        /// </summary>
        [SugarColumn(ColumnName = "item_id")]
        public virtual long? ItemId { get; set; }

        /// <summary>
        /// 产品物料编码
        /// </summary>
        [SugarColumn(ColumnName = "item_code")]
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 产品物料名称
        /// </summary>
        [SugarColumn(ColumnName = "item_name")]
        public virtual string? ItemName { get; set; }

        /// <summary>
        /// 物料产品类型Id
        /// </summary>
        [SugarColumn(ColumnName = "item_type_id")]
        public virtual long? ItemTypeId { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [SugarColumn(ColumnName = "unit_of_measure")]
        public virtual string? UnitOfMeasure { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        [SugarColumn(ColumnName = "specification")]
        public virtual string? Specification { get; set; }
        /// <summary>
        /// 叠板层数
        /// </summary>
        [SugarColumn(ColumnName = "panel_count")]
        public decimal? PanelCount { get; set; } = 1;
        /// <summary>
        /// 排产数量
        /// </summary>
        [SugarColumn(ColumnName = "quantity")]
        public virtual decimal? Quantity { get; set; }

        /// <summary>
        /// 本次报工数量
        /// </summary>
        [SugarColumn(ColumnName = "quantity_feedback")]
        public virtual decimal? QuantityFeedBack { get; set; }

        /// <summary>
        /// 良品数量
        /// </summary>
        [SugarColumn(ColumnName = "quantity_qualified")]
        public virtual decimal? QuantityQualified { get; set; }

        /// <summary>
        /// 不良品数量
        /// </summary>
        [SugarColumn(ColumnName = "quantity_unquanlified")]
        public virtual decimal? QuantityUnQuanlified { get; set; }

        /// <summary>
        /// 报工用户名
        /// </summary>
        [SugarColumn(ColumnName = "user_name")]
        public virtual string? UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [SugarColumn(ColumnName = "nick_name")]
        public virtual string? NickName { get; set; }

        /// <summary>
        /// 报工途径
        /// </summary>
        [SugarColumn(ColumnName = "feedback_channel")]
        public virtual string? FeedBackChannel { get; set; }

        /// <summary>
        /// 报工时间 
        ///</summary>
        [SugarColumn(ColumnName = "feedback_time")]
        public virtual DateTime? FeedBackTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark")]
        public virtual string? Remark { get; set; }

        /// <summary>
        /// 完工状态(DRAFT/COMMITED)
        /// </summary>
        [SugarColumn(ColumnName = "feedback_status")]
        public virtual string? FeedBackStatus { get; set; } = "DRAFT";

        /// <summary>
        /// 是否关键工序(0/1)
        /// </summary>
        [SugarColumn(ColumnName = "key_flag")]
        public virtual string? KeyFlag { get; set; }
    }
}
