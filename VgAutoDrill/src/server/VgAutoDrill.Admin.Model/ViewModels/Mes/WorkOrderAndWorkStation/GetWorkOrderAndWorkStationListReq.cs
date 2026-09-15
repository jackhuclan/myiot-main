namespace VgAutoDrill.Admin.Model.ViewModels.Mes.WorkOrderAndWorkStation
{
    public class GetWorkOrderAndWorkStationListReq : Page
    {
        /// <summary>
        /// 生产工单ID
        /// </summary>
        public virtual long? WorkOrderId { get; set; }
        /// <summary>
        /// 生产工单编号
        /// </summary>
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 工作站ID
        /// </summary>
        public virtual long? WorkStationId { get; set; }

        /// <summary>
        /// 工作站编码
        /// </summary>
        public virtual string? WorkStationCode { get; set; }

        /// <summary>
        /// 工作站名称
        /// </summary>
        public virtual string? WorkStationName { get; set; }

        /// <summary>
        /// 任务等待数量
        /// </summary>
        public virtual int? WaitTaskCount { get; set; }
    }
}
