using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Iot.Schedule;

public class UpdateScheduleTaskRequest
{
    public string TraceId { get; set; } = string.Empty;
    public string EventId { get; set; } = string.Empty;
    public InteractionBehavior InteractionBehavior { get; set; } = InteractionBehavior.Noop;
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
    }
}
