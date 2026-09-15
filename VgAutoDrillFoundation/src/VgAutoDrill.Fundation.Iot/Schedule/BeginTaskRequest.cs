namespace VgAutoDrill.Fundation.Iot.Schedule;

public class BeginTaskRequest
{
    public string ProductId { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string TraceId { get; set; } = string.Empty;
    public int? RealCount { get; set; } = 0;
    public Dictionary<string, object?> Params { get; set; } = new Dictionary<string, object?>();
}
