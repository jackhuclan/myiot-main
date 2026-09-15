namespace VgAutoDrill.Admin.Model.ViewModels.Mes.RouteProcessAndWorkStation
{
    public class AddRouteProcessAndWorkStationReq
    {
        /// <summary>
        /// 工作站集合
        /// </summary>
        public virtual List<WorkStationOrderNumInfo> WorkStationIds { get; set; } = new List<WorkStationOrderNumInfo>();

        /// <summary>
        /// 工艺路线与工序关系ID
        /// </summary>
        public virtual long? RouteAndProcessId { get; set; }
    }

    public class WorkStationOrderNumInfo
    {
        /// <summary>
        /// 工作站ID
        /// </summary>
        public virtual long? WorkStationId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public virtual long? OrderNum { get; set; }
    }
}
