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

namespace VgAutoDrill.Central.Core.Calculator.WipOutside;

/// <summary>
/// 空料仓从外部到线边仓
/// </summary>
internal class EmptyBoxFromOutsideToWipCalculator
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPartitionManager _partitionManager;
    private readonly IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> _wipManager;
    private readonly ILogger<EmptyBoxFromOutsideToWipCalculator> _logger;

    public EmptyBoxFromOutsideToWipCalculator(IServiceProvider serviceProvider)
    {
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _wipManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<EmptyBoxFromOutsideToWipCalculator>();
    }

    public async Task<TransferJob> TryGenerateTransferJob(string partitionCode)
    {
        _logger.LogInformation($"TryGenerateTransferJob for {partitionCode}...");
        if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN))
        {
            _logger.LogWarning($"系统维护，暂停料仓转运的任务，请稍候.");
            return null;
        }

        if (!_wipManager.AvailableLocations.Any())
        {
            _logger.LogInformation($"TryGenerateRawTrackInTransferJob: can't find AvailableLocations in wip");
            return null;
        }

        if (_partitionManager.TryGetPartition(partitionCode, out var partition)
             || partition == null)
        {
            _logger.LogInformation($"TryGenerateTransferJob: can't find partition with partitionCode={partitionCode}");
            return null;
        }

        if (_transferPlanManager.HasNotReadyPartitionFork(partition.PartCode))
        {
            _logger.LogInformation($"TryGenerateTransferJob: exist NotReadyFork");
            return null;
        }

        int minEmptyPayloadFork = await _sysConfigManager.GetIntValue("WipCountOfEmptyForkAtLeast", Admin.Model.Enum.SysConfigCategoryEnum.None, false);//最少空位数
        var countEmptyPayloadForkNow = _wipManager.PartitionEmptyPayloadNowCount(partition.PartCode);//现有空位数
        if (countEmptyPayloadForkNow <= minEmptyPayloadFork)
        {
            _logger.LogWarning($"TryGenerateTransferJob: {partition.PartCode}-当前剩余空库位，少于或等于系统限定的最少空库位，{countEmptyPayloadForkNow} <= {minEmptyPayloadFork} 不能移入空料仓");
            return null;
        }

        var countOfEmptySilo = _wipManager.PartitionEmptySiloBoxNowCount(partition.PartCode);
        _logger.LogInformation($"TryGenerateTransferJob: 现有空仓数={countOfEmptySilo}");

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
        _logger.LogInformation($"TryGenerateTransferJob: 已预约的空仓数={countOfToBeImportedEmptySilo}");

        int minPartitionEmptySiloNum = await _sysConfigManager.GetIntValue("WipMinPartitionEmptySiloNum", Admin.Model.Enum.SysConfigCategoryEnum.None, false);
        _logger.LogInformation($"TryGenerateTransferJob: 系统设置最小空仓数={minPartitionEmptySiloNum}");

        if (countOfEmptySilo + countOfToBeImportedEmptySilo >= minPartitionEmptySiloNum)
        {
            _logger.LogInformation($"TryGenerateTransferJob: 现有空仓数+已预约的空仓数 >= 系统设置最小空仓数, 不能转入空仓");
            return null;
        }

        var foundEmptyPayloadLocation = _wipManager.TryFindEmptyPayloadLocation(minEmptyPayloadFork, partition.PartCode, out var emptyPayloadForkLocation);
        if (!foundEmptyPayloadLocation
            || emptyPayloadForkLocation == null)
        {
            _logger.LogInformation($"TryGenerateTransferJob: can't find EmptyPayload from partition {partition.PartCode}");
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
            TransferBehavior = SiloTransferBehavior.EMPTY_BOX_FROM_OUTSIDE_TO_WIP,
            TransferDesc = $"empty box track out from outside to {emptyPayloadForkLocation.Code}",
        };

        if (emptyPayloadForkLocation.TryLock())
        {
            try
            {
                if (await _transferPlanManager.TryAddTransferJob(transTask))
                {
                    emptyPayloadForkLocation.Appoint(transTask.TransferDesc);
                    _logger.LogInformation($"TryGenerateTransferJob: {transTask.TransferDesc}");

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
}
