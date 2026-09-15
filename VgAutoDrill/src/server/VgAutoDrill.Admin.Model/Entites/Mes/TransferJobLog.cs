using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 料仓任务明细
    /// </summary>
    [SugarTable("t_transportation_task_log")]
    public class TransferJobLog : BaseEntity
    {
        /// <summary>
        /// 主表Id
        /// </summary>
        [SugarColumn(ColumnName = "master_id")]
        public virtual long? MasterId { get; set; }

        /// <summary>
        /// 料仓任务明细
        /// </summary>
        [SugarColumn(ColumnName = "message")]
        public virtual string? Message { get; set; }
    }
}
