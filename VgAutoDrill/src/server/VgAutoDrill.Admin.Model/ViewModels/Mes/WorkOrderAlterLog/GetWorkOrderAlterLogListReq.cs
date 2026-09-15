namespace VgAutoDrill.Admin.Model.ViewModels.Mes.WorkOrderAlterLog
{
    public class GetWorkOrderAlterLogListReq : Page
    {
        /// <summary>
        /// 工单编码
        /// </summary>
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }
    }
}
