using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Fundation.Iot.Schedule;

public class CancelScheduleTaskRequest
{
    public string ProductId { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string TraceId { get; set; } = string.Empty;
    public Dictionary<string, object?> Params { get; set; } = new Dictionary<string, object?>();
    /// <summary>
    /// 请求发出的库位编号
    /// </summary>
    public string LocationCode
    {
        get
        {
            return Params.ContainsKey("DeviceCode") ? Params["DeviceCode"].ToStr() : string.Empty;
        }
    }
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
    }
}
