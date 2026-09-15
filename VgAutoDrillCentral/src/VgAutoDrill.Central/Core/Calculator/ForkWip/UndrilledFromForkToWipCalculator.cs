using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;
using TransferJob = VgAutoDrill.Central.Core.Domain.TransferJob;

namespace VgAutoDrill.Central.Core.Calculator.ForkWip;

/// <summary>
/// 暂时不用/空闲超过时长的生料从中转位到线边仓
/// </summary>
internal class UndrilledFromForkToWipCalculator
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPartitionManager _partitionManager;
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;
    private readonly IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> _wipManager;
    private readonly ILogger<UndrilledFromForkToWipCalculator> _logger;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private static int _cycle = 0;

    public UndrilledFromForkToWipCalculator(IServiceProvider serviceProvider)
    {
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _panelSiloForkManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();
        _wipManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<UndrilledFromForkToWipCalculator>();
    }

    public async Task<TransferJob?> TryGenerateTransferJob(string partitionCode)
    {
        try
        {
            _autoResetEvent.WaitOne();
            _logger.LogInformation($"UndrilledFromForkToWipCalculator begin...");

            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN))
            {
                _logger.LogWarning($"系统维护，暂停料仓转运的任务，请稍候.");
                return null;
            }

            if (_cycle >= int.MaxValue) _cycle = 0;
            _cycle++;

            _logger.LogInformation($"UndrilledFromForkToWipCalculator_{_cycle},TryGenerateTransferJob for 【{partitionCode}】...");

            var rawPartition = _partitionManager.PublicRawPartitions.FirstOrDefault();
            _logger.LogInformation($"UndrilledFromForkToWipCalculator_{_cycle},生料仓线边区:【{rawPartition?.PartCode}】.\r\n");

            if (rawPartition != null
                && rawPartition.PartCode != null)
            {
                if (!_partitionManager.TryGetPartition(partitionCode, out var partition)
                    || partition == null)
                {
                    _logger.LogInformation($"UndrilledFromForkToWipCalculator_{_cycle},未找到中转区【{partitionCode}】信息.\r\n");
                    return null;
                }

                if (!_partitionManager.TryGetPartition(rawPartition.PartCode, out var wipPartition)
                    || wipPartition == null)
                {
                    _logger.LogInformation($"DrilledFromForkToWipCalculator_{_cycle},未找到生料线边仓【{rawPartition.PartCode}】信息.\r\n");
                    return null;
                }

                int minEmptyPayloadFork = await _sysConfigManager.GetIntValue("WipCountOfEmptyForkAtLeast");
                if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_STAND_ALONE_PARTITION_SETTINGS))
                {
                    minEmptyPayloadFork = wipPartition.MinEmptyLocationNum.ToInt();
                }
                _logger.LogInformation($"UndrilledFromForkToWipCalculator_{_cycle},线边仓设定最少空位数:【{minEmptyPayloadFork}】.\r\n");

                var foundWip = _wipManager.TryFindEmptyPayloadLocation(minEmptyPayloadFork, rawPartition.PartCode, out var wipLocation);
                if (!foundWip || wipLocation == null)
                {
                    _logger.LogInformation($"UndrilledFromForkToWipCalculator_{_cycle},生料线边仓:【{rawPartition.PartCode}】没有可以转运的空库位.\r\n");
                    return null;
                }
                _logger.LogInformation($"UndrilledFromForkToWipCalculator_{_cycle},生料线边仓找到可用库位:【{wipLocation.Code}】.\r\n");

                var undrilledItemOccupyLocationIdleTimeout = await _sysConfigManager.GetIntValue("UndrilledItemOccupyLocationIdleTimeout");
                if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_STAND_ALONE_PARTITION_SETTINGS))
                {
                    undrilledItemOccupyLocationIdleTimeout = partition.RawTrackOutTimeOutTime.ToInt();
                }
                if (undrilledItemOccupyLocationIdleTimeout == 0) { undrilledItemOccupyLocationIdleTimeout = 30; }
                _logger.LogInformation($"UndrilledFromForkToWipCalculator_{_cycle},生料转出超时时间设定:【{undrilledItemOccupyLocationIdleTimeout}】.\r\n");

                var foundTrackoutLocation = _panelSiloForkManager.TryFindIdleTimeoutUndrilledLocation(new ConfigParameters
                {
                    UndrilledItemOccupyLocationIdleTimeout = undrilledItemOccupyLocationIdleTimeout,
                }, partitionCode, out var trackoutLocation);
                if (!foundTrackoutLocation || trackoutLocation == null)
                {
                    _logger.LogInformation($"UndrilledFromForkToWipCalculator_{_cycle},库区:【{partitionCode}】,未找到呆滞的生料料仓.\r\n");
                    return null;
                }
                _logger.LogInformation($"UndrilledFromForkToWipCalculator_{_cycle},库区:【{partitionCode}】,找到一个超过【{undrilledItemOccupyLocationIdleTimeout}】，呆滞的生料料仓:【{trackoutLocation.Code}】.\r\n");

                var undrilledItemCode = trackoutLocation.UndrilledItemCodes.FirstOrDefault();
                var transTask = new TransferJob
                {
                    IsUrgent = 0,
                    InteractionSequence = InteractionSequence.UnloadOnly,
                    ScheduledTaskStatus = ScheduledTaskStatus.Created,
                    TransportationKind = TransportationKind.Raw,
                    InternalLotNo = trackoutLocation.UndrilledItemCodes.FirstOrDefault(),
                    PartitionCode = partitionCode,
                    ForkCode = trackoutLocation.Code,
                    TransferDesc = $"UNDRILLED_FROM_FORK_TO_WIP track out {undrilledItemCode} from {trackoutLocation.Code} to {wipLocation.Code}",
                    ClinkerCount = trackoutLocation.DrilledPanelsCount,
                    RawCount = trackoutLocation.UndrilledPanelsCount,
                    SiloCode = trackoutLocation.SiloCode,
                    MasterRouteCode = trackoutLocation.RouteCodes.FirstOrDefault(),
                    MasterDeviceKind = DeviceKind.PublicPanelSiloWIP,
                    AgvKind = DeviceKind.ShelfSiloAgv,
                    ForkLocationScheduleId = trackoutLocation.Schedule?.Code,
                    DeviceLocationScheduleId = wipLocation.Schedule?.Code,
                    StartLocationCode = trackoutLocation.Code,
                    StartDeviceId = trackoutLocation.DeviceId,
                    StartScheduleId = trackoutLocation.ScheduleId,
                    StartSchedule = trackoutLocation.ScheduleCode,
                    EndLocationCode = wipLocation.Code,
                    EndDeviceId = wipLocation.DeviceId,
                    EndScheduleId = wipLocation.ScheduleId,
                    EndSchedule = wipLocation.ScheduleCode,
                    TransferBehavior = SiloTransferBehavior.UNDRILLED_FROM_FORK_TO_WIP
                };

                transTask.InternalLotNo = undrilledItemCode;
                transTask.RawCount = trackoutLocation.UndrilledPanelsCount;

                if (await _transferPlanManager.TryAddTransferJob(transTask))
                {
                    trackoutLocation.Appoint(transTask.TransferDesc);
                    wipLocation.Appoint(transTask.TransferDesc);

                    _logger.LogInformation($"UndrilledFromForkToWipCalculator_{_cycle},已生成料仓任务:【{transTask.TransferDesc}】.\r\n");

                    return transTask;
                }
            }
            else
            {
                _logger.LogInformation($"UndrilledFromForkToWipCalculator_{_cycle},can't find partition function set rawPartition");
            }
            return null;
        }
        finally
        {
            _logger.LogInformation($"UndrilledFromForkToWipCalculator end");
            _autoResetEvent.Set();
        }
    }
}
