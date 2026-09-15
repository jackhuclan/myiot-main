using System.Text.Json;
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
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Calculator.ForkOutside;

/// <summary>
/// 生料从外部到中转位
/// </summary>
internal class UndrilledFromOutsideToForkCalculator
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly IDrillManager _drillManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPartitionManager _partitionManager;
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;
    private readonly IPanelAgvManager<BackPanelAgv> _backPanelAgvManager;
    private readonly ILogger<UndrilledFromOutsideToForkCalculator> _logger;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private readonly ICalcutorUseAPI _calcutorUseAPI;
    private static int _cycle = 0;

    public UndrilledFromOutsideToForkCalculator(IServiceProvider serviceProvider)
    {
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _drillManager = serviceProvider.GetRequiredService<IDrillManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _panelSiloForkManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();
        _backPanelAgvManager = serviceProvider.GetRequiredService<IPanelAgvManager<BackPanelAgv>>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<UndrilledFromOutsideToForkCalculator>();
        _calcutorUseAPI = serviceProvider.GetRequiredService<ICalcutorUseAPI>();
    }

    public async Task<List<TransferJob>> TryGenerateTransferJob(string partitionCode)
    {
        try
        {
            _autoResetEvent.WaitOne();
            _logger.LogInformation($"UndrilledFromOutsideToForkCalculator begin...");

            if (_cycle >= int.MaxValue) _cycle = 0;
            _cycle++;

            _logger.LogInformation($"UndrilledFromOutsideToForkCalculator_{_cycle} for {partitionCode}...");

            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN))
            {
                _logger.LogWarning($"系统维护，暂停料仓转运的任务，请稍候.");
                return null;
            }

            List<TransferJob> result = new List<TransferJob>();

            if (!_panelSiloForkManager.PartitionAvailableLocations(partitionCode).Any())
            {
                _logger.LogInformation($"UndrilledFromOutsideToForkCalculator_{_cycle}: can't find AvailableLocations in {partitionCode} fork");
                return null;
            }
            if (!_partitionManager.TryGetPartition(partitionCode, out var partition)
                 || partition == null)
            {
                _logger.LogInformation($"UndrilledFromOutsideToForkCalculator_{_cycle}: TryGenerateTransferJob can't find partition with partitionCode={partitionCode}");
                return null;
            }

            if (_transferPlanManager.HasNotReadyPartitionFork(partition.PartCode))
            {
                _logger.LogInformation($"UndrilledFromOutsideToForkCalculator_{_cycle},TryGenerateTransferJob: exist NotReadyFork");
                return result;
            }

            int minEmptyPayloadFork = await _sysConfigManager.GetIntValue("CountOfEmptyForkAtLeast");
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_STAND_ALONE_PARTITION_SETTINGS))
            {
                minEmptyPayloadFork = partition.MinEmptyLocationNum.ToInt();
            }
            _logger.LogInformation($"UndrilledFromOutsideToForkCalculator_{_cycle},中转区:【{partitionCode}】,最少空位数:【{minEmptyPayloadFork}】.\r\n");

            var countEmptyPayloadForkNow = _panelSiloForkManager.PartitionEmptyPayloadNowCount(partition.PartCode);//现有空位数
            if (countEmptyPayloadForkNow <= minEmptyPayloadFork)
            {
                _logger.LogWarning($"UndrilledFromOutsideToForkCalculator_{_cycle}: {partition.PartCode}-当前剩余空库位，少于或等于系统限定的最少空库位，{countEmptyPayloadForkNow} <= {minEmptyPayloadFork} 不能移入生料仓");
                return null;
            }

            var routeCodes = partition.RouteCodes;

            var undrilledSummaryFromDrill = _drillManager.RouteRequiredUndrilledItemSummaries(routeCodes);
            if (undrilledSummaryFromDrill.Count() <= 0)
            {
                _logger.LogInformation($"UndrilledFromOutsideToForkCalculator_{_cycle},分区:【{partitionCode}】,未有钻机发起叫料请求.\r\n");
                return result;
            }

            Func<ItemSummary, string> keySelector = x => x.ItemCode;
            Func<IGrouping<string, ItemSummary>, ItemSummary> selector = x => new ItemSummary
            {
                ItemCode = x.Key,
                ItemCount = x.Sum(x => x.ItemCount),
            };

            foreach (var drillRequired in undrilledSummaryFromDrill)
            {
                if (await _transferPlanManager.TryGetTransByItemCode(drillRequired.ItemCode, InteractionSequence.LoadOnly, TransportationKind.Raw))
                {
                    _logger.LogInformation($"UndrilledFromOutsideToForkCalculator_{_cycle},分区:【{partitionCode}】,已经有料号：{drillRequired.ItemCode}的流转任务.\r\n");
                    continue;
                }

                var undrilledSummariesFromFork = partition.RackUndrilledItemSummaries;
                var undrilltedSummaryFromAgv = _backPanelAgvManager.RouteUndrilledItemSummaries(routeCodes);
                var undrilledSummariesFromTransferJobs = _transferPlanManager.PartitionUndrilledItemSummaries(partition.PartCode);
                //所有提供的物料
                var totalProvidedItems = undrilledSummariesFromFork
                    .Concat(undrilltedSummaryFromAgv, keySelector, selector)
                    .Concat(undrilledSummariesFromTransferJobs, keySelector, selector);

                var fromFork = totalProvidedItems.FirstOrDefault(x => x.ItemCode.ToLower() == drillRequired.ItemCode.ToLower());

                var singleUndrilledFromFork = _panelSiloForkManager.PartitionLocations(partition.PartCode).Where(p => p.ContainsUndrilledItemCode(drillRequired.ItemCode))?.Select(t => new { t.Code, t.UndrilledPanelsCount });

                string remark = $@"UndrilledFromOutsideToForkCalculator_{_cycle},
                              fromFork:{undrilledSummariesFromFork.FirstOrDefault(x => x.ItemCode.ToLower() == drillRequired.ItemCode.ToLower())?.ItemCount},
                              fromAgv:{undrilltedSummaryFromAgv.FirstOrDefault(x => x.ItemCode.ToLower() == drillRequired.ItemCode.ToLower())?.ItemCount},
                              fromTransJob: {undrilledSummariesFromTransferJobs.FirstOrDefault(x => x.ItemCode.ToLower() == drillRequired.ItemCode.ToLower())?.ItemCount},
                              ItemCode:{drillRequired.ItemCode},
                              HavedItemCount: {fromFork?.ItemCount},
                              drillRequired:{drillRequired.ItemCount},
                              Singlefork:{JsonSerializer.Serialize(singleUndrilledFromFork)}";

                _logger.LogInformation(remark);

                if (fromFork == null || drillRequired.ItemCount > fromFork.ItemCount)
                {
                    if (!_panelSiloForkManager.TryFindEmptyPayloadLocation(minEmptyPayloadFork, partition.PartCode, out var emptyPayloadForkLocation)
                        || emptyPayloadForkLocation == null)
                    {
                        _logger.LogInformation($"UndrilledFromOutsideToForkCalculator_{_cycle}: can't find EmptyPayload location from partition={partition.PartCode}, itemcode={drillRequired.ItemCode}");
                        return null;
                    }

                    var transTask = new TransferJob
                    {
                        IsUrgent = 0,
                        InteractionSequence = InteractionSequence.LoadOnly,
                        PartitionCode = emptyPayloadForkLocation.PartitionCode,
                        ForkCode = emptyPayloadForkLocation.Code,
                        TransportationKind = TransportationKind.Raw,
                        InternalLotNo = drillRequired.ItemCode,
                        AgvKind = DeviceKind.ShelfSiloAgv,

                        RawCount = drillRequired.ItemCount,
                        RelatedDrillScheduleIds = string.Join(',', drillRequired.ScheduleIds),
                        MasterScheduleId = drillRequired.ScheduleIds.FirstOrDefault(),
                        EndLocationCode = emptyPayloadForkLocation.Code,
                        EndDeviceId = emptyPayloadForkLocation.DeviceId,
                        EndScheduleId = emptyPayloadForkLocation.ScheduleId,
                        EndSchedule = emptyPayloadForkLocation.ScheduleCode,
                        TransferBehavior = SiloTransferBehavior.UNDRILLED_FROM_OUTSIDE_TO_FORK,
                        TransferDesc = $"UNDRILLED_FROM_OUTSIDE_TO_FORK from outside to {emptyPayloadForkLocation.Code},log:{remark}",
                    };

                    if (emptyPayloadForkLocation.Appointed)
                    {
                        _logger.LogWarning($"UndrilledFromOutsideToForkCalculator_{_cycle}:  {emptyPayloadForkLocation.Code} has been Appointed Fail");
                        return null;
                    }

                    // 检查是否有指定的lot
                    if (_calcutorUseAPI != null && (await _sysConfigManager.GetBoolValue("EnableCheckRawFromLineSide")))
                    {
                        _logger.LogInformation($"UndrilledFromOutsideToForkCalculator_{_cycle}: {partitionCode}: IsHaveRawStock: 检查线边仓是否有 {drillRequired.ItemCode} 生料!");
                        if (!(await _calcutorUseAPI.IsHaveRawStock(TransportationKind.Raw, drillRequired.ItemCode, "1")))
                        {
                            _logger.LogWarning($"UndrilledFromOutsideToForkCalculator_{_cycle}: {partitionCode}: 线边仓没有 {drillRequired.ItemCode} 生料!");
                            continue;
                        }
                    }

                    if (emptyPayloadForkLocation.TryLock())
                    {
                        try
                        {
                            if (emptyPayloadForkLocation.Available
                                && !_transferPlanManager.HasNotReadyPartitionFork(partition.PartCode)
                                && await _transferPlanManager.TryAddTransferJob(transTask))
                            {
                                emptyPayloadForkLocation.Appoint(transTask.TransferDesc);
                                _logger.LogInformation($"UndrilledFromOutsideToForkCalculator_{_cycle}: {emptyPayloadForkLocation.Code} Appointed Success!");
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
        catch (Exception ex)
        {
            _logger.LogError(ex, $"UndrilledFromOutsideToForkCalculator_{_cycle} Error: {ex.Message}");
            return null;
        }
        finally
        {
            _logger.LogInformation($"UndrilledFromOutsideToForkCalculator end");
            _autoResetEvent.Set();
        }
    }
}
