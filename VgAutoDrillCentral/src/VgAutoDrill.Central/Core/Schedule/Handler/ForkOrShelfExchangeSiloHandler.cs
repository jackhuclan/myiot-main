using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Domain.Track;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Central.Core.Schedule.Deliver;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Central.Core.Schedule.Handler;

/// <summary>
/// 上下料AGV，待机超时一定时长后，触发agv与叉齿（中转区）换料仓，
/// 或者触发料架（线边仓）与叉齿（中转区）之间的流转；
/// </summary>
internal class ForkOrShelfExchangeSiloHandler : GeneralExchangeSiloHandler
{
    private readonly IDeviceManager _deviceManager;
    private readonly ILocationManager _locationManager;
    private readonly ILogger<ForkOrShelfExchangeSiloHandler> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;
    private readonly IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> _panelSiloShelfManager;
    private readonly IPartitionManager _partitionManager;
    private readonly IScheduleTaskDeliverPolicyFactory _scheduleTaskDeliverPolicyFactory;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly MysqlTaskSchedulerOptions _taskScheduleOptions;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly ITransferJobAdapter _transportationAdapter;

    public ForkOrShelfExchangeSiloHandler(IServiceProvider serviceProvider)
    : base(serviceProvider)
    {
        _loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = _loggerFactory.CreateLogger<ForkOrShelfExchangeSiloHandler>();
        _deviceManager = serviceProvider.GetRequiredService<IDeviceManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _scheduleTaskDeliverPolicyFactory = serviceProvider.GetRequiredService<IScheduleTaskDeliverPolicyFactory>();
        _taskScheduleOptions = serviceProvider.GetRequiredService<IOptions<MysqlTaskSchedulerOptions>>().Value;
        _transportationAdapter = serviceProvider.GetRequiredService<ITransferJobAdapter>();
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();

        _panelSiloForkManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();
        _panelSiloShelfManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>>();
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
    }

    protected override async Task ExchangeSilo(IEnumerable<DrillScheduleTask> orderedDrillSchedules)
    {
        _logger.LogInformation($"-----------------ForkOrShelfExchangeSiloHandler begin");

        int countOfEmptyForkAtLeast = await _sysConfigManager.GetIntValue("CountOfEmptyForkAtLeast", Admin.Model.Enum.SysConfigCategoryEnum.None, false);
        if (countOfEmptyForkAtLeast < 1)
        {
            countOfEmptyForkAtLeast = 1;
        }

        //var plan = PanelAgvTrackPlan.New();
        //var job1 = plan.NewJob();//from shelf to fork
        //var job2 = plan.NewJob(); //from fork to drill
        //var job3 = plan.NewJob(); //from drill to fork
        //_transferPlanManager.AddOrUpdatePlan(plan);

        var readyAgvDevices = _deviceManager.GetIdleBackPanelAgvs();
        foreach (var agv in readyAgvDevices.Where(x => x.RouteCodes.Any()))
        {
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN, Admin.Model.Enum.SysConfigCategoryEnum.None, false))
            {
                _logger.LogWarning($"中转区正在维护，暂停料仓转运的任务，请稍候.");
                return;
            }

            _logger.LogInformation($"ChangeSilo for agv {agv.DeviceKind} {agv.DeviceId}");
            await Task.Delay(5);

            //1.查找agv关联的工艺路线
            var filteredDrillSchedules = FilterDrillSchedulesByAgvRouteCode(agv, orderedDrillSchedules);

