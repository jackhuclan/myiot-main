using System.Collections.Concurrent;
using System.Diagnostics;
using Mediator.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Central.Core.Event;
using VgAutoDrill.Central.Core.Flusher;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core;

[DebuggerDisplay("DeviceId={DeviceId}")]
public class DeviceProxy
{
    private readonly IDeviceServiceInvoker _deviceServiceInvoker;
    private readonly ILocationManager _locationManager;
    private readonly IPartitionManager _partitionManager;
    private readonly ILogger<DeviceProxy> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly IPropertiesFlusher _propertiesFlusher;
    private readonly IMediator _mediator;
    private readonly List<string> _excludedRefreshProperties = new() { "TargetDevice", "LogInTime" };
    private readonly List<string> _routeCodes = new();
    private readonly AutoResetEvent _propertiesLocker = new AutoResetEvent(true);
    private readonly AutoResetEvent _panelsLocker = new AutoResetEvent(true);
    private static volatile int _refreshRouteCodes = 0;
    private DeviceStatus _status = DeviceStatus.Unknown;
    private DateTime _lastStatusUpdate = DateTime.Now;
    private volatile string _clientId = string.Empty;

    public event Func<DeviceStatusChangingEventArgs, Task> StatusChanging;

    public event Func<DeviceStatusChangedEventArgs, Task> StatusChanged;

    private readonly long _lastRefreshStatusTime;

    /// <summary>
    /// 默认构造函数
    /// </summary>
    public DeviceProxy()
    {
        LogInTime = DateTime.Now;
    }

    [ActivatorUtilitiesConstructor]
    public DeviceProxy(IServiceProvider serviceProvider)
    {
        _deviceServiceInvoker = serviceProvider.GetRequiredService<IDeviceServiceInvoker>();
        _loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _mediator = serviceProvider.GetRequiredService<IMediator>();
        _propertiesFlusher = serviceProvider.GetRequiredService<IPropertiesFlusher>();
        _logger = _loggerFactory.CreateLogger<DeviceProxy>();
        LogInTime = DateTime.Now;
    }

    public string AtpPath { get; set; } = string.Empty;
    public string ClientId { get => _clientId; set => _clientId = value; }
    public string ClientIp { get; set; } = string.Empty;
    public virtual DeviceDescriptor Descriptor { get; set; } = new();

    public string DeviceId => Descriptor.DeviceId;

    public DeviceKind DeviceKind => Descriptor.DeviceKind;

    public bool IsDrill => DeviceKindExtensions.IsDrill(DeviceKind);
    public virtual DateTime? DeviceStandbyTime { get; set; }

    public string DiaPath { get; set; } = string.Empty;

    public string DrlPath { get; set; } = string.Empty;

    public virtual bool IsAvailbleForAgv
    {
        get
        {
            return true;
        }
    }

    public DateTime? LogInTime
    {
        get
        {
            return Properties.ContainsKey("LogInTime") && DateTime.TryParse(Properties["LogInTime"].ToStr(), out var logInTime) ? logInTime : null;
        }
        set
        {
            Properties["LogInTime"] = value;
        }
    }

    public virtual CutterTrays PayloadCutterTrays { get; set; } = new();

    /// <summary>
    /// todo: 一个设备多个库位，一个库位一个panellist，从设备上获取panelList意义不大
    /// </summary>
    public virtual PanelList PayloadPanels { get; private set; } = new();

    /// <summary>
    /// 是否有StatusChanging事件
    /// </summary>
    public bool HasStatusChanging => StatusChanging != null;

    public string ProductId
    {
        get
        {
            return Descriptor.ProductId;
        }
    }

    public virtual ConcurrentDictionary<string, object> Properties { get; set; } = new();

    /// <summary>
    /// 最大能放几个料仓（暂未启用）
    /// </summary>
    public virtual int SiloLimit { get; set; } = 1;

    /// <summary>
    /// 能否放进料仓
    /// </summary>
    public virtual bool SiloPlaceable => true;

    /// <summary>
    /// 最近状态起始时间
    /// </summary>
    public DateTime LatestStatusStart => _lastStatusUpdate;

    /// <summary>
    /// 最近状态持续时间
    /// </summary>
    public TimeSpan LatestStatusDuration => DateTime.Now - _lastStatusUpdate;

    public virtual DeviceStatus Status
    {
        get => _status;
        set
        {
            DeviceStatus oldStatus = _status;
            bool statusChanged = false;
            if (_status != value)
            {
                DateTime lastStatusTime = _lastStatusUpdate;
                _lastStatusUpdate = DateTime.Now;
                statusChanged = true;

                StatusChanging?.Invoke(new DeviceStatusChangingEventArgs
                {
                    DeviceId = DeviceId,
                    DeviceName = Descriptor.DeviceName,
                    StartTime = lastStatusTime,
                    EndTime = _lastStatusUpdate,
                    Status = _status,
                });
            }

            _status = value;

            if (statusChanged)
            {
                StatusChanged?.Invoke(new DeviceStatusChangedEventArgs
                {
                    DeviceId = DeviceId,
                    DeviceName = Descriptor.DeviceName,
                    OldStatus = oldStatus,
                    NewStatus = _status,
                });
            }
        }
    }

    /// <summary>
    /// 工艺路线
    /// </summary>
    public IReadOnlyList<string> RouteCodes => _routeCodes;

    public async Task<DeviceServiceInvokeResponse> InvokeService(DeviceServiceInvokeRequest request)
    {
        return await _deviceServiceInvoker.InvokeService(request);
    }

