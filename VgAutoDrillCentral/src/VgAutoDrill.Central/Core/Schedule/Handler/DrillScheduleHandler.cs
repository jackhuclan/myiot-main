using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Schedule.Deliver;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Central.Core.Schedule.Handler;

/// <summary>
/// 钻机的调度任务处理
/// </summary>
internal class DrillScheduleHandler : BaseScheduleTaskHandler
{
    private readonly ILogger<DrillScheduleHandler> _logger;
    private readonly IDeviceManager _deviceManager;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly IPartitionManager _partitionManager;
    private readonly IDrillManager _drillManager;
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;

    public DrillScheduleHandler(IServiceProvider serviceProvider)
    : base(serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<DrillScheduleHandler>>();
        _deviceManager = serviceProvider.GetRequiredService<IDeviceManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _drillManager = serviceProvider.GetRequiredService<IDrillManager>();
        _panelSiloForkManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();
    }

    public override async Task Handle()
    {
        if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN, Admin.Model.Enum.SysConfigCategoryEnum.None, false))
        {
            _logger.LogWarning($"系统即将维护，暂停分配新任务，请稍候.");
            return;
        }

        var queryDrillSchedules = _scheduleTaskManager.NotStartedDrillSchedules.ToList();

        //queryDrillSchedules.Sort(new DrillScheduleTaskComparer());

        //排序调度记录
        //部分完成的最优先；紧急调度次之；钻机缺料；优先只上料任务;然后既上又下的任务;
        //同种类型的任务按钻孔进度百分比 进行排序；
        var partCompletedSchedules = queryDrillSchedules
            .Where(s => s.ScheduledTaskStatus == ScheduledTaskStatus.PartCompleted
                     && s.InteractionSequence != InteractionSequence.UnloadOnly)
            .OrderByDescending(s => s.Percentage).ThenBy(s => s.Id);
        await HandleDrillRequest(partCompletedSchedules);

        var partUrgentSchedules = queryDrillSchedules
            .Where(s => s.IsUrgent > 0 && s.ScheduledTaskStatus != ScheduledTaskStatus.Allocated)
            .OrderByDescending(s => s.Percentage).ThenBy(s => s.Id);
        await HandleDrillRequest(partUrgentSchedules);

        var lackMaterialSchedules = queryDrillSchedules
            .Where(s => s.ScheduledTaskStatus != ScheduledTaskStatus.Allocated
                        && s.InteractionSequence != InteractionSequence.UnloadOnly
                        && s.DrillBoardPositionStatus == 0)
             .OrderBy(s => s.Id);
        await HandleDrillRequest(lackMaterialSchedules);

        var loadSchedules = queryDrillSchedules
            .Where(s => s.ScheduledTaskStatus != ScheduledTaskStatus.Allocated
                        && s.InteractionSequence == InteractionSequence.LoadOnly)
            .OrderByDescending(s => s.Percentage).ThenBy(s => s.Id);
        await HandleDrillRequest(loadSchedules);

        var loadThenUnloadSchedules = queryDrillSchedules
            .Where(s => s.ScheduledTaskStatus != ScheduledTaskStatus.Allocated
                     && s.InteractionSequence == InteractionSequence.LoadThenUnload && !string.IsNullOrEmpty(s.ItemCode))
            .OrderByDescending(s => s.Percentage).ThenBy(s => s.Id);
        await HandleDrillRequest(loadThenUnloadSchedules);

        var otherSchedules = queryDrillSchedules
            .Where(s => s.ScheduledTaskStatus != ScheduledTaskStatus.Allocated)
            .OrderByDescending(s => s.Percentage).ThenBy(s => s.Id);
        await HandleDrillRequest(otherSchedules);
    }

    private async Task HandleDrillRequest(IOrderedEnumerable<DrillScheduleTask> schedules)
    {
        await AllocateAgvForEachDrillSchedule(schedules);
        await AssignTaskForIdleAgvs();
    }

    /// <summary>
    /// 为每台钻机的任务分配agv
    /// </summary>
    /// <param name="schedules"></param>
    /// <returns></returns>
    private async Task AllocateAgvForEachDrillSchedule(IOrderedEnumerable<DrillScheduleTask> schedules)
    {
        foreach (var schedule in schedules)
        {
            _logger.LogInformation($"DrillScheduleHandler: try schedule={schedule.Id},drill={schedule.CallerDeviceId}");
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN, Admin.Model.Enum.SysConfigCategoryEnum.None, false))
            {
                _logger.LogWarning($"系统即将维护，暂停分配新任务，请稍候.");
                return;
            }

            if (!_deviceManager.TryGetOnlineDrill(schedule.CallerDeviceId, out var drill)
                || drill == null)
            {
                _logger.LogWarning($"DrillScheduleHandler: CallerOffline,{schedule.CallerDeviceId}.");
                continue;
            }

            if (!drill.BufferAutomatic)
            {
                await _scheduleTaskManager.CancelSingleSchedule(schedule.Code, true, $"因BUFFER手动状态，自动取消 schedule {schedule.Id},{schedule.CallerDeviceId}.");
                _logger.LogWarning($"因BUFFER手动状态，自动取消调度， {schedule.Id},{schedule.CallerDeviceId}, .");

                continue;
            }

            if (!drill.IsAvailbleForAgv)
            {
                _logger.LogWarning($"DrillScheduleHandler: device is not avaible now,{drill.DeviceId}.");
                continue;
            }

            var idleBackPanelAgvs = _deviceManager.GetIdleBackPanelAgvs(schedule.RouteCode.ToLower());
            if (schedule.Requirement == null) continue;

            if (!idleBackPanelAgvs.Any())
            {
                _logger.LogWarning($"DrillScheduleHandler: orderedDrillSchedules break, there is not any ready panel agv now.");
                continue;
            }

            await Task.Delay(30);

            foreach (var agv in idleBackPanelAgvs)
            {
                _logger.LogInformation($"DrillScheduleHandler: try allocate Agv={agv.DeviceId} for schedule={schedule.Id},drill={schedule.CallerDeviceId}");
                if (_scheduleTaskManager.HasUncompletedTaskOfAgv(agv.DeviceId, out var uncompletedScheduleIds, out _))
                {
                    _logger.LogInformation($"DrillScheduleHandler: Agv={agv.DeviceId} has uncompletedScheduleIds={string.Join(",", uncompletedScheduleIds)}");
                    continue;
                }

                if (agv.Location.HasSilo && agv.Location.CanMatch(schedule.Requirement))
                {
                    _logger.LogInformation($"DrillScheduleHandler: Agv={agv.DeviceId} has silo and can match " +
                        $"schedule={schedule.Id},drill={schedule.CallerDeviceId}");
                    await DeliverScheduleTask(new DeliverScheduleTaskRequirement
                    {
                        CallerDevice = drill,
                        AgvDevice = agv,
                        ScheduleTask = schedule
                    });

                    break;
                }
            }
        }
    }

    /// <summary>
    /// 所有钻机调度都不能满足，还有agv空闲的情况；尝试放下agv上面的空料仓或者满料仓
    /// </summary>
    /// <returns></returns>
    private async Task AssignTaskForIdleAgvs()
    {
        _logger.LogInformation($"DrillScheduleHandler:try AssignTaskForIdleAgvs");
        var centralVerifyFunction01 = await _sysConfigManager.GetBoolValue("CentralVerifyFunction01", Admin.Model.Enum.SysConfigCategoryEnum.None, false);
        if (!centralVerifyFunction01) return;
        _logger.LogDebug($"DrillScheduleHandler: start AssignTaskForIdleAgvs");

        var idleAgvs = _deviceManager.GetIdleBackPanelAgvs();
        var minDrilledTrackOutNum = await _sysConfigManager.GetIntValue("MinDrilledTrackOutNum", Admin.Model.Enum.SysConfigCategoryEnum.None, false);
        if (minDrilledTrackOutNum == 0)
        {
            minDrilledTrackOutNum = 5;
        }

        int countOfEmptyForkAtLeast = await _sysConfigManager.GetIntValue("CountOfEmptyForkAtLeast", Admin.Model.Enum.SysConfigCategoryEnum.None, false);
        if (countOfEmptyForkAtLeast < 1)
        {
            countOfEmptyForkAtLeast = 1;
        }

        var agvStandbyTimeout = await _sysConfigManager.GetIntValue("AgvStandbyTimeout", Admin.Model.Enum.SysConfigCategoryEnum.None, false);
        if (agvStandbyTimeout == 0)
        {
            agvStandbyTimeout = 10;
        }

        _logger.LogInformation($"DrillScheduleHandler:has idleAgvs={idleAgvs.Any()}");
        foreach (var agv in idleAgvs)
        {
            _logger.LogInformation($"DrillScheduleHandler:AssignTaskForIdleAgvs for agv={agv.DeviceId}");
            if (!_partitionManager.TryGetPartition(agv.RouteCodes, out var partition)
                || partition == null)
            {
                _logger.LogDebug($"DrillScheduleHandler:agv={agv.DeviceId} agv.RouteCodes={string.Join(",", agv.RouteCodes)} 没有设置分区");
                continue;
            }

            if (_scheduleTaskManager.HasUncompletedTaskOfAgv(agv.DeviceId, out var uncompletedScheduleIds, out _))
            {
                _logger.LogInformation($"DrillScheduleHandler: Agv={agv.DeviceId} has uncompletedScheduleIds={string.Join(",", uncompletedScheduleIds)}");
                continue;
            }

            if (agv.Location.HasNothing)
            {
                _logger.LogDebug($"DrillScheduleHandler:agv={agv.DeviceId} agv.RouteCodes={string.Join(",", agv.RouteCodes)} has nothing, TryFindArbitraryDrillRequiredLocation...");
                if (_panelSiloForkManager.TryFindArbitraryDrillRequiredLocation(partition.PartCode, agv.RouteCodes, out var outboundingLocation)
                    && outboundingLocation != null)
                {
                    var forkSchedule = outboundingLocation.Schedule;
                    if (forkSchedule != null
                        && outboundingLocation.ServingForSchedule != null)
                    {
                        forkSchedule.Appointed = true;
                        forkSchedule.AppointedMessage = $"取料仓，预约AGV：{agv.DeviceId}";

                        outboundingLocation.ServingForSchedule.Appointed = true;
                        _ = agv.GetSiloFromForkThenServeDrill(forkSchedule, outboundingLocation.ServingForSchedule);
                    }
                }
                else
                {
                    _logger.LogDebug($"DrillScheduleHandler:agv={agv.DeviceId} agv.RouteCodes={string.Join(",", agv.RouteCodes)} has nothing, TryFindArbitraryDrillRequiredLocation failed!!!");
                }
            }
            else if (agv.Location.HasSilo
                && _drillManager.TryFindArbitraryDrillRequirementByAgv(agv, out var drillRequirement)
                && drillRequirement != null
                && drillRequirement.OriginalSchedule != null
                && _deviceManager.TryGetOnlineDrill(drillRequirement.OriginalSchedule.CallerDeviceId, out var drill)
                && drill != null)
            {
                _logger.LogDebug($"DrillScheduleHandler:agv={agv.DeviceId} agv.RouteCodes={string.Join(",", agv.RouteCodes)} has silo matched drillRequirement={drillRequirement.CallerDeviceId}," +
                    $" DeliverScheduleTask to {drill.DeviceId}");
                _ = DeliverScheduleTask(new DeliverScheduleTaskRequirement
                {
                    CallerDevice = drill,
                    AgvDevice = agv,
                    ScheduleTask = drillRequirement.OriginalSchedule
                });
            }
            //1.agv上的料仓不符合任何钻机的需求；2.插齿上有钻机需要的料；3.并且有空位
            else if (agv.Location.HasSilo
                && _panelSiloForkManager.TryFindArbitraryDrillRequiredLocation(partition.PartCode, agv.RouteCodes, out var fork1Location)
                && fork1Location != null
                && fork1Location.Schedule != null
                && fork1Location.ServingForSchedule != null
                && _panelSiloForkManager.TryFindEmptyPayloadLocation(partition.PartCode, agv.RouteCodes, out var forkEmptyPayloadLocation)
                && forkEmptyPayloadLocation != null
                && forkEmptyPayloadLocation.Schedule != null)
            {
                forkEmptyPayloadLocation.Schedule.Appointed = true;
                forkEmptyPayloadLocation.Schedule.AppointedMessage = $"放料仓，预约AGV：{agv.DeviceId}";
                fork1Location.Schedule.Appointed = true;
                fork1Location.Schedule.AppointedMessage = $"取料仓，预约AGV：{agv.DeviceId}";
                fork1Location.ServingForSchedule.Appointed = true;

                _logger.LogDebug($"DrillScheduleHandler:agv={agv.DeviceId} agv.RouteCodes={string.Join(",", agv.RouteCodes)} has silo, exchange silo...");
                _logger.LogDebug($"DrillScheduleHandler:agv={agv.DeviceId} agv.RouteCodes={string.Join(",", agv.RouteCodes)} has silo, PutSiloToFork={forkEmptyPayloadLocation.Code}");
                _ = agv.PutSiloToFork(forkEmptyPayloadLocation.Schedule);

                _logger.LogDebug($"DrillScheduleHandler:agv={agv.DeviceId} agv.RouteCodes={string.Join(",", agv.RouteCodes)} has silo, GetSiloFromFork={fork1Location.Code}");
                _logger.LogDebug($"DrillScheduleHandler:agv={agv.DeviceId} agv.RouteCodes={string.Join(",", agv.RouteCodes)} has silo, ServeDrill={fork1Location.ServingForSchedule.CallerDeviceId}");
                _ = agv.GetSiloFromForkThenServeDrill(fork1Location.Schedule, fork1Location.ServingForSchedule);
            }
            else if (agv.Location.DrilledPanelsCount >= minDrilledTrackOutNum
                || agv.IsIdleTimeout(agvStandbyTimeout))//todo:agv空闲时长放数据库配置
            {
                _logger.LogDebug($"DrillScheduleHandler:agv={agv.DeviceId} agv.RouteCodes={string.Join(",", agv.RouteCodes)} has silo, silo is full or waitting too loog, so that put down silo");
                if (_panelSiloForkManager.TryFindEmptyPayloadLocation(countOfEmptyForkAtLeast, partition.PartCode, agv.RouteCodes, out var inboundingLocation)
                        && inboundingLocation != null)
                {
                    var forkSchedule = inboundingLocation.Schedule;
                    if (forkSchedule != null)
                    {
                        forkSchedule.Appointed = true;
                        forkSchedule.AppointedMessage = $"放料仓，预约AGV：{agv.DeviceId}";
                        _ = agv.PutSiloToFork(forkSchedule);
                        _logger.LogDebug($"DrillScheduleHandler:agv={agv.DeviceId} agv.RouteCodes={string.Join(",", agv.RouteCodes)} has silo, PutSiloToFork={forkSchedule.Code}");
                    }
                }
            }
            else
            {
                _logger.LogDebug($"CentralVerifyFunction01--14 agv={agv.DeviceId} do nothing!");
            }
        }
    }
}
