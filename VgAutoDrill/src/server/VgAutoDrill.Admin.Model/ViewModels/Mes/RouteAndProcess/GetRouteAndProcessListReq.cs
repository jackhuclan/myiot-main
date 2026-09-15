namespace VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess
{
    public class GetRouteAndProcessListReq : Page
    {
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public virtual long? RouteId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public virtual long? ProcessId { get; set; }
    }
}
