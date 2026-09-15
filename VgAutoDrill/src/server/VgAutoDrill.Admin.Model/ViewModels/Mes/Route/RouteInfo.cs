namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Route
{
    public class RouteInfo : RouteDto
    {
        /// <summary>
        /// 工艺路线关联工序ID
        /// </summary>
        public virtual long? RouteAndProcessId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public virtual long? ProcessId { get; set; }

        /// <summary>
        /// 工艺路线、工序关联工作站ID
        /// </summary>
        public virtual long? RouteProcessAndWorkStationId { get; set; }
    }
}
