namespace VgAutoDrill.Fundation.Iot.Models;

public class DevicePanelChangedRequest
{
    private string? _locationCode;

    public string ProductId { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public PanelList PanelList { get; set; } = new();

    /// <summary>
    /// 请求发出的库位编号
    /// </summary>
    public string LocationCode
    {
        get
        {
            if (string.IsNullOrEmpty(_locationCode))
            {
                _locationCode = PanelList.LocationCode;
            }

            return _locationCode;
        }
        set
        {
            _locationCode = value;
            PanelList.SetLocationCode(value);
        }
    }
}
