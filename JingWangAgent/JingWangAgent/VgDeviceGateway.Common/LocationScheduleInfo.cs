using VgAutoDrill.Fundation.Iot;

namespace VgDeviceGateway.Devices.Common;

/// <summary>
/// 库位调度信息
/// </summary>
public class LocationScheduleInfo
{
    /// <summary>
    /// 库位编号
    /// </summary>
    public string LocationCode { get; set; } = string.Empty;

    public string SiloCode { get; set; } = string.Empty;

    public string DeviceId { get; set; } = string.Empty;
    public PanelList Panels { get; set; } = PanelList.Empty;
    public string? TraceId { get; set; } = string.Empty;
    public string InnerPoint { get; set; } = string.Empty;

    public string OutPoint { get; set; } = string.Empty;

    public string TransInnerPoint { get; set; } = string.Empty;

    public string TransOutPoint { get; set; } = string.Empty;
}
