using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 生产工单与工作站关系表
    ///</summary>
    [SugarTable("t_workorder_and_workstation")]
    public class WorkOrderAndWorkStation : BaseEntity
    {

        [SugarColumn(ColumnName = "work_order_id")]
        public virtual long? WorkOrderId { get; set; }
        /// <summary>
        /// 生产工单编号
        /// </summary>
        [SugarColumn(ColumnName = "work_order_code")]
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 工作站ID
        /// </summary>
        [SugarColumn(ColumnName = "work_station_id")]
        public virtual long? WorkStationId { get; set; }

        /// <summary>
        /// 工作站编码
        /// </summary>
        [SugarColumn(ColumnName = "work_station_code")]
        public virtual string? WorkStationCode { get; set; }

        /// <summary>
        /// 工作站名称
        /// </summary>
        [SugarColumn(ColumnName = "work_station_name")]
        public virtual string? WorkStationName { get; set; }
    }
}
