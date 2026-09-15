namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalXianJin
{
    public class DrillTaskDto
    {
        public string? DeviceId { get; set; }

        public List<DrillTaskInfo>? DrillTaskInfos { get; set; }
    }

    public class DrillTaskInfo
    {
        public string? WorkOrderCode { get; set; }

        public string? TaskCode { get; set; }

        public string? ItemCode { get; set; }

        public string? BarCode { get; set; }
    }
}
