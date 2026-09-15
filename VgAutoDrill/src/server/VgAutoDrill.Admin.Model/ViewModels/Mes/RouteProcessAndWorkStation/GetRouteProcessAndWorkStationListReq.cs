namespace VgAutoDrill.Admin.Model.ViewModels.Mes.RouteProcessAndWorkStation
{
    public class GetRouteProcessAndWorkStationListReq : Page
    {
        /// <summary>
        /// 工作站ID
        /// </summary>
        public virtual long? WorkStationId { get; set; }
        public virtual string? WorkStationCode { get; set; }

        /// <summary>
        /// 工艺路线与工序关系ID
        /// </summary>
        public virtual long? RouteAndProcessId { get; set; }
    }
}
