namespace VgAutoDrill.Central.Core.Mes.Model;

public class WorkOrderTaskRequest
{
    public string DeviceId { get; set; }
    public int RealNeedCount { get; set; }
    public int ExistRawNum { get; set; }
    public int SpindleUseNum { get; set; }
    public List<string> UndrilledItemCodesFromDrill { get; set; } = new();
}
