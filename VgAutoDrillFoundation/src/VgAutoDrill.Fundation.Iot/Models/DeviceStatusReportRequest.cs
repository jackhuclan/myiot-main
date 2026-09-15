using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Fundation.Iot.Models;

public class DeviceStatusReportRequest
{
    private string? _locationCode;

    public string ProductId { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public DeviceStatus OldStatus { get; set; }
    public DeviceStatus NewStatus { get; set; }
    public PanelList PayloadPanels { get; set; } = new();
    public CutterTrays PayloadCutterTrays { get; set; } = new();
    /// <summary>
    /// 请求发出的库位编号
    /// </summary>
    public string LocationCode
    {
        get
        {
            if (string.IsNullOrEmpty(_locationCode))
            {
                _locationCode = Params.ContainsKey("DeviceCode") ? Params["DeviceCode"].ToStr() : PayloadPanels.LocationCode;
            }

            return _locationCode;
        }
        set
        {
            _locationCode = value;
            PayloadPanels.SetLocationCode(value);
        }
    }

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
