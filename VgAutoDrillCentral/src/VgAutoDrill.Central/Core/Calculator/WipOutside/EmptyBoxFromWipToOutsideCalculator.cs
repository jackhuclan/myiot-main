using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Central.Core.Calculator.WipOutside;

/// <summary>
/// 空料仓从线边仓到外部
/// </summary>
internal class EmptyBoxFromWipToOutsideCalculator
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> _wipManager;
    private readonly ILogger<EmptyBoxFromWipToOutsideCalculator> _logger;
    private readonly IPartitionManager _partitionManager;

    public EmptyBoxFromWipToOutsideCalculator(IServiceProvider serviceProvider)
    {
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _wipManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<EmptyBoxFromWipToOutsideCalculator>();
    }

    public async Task<TransferJob?> TryGenerateTransferJob(string partitionCode)
    {
        _logger.LogInformation($"TryGenerateTransferJob for {partitionCode}...");
        if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN, Admin.Model.Enum.SysConfigCategoryEnum.None, false))
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

        var maxPartitionEmptySiloNum = await _sysConfigManager.GetIntValue("WipMaxPartitionEmptySiloNum");
        var partitionEmptySiloBoxNowCount = _wipManager.PartitionEmptySiloBoxNowCount(partition.PartCode);
        if (partitionEmptySiloBoxNowCount <= maxPartitionEmptySiloNum)
        {
            _logger.LogInformation($"TryGenerateTransferJob: partitionCode={partitionCode}, partitionEmptySiloBoxNowCount<={maxPartitionEmptySiloNum}");
            return null;
        }

        var foundEmptyBoxLocation = _wipManager.TryFindEmptySiloBoxLocation(partition.PartCode, out var emptyboxLocation);
        if (!foundEmptyBoxLocation
           || emptyboxLocation == null)
        {
            _logger.LogInformation($"TryGenerateTransferJob: can't find EmptySiloBox from partition with partitionCode={partitionCode}");
            return null;
        }

        //移出空料仓
        var transTask = new TransferJob
        {
            IsUrgent = 0,
            InteractionSequence = InteractionSequence.UnloadOnly,
            PartitionCode = partition.PartCode,
            ForkCode = emptyboxLocation.Code,
            TransportationKind = TransportationKind.EmptySilo,
            SiloCode = emptyboxLocation.SiloCode,
            StartLocationCode = emptyboxLocation.Code,
            StartDeviceId = emptyboxLocation.DeviceId,
            StartScheduleId = emptyboxLocation.ScheduleId,
            StartSchedule = emptyboxLocation.ScheduleCode,
            TransferBehavior = SiloTransferBehavior.EMPTY_BOX_FROM_WIP_TO_OUTSIDE,
            TransferDesc = $"empty box track out from {emptyboxLocation.Code} to outside",
        };

        if (emptyboxLocation.TryLock())
        {
            try
            {
                if (await _transferPlanManager.TryAddTransferJob(transTask))
                {
                    emptyboxLocation.Appoint(transTask.TransferDesc);
                    _logger.LogInformation($"TryGenerateTransferJob: {transTask.TransferDesc}");

                    return transTask;
                }
            }
            finally
            {
                emptyboxLocation.ReleaseLock();
            }
        }

        return null;
    }
}
