namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DrillWorkOrder
{
    public class GetMaterialDataReq : Page
    {
        /// <summary>
        /// 生产工单编号
        /// </summary>
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 产品物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }
    }
}
