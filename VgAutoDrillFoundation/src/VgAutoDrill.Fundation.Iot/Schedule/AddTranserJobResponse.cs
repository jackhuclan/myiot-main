using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace VgAutoDrill.Fundation.Iot.Schedule;

public class AddTranserJobResponse
{
    public string? DeviceId { get; set; }
    public long TransferJobId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
    }
}
