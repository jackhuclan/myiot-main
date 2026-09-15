namespace VgAutoDrill.Central.Core.Mes.Model;

public class WorkOrderTaskResponse
{
    public string Message { get; set; } = string.Empty;
    public WorkOrderTask? Data { get; set; }
}
