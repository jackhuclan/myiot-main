using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Schedule;
using VgAutoDrill.Central.Core.Schedule.Handler;
using VgAutoDrill.Infrastructure;

namespace VgAutoDrill.Central.Core.Mysql;

/// <summary>
/// 调度任务处理策略
/// </summary>
internal class MysqlTaskScheduleStrategy : ITaskScheduleStrategy
{
    private readonly ILogger<MysqlTaskScheduleStrategy> _logger;
    private readonly IPartitionManager _partitionManager;
    private readonly IObjectFactory _objectFactory;
    private readonly IDeviceManager _deviceManager;
    private readonly IDeviceService _deviceService;
    private readonly IDistributedCache _distributedCache;
    private readonly IScheduleTaskAdapter _scheduleTaskAdapter;
    private readonly MysqlTaskSchedulerOptions _taskScheduleOptions;
    private readonly IScheduleTaskHandler[] _scheduleTaskHandlers;
    private readonly ISysConfigManager _sysConfigManager;
    private static int _cycle = 1;

    public MysqlTaskScheduleStrategy(IDeviceManager deviceHolder,
        IPartitionManager partitionManager,
        ISysConfigManager sysConfigManager,
        IObjectFactory objectFactory,
        IDeviceService deviceService,
        IDistributedCache distributedCache,
        IScheduleTaskAdapter scheduleTaskAdapter,
        IOptions<MysqlTaskSchedulerOptions> options,
        ILogger<MysqlTaskScheduleStrategy> logger)
    {
        _sysConfigManager = sysConfigManager;
        _objectFactory = objectFactory;
        _logger = logger;
        _deviceManager = deviceHolder;
        _deviceService = deviceService;
        _distributedCache = distributedCache;
        _scheduleTaskAdapter = scheduleTaskAdapter;
        _partitionManager = partitionManager;
        _taskScheduleOptions = options.Value;
        _scheduleTaskHandlers = new IScheduleTaskHandler[] {
                _objectFactory.GetOrCreate<SpecifiedAgvScheduleHandler>(),
                _objectFactory.GetOrCreate<FollowedScheduleHandler>(),
                _objectFactory.GetOrCreate<DrillScheduleHandler>(),
                _objectFactory.GetOrCreate<TransferRackExchangeSiloHandler>(),
        };
    }

    public async Task HandleScheduleTask()
    {
        foreach (var handler in _scheduleTaskHandlers)
        {
            await Task.Delay(30);
            _logger.LogInformation($"{_cycle}-{handler.GetType().Name}");

            await handler.Handle();
        }

        _cycle++;
    }

    ///// <summary>
    ///// 更新设备状态
    ///// </summary>
    ///// <returns></returns>
    //public async Task RefreshDevicesStatus()
    //{
    //    var onlineDevices = _deviceManager.OnlineDevices
    //        .Select(x => new AddOrUpdateDeviceReq { Code = x.DeviceId.ToLower(), DeviceStatus = (Admin.Model.CentralModels.DeviceStatus)x.Status }).ToList();

    //    var affectedRows = await _deviceService.RefreshDeviceStatus(onlineDevices);
    //    _logger.LogDebug($"RefreshDevicesStatus, onlineDevices:{onlineDevices.Count}, device status affectedRows:{affectedRows}");

    //    var keys = _distributedCache.GetStringAsync("event");
    //    var count = 0;
    //    foreach (var key in keys)
    //    {
    //        var existsActiveSchedules = await _scheduleTaskAdapter.HasUnstartedOrDoingScheduleTaskByRoutingKey(key);

    //        if (!existsActiveSchedules)
    //        {
    //            count++;
    //            await _distributedCache.RemoveAsync(key);
    //        }
    //    }
    //    _logger.LogDebug($"RefreshDevicesStatus, reset redis key count:{count}");
    //}

