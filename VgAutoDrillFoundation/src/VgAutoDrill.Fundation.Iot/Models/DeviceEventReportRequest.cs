using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Fundation.Iot.Models;

public class DeviceEventReportRequest
{
    private string _locationCode = string.Empty;

    public string ProductId { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string EventId { get; set; } = string.Empty;
    public string? TraceId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public ushort RequestInteractionBehavior { get; set; } = InteractionBehavior.Noop;

    /// <summary>
    /// agv上下料交互方向
    /// </summary>
    public InteractionPosition RequestInteractionDirection { get; set; } = InteractionPosition.Rear;

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

    /// <summary>
    /// 请求发出的分区
    /// </summary>
    public string? PartitionCode
    {
        get
        {
            return Params.ContainsKey("PartitionCode") ? Params["PartitionCode"].ToStr() : string.Empty;
        }
        set
        {
            Params["PartitionCode"] = value;
        }
    }

    public DeviceKind RequestDeviceKind { get; set; } = DeviceKind.Unknown;
    public MaterialKind RequestMaterialKind { get; set; } = MaterialKind.Panel;
    public ProductStatus RequestInputProductStatus { get; set; } = ProductStatus.Noop;
    public ProductStatus RequestOutputProductStatus { get; set; } = ProductStatus.Noop;
    public PanelList PayloadPanels { get; set; } = new();
    public CutterTrays PayloadCutterTrays { get; set; } = new();
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
