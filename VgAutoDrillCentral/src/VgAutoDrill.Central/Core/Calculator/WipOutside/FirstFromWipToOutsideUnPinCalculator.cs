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
/// 首件从线边仓到外部退pin线
/// </summary>
internal class FirstFromWipToOutsideUnPinCalculator
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPartitionManager _partitionManager;
    private readonly IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> _wipManager;
    private readonly ILogger<FirstFromWipToOutsideUnPinCalculator> _logger;

    public FirstFromWipToOutsideUnPinCalculator(IServiceProvider serviceProvider)
    {
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _wipManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<FirstFromWipToOutsideUnPinCalculator>();
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

        if (!_partitionManager.TryGetPartition(partitionCode, out var partition)
             || partition == null)
        {
            _logger.LogInformation($"TryGenerateTransferJob can't find partition with partitionCode={partitionCode}");
            return null;
        }

        var minFirstDrilledTrackOutNum = await _sysConfigManager.GetIntValue("WipMinFirstDrilledTrackOutNum", Admin.Model.Enum.SysConfigCategoryEnum.None, false);
        if (minFirstDrilledTrackOutNum == 0) { minFirstDrilledTrackOutNum = 5; }

        var foundFirstTrackOutLocation = _wipManager.TryFindFirstDrilledLocation(new ConfigParameters
        {
            MinFirstDrilledTrackOutNum = minFirstDrilledTrackOutNum
        }, partitionCode, out var firstTrackOutLocation);

        if (!foundFirstTrackOutLocation || firstTrackOutLocation == null)
        {
            _logger.LogInformation($"TryGenerateTransferJob can't find first panel");
            return null;
        }

        var firstItemCode = firstTrackOutLocation.FirstItemCodes.FirstOrDefault();
        var transTask = new TransferJob
        {
            IsUrgent = 0,
            InteractionSequence = InteractionSequence.UnloadOnly,
            PartitionCode = partitionCode,
            ForkCode = firstTrackOutLocation.Code,
            TransportationKind = TransportationKind.First,
            SiloCode = firstTrackOutLocation.SiloCode,
            StartLocationCode = firstTrackOutLocation.Code,
            StartDeviceId = firstTrackOutLocation.DeviceId,
            StartScheduleId = firstTrackOutLocation.ScheduleId,
            StartSchedule = firstTrackOutLocation.ScheduleCode,
            TransferBehavior = SiloTransferBehavior.FIRST_DRILLED_FROM_WIP_TO_OUTSIDE_UNPIN,
            TransferDesc = $"first drilled track out {firstItemCode} from {firstTrackOutLocation.Code} to 外部退pin线",
        };

        transTask.InternalLotNo = firstItemCode;
        transTask.ExternalLotNo = GenNextExternalLotNo(firstItemCode);
        transTask.ClinkerCount = firstTrackOutLocation.FirstPanelsCount;

        if (firstTrackOutLocation.TryLock())
        {
            try
            {
                if (await _transferPlanManager.TryAddTransferJob(transTask))
                {
                    firstTrackOutLocation.Appoint(transTask.TransferDesc);
                    _logger.LogInformation($"TryGenerateTransferJob: {transTask.TransferDesc}");

                    return transTask;
                }
            }
            finally
            {
                firstTrackOutLocation.ReleaseLock();
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