            //2.匹配料仓（插齿或者料架）
            //2.1.从插齿里面找，如果找到, 在插齿的单独处理方法里面去处理。
            //2.2.找不到，找料架，然后在料架方法去处理
            if (!await TryExchangingForkSilo(filteredDrillSchedules, agv, countOfEmptyForkAtLeast))
            {
                await TryHandExchangingShelfSilo(filteredDrillSchedules, agv, countOfEmptyForkAtLeast);
            }
        }

        _logger.LogInformation($"-----------------ForkOrShelfExchangeSiloHandler end");
    }

    protected override async Task HandleExportSiloFromOneFork(TrackJob trackJob, BackPanelAgv agv, int countOfEmptyForkAtLeast)
    {
        if (trackJob.MasterSchedule.TrackOutAppointed)
        {
            _logger.LogWarning($"HandleExportSiloFromOneFork,the drillAndForkPair MasterSchedule,{trackJob.MasterSchedule.Id} is already TrackOutAppointed, please waiting for a moment.");
        }
        else
        {
            if (!_taskScheduleOptions.EnableTransferSiloAgvHandler)
            {
                if (_locationManager.TryGetLocation(trackJob.ServantSchedule?.LocationCode, out var forkLocation)
                    && forkLocation != null
                    && forkLocation.Partition != null)
                {
                    //触发转出料仓, 限定料仓所在的插齿分区
                    ScheduleTaskWithRequest? unloadSchedule = null;
                    ScheduleTaskWithRequest? loadSchedule = null;
                    TransportationKind transportationKind = TransportationKind.None;
                    Partition? partition = null;

                    if (trackJob?.MasterSchedule.TrackOutAppointed == true)
                    {
                        _logger.LogWarning($"HandleExportSiloFromOneFork,the drillScheduleTask,{trackJob.MasterSchedule.Id} is already TrackOutAppointed, please waiting for a moment.");
                        return;
                    }

                    var readyShelfSiloAgv = _deviceManager.GetIdleTransferSiloAgvs().FirstOrDefault();
                    if (readyShelfSiloAgv == null)
                    {
                        _logger.LogWarning("Cannot find any ready ShelfSiloAgv to deal, please wait.");
                        return;
                    }

                    partition = forkLocation?.Partition;

                    if (!_panelSiloShelfManager.TryFindEmptyPayloadLocation(countOfEmptyForkAtLeast, out var emptyShelfLocation)
                        || emptyShelfLocation == null
                        || emptyShelfLocation.Schedule == null)
                    {
                        _logger.LogError("Cannot find any load shelf request to import, please check whether any shelf is disabled?");
                        return;
                    }
                    loadSchedule = emptyShelfLocation.Schedule;
                    unloadSchedule = await TransferSiloFromForkToShelfAsync(partition, trackJob, countOfEmptyForkAtLeast);

                    if (unloadSchedule == null)
                    {
                        _logger.LogError($"Cannot find any unload fork request in this fork partition {partition.PartCode} to export, please check whether any fork is disabled?");
                        return;
                    }
                    transportationKind = unloadSchedule.EventRequest.PayloadPanels.CalculateTransportationKind();

                    var allocationResult = await readyShelfSiloAgv.TransferSiloBetweenForkAndShelf(unloadSchedule, loadSchedule);
                    if (allocationResult == AgvAllocationResultCode.Success)
                    {
                        var transTask = new TransferJob
                        {
                            IsUrgent = 0,
                            InteractionSequence = InteractionSequence.UnloadOnly,
                            TransferDesc = $"钻机schedule：{trackJob.MasterSchedule.Id},钻机： {trackJob.MasterSchedule.CallerDeviceId};{Environment.NewLine} Panel AGV： {agv.DeviceId};{Environment.NewLine} Shelf AGV： {readyShelfSiloAgv.DeviceId};{Environment.NewLine}From：{unloadSchedule.LocationCode},to：{loadSchedule.LocationCode} ",
                            MasterScheduleId = trackJob.MasterSchedule.Id,
                            MasterLocationCode = trackJob.MasterSchedule.CallerDeviceId,
                            MasterRouteCode = trackJob.MasterSchedule.RouteCode,
                            PartitionCode = unloadSchedule.EventRequest.PartitionCode,
                            ForkCode = unloadSchedule.LocationCode,
                            OtherForkCode = loadSchedule.LocationCode,
                            TransportationKind = transportationKind,
                            SiloCode = unloadSchedule.EventRequest.PayloadPanels.SiloCode,
                            //InternalLotNo = requestOfUnloadSchedule.PayloadPanels.FirstOrDefault().ItemCode,
                            ClinkerCount = unloadSchedule.EventRequest.PayloadPanels.Count(x => ProductStatusConstants.Finished_DRILL.Contains(x.ProductStatus))
                        };

                        trackJob.MasterSchedule.TrackOutAppointed = true;
                        unloadSchedule.Appointed = true;
                        loadSchedule.Appointed = true;
                        await _transportationAdapter.Create(transTask);

                        _logger.LogInformation($"forkTrackOut TransportationTask:{transTask}");
                    }
                }
            }
        }
    }

    private async Task<bool> TryHandExchangingShelfSilo(IEnumerable<DrillScheduleTask> filteredDrillSchedules, BackPanelAgv agv, int countOfEmptyForkAtLeast)
    {
        var drillAndForkPair = MatchDrillAndShelfRequestPair(filteredDrillSchedules, agv);
        if (drillAndForkPair == null || drillAndForkPair.ServantSchedule == null)
        {
            _logger.LogWarning($"Not any drill request found in the shelf silos when changing silo for agv {agv.DeviceId},{agv.DeviceKind},{agv.DeviceStandbyTime}");
            return await Task.FromResult(false);
        }

        _logger.LogWarning($"{Environment.NewLine}-----------------尝试从料架处 拉取料仓,shelf:{drillAndForkPair.ServantSchedule.LocationCode} agv:{agv.DeviceId} {Environment.NewLine}");
        agv.TargetDevice = string.Empty;
        if (!_taskScheduleOptions.EnableTransferSiloAgvHandler)
        {
            //遍历插齿所在分区，查找尚未被预约的插齿分区
            //查找该分区下的空插齿位、并预约该分区；如果没有空位，则腾出空位置
            Mes.Model.Partition matchedPartition = null;
            var availablePartitions = _panelSiloForkManager.Partitions.Where(x => x != null
                && (string.IsNullOrEmpty(x.PreBookAGV) || x.PreBookAGV.ToLower() == agv.DeviceId.ToLower())
                //&& (string.IsNullOrEmpty(x.PreBookRoute) || x.PreBookRoute.ToLower() == drillAndForkPair.MasterSchedule.RouteCode?.ToLower())
                );

            //查找空插齿，准备放料仓
            ScheduleTaskWithRequest? emptyForkScheduleTask = null;
            foreach (var partition in availablePartitions)
            {
                if (_panelSiloForkManager.TryFindEmptyPayloadLocation(countOfEmptyForkAtLeast, partition?.PartCode, out var emptyForkLocation)
                    && emptyForkLocation != null)
                {
                    emptyForkScheduleTask = emptyForkLocation.Schedule;
                    matchedPartition = partition;
                    break;
                }
            }

            if (emptyForkScheduleTask != null && matchedPartition != null)
            {
                //触发料架到插齿的转运
                await TransferOneSiloFromShelfToFork(matchedPartition, emptyForkScheduleTask, drillAndForkPair.ServantSchedule, drillAndForkPair.MasterSchedule, agv);
            }
        }

        return await Task.FromResult(true);
    }

    private async Task TransferOneSiloFromShelfToFork(Partition matchedPartition,
        ScheduleTaskWithRequest forkLoadSchedule,
        ScheduleTaskWithRequest shelfUnloadSchedule,
        ScheduleTaskWithRequest drillScheduleTask,
        Agv agv)
    {
        if (drillScheduleTask.TrackInAppointed)
        {
            _logger.LogWarning($"TransferOneSiloFromShelfToFork,the drillScheduleTask,{drillScheduleTask.Id} is already TrackInAppointed, please waiting for a moment.");
            return;
        }
        var requestOfEmptyFork = forkLoadSchedule.EventRequest;
        if (requestOfEmptyFork == null)
        {
            _logger.LogWarning("HandleImportSiloToOneFork DeviceEventReportRequest requestOfEmptyFork is null.");
            return;
        }

        Location? locationOfForkLoadSchedule = null;
        if (_locationManager.TryGetLocation(forkLoadSchedule.LocationCode, out var location)
                    && location != null
                    && location.Partition != null)
        {
            locationOfForkLoadSchedule = location;
        }
        if (locationOfForkLoadSchedule == null)
        {
            _logger.LogWarning("HandleImportSiloToOneFork locationOfForkLoadSchedule is null.");
            return;
        }

        if (shelfUnloadSchedule == null)
        {
            _logger.LogError("Cannot find any unload shelf request to deal, please check whether all shelves are disabled or transfer any silo away manually.");
            return;
        }

        var readyShelfSiloAgv = _deviceManager.GetIdleTransferSiloAgvs().FirstOrDefault();

        if (readyShelfSiloAgv == null)
        {
            _logger.LogWarning("HandleImportSiloToOneFork, Cannot find any ready ShelfSiloAgv to deal, please wait.");
            return;
        }

        var allocationResult = await readyShelfSiloAgv.TransferSiloBetweenForkAndShelf(shelfUnloadSchedule, forkLoadSchedule);
        if (allocationResult == AgvAllocationResultCode.Success)
        {
            var transportationKind = shelfUnloadSchedule.EventRequest.PayloadPanels.CalculateTransportationKind();
            var transTask = new TransferJob
            {
                IsUrgent = 0,
                InteractionSequence = InteractionSequence.LoadOnly,
                TransferDesc = $"钻机：schedule:{drillScheduleTask.Id},drill {drillScheduleTask.CallerDeviceId};AGV {agv.DeviceId};来源：{shelfUnloadSchedule.LocationCode} ",
                InternalLotNo = drillScheduleTask.ItemCode.ToUpper(),
                RawCount = shelfUnloadSchedule.EventRequest.PayloadPanels.CountUndrilledPanels(),
                SiloCode = shelfUnloadSchedule.EventRequest.PayloadPanels.SiloCode,
                MasterScheduleId = drillScheduleTask.Id,
                MasterLocationCode = drillScheduleTask.CallerDeviceId,
                MasterRouteCode = drillScheduleTask.RouteCode,
                TransportationKind = transportationKind,
                PartitionCode = requestOfEmptyFork.PartitionCode,
                ForkCode = requestOfEmptyFork.LocationCode,
                OtherForkCode = shelfUnloadSchedule.LocationCode,
            };

            drillScheduleTask.TrackInAppointed = true;
            shelfUnloadSchedule.Appointed = true;
            forkLoadSchedule.Appointed = true;

            if (string.IsNullOrEmpty(matchedPartition.PreBookAGV))
            {
                var bookSolution = await _partitionManager.TryBookPartition(agv, locationOfForkLoadSchedule);
                if (!string.IsNullOrEmpty(bookSolution.ResponseMessage))
                {
                    _logger.LogWarning($"预约失败，{bookSolution.ResponseMessage}");
                    //continue;
                }
            }

            await _transportationAdapter.Create(transTask);
            _logger.LogInformation($"_forkTrackIn TransportationTask:{transTask}");
        }
    }

    private async Task<ScheduleTaskWithRequest> TransferSiloFromForkToShelfAsync(Partition? partition, TrackJob trackJob, int countOfEmptyForkAtLeast)
    {
        var countOfEmptyFork = _scheduleTaskManager.NotStartedForkSchedules.Count(x => x.InteractionSequence == InteractionSequence.LoadOnly && !x.Appointed);
        var countOfMovingOutFork = _panelSiloForkManager.EmptyPayloadSoonCount;
        int sumEmptyForks = countOfEmptyFork + countOfMovingOutFork;
        ScheduleTaskWithRequest result = null;
        var excludedLocations = new List<string>();

        if (trackJob.ServantSchedule.RequestDeviceKind == DeviceKind.PanelSiloFork)
        {
            excludedLocations.Add(trackJob.ServantSchedule.LocationCode);
        }

        var minDrilledTrackOutNum = await _sysConfigManager.GetIntValue("MinDrilledTrackOutNum", Admin.Model.Enum.SysConfigCategoryEnum.None, false);
        if (minDrilledTrackOutNum == 0) { minDrilledTrackOutNum = 5; }
        var autoTrackOutSilo = await _sysConfigManager.GetBoolValue("AutoTrackOutSilo", Admin.Model.Enum.SysConfigCategoryEnum.None, false);

        if (_panelSiloForkManager.TryFindFullDrilledLocation(new ConfigParameters
        {
            MinDrilledTrackOutNum = minDrilledTrackOutNum,
            AutoDrilledTrackOutSilo = autoTrackOutSilo,
        }, partition.PartCode, out var outboundingLocation)
            && outboundingLocation != null
            && _panelSiloShelfManager.TryFindEmptyPayloadLocation(countOfEmptyForkAtLeast, out var inboundingLocation)
            && inboundingLocation != null)
        {
            result = outboundingLocation.Schedule;
        }
        else if (_panelSiloForkManager.TryFindTimeoutUnloadingLocation(partition.PartCode, out var outboundingLocation2)
            && outboundingLocation2 != null
            && _panelSiloShelfManager.TryFindEmptyPayloadLocation(countOfEmptyForkAtLeast, out var inboundingLocation2)
            && inboundingLocation2 != null)
        {
            result = outboundingLocation2.Schedule;
        }
        else if (_panelSiloForkManager.TryFindNoDrillRequiredUndrilledLocation(partition.PartCode, out var outboundingLocation3)
            && outboundingLocation3 != null
            && _panelSiloShelfManager.TryFindEmptyPayloadLocation(countOfEmptyForkAtLeast, out var inboundingLocation3)
            && inboundingLocation3 != null)
        {
            result = outboundingLocation3.Schedule;
        }
        else if (sumEmptyForks < await _sysConfigManager.GetIntValue("CountOfEmptyForkAtLeast", Admin.Model.Enum.SysConfigCategoryEnum.None, false)
            && _panelSiloForkManager.TryFindLocation(partition.PartCode, excludedLocations.ToArray(), out var outboundingLocation4)
            && outboundingLocation4 != null
            && _panelSiloShelfManager.TryFindEmptyPayloadLocation(countOfEmptyForkAtLeast, out var inboundingLocation4)
            && inboundingLocation4 != null)
        {
            result = outboundingLocation4.Schedule;
        }

        return result;
    }
}
