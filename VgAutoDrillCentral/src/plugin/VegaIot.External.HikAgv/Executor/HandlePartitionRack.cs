using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Models;

namespace VegaIot.External.HikAgv.Executor;

internal class HandlePartitionRack
{
    private readonly ILogger<HandlePartitionRack> _logger;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly IPartitionManager _partitionManager;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> _wipManager;

    public HandlePartitionRack(ILogger<HandlePartitionRack> logger,
        IScheduleTaskManager scheduleTaskManager,
        ITransferPlanManager transferPlanManager,
        IPartitionManager partitionManager,
        ISysConfigManager sysConfigManager,
        IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> wipManager)
    {
        _logger = logger;
        _scheduleTaskManager = scheduleTaskManager;
        _partitionManager = partitionManager;
        _sysConfigManager = sysConfigManager;
        _wipManager = wipManager;
    }

    public async Task<Location?> Handle(TransferJob job)
    {
        var transferBehavior = job.TransferBehavior;
        var InternalLotNo = job.InternalLotNo;
        var location = new Location();
        Partition? partition;
        int minEmptyPayloadFork = await _sysConfigManager.GetIntValue("WipCountOfEmptyForkAtLeast", VgAutoDrill.Admin.Model.Enum.SysConfigCategoryEnum.None, false);//最少空位数

        switch (transferBehavior)
        {
            case SiloTransferBehavior.DRILLED_FROM_FORK_TO_WIP:

                _logger.LogInformation($"EmptyBoxFromForkToWipCalculator - drilledfromforktowip start calculator wiplocation ···");
                partition = _partitionManager.PublicClinkerPartitions.FirstOrDefault();
                if (partition != null && partition.PartCode != null)
                {
                    var foundWip = _wipManager.TryFindEmptyPayloadLocation(minEmptyPayloadFork, partition.PartCode, out var wipLocation);
                    if (!foundWip || wipLocation == null)
                    {
                        _logger.LogInformation($"EmptyBoxFromForkToWipCalculator, can't find EmptyPayload in wip.minEmptyPayloadFork is {minEmptyPayloadFork}");
                        return null;
                    }
                    return wipLocation;
                }
                break;

            case SiloTransferBehavior.EMPTY_BOX_FROM_FORK_TO_WIP:
                _logger.LogInformation($"EmptyBoxFromForkToWipCalculator - emptyboxfromforktowip start calculator wiplocation ···");
                partition = _partitionManager.PublicEmptySiloPartitions.FirstOrDefault();
                if (partition != null && partition.PartCode != null)
                {
                    var foundWip = _wipManager.TryFindEmptyPayloadLocation(minEmptyPayloadFork, partition.PartCode, out var wipLocation);
                    if (!foundWip || wipLocation == null)
                    {
                        _logger.LogInformation($"EmptyBoxFromForkToWipCalculator, can't find EmptyPayload in wip.minEmptyPayloadFork is {minEmptyPayloadFork}");
                        return null;
                    }
                    return wipLocation;
                }
                break;

            case SiloTransferBehavior.EMPTY_BOX_FROM_WIP_TO_FORK:
                _logger.LogInformation($"EmptyBoxFromForkToWipCalculator - emptyboxfromwiptofork start calculator wiplocation ···");
                partition = _partitionManager.PublicEmptySiloPartitions.FirstOrDefault();
                if (partition != null && partition.PartCode != null)
                {
                    var foundWip = _wipManager.TryFindEmptySiloBoxLocation(partition.PartCode, out var wipLocation);
                    if (!foundWip || wipLocation == null)
                    {
                        _logger.LogInformation($"EmptyBoxFromForkToWipCalculator, can't find EmptyPayload in wip.minEmptyPayloadFork is {minEmptyPayloadFork}");
                        return null;
                    }
                    return wipLocation;
                }
                break;

            case SiloTransferBehavior.UNDRILLED_FROM_FORK_TO_WIP:
                _logger.LogInformation($"EmptyBoxFromForkToWipCalculator - undrilledfromforktowip start calculator wiplocation ···");
                partition = _partitionManager.PublicRawPartitions.FirstOrDefault();
                if (partition != null && partition.PartCode != null)
                {
                    var foundWip = _wipManager.TryFindEmptyPayloadLocation(minEmptyPayloadFork, partition.PartCode, out var wipLocation);
                    if (!foundWip || wipLocation == null)
                    {
                        _logger.LogInformation($"EmptyBoxFromForkToWipCalculator, can't find EmptyPayload in wip.minEmptyPayloadFork is {minEmptyPayloadFork}");
                        return null;
                    }
                    return wipLocation;
                }
                break;

            case SiloTransferBehavior.UNDRILLED_FROM_WIP_TO_FORK:
                _logger.LogInformation($"EmptyBoxFromForkToWipCalculator - undrilledfromwiptowip start calculator wiplocation ···");
                partition = _partitionManager.PublicRawPartitions.FirstOrDefault();
                if (partition != null && partition.PartCode != null)
                {
                    var foundWip = _wipManager.TryFindHasUndrilledPanelsLocation(InternalLotNo!, job.RawCount ?? 1, out var wipLocation);
                    if (!foundWip || wipLocation == null)
                    {
                        _logger.LogInformation($"EmptyBoxFromForkToWipCalculator, can't find EmptyPayload in wip.minEmptyPayloadFork is {minEmptyPayloadFork}");
                        return null;
                    }
                    return wipLocation;
                }
                break;
        }

        return location;
    }
}
