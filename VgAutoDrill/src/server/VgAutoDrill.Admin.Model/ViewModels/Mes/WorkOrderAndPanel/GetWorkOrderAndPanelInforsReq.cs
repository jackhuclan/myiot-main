namespace VgAutoDrill.Admin.Model.ViewModels.Mes.WorkOrderAndPanel
{
    public class GetWorkOrderAndPanelInforsReq
    {
        /// <summary>
        /// 工单
        /// </summary>
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// task
        /// </summary>
        public virtual string? TaskCode { get; set; }
        /// <summary>
        /// 料号
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 板材编码
        /// </summary>
        public virtual string? PanelCode { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        public virtual string? BatchCode { get; set; }

        /// <summary>
        /// 外部工单
        /// </summary>
        public virtual string? ExternalWorkerOrder { get; set; }
    }
}
