namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DrillWorkOrder
{
    public class CommitDrillWorkOrderReq
    {
        /// <summary>
        /// 生产工单编号
        /// </summary>
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 工序编号
        /// </summary>
        public virtual string? ProcessCode { get; set; }
    }
}
