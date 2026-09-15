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

namespace VgAutoDrill.Central.Core.Calculator.ForkOutside;

/// <summary>
/// 空料仓从外部到中转位
/// </summary>
internal class EmptyBoxFromOutsideToForkCalculator
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPartitionManager _partitionManager;
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;
    private readonly ILogger<EmptyBoxFromOutsideToForkCalculator> _logger;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private static int _cycle = 0;
    private readonly ICalcutorUseAPI _calcutorUseAPI;
    public EmptyBoxFromOutsideToForkCalculator(IServiceProvider serviceProvider)
    {
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _panelSiloForkManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<EmptyBoxFromOutsideToForkCalculator>();
        _calcutorUseAPI = serviceProvider.GetRequiredService<ICalcutorUseAPI>();
    }

    public async Task<TransferJob> TryGenerateTransferJob(string partitionCode)
    {
        try
        {
            _autoResetEvent.WaitOne();
            _logger.LogInformation($"EmptyBoxFromOutsideToForkCalculator begin...");

            if (_cycle >= int.MaxValue) _cycle = 0;
            _cycle++;

            _logger.LogInformation($"EmptyBoxFromOutsideToForkCalculator_{_cycle} for {partitionCode}...");
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN))
            {
                _logger.LogWarning($"系统维护，暂停料仓转运的任务，请稍候.");
                return null;
            }

            if (!_panelSiloForkManager.PartitionAvailableLocations(partitionCode).Any())
            {
                _logger.LogInformation($"EmptyBoxFromOutsideToForkCalculator_{_cycle} :{partitionCode}: can't find AvailableLocations in {partitionCode} fork");
                return null;
            }

            if (!_partitionManager.TryGetPartition(partitionCode, out var partition)
                 || partition == null)
            {
                _logger.LogInformation($"EmptyBoxFromOutsideToForkCalculator_{_cycle}: can't find partition with partitionCode= {partitionCode}");
                return null;
            }

            if (_transferPlanManager.HasNotReadyPartitionFork(partition.PartCode))
            {
                _logger.LogInformation($"EmptyBoxFromOutsideToForkCalculator_{_cycle}_{partition.PartCode}: exist NotReadyFork");
                return null;
            }

            int minEmptyPayloadFork = await _sysConfigManager.GetIntValue("CountOfEmptyForkAtLeast");
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_STAND_ALONE_PARTITION_SETTINGS))
            {
                minEmptyPayloadFork = partition.MinEmptyLocationNum.ToInt();
            }
            _logger.LogInformation($"EmptyBoxFromOutsideToForkCalculator_{_cycle},中转区:【{partitionCode}】,最少空位数:【{minEmptyPayloadFork}】.\r\n");

            var countEmptyPayloadForkNow = _panelSiloForkManager.PartitionEmptyPayloadNowCount(partition.PartCode);//现有空位数
            if (countEmptyPayloadForkNow <= minEmptyPayloadFork)
            {
                _logger.LogWarning($"EmptyBoxFromOutsideToForkCalculator_{_cycle}: {partition.PartCode}-当前剩余空库位，少于或等于系统限定的最少空库位，{countEmptyPayloadForkNow} <= {minEmptyPayloadFork} 不能移入空料仓");
                return null;
            }

            var countOfEmptySilo = _panelSiloForkManager.PartitionEmptySiloBoxNowCount(partition.PartCode);
            _logger.LogInformation($"EmptyBoxFromOutsideToForkCalculator_{_cycle}:{partition.PartCode}: 现有空仓数={countOfEmptySilo}");

            //已预约的不计算在内
            var scheduledTaskStatusList = new List<ScheduledTaskStatus?>
            {
                ScheduledTaskStatus.Created,
                ScheduledTaskStatus.Allocated,
                ScheduledTaskStatus.Running
            };
            var countOfToBeImportedEmptySilo = _transferPlanManager.TransferJobs.Count(x => scheduledTaskStatusList.Contains(x.ScheduledTaskStatus)
                    && x.InteractionSequence == InteractionSequence.LoadOnly
                    && x.PartitionCode == partition.PartCode
                    && x.TransportationKind == TransportationKind.EmptySilo);
            _logger.LogInformation($"EmptyBoxFromOutsideToForkCalculator_{_cycle}:{partition.PartCode}: 已预约的空仓数={countOfToBeImportedEmptySilo}");

            int minPartitionEmptySiloNum = await _sysConfigManager.GetIntValue("MinPartitionEmptySiloNum");
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_STAND_ALONE_PARTITION_SETTINGS))
            {
                minPartitionEmptySiloNum = partition.MinEmptyBoxNum.ToInt();
            }

            _logger.LogInformation($"EmptyBoxFromOutsideToForkCalculator_{_cycle},中转区:【{partition.PartCode}】,系统设置最小空仓数=【{minPartitionEmptySiloNum}】");

            if (countOfEmptySilo + countOfToBeImportedEmptySilo >= minPartitionEmptySiloNum)
            {
                _logger.LogInformation($"EmptyBoxFromOutsideToForkCalculator_{_cycle}:{partition.PartCode}: 现有空仓数+已预约的空仓数 >= 系统设置最小空仓数, 不能转入空仓");
                return null;
            }

            var foundEmptyPayloadLocation = _panelSiloForkManager.TryFindEmptyPayloadLocation(minEmptyPayloadFork, partition.PartCode, out var emptyPayloadForkLocation);
            if (!foundEmptyPayloadLocation
                || emptyPayloadForkLocation == null)
            {
                _logger.LogInformation($"EmptyBoxFromOutsideToForkCalculator_{_cycle}:{partition.PartCode}: can't find EmptyPayload from partition {partition.PartCode}");
                return null;
            }

            var transTask = new TransferJob
            {
                IsUrgent = 0,
                InteractionSequence = InteractionSequence.LoadOnly,
                PartitionCode = partition.PartCode,
                ForkCode = emptyPayloadForkLocation.Code,
                TransportationKind = TransportationKind.EmptySilo,
                EndLocationCode = emptyPayloadForkLocation.Code,
                EndDeviceId = emptyPayloadForkLocation.DeviceId,
                EndScheduleId = emptyPayloadForkLocation.ScheduleId,
                EndSchedule = emptyPayloadForkLocation.ScheduleCode,
                TransferBehavior = SiloTransferBehavior.EMPTY_BOX_FROM_OUTSIDE_TO_FORK,
                TransferDesc = $"EMPTY_BOX_FROM_OUTSIDE_TO_FORK from outside to {emptyPayloadForkLocation.Code}",
            };

            if (emptyPayloadForkLocation.Appointed)
            {
                _logger.LogWarning($"EmptyBoxFromOutsideToForkCalculator_{_cycle} :{partition.PartCode}:  {emptyPayloadForkLocation.Code} has been Appointed Fail");
                return null;
            }

            // 检查是否有空仓
            if (_calcutorUseAPI != null && (await _sysConfigManager.GetBoolValue("EnableCheckEmptySiloFromLineSide")))
            {
                _logger.LogInformation($"EmptyBoxFromOutsideToForkCalculator_{_cycle}: {partitionCode}: IsHaveEmptySilo: 检查线边仓是否有空仓");
                if (!(await _calcutorUseAPI.IsHaveEmptySilo(TransportationKind.EmptySilo)))
                {
                    _logger.LogWarning($"EmptyBoxFromOutsideToForkCalculator_{_cycle}: {partitionCode}: 线边仓没有 空仓!");
                    return null;
                }
            }

            if (emptyPayloadForkLocation.TryLock())
            {
                try
                {
                    if (await _transferPlanManager.TryAddTransferJob(transTask))
                    {
                        emptyPayloadForkLocation.Appoint(transTask.TransferDesc);
                        _logger.LogInformation($"EmptyBoxFromOutsideToForkCalculator_{_cycle} :{partition.PartCode}: {emptyPayloadForkLocation.Code} Appointed Success, {transTask.TransferDesc}");

                        return transTask;
                    }
                }
                finally
                {
                    emptyPayloadForkLocation.ReleaseLock();
                }
            }

            return null;
        }
        finally
        {
            _logger.LogInformation($"EmptyBoxFromOutsideToForkCalculator end");
            _autoResetEvent.Set();
        }
    }
}
