using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Transportation;
using VgAutoDrill.Fundation.Utils;
using TransferJob = VgAutoDrill.Central.Core.Domain.TransferJob;

namespace VgAutoDrill.Central.Core.Calculator.ForkOutside;

/// <summary>
/// 熟料从中转位到外部退pin
/// </summary>
internal class DrilledFromForkToOutsideUnPinCalculator
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPartitionManager _partitionManager;
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;
    private readonly ILogger<DrilledFromForkToOutsideUnPinCalculator> _logger;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private static int _cycle = 0;
    private readonly ICalcutorUseAPI _calcutorUseAPI;
    public DrilledFromForkToOutsideUnPinCalculator(IServiceProvider serviceProvider)
    {
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _panelSiloForkManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<DrilledFromForkToOutsideUnPinCalculator>();
        _calcutorUseAPI = serviceProvider.GetRequiredService<ICalcutorUseAPI>();
    }

    public async Task<TransferJob?> TryGenerateTransferJob(string partitionCode)
    {
        try
        {
            _autoResetEvent.WaitOne();
            _logger.LogInformation($"DrilledFromForkToOutsideUnPinCalculator begin...");

            if (_cycle >= int.MaxValue) _cycle = 0;
            _cycle++;

            _logger.LogInformation($"DrilledFromForkToOutsideUnPinCalculator_{_cycle} for {partitionCode}...");
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN, Admin.Model.Enum.SysConfigCategoryEnum.None, false))
            {
                _logger.LogWarning($"系统维护，暂停料仓转运的任务，请稍候.");
                return null;
            }

            if (!_panelSiloForkManager.PartitionAvailableLocations(partitionCode).Any())
            {
                _logger.LogInformation($"DrilledFromForkToOutsideUnPinCalculator_{_cycle}: can't find AvailableLocations in {partitionCode} fork");
                return null;
            }

            if (!_partitionManager.TryGetPartition(partitionCode, out var partition)
                 || partition == null)
            {
                _logger.LogInformation($"DrilledFromForkToOutsideUnPinCalculator_{_cycle} can't find partition with partitionCode={partitionCode}");
                return null;
            }

            var minDrilledTrackOutNum = await _sysConfigManager.GetIntValue("MinDrilledTrackOutNum");
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_STAND_ALONE_PARTITION_SETTINGS))
            {
                minDrilledTrackOutNum = partition.MinDrilledTrackOutNum.ToInt();
            }

            _logger.LogInformation($"DrilledFromForkToOutsideUnPinCalculator_{_cycle},{partitionCode} :系统设定熟料最小转出数量:【{minDrilledTrackOutNum}】.\r\n");


            var autoTrackOutSilo = await _sysConfigManager.GetBoolValue("AutoTrackOutSilo");
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_STAND_ALONE_PARTITION_SETTINGS))
            {
                autoTrackOutSilo = partition.IsAutoTrackOutDrilledSilo.ToBool();
            }
            _logger.LogInformation($"DrilledFromForkToOutsideUnPinCalculator_{_cycle},{partitionCode} :当料仓中没有钻机中相同的料号时是否立即转出:【{autoTrackOutSilo}】.\r\n");

            var drilledItemOccupyLocationIdleTimeout = await _sysConfigManager.GetIntValue("DrilledItemOccupyLocationIdleTimeout");
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_STAND_ALONE_PARTITION_SETTINGS))
            {
                drilledItemOccupyLocationIdleTimeout = partition.DrilledTrackOutTimeMinutes.ToInt();
            }

            _logger.LogInformation($"DrilledFromForkToOutsideUnPinCalculator_{_cycle},{partitionCode}: 熟料转出超时时间设定:【{drilledItemOccupyLocationIdleTimeout} 分钟】.\r\n");

            var countEmptyPayloadForkNow = _panelSiloForkManager.PartitionEmptyPayloadNowCount(partition.PartCode);//现有空位数

            var foundTrackoutLocation = _panelSiloForkManager.TryFindFullDrilledLocation(new ConfigParameters
            {
                MinDrilledTrackOutNum = minDrilledTrackOutNum,
                AutoDrilledTrackOutSilo = autoTrackOutSilo,
                DrilledItemOccupyLocationIdleTimeout = drilledItemOccupyLocationIdleTimeout,
                CountEmptyPayloadForkNow = countEmptyPayloadForkNow
            }, partitionCode, out var trackoutLocation);

            if (!foundTrackoutLocation
                || trackoutLocation == null)
            {
                _logger.LogInformation($"DrilledFromForkToOutsideUnPinCalculator_{_cycle} {partitionCode}: can't find any matched location");
                return null;
            }

            // 检查是否有空库位
            if (_calcutorUseAPI != null && (await _sysConfigManager.GetBoolValue("EnableCheckEmptyLocationFromLineSide")))
            {
                _logger.LogInformation($"DrilledFromForkToOutsideUnPinCalculator_{_cycle}: {partitionCode}: IsHaveEmptyLocation: 检查待退pin 区是否有空位!");
                if (!(await _calcutorUseAPI.IsHaveEmptyLocation(TransportationKind.Clinker)))
                {
                    _logger.LogWarning($"DrilledFromForkToOutsideUnPinCalculator_{_cycle}: {partitionCode}: 熟料区没有空位!");
                    return null;
                }
            }

            var drilledItemCode = trackoutLocation.DrilledItemCodes.FirstOrDefault();
            var transTask = new TransferJob
            {
                IsUrgent = 0,
                InteractionSequence = InteractionSequence.UnloadOnly,
                PartitionCode = partitionCode,
                ForkCode = trackoutLocation.Code,
                TransportationKind = TransportationKind.Clinker,
                AgvKind = DeviceKind.ShelfSiloAgv,
                SiloCode = trackoutLocation.SiloCode,
                StartLocationCode = trackoutLocation.Code,
                StartDeviceId = trackoutLocation.DeviceId,
                StartScheduleId = trackoutLocation.ScheduleId,
                StartSchedule = trackoutLocation.ScheduleCode,
                TransferBehavior = SiloTransferBehavior.DRILLED_FROM_FORK_TO_OUTSIDE_UNPIN,
                TransferDesc = $"DRILLED_FROM_FORK_TO_OUTSIDE_UNPIN {drilledItemCode} from {trackoutLocation.Code} to 外部退pin线",
                InternalLotNo = drilledItemCode,
                ExternalLotNo = GenNextExternalLotNo(drilledItemCode),
                ClinkerCount = trackoutLocation.DrilledPanelsCount
            };

            if (trackoutLocation.Appointed)
            {
                _logger.LogWarning($"DrilledFromForkToOutsideUnPinCalculator_{_cycle}: {partitionCode} : {trackoutLocation.Code} has been Appointed Fail");
                return null;
            }

            if (trackoutLocation.TryLock())
            {
                try
                {
                    if (await _transferPlanManager.TryAddTransferJob(transTask))
                    {
                        trackoutLocation.Appoint(transTask.TransferDesc);
                        _logger.LogInformation($"DrilledFromForkToOutsideUnPinCalculator_{_cycle}:{partitionCode} : {trackoutLocation.Code} Appointed Success, {transTask.TransferDesc}");

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
            _logger.LogInformation($"DrilledFromForkToOutsideUnPinCalculator {partitionCode}  end");
            _autoResetEvent.Set();
        }
    }

    private static int seed = 1;

    private string? GenNextExternalLotNo(string itemCode)
    {
        Random random = new Random(seed++);
        var randomNumber = random.Next(1001, 400000);

        while (_transferPlanManager.TransferJobs.Any(x => x.ExternalLotNo == $"{itemCode}@{randomNumber}"
            && (x.TransportationKind == TransportationKind.Clinker || x.TransportationKind == TransportationKind.First)))
        {
            randomNumber = random.Next(1001, 400000);
        }

        return $"{itemCode}@{randomNumber}";
    }
}
