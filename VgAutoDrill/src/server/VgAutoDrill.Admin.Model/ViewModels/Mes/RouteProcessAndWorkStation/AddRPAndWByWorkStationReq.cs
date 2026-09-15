using VgAutoDrill.Admin.Model.ViewModels.Mes.Route;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.RouteProcessAndWorkStation
{
    public class AddRPAndWByWorkStationReq
    {
        /// <summary>
        /// 工艺路线集合
        /// </summary>
        public virtual List<RouteInfo> RouteInfos { get; set; } = new List<RouteInfo>();

        /// <summary>
        /// 工作站ID
        /// </summary>
        public virtual long? WorkStationId { get; set; }
    }
}
