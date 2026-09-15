using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Iot.Configuration;

public class DeviceDescriptor
{
    public const string Options = "DeviceOptions";
    private volatile bool _autoMode = false;
    private volatile int _layerLimit = 18;
    private volatile int _lotLimit = 144;
    private volatile int _panelLimit = 18;
    private volatile int _siloLimit = 1;
    private volatile int _spindleNum = 1;
    private volatile int _dataCollectingPerSeconds = 10;
    private volatile uint _keepingPlcConnectionPerMilliSeconds = 500;
    private volatile int _scheduleTaskExecutingPerSeconds = 10;

    public Dictionary<string, object> Extra { get; set; } = new Dictionary<string, object>();
    public string ProductId { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    /// <summary>
    /// device's host ip address & port, ex: http://192.168.1.100:8004
    /// </summary>
    public string HostAddress { get; set; } = string.Empty;
    public uint HostPort { get; set; } = 8004;
    public bool AutoMode { get => _autoMode; set => _autoMode = value; }
    /// <summary>
    /// the interacted device will return success directly when soloMode is true.
    /// </summary>
    public bool SoloMode { get; set; } = false;
    /// <summary>
    /// allow Data Report or not, default is false
    /// </summary>
    public bool ExternalDataReportEnabled { get; set; } = false;
    public string DeviceClazz { get; set; } = string.Empty;
    /// <summary>
    /// 调度任务延迟执行时间，默认3秒
    /// </summary>
    public int ScheduleTaskDelaySeconds { get; set; } = 3;
    public string DeviceDllFilePath { get; set; } = string.Empty;
    public int DataCollectingPerSeconds { get => _dataCollectingPerSeconds; set => _dataCollectingPerSeconds = value; }
    public uint KeepingPlcConnectionPerMilliSeconds { get => _keepingPlcConnectionPerMilliSeconds; set => _keepingPlcConnectionPerMilliSeconds = value; }
    public int ScheduleTaskExecutingPerSeconds { get => _scheduleTaskExecutingPerSeconds; set => _scheduleTaskExecutingPerSeconds = value; }
    /// <summary>
    /// the number of spindle
    /// </summary>
    public int SpindleNum { get => _spindleNum; set => _spindleNum = value; }
    /// <summary>
    /// max number of silo 
    /// </summary>
    public int SiloLimit { get => _siloLimit; set => _siloLimit = value; }
    /// <summary>
    /// max number of panel 
    /// </summary>
    public int PanelLimit { get => _panelLimit; set => _panelLimit = value; }
    /// <summary>
    /// max panel number of one lot 
    /// </summary>
    public int LotLimit { get => _lotLimit; set => _lotLimit = value; }
    /// <summary>
    /// max number of layer 
    /// </summary>
    public int LayerLimit { get => _layerLimit; set => _layerLimit = value; }
    public DeviceKind DeviceKind { get; set; } = DeviceKind.Unknown;
    /// <summary>
    /// 设备连接协议
    /// </summary>
    public DeviceProtocolKind ConnectorProtocol { get; set; } = DeviceProtocolKind.Unknown;

    [Obsolete]
    public List<ProductStatus> InputCapabilities { get; set; } = new List<ProductStatus>();
    [Obsolete]
    public List<ProductStatus> OutputCapabilities { get; set; } = new List<ProductStatus>();
    /// <summary>
    /// 当前使用的Fundation版本号
    /// </summary>
    public Version FundationVersion { get; set; } = Version.Parse("1.0.0.0");
    /// <summary>
    /// 当前使用的agent版本号
    /// </summary>
    public Version AgentVersion { get; set; } = Version.Parse("1.0.0.0");
}
