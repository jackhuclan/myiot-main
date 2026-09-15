using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 工作站表
    ///</summary>
    [SugarTable("t_workstation")]
    public class WorkStation : BaseEntity
    {
        [SugarColumn(ColumnName = "code")]
        public virtual string? Code { get; set; }

        [SugarColumn(ColumnName = "name")]
        public virtual string? Name { get; set; }
        /// <summary>
        /// 所在车间ID
        /// </summary>
        [SugarColumn(ColumnName = "workshop_id")]
        public virtual int? WorkshopId { get; set; }
        /// <summary>
        /// 所在车间编码
        /// </summary>
        [SugarColumn(ColumnName = "workshop_code")]
        public virtual string? WorkshopCode { get; set; }
        /// <summary>
        /// 所在车间名称
        /// </summary>
        [SugarColumn(ColumnName = "workshop_name")]
        public virtual string? WorkshopName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark")]
        public virtual string? Remark { get; set; }
        /// <summary>
        /// 默认工序ID
        /// </summary>
        [SugarColumn(ColumnName = "process_id")]
        public virtual long? ProcessId { get; set; }
        /// <summary>
        /// 默认工序编码
        /// </summary>
        [SugarColumn(ColumnName = "process_code")]
        public virtual string? ProcessCode { get; set; }
        /// <summary>
        /// 默认工序名称
        /// </summary>
        [SugarColumn(ColumnName = "process_name")]
        public virtual string? ProcessName { get; set; }
    }
}
