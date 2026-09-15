namespace VgAutoDrill.Admin.Model.ViewModels.Mes.RouteProcessAndWorkStation
{
    public class RouteAndWorkStationInfo
    {
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public virtual long? RouteId { get; set; }

        /// <summary>
        /// 工艺路线编号
        /// </summary>
        public virtual string? RouteCode { get; set; }

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public virtual string? RouteName { get; set; }

        /// <summary>
        /// 工作站ID
        /// </summary>
        public virtual long? WorkStationId { get; set; }

        /// <summary>
        /// 工作站编号 
        ///</summary>   
        public virtual string? WorkStationCode { get; set; }

        /// <summary>
        /// 工作站名称
        /// </summary>
        public virtual string? WorkStationName { get; set; }
    }
}
