
namespace VgAutoDrill.Fundation.Iot.Configuration;

public class DeviceGatewayOptions
{
    /// <summary>
    /// agent's name
    /// </summary>
    public string AppName { get; set; } = string.Empty;
    /// <summary>
    /// windows service name
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;
    /// <summary>
    /// agent's version as 1.0.0.0
    /// </summary>
    public string AppVersion { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string FundationVersion { get; set; } = string.Empty;
    public string InstalledLocation { get; set; } = string.Empty;
    /// <summary>
    /// agent's host ip address & port, ex: http://192.168.1.100:5257
    /// </summary>
    public string HostAddress { get; set; } = string.Empty;
    /// <summary>
    /// 以间隔多少时间通知中控，如：5ms,5m,5s,5h,5d
    /// </summary>
    public TimeSpan NoticeCentralHeartbeat { get; set; } = TimeSpan.FromMinutes(5);
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; } = false;
    /// <summary>
    /// 是否在线
    /// </summary>
    public bool Online { get; set; } = false;
}
