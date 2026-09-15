using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Schedule.Summary;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Central.Core.Calculator.ForkWip;

/// <summary>
/// 生料从线边仓到中转位
/// </summary>
internal class UndrilledFromWipToForkCalculator
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly IDrillManager _drillManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPartitionManager _partitionManager;
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;
    private readonly IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> _wipManager;
    private readonly IPanelAgvManager<BackPanelAgv> _backPanelAgvManager;
    private readonly ILogger<UndrilledFromWipToForkCalculator> _logger;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private static int _cycle = 0;

    public UndrilledFromWipToForkCalculator(IServiceProvider serviceProvider)
    {
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _drillManager = serviceProvider.GetRequiredService<IDrillManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _panelSiloForkManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();
        _wipManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>>();
        _backPanelAgvManager = serviceProvider.GetRequiredService<IPanelAgvManager<BackPanelAgv>>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<UndrilledFromWipToForkCalculator>();
    }

    public async Task<List<TransferJob>> TryGenerateTransferJob(string partitionCode)
    {
        try
        {
            _autoResetEvent.WaitOne();
            _logger.LogInformation($"UndrilledFromWipToForkCalculator begin...");

            List<TransferJob> result = new List<TransferJob>();

            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN))
            {
                _logger.LogWarning($"系统维护，暂停料仓转运的任务，请稍候.\r\n");
                return result;
            }

            if (_cycle >= int.MaxValue) _cycle = 0;
            _cycle++;

            _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle},TryGenerateTransferJob for 【{partitionCode}】...");

            if (!_partitionManager.TryGetPartition(partitionCode, out var partition)
                 || partition == null)
            {
                _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle},can't find partition with partitionCode=【{partitionCode}】.\r\n");
                return result;
            }

            if (_transferPlanManager.HasNotReadyPartitionFork(partition.PartCode))
            {
                _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle},TryGenerateTransferJob: exist NotReadyFork");
                return result;
            }

            int minEmptyPayloadFork = await _sysConfigManager.GetIntValue("CountOfEmptyForkAtLeast");//最少空位数
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_STAND_ALONE_PARTITION_SETTINGS))
            {
                minEmptyPayloadFork = partition.MinEmptyLocationNum.ToInt();
            }
            _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle},中转区设定最少空位数:【{minEmptyPayloadFork}】.\r\n");

            var countEmptyPayloadForkNow = _panelSiloForkManager.PartitionEmptyPayloadNowCount(partition.PartCode);
            _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle},分区:【{partition.PartCode}】，现有空位数:【{countEmptyPayloadForkNow}】.\r\n");

            if (countEmptyPayloadForkNow <= minEmptyPayloadFork)
            {
                _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle},分区:【{partition.PartCode}】，当前剩余空库位:【{countEmptyPayloadForkNow}】，少于或等于系统限定的最少空库位:【{minEmptyPayloadFork}】,不能移入生料仓.\r\n");
                return result;
            }

            var routeCodes = partition.RouteCodes;
            _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle},分区:【{partitionCode}】,所属工艺路线:【{string.Join(",", routeCodes)}】.\r\n");

            var undrilledSummaryFromDrill = _drillManager.RouteRequiredUndrilledItemSummaries(routeCodes);
            if (undrilledSummaryFromDrill.Count() <= 0)
            {
                _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle},分区:【{partitionCode}】,未有钻机发起叫料请求.\r\n");
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
                    _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle},分区:【{partitionCode}】,已经有料号：{drillRequired.ItemCode}的流转任务.\r\n");
                    continue;
                }

                //var undrilledSummariesFromFork = _panelSiloForkManager.PartitionUndrilledItemSummaries(partition.PartCode);
                var undrilledSummariesFromFork = partition.RackUndrilledItemSummaries;
                var undrilltedSummaryFromAgv = _backPanelAgvManager.RouteUndrilledItemSummaries(routeCodes);
                var undrilledSummariesFromTransferJobs = _transferPlanManager.PartitionUndrilledItemSummaries(partition.PartCode);

                //所有提供的物料
                var totalProvidedItems = undrilledSummariesFromFork
                    .Concat(undrilltedSummaryFromAgv, keySelector, selector)
                    .Concat(undrilledSummariesFromTransferJobs, keySelector, selector);

                var availableItem = totalProvidedItems.FirstOrDefault(x => x.ItemCode.ToLower() == drillRequired.ItemCode.ToLower());

                _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle}," +
                    $"【分区】:{partitionCode}," +
                    $"【钻机所需总板数】:{drillRequired.ItemCount.ToInt()}," +
                    $"【物料】:{drillRequired.ItemCode}," +
                    $"【中转区+AGV在途+已分配任务】:{availableItem?.ItemCount ?? 0}, " +
                    $"【中转区】:{undrilledSummariesFromFork.FirstOrDefault(x => x.ItemCode.ToLower() == drillRequired.ItemCode.ToLower())?.ItemCount ?? 0}，" +
                    $"【AGV在途】:{undrilltedSummaryFromAgv.FirstOrDefault(x => x.ItemCode.ToLower() == drillRequired.ItemCode.ToLower())?.ItemCount ?? 0}, " +
                    $"【料仓任务】:{undrilledSummariesFromTransferJobs.FirstOrDefault(x => x.ItemCode.ToLower() == drillRequired.ItemCode.ToLower())?.ItemCount ?? 0}。");

                var availableCount = availableItem?.ItemCount ?? 0;
                if (availableItem == null || drillRequired.ItemCount > availableCount)
                {
                    if (!_panelSiloForkManager.TryFindEmptyPayloadLocation(minEmptyPayloadFork, partition.PartCode, out var emptyPayloadForkLocation)
                    || emptyPayloadForkLocation == null)
                    {
                        _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle},can't find EmptyPayload location from partition=【{partition.PartCode}】, itemcode=【{drillRequired.ItemCode}】.\r\n");
                        continue;
                    }

                    var minTrackInQuantity = drillRequired.ItemCount - availableCount;//最少转入数量
                    var foundUndrilledLocation = _wipManager.TryFindHasUndrilledPanelsLocation(drillRequired.ItemCode, minTrackInQuantity, out var wipLocation);
                    _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle},wipLocation:{wipLocation?.Code}.\r\n");
                    if (!foundUndrilledLocation || wipLocation == null)
                    {
                        _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle},can't find undrilled wip location with itemcode=【{drillRequired.ItemCode}】.\r\n");
                        continue;
                    }

                    _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle},线边仓库位:【{wipLocation.Code}】存在物料:【{drillRequired.ItemCode}】,板料数量:【{wipLocation.UndrilledPanelsCount}】.\r\n");

                    _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle},分区:【{partitionCode}】,触发生成料仓转运任务···");

                    if (wipLocation.TryLock())
                    {
                        try
                        {
                            var transTask = new TransferJob
                            {
                                IsUrgent = 0,
                                InteractionSequence = InteractionSequence.LoadOnly,
                                ScheduledTaskStatus = ScheduledTaskStatus.Created,
                                TransportationKind = TransportationKind.Raw,
                                InternalLotNo = drillRequired.ItemCode,
                                PartitionCode = partition.PartCode,
                                ForkCode = emptyPayloadForkLocation.Code,
                                TransferDesc = $"生料:【{drillRequired.ItemCode}】,从生料区【{wipLocation.Code}】转运到中转区【{emptyPayloadForkLocation.Code}】",
                                ClinkerCount = wipLocation.DrilledPanelsCount,
                                RawCount = wipLocation.UndrilledPanelsCount,
                                SiloCode = wipLocation.SiloCode,
                                MasterRouteCode = emptyPayloadForkLocation.RouteCodes.FirstOrDefault(),
                                RelatedDrillScheduleIds = string.Join(',', drillRequired.ScheduleIds),
                                MasterDeviceKind = DeviceKind.PublicPanelSiloWIP,
                                AgvKind = DeviceKind.ShelfSiloAgv,
                                ForkLocationScheduleId = emptyPayloadForkLocation.Schedule?.Code,
                                DeviceLocationScheduleId = wipLocation.Schedule?.Code,
                                StartLocationCode = wipLocation.Code,
                                StartDeviceId = wipLocation.DeviceId,
                                StartScheduleId = wipLocation.ScheduleId,
                                StartSchedule = wipLocation.ScheduleCode,
                                EndLocationCode = emptyPayloadForkLocation.Code,
                                EndDeviceId = emptyPayloadForkLocation.DeviceId,
                                EndScheduleId = emptyPayloadForkLocation.ScheduleId,
                                EndSchedule = emptyPayloadForkLocation.ScheduleCode,
                                TransferBehavior = SiloTransferBehavior.UNDRILLED_FROM_WIP_TO_FORK
                            };

                            if (emptyPayloadForkLocation.Available
                                && !_transferPlanManager.HasNotReadyPartitionFork(partition.PartCode)
                                && await _transferPlanManager.TryAddTransferJob(transTask))
                            {
                                wipLocation.Appoint(transTask.TransferDesc);
                                emptyPayloadForkLocation.Appoint(transTask.TransferDesc);

                                _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle},已生成料仓任务:【{transTask.TransferDesc}】.\r\n");

                                result.Add(transTask);

                                countEmptyPayloadForkNow--;
                                if (countEmptyPayloadForkNow <= minEmptyPayloadFork)
                                {
                                    _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle},现有空位数:【{countEmptyPayloadForkNow}】<=线边仓设定最少空位数:【{minEmptyPayloadFork}】,停止生成转运任务.\r\n");
                                    break;
                                }
                            }
                        }
                        finally
                        {
                            wipLocation.ReleaseLock();
                        }
                    }
                    else
                    {
                        _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle},【{wipLocation.Code}】 or 【{emptyPayloadForkLocation.Code}】 location is locked.\r\n");
                    }
                }
                else
                {
                    _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle},钻机所需数量:【{drillRequired.ItemCount}】>AGV+中转位+料仓任务:【{availableItem?.ItemCount}】,无需转运料仓.\r\n");
                    continue;
                }
            }

            return result;
        }
        finally
        {
            _logger.LogInformation($"UndrilledFromWipToForkCalculator end");
            _autoResetEvent.Set();
        }
    }
}
