using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 工艺路线与工序关系表
    ///</summary>
    [SugarTable("t_route_and_process")]
    public class RouteAndProcess : BaseEntity
    {
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        [SugarColumn(ColumnName = "route_id")]
        public virtual long? RouteId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        [SugarColumn(ColumnName = "process_id")]
        public virtual long? ProcessId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(ColumnName = "order_num")]
        public virtual long? OrderNum { get; set; }

        /// <summary>
        /// TASK的颜色
        /// </summary>
        [SugarColumn(ColumnName = "color")]
        public virtual string? Color { get; set; }

        /// <summary>
        /// 是否关键工序(0/1)
        /// </summary>
        [SugarColumn(ColumnName = "key_flag")]
        public virtual string? KeyFlag { get; set; }

        /// <summary>
        /// 本工序耗时
        /// </summary>
        [SugarColumn(ColumnName = "required_time")]
        public virtual int? RequiredTime { get; set; }

        /// <summary>
        /// 是否需要手动检查(0/1)
        /// </summary>
        [SugarColumn(ColumnName = "is_manual_check")]
        public virtual string? IsManualCheck { get; set; }

        /// <summary>
        /// 自检数量
        /// </summary>
        [SugarColumn(ColumnName = "self_check_num")]
        public virtual int? SelfCheckNum { get; set; }
    }
}
