using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Central.Core.Schedule.Handler.Base;

internal abstract class GeneralExchangeSiloHandler : BaseScheduleTaskHandler
{
    private readonly ILogger<GeneralExchangeSiloHandler> _logger;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ILoggerFactory _loggerFactory;
    private readonly MysqlTaskSchedulerOptions _taskScheduleOptions;
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;
    private readonly IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> _panelSiloShelfManager;
    private readonly ILocationManager _locationManager;
    private readonly IPartitionManager _partitionManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IDeviceManager _deviceManager;

    public GeneralExchangeSiloHandler(IServiceProvider serviceProvider)
    : base(serviceProvider)
    {
        _loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = _loggerFactory.CreateLogger<GeneralExchangeSiloHandler>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _taskScheduleOptions = serviceProvider.GetRequiredService<IOptions<MysqlTaskSchedulerOptions>>().Value;

        _panelSiloForkManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();
        _panelSiloShelfManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _deviceManager = serviceProvider.GetRequiredService<IDeviceManager>();
    }

    public override async Task Handle()
    {
        if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN, Admin.Model.Enum.SysConfigCategoryEnum.None, false))
        {
            _logger.LogWarning($"系统即将维护，暂停分配新任务，请稍候.");
            return;
        }

        var orderedDrillSchedules = _scheduleTaskManager.NotStartedDrillSchedules;
        await ExchangeSilo(orderedDrillSchedules);
    }

    protected abstract Task ExchangeSilo(IEnumerable<DrillScheduleTask> orderedDrillSchedules);

    /// <summary>
    /// 根据工艺路线，筛选钻机请求
    /// </summary>
    /// <param name="agv"></param>
    /// <param name="orderedDrillSchedules"></param>
    /// <returns></returns>
    protected IEnumerable<DrillScheduleTask> FilterDrillSchedulesByAgvRouteCode(PanelAgv agv, IEnumerable<DrillScheduleTask> orderedDrillSchedules)
    {
        var filteredDrillSchedules = orderedDrillSchedules
                .Where(s => s.ScheduledTaskStatus != ScheduledTaskStatus.Allocated
                    && s.RequestDeviceKind.HasValue
                    && DeviceKindExtensions.IsDrill(s.RequestDeviceKind.Value)
                    && OnlyEnableClickerItem(s));

        var routeCodes = agv.RouteCodes;
        if (routeCodes.Any())
        {
            return filteredDrillSchedules.Where(s => routeCodes.Contains(s.RouteCode.ToLower()));
        }
        else
        {
            return new List<DrillScheduleTask>();
        }
    }

    /// <summary>
    /// 只允许熟料
    /// </summary>
    /// <param name="scheduleTask"></param>
    /// <returns></returns>
    private bool OnlyEnableClickerItem(ScheduleTaskWithRequest scheduleTask)
    {
        if (scheduleTask.EventRequest == null) return false;
        return
            scheduleTask.InteractionSequence == InteractionSequence.LoadOnly
            ||
            scheduleTask.InteractionSequence != InteractionSequence.LoadOnly
                && scheduleTask.EventRequest.PayloadPanels.DrilledPanels.Any();
    }

    /// <summary>
    /// 查找钻机请求和插齿请求的对子
    /// </summary>
    /// <param name="filteredDrillSchedules">钻机请求</param>
    /// <returns></returns>
    protected DrillRequirementMatchResult? MatchDrillAndForkRequestPair(IEnumerable<DrillScheduleTask> filteredDrillSchedules, BackPanelAgv agv)
    {
        //部分完成优先
        foreach (var schedule in filteredDrillSchedules
                        .Where(s => s.ScheduledTaskStatus.HasValue
                                    && s.ScheduledTaskStatus == ScheduledTaskStatus.PartCompleted
                                    && s.InteractionSequence != InteractionSequence.UnloadOnly)
                        .OrderByDescending(s => s.Percentage).ThenBy(s => s.Id))
        {
            var result = FindMatchedFork(schedule, agv);
            if (result != null)
            {
                return result;
            }
        }

        var ignoreScheduleIds = filteredDrillSchedules
                        .Where(s => s.ScheduledTaskStatus.HasValue
                                 && s.ScheduledTaskStatus == ScheduledTaskStatus.PartCompleted
                                 && s.InteractionSequence != InteractionSequence.UnloadOnly)
                        .Select(s => s.Id)
                        .ToList();

        //紧急任务优先
        foreach (var schedule in filteredDrillSchedules
                        .Where(s => s.IsUrgent > 0 && !ignoreScheduleIds.Contains(s.Id))
                        .OrderByDescending(s => s.Percentage).ThenBy(s => s.Id))
        {
            var result = FindMatchedFork(schedule, agv);
            if (result != null)
            {
                return result;
            }
        }

        ignoreScheduleIds.AddRange(filteredDrillSchedules
                        .Where(s => s.IsUrgent > 0 && !ignoreScheduleIds.Contains(s.Id))
                         .Select(s => s.Id)
                        .ToList());

        //钻机缺料的任务
        foreach (var schedule in filteredDrillSchedules
                        .Where(s => s.InteractionSequence != InteractionSequence.UnloadOnly
                                && s.DrillBoardPositionStatus == 0
                                && !ignoreScheduleIds.Contains(s.Id))
                        .OrderByDescending(s => s.Id))
        {
            var result = FindMatchedFork(schedule, agv);
            if (result != null)
            {
                return result;
            }
        }

        ignoreScheduleIds.AddRange(filteredDrillSchedules
                        .Where(s => s.InteractionSequence != InteractionSequence.UnloadOnly
                                && s.DrillBoardPositionStatus == 0
                                && !ignoreScheduleIds.Contains(s.Id))
                        .Select(s => s.Id)
                        .ToList());

        //只上料的任务
        foreach (var schedule in filteredDrillSchedules
                .Where(s => s.InteractionSequence == InteractionSequence.LoadOnly && !ignoreScheduleIds.Contains(s.Id))
                .OrderByDescending(s => s.Percentage).ThenBy(s => s.Id))
        {
            var result = FindMatchedFork(schedule, agv);
            if (result != null)
            {
                return result;
            }
        }

        ignoreScheduleIds.AddRange(filteredDrillSchedules
                        .Where(s => s.InteractionSequence == InteractionSequence.LoadOnly && !ignoreScheduleIds.Contains(s.Id))
                         .Select(s => s.Id)
                        .ToList());

        //既上又下(过滤掉只下料)的任务
        foreach (var schedule in filteredDrillSchedules
                .Where(s => s.InteractionSequence == InteractionSequence.LoadThenUnload && !string.IsNullOrEmpty(s.ItemCode)
                            && !ignoreScheduleIds.Contains(s.Id))
                .OrderByDescending(s => s.Percentage).ThenBy(s => s.Id))
        {
            var result = FindMatchedFork(schedule, agv);
            if (result != null)
            {
                return result;
            }
        }

        ignoreScheduleIds.AddRange(filteredDrillSchedules
                        .Where(s => s.InteractionSequence == InteractionSequence.LoadThenUnload && !string.IsNullOrEmpty(s.ItemCode)
                                    && !ignoreScheduleIds.Contains(s.Id))
                         .Select(s => s.Id)
                        .ToList());

        //其他任意请求
        foreach (var schedule in filteredDrillSchedules.Where(s => !ignoreScheduleIds.Contains(s.Id)).OrderByDescending(s => s.Percentage).ThenBy(s => s.Id))
        {
            var result = FindMatchedFork(schedule, agv);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    /// <summary>
    /// 查找钻机请求和料架请求的对子
    /// </summary>
    /// <param name="filteredDrillSchedules">钻机请求</param>
    /// <returns></returns>
    protected DrillRequirementMatchResult? MatchDrillAndShelfRequestPair(IEnumerable<DrillScheduleTask> filteredDrillSchedules, BackPanelAgv agv)
    {
        //部分完成优先
        foreach (var schedule in filteredDrillSchedules
                        .Where(s => s.ScheduledTaskStatus.HasValue
                                 && s.ScheduledTaskStatus == ScheduledTaskStatus.PartCompleted
                                 && s.InteractionSequence != InteractionSequence.UnloadOnly)
                        .OrderByDescending(s => s.Percentage).ThenBy(s => s.Id))
        {
            var result = FindMatchedFork(schedule, agv);
            if (result != null)
            {
                return result;
            }
        }

        var ignoreScheduleIds = filteredDrillSchedules
                        .Where(s => s.ScheduledTaskStatus.HasValue
                                 && s.ScheduledTaskStatus == ScheduledTaskStatus.PartCompleted
                                 && s.InteractionSequence != InteractionSequence.UnloadOnly)
                        .Select(s => s.Id)
                        .ToList();

        //紧急任务优先
        foreach (var schedule in filteredDrillSchedules
                        .Where(s => s.IsUrgent > 0 && !ignoreScheduleIds.Contains(s.Id))
                        .OrderByDescending(s => s.Percentage).ThenBy(s => s.Id))
        {
            var result = FindMatchedFork(schedule, agv);
            if (result != null)
            {
                return result;
            }
        }

        ignoreScheduleIds.AddRange(filteredDrillSchedules
                        .Where(s => s.IsUrgent > 0 && !ignoreScheduleIds.Contains(s.Id))
                         .Select(s => s.Id)
                        .ToList());

        //钻机缺料的任务
        foreach (var schedule in filteredDrillSchedules
                .Where(s => s.InteractionSequence != InteractionSequence.UnloadOnly
                        && s.DrillBoardPositionStatus == 0
                        && !ignoreScheduleIds.Contains(s.Id))
                .OrderByDescending(s => s.Id))
        {
            var result = FindMatchedFork(schedule, agv);
            if (result != null)
            {
                return result;
            }
        }

        ignoreScheduleIds.AddRange(filteredDrillSchedules
                        .Where(s => s.InteractionSequence != InteractionSequence.UnloadOnly
                                && s.DrillBoardPositionStatus == 0
                                && !ignoreScheduleIds.Contains(s.Id))
                        .Select(s => s.Id)
                        .ToList());

        //只上料的任务
        foreach (var schedule in filteredDrillSchedules
                .Where(s => s.InteractionSequence == InteractionSequence.LoadOnly && !ignoreScheduleIds.Contains(s.Id))
                .OrderByDescending(s => s.Percentage).ThenBy(s => s.Id))
        {
            var result = FindMatchedFork(schedule, agv);
            if (result != null)
            {
                return result;
            }
        }

        ignoreScheduleIds.AddRange(filteredDrillSchedules
                        .Where(s => s.InteractionSequence == InteractionSequence.LoadOnly && !ignoreScheduleIds.Contains(s.Id))
                         .Select(s => s.Id)
                        .ToList());

        //既上又下(过滤掉只下料)的任务
        foreach (var schedule in filteredDrillSchedules
                .Where(s => s.InteractionSequence == InteractionSequence.LoadThenUnload && !string.IsNullOrEmpty(s.ItemCode)
                            && !ignoreScheduleIds.Contains(s.Id))
                .OrderByDescending(s => s.Percentage).ThenBy(s => s.Id))
        {
            var result = FindMatchedFork(schedule, agv);
            if (result != null)
            {
                return result;
            }
        }

        ignoreScheduleIds.AddRange(filteredDrillSchedules
                        .Where(s => s.InteractionSequence == InteractionSequence.LoadThenUnload && !string.IsNullOrEmpty(s.ItemCode)
                                && !ignoreScheduleIds.Contains(s.Id))
                         .Select(s => s.Id)
                        .ToList());

        //其他任意请求
        foreach (var schedule in filteredDrillSchedules.Where(s => !ignoreScheduleIds.Contains(s.Id)).OrderByDescending(s => s.Percentage).ThenBy(s => s.Id))
        {
            var result = FindMatchedFork(schedule, agv);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    /// <summary>
    /// new, 找到接替钻机请求的料架或者插齿任务
    /// </summary>
    /// <param name="drillSchedule"></param>
    /// <returns></returns>
    protected DrillRequirementMatchResult? FindMatchedFork(DrillScheduleTask drillSchedule, BackPanelAgv agv)
    {
        var preBookedPartition = _panelSiloForkManager.Partitions.FirstOrDefault(x => x != null
            && x.PreBookAGV == agv.DeviceId
             //|| x.PreBookRoute == drillSchedule.RouteCode
             );

        if (preBookedPartition != null)
        {
            if (_panelSiloForkManager.TryFindLocation(drillSchedule.Requirement, preBookedPartition?.PartCode, out var forkLocation)
                && forkLocation != null)
            {
                var phase = new DrillRequirementMatchResult();
                phase.MasterSchedule = drillSchedule;
                phase.ServantSchedule = forkLocation.Schedule;
                phase.ForkPartition = preBookedPartition;
                return phase;
            }
        }

        var availableForkPartitions = _panelSiloForkManager.Partitions.Where(x => x != null
            && (string.IsNullOrEmpty(x.PreBookAGV) || x.PreBookAGV == agv.DeviceId)
            && x.RouteCodes.Contains(drillSchedule.RouteCode?.ToLower())
            //&& (string.IsNullOrEmpty(x.PreBookRoute) || x.PreBookRoute == drillSchedule.RouteCode)
            );

        if (!availableForkPartitions.Any())
        {
            //var partitionStatus = _panelSiloForkManager.Partitions.Select(p => $"Partition:{p.PartCode},{p.PreBookAGV},{p.PreBookRoute}").ToList();
            var partitionStatus = _panelSiloForkManager.Partitions.Select(p => $"Partition:{p.PartCode},{p.PreBookAGV}").ToList();
            _logger.LogWarning($"没有可用的插齿分区，{string.Join(";", partitionStatus)}");
            return null;
        }

        foreach (var partition in availableForkPartitions)
        {
            if (_panelSiloForkManager.TryFindLocation(drillSchedule.Requirement, partition?.PartCode, agv.RouteCodes, out var forkLocation)
                && forkLocation != null)
            {
                var phase = new DrillRequirementMatchResult();
                phase.MasterSchedule = drillSchedule;
                phase.ServantSchedule = forkLocation.Schedule;
                phase.ForkPartition = partition;
                return phase;
            }
        }

        return null;
    }

    /// <summary>
    /// new, 找到接替钻机请求的料架或者插齿任务
    /// </summary>
    /// <param name="schedule"></param>
    /// <returns></returns>
    protected DrillRequirementMatchResult? FindMatchedShelf(DrillScheduleTask schedule, BackPanelAgv agv)
    {
        if (_panelSiloShelfManager.TryFindLocation(schedule.Requirement, out var shelfLocation)
                && shelfLocation != null)
        {
            var phase = new DrillRequirementMatchResult();
            phase.MasterSchedule = schedule;
            phase.ServantSchedule = shelfLocation.Schedule;
            return phase;
        }

        return null;
    }

    /// <summary>
    /// 找空库位
    /// </summary>
    /// <param name="followedForkSchedule"></param>
    /// <returns></returns>
    private async Task<ScheduleTaskWithRequest?> FindEmptyPayloadFork(ForkScheduleTask? followedForkSchedule)
    {
        if (followedForkSchedule != null)
        {
            if (_panelSiloForkManager.TryFindEmptyPayloadLocation(0, followedForkSchedule?.EventRequest?.PartitionCode, out var emptyForkLocation)
                && emptyForkLocation != null)
            {
                return emptyForkLocation.Schedule;
            }
        }

        //尝试取消料仓转入的任务，释放空叉齿位的调度请求
        return await _transferPlanManager.TryReleaseTransferJobAsync(followedForkSchedule?.EventRequest?.PartitionCode);
    }

    protected virtual Task<ForkScheduleTask?> TryReleaseTransferJobAsync(string? partitionCode)
    {
        ForkScheduleTask? result = null;

        return Task.FromResult(result);
    }

    protected async Task<bool> TryExchangingForkSilo(IEnumerable<DrillScheduleTask> filteredDrillSchedules, BackPanelAgv agv)
    {
        //foreach (var schedule in filteredDrillSchedules)
        //{
        //    if (!_deviceManager.TryGetOnlineDrill(schedule.CallerDeviceId, out var drillDevice)
        //        || drillDevice == null)
        //    {
        //        _logger.LogWarning($"schedulePath:CallerOffline,{schedule.CallerDeviceId}.");
        //        return false;
        //    }

        //    if (await TryExecuteDrillTask(agv, drillDevice, schedule))
        //    {
        //        _logger.LogInformation($"schedulePath:MatchDrillAndForkRequestPair ->换料仓前，agv上有合适的料仓: {agv.DeviceId},{drillDevice.DeviceId}，[drillSchedule={schedule.Id}]");
        //        return true;
        //    }
        //}

        _logger.LogInformation($"ChangeSilo for agv {agv.DeviceKind} {agv.DeviceId}--after foreach filteredDrillSchedules");

        var drillAndForkPair = MatchDrillAndForkRequestPair(filteredDrillSchedules, agv);
        if (drillAndForkPair == null
            || drillAndForkPair.ServantSchedule == null)
        {
            _logger.LogWarning($"schedulePath:Not any drill request found in the Fork silos when changing silo for agv {agv.DeviceId},{agv.DeviceKind},{agv.DeviceStandbyTime}");
            agv.PreBookedInfo = $"AGV已空闲，但在现有的钻机请求和库位请求中，未找到匹配的料仓。";
            return false;
        }

        var drillSchedule = drillAndForkPair.MasterSchedule as DrillScheduleTask;
        if (drillSchedule == null)
            return false;

        var drillRequirement = drillSchedule.Requirement;
        if (drillRequirement == null)
            return false;

        if (!_deviceManager.TryGetOnlineDrill(drillSchedule.CallerDeviceId, out var drill)
            || drill == null)
        {
            _logger.LogWarning($"schedulePath:CallerOffline,{drillSchedule.CallerDeviceId}.");
            return false;
        }

        _logger.LogInformation($"schedulePath:MatchDrillAndForkRequestPair {agv.DeviceId},{drillAndForkPair.MasterSchedule.CallerDeviceId}，fork {drillAndForkPair.ServantSchedule.LocationCode}");

        //查找备用料仓
        ForkScheduleTask? followedForkSchedule = null;
        Location? locationOfFollowedForkSchedule = null;
        if (drillAndForkPair.ServantSchedule != null && drillAndForkPair.ServantSchedule.RequestDeviceKind == DeviceKind.PanelSiloFork)
        {
            followedForkSchedule = drillAndForkPair.ServantSchedule as ForkScheduleTask;
            if (followedForkSchedule != null
                && _locationManager.TryGetLocation(followedForkSchedule.LocationCode, out var location)
                && location != null
                && location.Partition != null)
            {
                locationOfFollowedForkSchedule = location;
            }
            if (locationOfFollowedForkSchedule == null)
            {
                _logger.LogWarning("schedulePath:ExchangeSilo locationOfFollowedForkSchedule is null.");
                return false;
            }
        }

        BookPartResult? bookSolution = null;
        //空负载agv，直接去插齿取料仓
        if (agv.PayloadPanels.IsEmptyPayload)
        {
            if (_taskScheduleOptions.EnableAutoSetAgvRestPoint)
            {
                bookSolution = await _partitionManager.TryBookPartition(agv, locationOfFollowedForkSchedule);
                if (!string.IsNullOrEmpty(bookSolution.ResponseMessage))
                {
                    _logger.LogWarning($"预约失败，{bookSolution.ResponseMessage}");
                    return false;
                }
            }

            if (!await agv.GetSiloFromFork(followedForkSchedule))
            {
                _logger.LogWarning($"MatchDrillAndForkRequestPair 下发任务失败，UnloadSiloFromFork");

                if (_taskScheduleOptions.EnableAutoSetAgvRestPoint)
                {
                    var unbookResponse = await _partitionManager.TryUnbookPart(bookSolution?.PartitionCode);
                    if (!string.IsNullOrEmpty(unbookResponse))
                    {
                        _logger.LogError($"取消预约失败，返回信息：{unbookResponse}");
                    }
                }
            }
            else
            {
                if (_taskScheduleOptions.EnableAutoSetAgvRestPoint)
                {
                    //下发成功后，记录agv的预约信息
                    agv.PreBookedInfo = $"Partition:{bookSolution?.PartitionCode}";
                }

                followedForkSchedule.Appointed = true;
                followedForkSchedule.AppointedMessage = $"取料仓，预约AGV：{agv.DeviceId}";
                _logger.LogInformation($"MatchDrillAndForkRequestPair 直接去插齿取料仓 {agv.DeviceId},{drill.DeviceId}，[drillSchedule={drillSchedule.Id}]  空料仓库位={drillAndForkPair.ServantSchedule.LocationCode}");
                agv.PreBookedInfo = $"{agv.DeviceId}正在响应{drillSchedule.CallerDeviceId}的调度请求，调度ID：{drillSchedule.Id};" +
                                    $"调度路线：到库位={drillAndForkPair.ServantSchedule.LocationCode}";
            }
        }
        else if (await TryExecuteDrillTask(agv, drill, drillSchedule))
        {
            _logger.LogInformation($"schedulePath:MatchDrillAndForkRequestPair ->TryExecuteDrillTask: {agv.DeviceId},{drill.DeviceId}，[drillSchedule={drillSchedule.Id}] 空料仓库位={drillAndForkPair.ServantSchedule.LocationCode}");
            return true;
        }
        else
        {
            //查找空插齿， 准备放料仓
            var emptyPayloadForkSchedule = await FindEmptyPayloadFork(followedForkSchedule);
            if (emptyPayloadForkSchedule != null)
            {
                if (_taskScheduleOptions.EnableAutoSetAgvRestPoint)
                {
                    bookSolution = await _partitionManager.TryBookPartition(agv, locationOfFollowedForkSchedule);
                    if (!string.IsNullOrEmpty(bookSolution.ResponseMessage))
                    {
                        _logger.LogWarning($"schedulePath:预约失败，{bookSolution.ResponseMessage}");
                        return false;
                    }
                }

                //插齿上有所需要的料仓，agv需要把自身的料仓放到空插齿上，然后去托这个料仓
                if (!await agv.TryExchangeSilo(emptyPayloadForkSchedule, followedForkSchedule))
                {
                    _logger.LogWarning($"schedulePath:更换料仓 下发失败");

                    if (_taskScheduleOptions.EnableAutoSetAgvRestPoint)
                    {
                        _logger.LogWarning($"schedulePath:更换料仓 下发失败，开始取消预约，{bookSolution?.ResponseMessage}");
                        var unbookResponse = await _partitionManager.TryUnbookPart(bookSolution?.PartitionCode);
                        if (!string.IsNullOrEmpty(unbookResponse))
                        {
                            _logger.LogError($"取消预约失败，返回信息：{unbookResponse}");
                        }
                    }

                    return false;
                }

                //下发成功后，记录agv的预约信息
                if (_taskScheduleOptions.EnableAutoSetAgvRestPoint)
                {
                    agv.PreBookedInfo = $"Partition:{bookSolution?.PartitionCode}";
                }

                followedForkSchedule.Appointed = true;
                followedForkSchedule.AppointedMessage = $"取料仓，预约AGV：{agv.DeviceId}";
                followedForkSchedule.ScheduledTaskStatus = ScheduledTaskStatus.Allocated;
                emptyPayloadForkSchedule.Appointed = true;
                emptyPayloadForkSchedule.AppointedMessage = $"放料仓，预约AGV：{agv.DeviceId}";
                emptyPayloadForkSchedule.ScheduledTaskStatus = ScheduledTaskStatus.Allocated;
                _logger.LogInformation($"schedulePath:MatchDrillAndForkRequestPair 更换料仓成功， {agv.DeviceId},{drill.DeviceId}，[drillSchedule={drillSchedule.Id}]" +
                    $" 调度路线：无料仓的库位={emptyPayloadForkSchedule.LocationCode} => 空料仓的库位={drillAndForkPair.ServantSchedule.LocationCode}");

                agv.PreBookedInfo = $"{agv.DeviceId}正在响应{drillSchedule.CallerDeviceId}的调度请求，调度ID：{drillSchedule.Id};" +
                    $"调度路线：从库位={emptyPayloadForkSchedule.LocationCode} => 到库位={drillAndForkPair.ServantSchedule.LocationCode}";
            }
            else  //没有空插齿，所以要腾空叉齿位，移出料仓
            {
                _logger.LogInformation($"尝试更换料仓时，未能找到空库位。AGV:{agv.DeviceId}");
                agv.PreBookedInfo = $"尝试更换料仓时，未能找到空库位。";
                await HandleExportSiloFromOneFork(drillAndForkPair, agv);
            }
        }
        return true;
    }

    protected virtual Task HandleExportSiloFromOneFork(DrillRequirementMatchResult trackJob, BackPanelAgv agv)
    {
        return Task.CompletedTask;
    }
}
