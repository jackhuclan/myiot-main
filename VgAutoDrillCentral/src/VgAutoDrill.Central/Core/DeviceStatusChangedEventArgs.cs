using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Central.Core;

/// <summary>
/// 设备状态发生改变后的事件参数
/// </summary>
public class DeviceStatusChangedEventArgs : EventArgs
{
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public DeviceStatus OldStatus { get; set; } = DeviceStatus.Unknown;
    public DeviceStatus NewStatus { get; set; } = DeviceStatus.Unknown;
}
