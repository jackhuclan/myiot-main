using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Transportation;
using TransferJob = VgAutoDrill.Central.Core.Domain.TransferJob;

namespace VgAutoDrill.Central.Core.Calculator.WipOutside;

/// <summary>
/// 熟料从线边仓到外部退pin
/// </summary>
internal class DrilledFromWipToOutsideUnPinCalculator
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPartitionManager _partitionManager;
    private readonly IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> _wipManager;
    private readonly ILogger<DrilledFromWipToOutsideUnPinCalculator> _logger;

    public DrilledFromWipToOutsideUnPinCalculator(IServiceProvider serviceProvider)
    {
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _wipManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<DrilledFromWipToOutsideUnPinCalculator>();
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
            _logger.LogInformation($"TryGenerateTransferJob: can't find AvailableLocations in wip");
            return null;
        }

        if (_partitionManager.TryGetPartition(partitionCode, out var partition)
             || partition == null)
        {
            _logger.LogInformation($"TryGenerateTransferJob can't find partition with partitionCode={partitionCode}");
            return null;
        }

        var minDrilledTrackOutNum = await _sysConfigManager.GetIntValue("WipMinDrilledTrackOutNum", Admin.Model.Enum.SysConfigCategoryEnum.None, false);
        if (minDrilledTrackOutNum == 0) { minDrilledTrackOutNum = 5; }
        var autoTrackOutSilo = await _sysConfigManager.GetBoolValue("WipAutoTrackOutSilo", Admin.Model.Enum.SysConfigCategoryEnum.None, false);

        var foundTrackoutLocation = _wipManager.TryFindFullDrilledLocation(new ConfigParameters
        {
            MinDrilledTrackOutNum = minDrilledTrackOutNum,
            AutoDrilledTrackOutSilo = autoTrackOutSilo,
        }, partitionCode, out var trackoutLocation);

        if (!foundTrackoutLocation
            || trackoutLocation == null)
        {
            _logger.LogInformation($"TryGenerateTransferJob can't find drilled panel");
            return null;
        }

        var drilledItemCode = trackoutLocation.DrilledItemCodes.FirstOrDefault();
        var transTask = new TransferJob
        {
            IsUrgent = 0,
            InteractionSequence = InteractionSequence.UnloadOnly,
            PartitionCode = partitionCode,
            ForkCode = trackoutLocation.Code,
            TransportationKind = TransportationKind.Clinker,
            SiloCode = trackoutLocation.SiloCode,
            StartLocationCode = trackoutLocation.Code,
            StartDeviceId = trackoutLocation.DeviceId,
            StartScheduleId = trackoutLocation.ScheduleId,
            StartSchedule = trackoutLocation.ScheduleCode,
            TransferBehavior = SiloTransferBehavior.DRILLED_FROM_WIP_TO_OUTSIDE_UNPIN,
            TransferDesc = $"DRILLED_FROM_WIP_TO_OUTSIDE_UNPIN {drilledItemCode} from {trackoutLocation.Code} to 外部退pin线",
        };

        transTask.InternalLotNo = drilledItemCode;
        transTask.ExternalLotNo = GenNextExternalLotNo(drilledItemCode);
        transTask.ClinkerCount = trackoutLocation.DrilledPanelsCount;

        if (trackoutLocation.TryLock())
        {
            try
            {
                if (await _transferPlanManager.TryAddTransferJob(transTask))
                {
                    trackoutLocation.Appoint(transTask.TransferDesc);
                    _logger.LogInformation($"TryGenerateTransferJob: {transTask.TransferDesc}");

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
