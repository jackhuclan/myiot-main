using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Schedule.Summary;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Central.Core.Calculator.WipOutside;

/// <summary>
/// 生料从外部到线边仓
/// </summary>
internal class UndrilledFromOutsideToWipCalculator
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly IDrillManager _drillManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPartitionManager _partitionManager;
    private readonly IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> _wipManager;
    private readonly IPanelAgvManager<BackPanelAgv> _backPanelAgvManager;
    private readonly ILogger<UndrilledFromOutsideToWipCalculator> _logger;
    private static int _cycle = 0;

    public UndrilledFromOutsideToWipCalculator(IServiceProvider serviceProvider)
    {
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _drillManager = serviceProvider.GetRequiredService<IDrillManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _wipManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>>();
        _backPanelAgvManager = serviceProvider.GetRequiredService<IPanelAgvManager<BackPanelAgv>>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<UndrilledFromOutsideToWipCalculator>();
    }

    public async Task<List<TransferJob>> TryGenerateTransferJob(string partitionCode)
    {
        _logger.LogInformation($"TryGenerateTransferJob for {partitionCode}...");
        List<TransferJob> result = new List<TransferJob>();

        _cycle++;

        if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN))
        {
            _logger.LogWarning($"系统维护，暂停料仓转运的任务，请稍候.");
            return result;
        }

        if (!_wipManager.AvailableLocations.Any())
        {
            _logger.LogInformation($"TryGenerateTransferJob: can't find AvailableLocations in wip");
            return null;
        }

        if (!_partitionManager.TryGetPartition(partitionCode, out var partition)
             || partition == null)
        {
            _logger.LogInformation($"TryGenerateTransferJob can't find partition with partitionCode={partitionCode}");
            return result;
        }

        int minEmptyPayloadFork = await _sysConfigManager.GetIntValue("WipCountOfEmptyForkAtLeast", Admin.Model.Enum.SysConfigCategoryEnum.None, false);//最少空位数
        var countEmptyPayloadForkNow = _wipManager.PartitionEmptyPayloadNowCount(partition.PartCode);//现有空位数
        if (countEmptyPayloadForkNow <= minEmptyPayloadFork)
        {
            _logger.LogWarning($"TryGenerateEmptyBoxTrackInTransferJob: {partition.PartCode}-当前剩余空库位，少于或等于系统限定的最少空库位，{countEmptyPayloadForkNow} <= {minEmptyPayloadFork} 不能移入空料仓");
            return null;
        }

        var routeCodes = partition.RouteCodes;
        var undrilledSummariesFromFork = _wipManager.PartitionUndrilledItemSummaries(partition.PartCode).ToList();
        var undrilledSummaryFromDrill = _drillManager.RequiredUndrilledItemSummaries;
        var undrilltedSummaryFromAgv = _backPanelAgvManager.UndrilledItemSummaries;
        var undrilledSummariesFromTransferJobs = _transferPlanManager.PartitionUndrilledItemSummaries(partition.PartCode);

        if (undrilledSummaryFromDrill.Count() <= 0)
        {
            _logger.LogInformation($"UndrilledFromOutsideToWipCalculator{_cycle},分区:【{partitionCode}】,未有钻机发起叫料请求.\r\n");
            return result;
        }

        Func<ItemSummary, string> keySelector = x => x.ItemCode;
        Func<IGrouping<string, ItemSummary>, ItemSummary> selector = x => new ItemSummary
        {
            ItemCode = x.Key,
            ItemCount = x.Sum(x => x.ItemCount),
        };

        //所有提供的物料
        var totalProvidedItems = undrilledSummariesFromFork
            .Concat(undrilltedSummaryFromAgv, keySelector, selector)
            .Concat(undrilledSummariesFromTransferJobs, keySelector, selector);

        var todoRawInItemCodes = undrilledSummariesFromTransferJobs.Select(x => x.ItemCode).ToList();

        foreach (var drillRequired in undrilledSummaryFromDrill.Where(x => !todoRawInItemCodes.Contains(x.ItemCode)).ToList())
        {
            var fromFork = undrilledSummariesFromFork.FirstOrDefault(x => x.ItemCode.ToLower() == drillRequired.ItemCode.ToLower());
            if (fromFork == null || drillRequired.ItemCount > fromFork.ItemCount)
            {
                if (!_wipManager.TryFindEmptyPayloadLocation(minEmptyPayloadFork, partition.PartCode, out var emptyPayloadForkLocation)
                    || emptyPayloadForkLocation == null)
                {
                    _logger.LogInformation($"TryGenerateTransferJob: can't find EmptyPayload location from partition={partition.PartCode}, itemcode={drillRequired.ItemCode}");
                    return null;
                }

                //var foundUndrilledLocation = _wipManager.TryFindHasUndrilledPanelsLocation(drillRequired.ItemCode, out var wipLocation);
                //if (!foundUndrilledLocation
                //    || wipLocation == null)
                //{
                //    _logger.LogInformation($"TryGenerateTransferJob: can't find undrilled wip location with itemcode={drillRequired.ItemCode}");
                //    return null;
                //}

                var transTask = new TransferJob
                {
                    IsUrgent = 0,
                    InteractionSequence = InteractionSequence.LoadOnly,
                    PartitionCode = emptyPayloadForkLocation.PartitionCode,
                    ForkCode = emptyPayloadForkLocation.Code,
                    TransportationKind = TransportationKind.Raw,
                    InternalLotNo = drillRequired.ItemCode,
                    RelatedDrillScheduleIds = string.Join(',', drillRequired.ScheduleIds),
                    MasterScheduleId = drillRequired.ScheduleIds.FirstOrDefault(),

                    //StartLocationCode = wipLocation.Code,
                    //StartDeviceId = wipLocation.DeviceId,
                    //StartScheduleId = wipLocation.ScheduleId,
                    EndLocationCode = emptyPayloadForkLocation.Code,
                    EndDeviceId = emptyPayloadForkLocation.DeviceId,
                    EndScheduleId = emptyPayloadForkLocation.ScheduleId,
                    EndSchedule = emptyPayloadForkLocation.ScheduleCode,
                    TransferBehavior = SiloTransferBehavior.UNDRILLED_FROM_OUTSIDE_TO_WIP,
                    TransferDesc = $"undrilled panels track out from outside to {emptyPayloadForkLocation.Code}",
                };

                if (emptyPayloadForkLocation.TryLock())
                {
                    try
                    {
                        if (await _transferPlanManager.TryAddTransferJob(transTask))
                        {
                            //wipLocation.Appoint();
                            emptyPayloadForkLocation.Appoint(transTask.TransferDesc);
                            _logger.LogInformation($"TryGenerateTransferJob: {transTask.TransferDesc}");
                            result.Add(transTask);

                            countEmptyPayloadForkNow--;
                            if (countEmptyPayloadForkNow <= minEmptyPayloadFork)
                            {
                                break;
                            }
                        }
                    }
                    finally
                    {
                        emptyPayloadForkLocation.ReleaseLock();
                    }
                }
            }
        }

        return result;
    }
}
