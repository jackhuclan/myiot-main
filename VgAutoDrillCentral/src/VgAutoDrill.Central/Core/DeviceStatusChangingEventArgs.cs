using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Central.Core;

/// <summary>
/// 设备状态发生改变前的事件参数
/// </summary>
public class DeviceStatusChangingEventArgs : EventArgs
{
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public DeviceStatus Status { get; set; } = DeviceStatus.Unknown;
    public DateTime StartTime { get; set; } = DateTime.Now;
    public DateTime EndTime { get; set; } = DateTime.Now;
}
