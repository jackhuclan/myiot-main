using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 检验记录表
    ///</summary>
    [SugarTable("t_check_records")]
    public class CheckRecords : BaseEntity
    {
        /// <summary>
        /// 生产任务ID
        /// </summary>
        [SugarColumn(ColumnName = "task_id")]
        public virtual long? TaskId { get; set; }

        /// <summary>
        /// 生产任务名称
        /// </summary>
        [SugarColumn(ColumnName = "task_name")]
        public virtual string? TaskName { get; set; }

        /// <summary>
        /// 生产任务编号
        /// </summary>
        [SugarColumn(ColumnName = "task_code")]
        public virtual string? TaskCode { get; set; }

        /// <summary>
        /// 生产工单ID
        /// </summary>
        [SugarColumn(ColumnName = "work_order_id")]
        public virtual long? WorkOrderId { get; set; }

        /// <summary>
        /// 生产工单名称
        /// </summary>
        [SugarColumn(ColumnName = "work_order_name")]
        public virtual string? WorkOrderName { get; set; }

        /// <summary>
        /// 生产工单编号
        /// </summary>
        [SugarColumn(ColumnName = "work_order_code")]
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 检验人员ID
        /// </summary>
        [SugarColumn(ColumnName = "user_id")]
        public virtual long? UserId { get; set; }

        /// <summary>
        /// 检验人员姓名
        /// </summary>
        [SugarColumn(ColumnName = "user_name")]
        public virtual string? UserName { get; set; }

        /// <summary>
        /// 是否检验合格(-1/0/1)
        /// </summary>
        [SugarColumn(ColumnName = "is_check_ok")]
        public virtual string? IsCheckOk { get; set; }

        /// <summary>
        /// 检验日期
        /// </summary>
        [SugarColumn(ColumnName = "check_time")]
        public virtual DateTime? CheckTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark")]
        public virtual string? Remark { get; set; }
    }
}
