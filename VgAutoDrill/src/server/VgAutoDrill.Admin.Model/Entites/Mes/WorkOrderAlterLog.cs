using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 工单变更记录表
    ///</summary>
    [SugarTable("t_work_order_alter_log")]
    public class WorkOrderAlterLog : BaseEntity
    {
        /// <summary>
        /// 工单编码
        /// </summary>
        [SugarColumn(ColumnName = "work_order_code")]
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        [SugarColumn(ColumnName = "device_code")]
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        [SugarColumn(ColumnName = "item_code")]
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [SugarColumn(ColumnName = "action_time")]
        public virtual DateTime? ActionTime { get; set; }

        /// <summary>
        /// 具体操作
        /// </summary>
        [SugarColumn(ColumnName = "action_detail")]
        public virtual string? ActionDetail { get; set; }

        /// <summary>
        /// 转换前文件路径
        /// </summary>
        [SugarColumn(ColumnName = "before_drill_file_path")]
        public virtual string? BeforeDrillFilePath { get; set; }

        /// <summary>
        /// 转换后文件路径
        /// </summary>
        [SugarColumn(ColumnName = "after_drill_file_path")]
        public virtual string? AfterDrillFilePath { get; set; }
    }
}
