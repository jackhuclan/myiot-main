using System.Text.Json;

namespace VgAutoDrill.Fundation.Iot.Schedule;

[Obsolete("请使用FinishTaskRequest代替")]
public class CompleteDrillTaskRequest
{
    public string ProductId { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string TraceId { get; set; } = string.Empty;
    public Dictionary<string, object?> Params { get; set; } = new Dictionary<string, object?>();
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
