using System.Text.Json;
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

namespace VgAutoDrill.Central.Core.Calculator.ForkOutside;

/// <summary>
/// 暂时不用/空闲超过时长的生料从中转位到线边仓
/// </summary>
internal class UndrilledFromForkToOutsideCalculator
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IDrillManager _drillManager;
    private readonly IPartitionManager _partitionManager;
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;
    private readonly IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> _wipManager;
    private readonly ILogger<UndrilledFromForkToOutsideCalculator> _logger;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private static int _cycle = 0;
    private readonly ICalcutorUseAPI _calcutorUseAPI;
    public UndrilledFromForkToOutsideCalculator(IServiceProvider serviceProvider)
    {
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _drillManager = serviceProvider.GetRequiredService<IDrillManager>();
        _panelSiloForkManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();
        _wipManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<UndrilledFromForkToOutsideCalculator>();
        _calcutorUseAPI = serviceProvider.GetRequiredService<ICalcutorUseAPI>();
    }

    public async Task<TransferJob?> TryGenerateTransferJob(string partitionCode)
    {
        try
        {
            _autoResetEvent.WaitOne();
            _logger.LogInformation($"UndrilledFromForkToOutsideCalculator begin...");

            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN))
            {
                _logger.LogWarning($"系统维护，暂停料仓转运的任务，请稍候.");
                return null;
            }

            if (_cycle >= int.MaxValue) _cycle = 0;
            _cycle++;

            _logger.LogInformation($"UndrilledFromForkToOutsideCalculator{_cycle},TryGenerateTransferJob for 【{partitionCode}】...");

            if (!_partitionManager.TryGetPartition(partitionCode, out var partition)
                || partition == null)
            {
                _logger.LogInformation($"UndrilledFromForkToOutsideCalculator{_cycle},未找到中转区【{partitionCode}】信息.\r\n");
                return null;
            }

            var undrilledItemOccupyLocationIdleTimeout = await _sysConfigManager.GetIntValue("UndrilledItemOccupyLocationIdleTimeout");
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_STAND_ALONE_PARTITION_SETTINGS))
            {
                undrilledItemOccupyLocationIdleTimeout = partition.RawTrackOutTimeOutTime.ToInt();
            }

            _logger.LogInformation($"UndrilledFromForkToOutsideCalculator{_cycle},生料转出超时时间设定:【{undrilledItemOccupyLocationIdleTimeout} 秒】.\r\n");

            var partionUdrillerItemsCodes = _drillManager.PartitionRequiredUndrilledItemCodes(partitionCode);
            _logger.LogInformation($"UndrilledFromForkToOutsideCalculator{_cycle},分区:{partitionCode} 钻机所需的生料有:{JsonSerializer.Serialize(partionUdrillerItemsCodes)} \r\n");

            var foundTrackoutLocation = _panelSiloForkManager.TryFindIdleTimeoutUndrilledLocation(new ConfigParameters
            {
                UndrilledItemOccupyLocationIdleTimeout = undrilledItemOccupyLocationIdleTimeout,
            }, partitionCode, out var trackoutLocation);
            if (!foundTrackoutLocation || trackoutLocation == null)
            {
                _logger.LogInformation($"UndrilledFromForkToOutsideCalculator{_cycle},未能找到滞留的生料料仓.\r\n");
                return null;
            }
            _logger.LogInformation($"UndrilledFromForkToOutsideCalculator{_cycle},发现一个超过【{undrilledItemOccupyLocationIdleTimeout}】秒，滞留的生料料仓:【{trackoutLocation.Code}】.\r\n");

            // 检查是否有空库位
            if (_calcutorUseAPI != null && (await _sysConfigManager.GetBoolValue("EnableCheckEmptyLocationFromLineSide")))
            {
                _logger.LogInformation($"UndrilledFromForkToOutsideCalculator{_cycle}: {partitionCode}: IsHaveEmptyLocation: 检查线边仓是否有空位!");
                if (!(await _calcutorUseAPI.IsHaveEmptyLocation(TransportationKind.Raw)))
                {
                    _logger.LogWarning($"UndrilledFromForkToOutsideCalculator{_cycle}: {partitionCode}: 线边仓没有空位!");
                    return null;
                }
            }

            var undrilledItemCode = trackoutLocation.UndrilledItemCodes.FirstOrDefault();
            var transTask = new TransferJob
            {
                IsUrgent = 0,
                InteractionSequence = InteractionSequence.UnloadOnly,
                ScheduledTaskStatus = ScheduledTaskStatus.Created,
                TransportationKind = TransportationKind.Raw,
                InternalLotNo = undrilledItemCode,
                PartitionCode = partitionCode,
                ForkCode = trackoutLocation.Code,
                TransferDesc = $"UNDRILLED_FROM_FORK_TO_OUTSIDE track out {undrilledItemCode} from {trackoutLocation.Code} to Outside",
                ClinkerCount = trackoutLocation.DrilledPanelsCount,
                RawCount = trackoutLocation.UndrilledPanelsCount,
                SiloCode = trackoutLocation.SiloCode,
                MasterRouteCode = trackoutLocation.RouteCodes.FirstOrDefault(),
                MasterDeviceKind = DeviceKind.PanelSiloFork,
                AgvKind = DeviceKind.ShelfSiloAgv,
                ForkLocationScheduleId = trackoutLocation.Schedule?.Code,
                StartLocationCode = trackoutLocation.Code,
                StartDeviceId = trackoutLocation.DeviceId,
                StartScheduleId = trackoutLocation.ScheduleId,
                StartSchedule = trackoutLocation.ScheduleCode,
                TransferBehavior = SiloTransferBehavior.UNDRILLED_FROM_FORK_TO_OUTSIDE
            };

            if (trackoutLocation.Appointed)
            {
                _logger.LogWarning($"UndrilledFromForkToOutsideCalculator{_cycle}:  {trackoutLocation.Code} has been Appointed");
                return null;
            }

            if (trackoutLocation.TryLock())
            {
                try
                {
                    if (await _transferPlanManager.TryAddTransferJob(transTask))
                    {
                        trackoutLocation.Appoint(transTask.TransferDesc);

                        _logger.LogInformation($"UndrilledFromForkToOutsideCalculator{_cycle},成功生成料仓任务:【{transTask.TransferDesc}】.\r\n");

                        return transTask;
                    }
                }
                finally
                {
                    trackoutLocation.ReleaseLock();
                }
            }

            return null;
        }
        finally
        {
            _logger.LogInformation($"UndrilledFromForkToOutsideCalculator end");
            _autoResetEvent.Set();
        }
    }
}
