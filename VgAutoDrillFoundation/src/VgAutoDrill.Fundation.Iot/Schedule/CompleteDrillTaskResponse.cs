using System.Text.Json;

namespace VgAutoDrill.Fundation.Iot.Schedule;

[Obsolete("请使用FinishTaskResponse代替")]
public class CompleteDrillTaskResponse
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
