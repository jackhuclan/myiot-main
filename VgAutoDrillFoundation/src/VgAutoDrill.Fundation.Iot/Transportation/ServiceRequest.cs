using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace VgAutoDrill.Fundation.Iot.Transportation;

public class ServiceRequest
{
    public string ProductId { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string TraceId { get; set; } = string.Empty;
    public Dictionary<string, object?> Params { get; set; } = new Dictionary<string, object?>();
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
    }
}
