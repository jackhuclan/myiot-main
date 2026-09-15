using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace VgAutoDrill.Fundation.Iot.Models;

/// <summary>
/// 钻刀换刀信息
/// </summary>
public class ApplyCutterArrangeResponse
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public CutterArrange? Data { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
    }
}
