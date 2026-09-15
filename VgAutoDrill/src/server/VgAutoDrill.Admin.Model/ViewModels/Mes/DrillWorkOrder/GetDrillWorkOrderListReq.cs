namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DrillWorkOrder
{
    public class GetDrillWorkOrderListReq : Page
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
        /// 产品物料ID
        /// </summary>
        public virtual long? ItemId { get; set; }

        /// <summary>
        /// 产品物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

    }
}
