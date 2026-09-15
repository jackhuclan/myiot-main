using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;
using VgAutoDrill.Fundation.Utils;
using TransferJob = VgAutoDrill.Central.Core.Domain.TransferJob;

namespace VgAutoDrill.Central.Core.Calculator.ForkWip;

/// <summary>
/// 熟料从线边仓到中转位
/// </summary>
internal class DrilledFromWipToForkCalculator
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPartitionManager _partitionManager;
    private readonly IDrillManager _drillManager;
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;
    private readonly IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> _wipManager;
    private readonly IPanelAgvManager<BackPanelAgv> _backPanelAgvManager;
    private readonly ILogger<DrilledFromForkToWipCalculator> _logger;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private static int _cycle = 0;

    public DrilledFromWipToForkCalculator(IServiceProvider serviceProvider)
    {
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _drillManager = serviceProvider.GetRequiredService<IDrillManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _panelSiloForkManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();
        _wipManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>>();
        _backPanelAgvManager = serviceProvider.GetRequiredService<IPanelAgvManager<BackPanelAgv>>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<DrilledFromForkToWipCalculator>();
    }

    public async Task<List<TransferJob>> TryGenerateTransferJob(string partitionCode)
    {
        try
        {
            _autoResetEvent.WaitOne();

            _logger.LogInformation($"DrilledFromWipToForkCalculator begin...");

            List<TransferJob> result = new List<TransferJob>();

            if (_cycle >= int.MaxValue) _cycle = 0;
            _cycle++;

            if (!_partitionManager.TryGetPartition(partitionCode, out var partition)
                 || partition == null)
            {
                _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle},can't find partition with partitionCode=【{partitionCode}】.\r\n");
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

            foreach (var drillLocation in _drillManager.PartitionNotStartedLocations(partitionCode).Where(x => x.CanUnloadDrilledPanel))
            {
                var drilledItemCode = drillLocation.DrilledItemCodes.FirstOrDefault();
                var canAcceptDrilledPanelFromDrillByAgv = _backPanelAgvManager.PartitionPayloadDrilledSiloSummaries(partitionCode)
                        .Any(x => string.Equals(x.ItemCode, drilledItemCode, StringComparison.OrdinalIgnoreCase) && x.EmptyLayerCounts.Any(y => y >= drillLocation.DrilledPanelsCount));
                if (canAcceptDrilledPanelFromDrillByAgv)
                    continue;

                var canAcceptDrilledPanelFromDrillByFork = _panelSiloForkManager.PartitionPayloadDrilledSiloSummaries(partitionCode)
                        .Any(x => string.Equals(x.ItemCode, drilledItemCode, StringComparison.OrdinalIgnoreCase) && x.EmptyLayerCounts.Any(y => y >= drillLocation.DrilledPanelsCount));
                if (canAcceptDrilledPanelFromDrillByFork)
                    continue;

                var canAcceptDrilledPanelFromDrillByWip = _wipManager.PayloadDrilledSiloSummaries
                        .Any(x => string.Equals(x.ItemCode, drilledItemCode, StringComparison.OrdinalIgnoreCase) && x.EmptyLayerCounts.Any(y => y >= drillLocation.DrilledPanelsCount));

                if (canAcceptDrilledPanelFromDrillByWip)
                {
                    if (!_panelSiloForkManager.TryFindEmptyPayloadLocation(minEmptyPayloadFork, partitionCode, out var emptyPayloadForkLocation)
                        || emptyPayloadForkLocation == null)
                    {
                        _logger.LogInformation($"UndrilledFromWipToForkCalculator_{_cycle},can't find EmptyPayload location from partition=【{partition.PartCode}】, itemcode=【{drilledItemCode}】.\r\n");
                        return null;
                    }

                    var wipLocation = _wipManager.AvailableLocations.FirstOrDefault(x => x.CanAcceptDrilledItems(drillLocation.Requirement) && !x.IsEmptySiloBox);

                    var transTask = new TransferJob
                    {
                        IsUrgent = 0,
                        InteractionSequence = InteractionSequence.LoadOnly,
                        ScheduledTaskStatus = ScheduledTaskStatus.Created,
                        TransportationKind = TransportationKind.Clinker,
                        InternalLotNo = drilledItemCode,
                        PartitionCode = partition.PartCode,
                        ForkCode = emptyPayloadForkLocation.Code,
                        TransferDesc = $"DRILLED_FROM_WIP_TO_FORK track out {drilledItemCode} from {wipLocation.Code} to {emptyPayloadForkLocation.Code}",
                        ClinkerCount = wipLocation.DrilledPanelsCount,
                        RawCount = wipLocation.UndrilledPanelsCount,
                        SiloCode = wipLocation.SiloCode,
                        MasterRouteCode = emptyPayloadForkLocation.RouteCodes.FirstOrDefault(),
                        RelatedDrillScheduleIds = string.Join(',', drillLocation.ScheduleId),
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
                        TransferBehavior = SiloTransferBehavior.DRILLED_FROM_WIP_TO_FORK
                    };

                    if (await _transferPlanManager.TryAddTransferJob(transTask))
                    {
                        wipLocation.Appoint(transTask.TransferDesc);
                        emptyPayloadForkLocation.Appoint(transTask.TransferDesc);

                        _logger.LogInformation($"DrilledFromWipToForkCalculator_{_cycle},已生成料仓任务:【{transTask.TransferDesc}】.\r\n");

                        result.Add(transTask);

                        countEmptyPayloadForkNow--;
                        if (countEmptyPayloadForkNow <= minEmptyPayloadFork)
                        {
                            _logger.LogInformation($"DrilledFromWipToForkCalculator_{_cycle},现有空位数:【{countEmptyPayloadForkNow}】<=线边仓设定最少空位数:【{minEmptyPayloadFork}】,停止生成转运任务.\r\n");
                            break;
                        }
                    }
                }
            }

            return result;
        }
        finally
        {
            _logger.LogInformation($"DrilledFromWipToForkCalculator end");
            _autoResetEvent.Set();
        }
    }
}