    public async Task RefreshProperties(Dictionary<string, object> latestProperties)
    {
        await Task.Run(() =>
        {
            try
            {
                _propertiesLocker.WaitOne(TimeSpan.FromMilliseconds(300));

                foreach (var property in latestProperties)
                {
                    if (_excludedRefreshProperties.Contains(property.Key)) continue;
                    Properties[property.Key] = property.Value;
                }

                if (_propertiesFlusher != null) _propertiesFlusher.OnPropertiesRefreshed(this);
            }
            finally
            {
                _propertiesLocker.Set();
            }
        });
    }

    public virtual Task SetCutterTrays(CutterTrays cutterTrays)
    {
        PayloadCutterTrays = cutterTrays;
        return Task.CompletedTask;
    }

    /// <summary>
    /// 设置板料，参数<paramref name="forceRaiseChangedEvent"/>是否强制发生板料变化事件
    /// </summary>
    /// <param name="panels">板料</param>
    /// <param name="forceRaiseChangedEvent">强制调用板料变化事件</param>
    /// <returns></returns>
    public async Task<bool> SetPanels(PanelList panels, bool forceRaiseChangedEvent = false)
    {
        var result = await Task.Run(() =>
           {
               try
               {
                   _logger.LogDebug(string.Format("{0}, SetPanels=Count:{1} waitting...", DeviceId, panels == null ? 0 : panels.Count()));
                   if (panels == null || panels.Count == 0) return false;

                   if (_panelsLocker.WaitOne(TimeSpan.FromMilliseconds(300)))
                   {
                       UpdateLocationCode(panels);
                       SetPanelsToLocations(panels, forceRaiseChangedEvent);
                       _logger.LogDebug(string.Format("{0}, SetPanels=Count:{1} succeed", DeviceId, panels == null ? 0 : panels.Count()));
                       return true;
                   }
                   else
                   {
                       _logger.LogDebug(string.Format("{0}, SetPanels=Count:{1} failed", DeviceId, panels == null ? 0 : panels.Count()));
                       return false;
                   }
               }
               finally
               {
                   _panelsLocker.Set();
               }
           });

        PayloadPanels = panels;
        return result;
    }

    public void RefreshRouteCodes(List<string> routeCodes)
    {
        if (0 == Interlocked.Exchange(ref _refreshRouteCodes, 1))
        {
            _routeCodes.Clear();
            _routeCodes.AddRange(routeCodes);
            Interlocked.Exchange(ref _refreshRouteCodes, 0);
        }
    }

    protected virtual void UpdateLocationCode(PanelList panels)
    {
        if (panels == null || panels.Count == 0) return;
        var positions = panels.Select(x => x.Position).Distinct().OrderBy(x => x).ToList();

        foreach (var p in positions)
        {
            var panelsOfPosition = PanelList.FromList(panels.Where(x => x.Position == p).ToList());
            var locationCode = panelsOfPosition.LocationCode;
            if (string.IsNullOrEmpty(locationCode))
            {
                locationCode = GetLocationCode(p);
                panelsOfPosition.SetLocationCode(locationCode);
            }
        }
    }

    /// <summary>
    /// 设置板料到板料信息对应的位置（轴），参数<paramref name="forceRaiseChangedEvent"/>是否强制发生板料变化事件
    /// </summary>
    /// <param name="panels">板料</param>
    /// <param name="forceRaiseChangedEvent">强制调用板料变化事件</param>
    protected virtual void SetPanelsToLocations(PanelList panels, bool forceRaiseChangedEvent = false)
    {
        if (panels == null || panels.Count == 0) return;

        var positions = panels.GetPositions();

        foreach (var p in positions)
        {
            var panelsOfPosition = PanelList.FromList(panels.Where(x => x.Position == p).ToList());
            SetPanelsToLocation(panelsOfPosition, p, forceRaiseChangedEvent);
        }
    }

    /// <summary>
    /// 设置板料到指定位置（轴），参数<paramref name="forceRaiseChangedEvent"/>是否强制发生板料变化事件
    /// </summary>
    /// <param name="panels">板料</param>
    /// <param name="postion">位置（轴）</param>
    /// <param name="forceRaiseChangedEvent">强制调用板料变化事件</param>
    protected void SetPanelsToLocation(PanelList panels, int postion, bool forceRaiseChangedEvent = false)
    {
        if (panels == null || panels.Count == 0) return;

        var locationCode = panels.LocationCode;
        var siloCode = panels.SiloCode;

        if (_locationManager.TryGetLocation(locationCode, out var location)
            && location != null)
        {
            location.DeviceId = DeviceId;
            location.HostDevice = this;
            location.Index = postion;
        }
        else
        {
            location = _locationManager.Create(locationCode);
            location.DeviceId = DeviceId;
            location.HostDevice = this;
            location.Index = postion;
        }

        if (RouteCodes.Any()
            && _partitionManager.TryGetPartition(RouteCodes, out var partition)
            && partition != null
            && location.Partition == null)
        {
            location.PartitionCode = partition.PartCode ?? string.Empty;
            location.Partition = partition;
        }

        if (!location.HasPanelChangedEvent)
            location.OnPanelChanged += (loc) => _mediator.PublishAsync(new PanelChangedEvent(loc));

        _locationManager.TryAddOrUpdateLocation(location);

        if (_scheduleTaskManager.TryGetLatestScheduleTaskOfLocation(locationCode, out var scheduleTask))
        {
            location.Schedule = scheduleTask;
        }

        location.SetPanels(panels, forceRaiseChangedEvent);
    }

    public virtual string GetLocationCode(int position = 1) => DeviceId;
}
