using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace VgAutoDrill.Fundation.Iot.Schedule;

public class AddScheduleLogRequest
{
    public string DeviceId { get; set; } = string.Empty;
    public string TraceId { get; set; } = string.Empty;
    /// <summary>
    /// 轴号
    /// </summary>
    public int? Spindle { get; set; }
    public string? Message { get; set; }
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
    }
}