    ///// <summary>
    ///// 处理没有在途调度记录的redis key
    ///// </summary>
    ///// <returns></returns>
    //public async Task RefreshTimeoutRedisKey()
    //{
    //    var count = 0;
    //    foreach (var key in keys)
    //    {
    //        var existsActiveSchedules = await _scheduleTaskAdapter.HasUnstartedOrDoingScheduleTaskByRoutingKey(key);

    //        if (!existsActiveSchedules)
    //        {
    //            count++;
    //            await _distributedCache.RemoveAsync(key);
    //        }
    //    }
    //    _logger.LogDebug($"RefreshTimeoutRedisKey, reset redis key count:{count}");
    //}

    public async Task SetIdleAgvToRestPoint()
    {
        if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN, Admin.Model.Enum.SysConfigCategoryEnum.None, false))
        {
            _logger.LogWarning($"系统即将维护，暂停分配新任务，请稍候.");
            return;
        }

        var agvStandbyTimeout = await _sysConfigManager.GetIntValue("AgvStandbyTimeout", Admin.Model.Enum.SysConfigCategoryEnum.None, false);
        if (agvStandbyTimeout == 0)
        {
            agvStandbyTimeout = 10;
        }

        if (!_taskScheduleOptions.EnableAutoSetAgvRestPoint || !CentralFlags.SystemPreloadCompleted)
        {
            return;
        }

        var idleAgvs = _deviceManager.Agvs.Where(x => x.IsIdleTimeout(agvStandbyTimeout));

        foreach (var agv in idleAgvs)
        {
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN, Admin.Model.Enum.SysConfigCategoryEnum.None, false))
            {
                _logger.LogWarning($"系统即将维护，暂停分配新任务，请稍候.");
                return;
            }

            if (string.IsNullOrEmpty(agv.CarCurrentPos))
            {
                _logger.LogWarning($"{agv.DeviceId}不能自动分配休息点,因为未上报当前点位CarCurrentPos.");
                return;
            }

            /*
            1. 低电量，中控不会分配休息点--已有逻辑；
            2. 非低电量，当前点位 是充电点位（参数配置后台管理中如(小车充电桩点位：00030；大车充电桩点位：00010)）时，不分配休息点
            分隔符|
            */
            var positions = await _sysConfigManager.GetStringValue("AgvChargingPosition");
            if (string.IsNullOrWhiteSpace(positions))
            {
                _logger.LogWarning("不能自动分配休息点；后端系统管理中，未定义充电点位 AgvChargingPosition.");
                return;
            }

            var chargingPosition = positions.Split('|');
            if (chargingPosition.Contains(agv.CarTargetPos))
            {
                _logger.LogWarning($"{agv.DeviceId}正在前往充电桩位置{agv.CarTargetPos},不能自动分配休息点.");
                continue;
            }

            if (chargingPosition.Contains(agv.CarCurrentPos))
            {
                _logger.LogWarning($"{agv.DeviceId}不能自动分配休息点,因为当前在充电位置{agv.CarCurrentPos}.");
                continue;
            }

            if (!_partitionManager.TryFindBookedRest(agv.DeviceId, out var fitRestPoint)
                    && fitRestPoint == null
                    && !chargingPosition.Contains(agv.CarCurrentPos))
            {
                _logger.LogDebug($"没有提前预约的休息点，agv:{agv.DeviceId},{agv.DeviceKind}");
                _logger.LogDebug($"自主查找可用休息点，agv:{agv.DeviceId},{agv.DeviceKind}");
                fitRestPoint = await _partitionManager.TryBookRest(agv);
            }

            if (fitRestPoint != null && fitRestPoint.Point.ToLower() != agv.CarCurrentPos.ToLower())
            {
                _logger.LogInformation($"自主查找可用休息点，agv当前位置:{agv.CarCurrentPos},agv:{agv.DeviceId},{agv.DeviceKind}, 已预约点位：{fitRestPoint.RestCode} {fitRestPoint.Point} ");
                var moveResult = await agv.TryMoveTo(fitRestPoint);
                //todo, if false, 尝试查找其他休息点
            }
        }
    }
}
