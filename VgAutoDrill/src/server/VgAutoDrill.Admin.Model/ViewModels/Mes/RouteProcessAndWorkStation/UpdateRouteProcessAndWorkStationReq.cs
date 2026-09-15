namespace VgAutoDrill.Admin.Model.ViewModels.Mes.RouteProcessAndWorkStation
{
    public class UpdateRouteProcessAndWorkStationReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 工作站ID
        /// </summary>
        public virtual long? WorkStationId { get; set; }

        /// <summary>
        /// 工艺路线与工序关系ID
        /// </summary>
        public virtual long? RouteAndProcessId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public virtual long? OrderNum { get; set; }
    }
}
