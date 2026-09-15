using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Domain;

public class Agv : DeviceProxy
{
    private readonly IDeviceManager _deviceHolder;
    private readonly ILogger<Agv> _logger;
    private readonly IPartitionAdapter _partitionAdapter;
    private readonly IPartitionManager _partitionManager;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly MysqlTaskSchedulerOptions _taskScheduleOptions;

    /// <summary>
    /// 上一次休息点
    /// </summary>
    private readonly string _lastRestPoint = string.Empty;

    /// <summary>
    /// 当前点位
    /// </summary>
    private readonly string _currentPoint = string.Empty;

    [ActivatorUtilitiesConstructor]
    public Agv(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _deviceHolder = serviceProvider.GetRequiredService<IDeviceManager>();
        _partitionAdapter = serviceProvider.GetRequiredService<IPartitionAdapter>();
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _taskScheduleOptions = serviceProvider.GetRequiredService<IOptions<MysqlTaskSchedulerOptions>>().Value;
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<Agv>();

        TargetDevice = string.Empty;
        LastTargetRestPoint = string.Empty;
        LastAgvPoint = string.Empty;
    }

    public string CarCurrentPos
    {
        get
        {
            return Properties.ContainsKey("CarCurrentPos") ? Properties["CarCurrentPos"].ToStr() : string.Empty;
        }
        set
        {
            Properties["CarCurrentPos"] = value;
        }
    }

    public bool IsFullSilo
    {
        get
        {
            return Properties.ContainsKey("IsFullSilo") && Properties["IsFullSilo"].ToBool();
        }
    }

    /// <summary>
    /// 是否空闲
    /// </summary>
    public virtual bool IsIdle => Status == DeviceStatus.Ready
                    && string.IsNullOrEmpty(TargetDevice);

    /// <summary>
    /// 是否空闲超时
    /// </summary>
    public virtual bool IsIdleTimeout(int seconds) => IsIdle
                    && DeviceStandbyTime.HasValue
                    && DateTime.Now.Subtract(DeviceStandbyTime.Value).TotalSeconds > seconds;

    /// <summary>
    /// 上一次的AGV当前点位
    /// </summary>
    public string LastAgvPoint { get; private set; }

    /// <summary>
    /// 上一次的休息点
    /// </summary>
    public string LastTargetRestPoint { get; private set; }

    public string PreBookedInfo
    {
        get
        {
            return Properties.ContainsKey("PreBookedInfo") ? Properties["PreBookedInfo"].ToStr() : string.Empty;
        }
        set
        {
            Properties["PreBookedInfo"] = value;
        }
    }

    public string TargetDevice
    {
        get
        {
            return Properties.ContainsKey("TargetDevice") ? Properties["TargetDevice"].ToStr() : string.Empty;
        }
        set
        {
            Properties["TargetDevice"] = value;
            if (string.IsNullOrEmpty(value))
            {
                TargetLocation = null;
            }
        }
    }

    public string? TargetLocation
    {
        get
        {
            return Properties.ContainsKey("TargetLocation") ? Properties["TargetLocation"].ToStr() : string.Empty;
        }
        set
        {
            Properties["TargetLocation"] = value ?? string.Empty;
        }
    }

    public async Task RefreshCarCurrentPos(DeviceStatusReportRequest request)
    {
        if (!request.Params.ContainsKey("CarCurrentPos") || !CentralFlags.SystemPreloadCompleted)
        {
            return;
        }

        CarCurrentPos = request.Params["CarCurrentPos"].ToStr();
        if (LastAgvPoint.ToLower() == CarCurrentPos.ToLower())
        {
            return;
        }

        LastAgvPoint = CarCurrentPos;
        if (_taskScheduleOptions.EnableAutoSetAgvRestPoint)
        {
            await _partitionManager.SetCarPosition(DeviceId, CarCurrentPos);
        }

        if (_taskScheduleOptions.EnableAutoSetAgvRestPoint
            && LastTargetRestPoint.ToLower() == CarCurrentPos.ToLower())
        {
            //取消允许下一次的移动
            LastTargetRestPoint = string.Empty;
            PreBookedInfo = string.Empty;
            //取消库位分区的锁定
            await _partitionManager.TryUnbookPartition(DeviceId);
        }
    }

    public void ReleaseAgv()
    {
        if (!CentralFlags.SystemPreloadCompleted)
        {
            return;
        }

        var exsistSchedule = _scheduleTaskManager.HasUncompletedTaskOfAgv(DeviceId, out _, out _);
        if (!exsistSchedule)
        {
            TargetDevice = string.Empty;
        }
    }

    public async Task<bool> TryMoveTo(RestPoint? fitRestPoint)
    {
        if (!string.IsNullOrEmpty(LastTargetRestPoint)
            && fitRestPoint != null
            && LastTargetRestPoint.ToLower() == fitRestPoint.Point.ToLower())
        {
            _logger.LogInformation($"agv当前位置:{this.CarCurrentPos}");
            _logger.LogInformation($"Note,重复移动命令，不需要下发；agv:{DeviceId},Target Point:{fitRestPoint.Point} ");
            return false;
        }

        if (fitRestPoint != null && !string.IsNullOrEmpty(fitRestPoint.Point))
        {
            //发送车辆移动命令 agv,设备端使用的是属性：MoveTargetPos
            var moveActionResponse = await InvokeService(new DeviceServiceInvokeRequest
            {
                ProductId = ProductId,
                DeviceId = DeviceId,
                ClientId = ClientId,
                TargetDeviceId = DeviceId,
                TargetProductId = ProductId,
                TargetClientId = ClientId,
                HostAddress = Descriptor.HostAddress,
                TargetHostAddress = Descriptor.HostAddress,
                ServiceId = "RemoteCommand",
                ServiceName = "AgvMoveCommand",
                Params = { { "MoveTargetPos", fitRestPoint.Point } },
            });

            if (moveActionResponse != null && moveActionResponse.Code == ErrorCodes.Sys.SUCCESS)
            {
                PreBookedInfo = $"RestCode:{fitRestPoint.RestCode},RestPoint:{fitRestPoint.Point}";
                LastTargetRestPoint = fitRestPoint.Point;
                _logger.LogInformation($"Success,发送车辆移动命令 agv:{DeviceId},target Point:{fitRestPoint.Point} ");
                return true;
            }
            else
            {
                LastTargetRestPoint = string.Empty;
                _logger.LogInformation($"Fail,发送车辆移动命令 agv:{DeviceId},target Point:{fitRestPoint.Point} ");
                PreBookedInfo = string.Empty;
            }
        }

        return false;
    }

    protected override void UpdateLocationCode(PanelList panels)
    {
        if (panels == null || panels.Count == 0) return;
        if (string.IsNullOrEmpty(panels.LocationCode))
            panels.SetLocationCode(GetLocationCode());
    }

    protected override void SetPanelsToLocations(PanelList panels, bool forceRaiseChangedEvent = false)
    {
        SetPanelsToLocation(panels, 1, forceRaiseChangedEvent);
    }

    /// <summary>
    /// 目标位置
    /// </summary>
    public string CarTargetPos
    {
        get
        {
            return Properties.ContainsKey("CarTargetPos") ? Properties["CarTargetPos"].ToStr() : string.Empty;
        }
        set
        {
            Properties["CarTargetPos"] = value;
        }
    }
}
