using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 工序与工作站表
    ///</summary>
    [SugarTable("t_route_process_and_work_station")]
    public class RouteProcessAndWorkStation : BaseEntity
    {
        /// <summary>
        /// 工作站ID
        /// </summary>
        [SugarColumn(ColumnName = "work_station_id")]
        public virtual long? WorkStationId { get; set; }

        /// <summary>
        /// 工艺路线与工序关系ID
        /// </summary>
        [SugarColumn(ColumnName = "route_and_process_id")]
        public virtual long? RouteAndProcessId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(ColumnName = "order_num")]
        public virtual long? OrderNum { get; set; }
    }
}
