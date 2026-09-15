using VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder;

namespace VgAutoDrill.External.Model.V3.ExternalWorkOrder
{
    public class ExternalWorkOrderReq : ExternalBaseReq
    {

    }

    /// <summary>
    /// 工单接口相关Param
    /// </summary>
    public class WorkOrderParameter
    {
        /// <summary>
        /// 新增工单
        /// </summary>
        public virtual List<AddWorkOrderParam>? AddWorkOrderParams { get; set; }

        /// <summary>
        /// 修改工单
        /// </summary>
        public virtual List<UpdateWorkOrderParam>? UpdateWorkOrderParams { get; set; }

        /// <summary>
        /// 删除工单
        /// </summary>
        public virtual List<int>? DeleteWorkOrderIDList { get; set; }

        /// <summary>
        /// 查询工单
        /// </summary>
        public virtual SelectWorkOrderParam? SelectWorkOrderParam { get; set; }
    }

    /// <summary>
    /// 新增
    /// </summary>
    public class AddWorkOrderParam : AddOrUpdateWorkOrderReq
    {

    }

    /// <summary>
    /// 更新
    /// </summary>
    public class UpdateWorkOrderParam : AddOrUpdateWorkOrderReq
    {

    }

    /// <summary>
    /// 查询工单信息
    /// </summary>
    public class SelectWorkOrderParam
    {
        /// <summary>
        /// 外部工单编号
        public virtual string? SourceCode { get; set; }
        /// <summary>
        /// 内部工单号
        /// </summary>
        public virtual string? InnerCode { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public virtual string? IncodeNumber { get; set; }
    }
}
