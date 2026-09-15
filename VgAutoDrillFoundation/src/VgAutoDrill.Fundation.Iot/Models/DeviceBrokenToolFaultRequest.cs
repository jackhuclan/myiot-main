using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace VgAutoDrill.Fundation.Iot.Models;

/// <summary>
/// 设备刀具故障上报
/// </summary>
public class DeviceBrokenToolFaultRequest
{
    public string ProductId { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    /// <summary>
    /// 故障原因
    /// </summary>
    public string FaultReason { get; set; } = string.Empty;
    public CutterTrays CutterTrayList { get; set; } = new();
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
    }
}
