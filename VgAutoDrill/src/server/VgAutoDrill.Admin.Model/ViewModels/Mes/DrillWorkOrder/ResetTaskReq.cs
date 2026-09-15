namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DrillWorkOrder
{
    public class ResetTaskReq
    {
        public List<ResetTaskData>? ResetReq { get; set; }

        public virtual string? ProcessCode { get; set; }
    }

    public class ResetTaskData
    {
        public virtual string? WorkOrderCode { get; set; }

        public virtual string? ItemCode { get; set; }

    }
}
