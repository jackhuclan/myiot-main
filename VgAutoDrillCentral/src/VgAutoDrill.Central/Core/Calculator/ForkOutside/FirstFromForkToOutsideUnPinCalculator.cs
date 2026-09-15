using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Central.Core.Calculator.ForkOutside;

/// <summary>
/// 首件从中转位到外部退pin线
/// </summary>
internal class FirstFromForkToOutsideUnPinCalculator
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPartitionManager _partitionManager;
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;
    private readonly ILogger<FirstFromForkToOutsideUnPinCalculator> _logger;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);

    public FirstFromForkToOutsideUnPinCalculator(IServiceProvider serviceProvider)
    {
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _panelSiloForkManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<FirstFromForkToOutsideUnPinCalculator>();
    }

    public async Task<TransferJob> TryGenerateTransferJob(string partitionCode)
    {
        try
        {
            _autoResetEvent.WaitOne();
            _logger.LogInformation($"FirstFromForkToOutsideUnPinCalculator begin...");

            _logger.LogInformation($"FirstFromForkToOutsideUnPinCalculator for {partitionCode}...");
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN))
            {
                _logger.LogWarning($"系统维护，暂停料仓转运的任务，请稍候.");
                return null;
            }

            if (!_partitionManager.TryGetPartition(partitionCode, out var partition)
                || partition == null)
            {
                _logger.LogInformation($"FirstFromForkToOutsideUnPinCalculator can't find partition with partitionCode={partitionCode}");
                return null;
            }

            var minFirstDrilledTrackOutNum = await _sysConfigManager.GetIntValue("MinFirstDrilledTrackOutNum", Admin.Model.Enum.SysConfigCategoryEnum.None, false);
            if (minFirstDrilledTrackOutNum == 0) { minFirstDrilledTrackOutNum = 5; }

            var foundFirstTrackOutLocation = _panelSiloForkManager.TryFindFirstDrilledLocation(new ConfigParameters
            {
                MinFirstDrilledTrackOutNum = minFirstDrilledTrackOutNum
            }, partitionCode, out var firstTrackOutLocation);

            if (!foundFirstTrackOutLocation || firstTrackOutLocation == null)
            {
                _logger.LogInformation($"FirstFromForkToOutsideUnPinCalculator can't find first panel");
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
                TransferBehavior = SiloTransferBehavior.FIRST_DRILLED_FROM_FORK_TO_OUTSIDE_UNPIN,
                TransferDesc = $"FIRST_DRILLED_FROM_FORK_TO_OUTSIDE_UNPIN {firstItemCode} from {firstTrackOutLocation.Code} to 外部退pin线",
            };

            transTask.InternalLotNo = firstItemCode;
            transTask.ExternalLotNo = GenNextExternalLotNo(firstItemCode);
            transTask.ClinkerCount = firstTrackOutLocation.FirstPanelsCount;

            if (firstTrackOutLocation.Appointed)
            {
                _logger.LogWarning($"FirstFromForkToOutsideUnPinCalculator:  {firstTrackOutLocation.Code} has been Appointed Fail");
                return null;
            }

            if (firstTrackOutLocation.TryLock())
            {
                try
                {
                    if (await _transferPlanManager.TryAddTransferJob(transTask))
                    {
                        firstTrackOutLocation.Appoint(transTask.TransferDesc);
                        _logger.LogInformation($"FirstFromForkToOutsideUnPinCalculator: {firstTrackOutLocation.Code} Appointed Success, {transTask.TransferDesc}");

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
        finally
        {
            _logger.LogInformation($"FirstFromForkToOutsideUnPinCalculator end");
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
