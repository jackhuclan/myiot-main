using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 调度记录明细
    ///</summary>
    [SugarTable("t_schedule_log")]
    public class ScheduleLog : BaseEntity
    {
        /// <summary>
        /// 主表Id
        /// </summary>
        [SugarColumn(ColumnName = "master_id")]
        public virtual long? MasterId { get; set; }

        /// <summary>
        /// 调度记录明细
        /// </summary>
        [SugarColumn(ColumnName = "message")]
        public virtual string? Message { get; set; }

        /// <summary>
        /// 轴号
        /// </summary>
        [SugarColumn(ColumnName = "spindle")]
        public virtual int? Spindle { get; set; }
    }
}
