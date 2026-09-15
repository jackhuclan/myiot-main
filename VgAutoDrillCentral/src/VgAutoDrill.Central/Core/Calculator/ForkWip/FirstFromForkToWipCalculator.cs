using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Central.Core.Calculator.ForkWip;

/// <summary>
/// 首件从中转位到线边仓
/// </summary>
internal class FirstFromForkToWipCalculator
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;
    private readonly IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> _wipManager;
    private readonly IPartitionManager _partitionManager;
    private readonly ILogger<FirstFromForkToWipCalculator> _logger;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);

    public FirstFromForkToWipCalculator(IServiceProvider serviceProvider)
    {
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _panelSiloForkManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();
        _wipManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<FirstFromForkToWipCalculator>();
    }

    public async Task<TransferJob?> TryGenerateTransferJob(string partitionCode)
    {
        try
        {
            _autoResetEvent.WaitOne();
            _logger.LogInformation($"FirstFromForkToWipCalculator begin...");

            _logger.LogInformation($"TryGenerateTransferJob for {partitionCode}...");
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN))
            {
                _logger.LogWarning($"系统维护，暂停料仓转运的任务，请稍候.");
                return null;
            }

            var foundWip = _wipManager.TryFindEmptyPayloadLocation(1, out var wipLocation);
            if (!foundWip || wipLocation == null)
            {
                _logger.LogInformation($"TryGenerateTransferJob can't find EmptyPayload in wip");
                return null;
            }

            if (!_partitionManager.TryGetPartition(partitionCode, out var partition)
                 || partition == null)
            {
                _logger.LogInformation($"TryGenerateTransferJob can't find partition with partitionCode={partitionCode}");
                return null;
            }

            var minFirstDrilledTrackOutNum = await _sysConfigManager.GetIntValue("MinFirstDrilledTrackOutNum", Admin.Model.Enum.SysConfigCategoryEnum.None, false);
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_STAND_ALONE_PARTITION_SETTINGS))
            {
                minFirstDrilledTrackOutNum = partition.MinFirstTrackOutNum.ToInt();
            }
            if (minFirstDrilledTrackOutNum == 0) { minFirstDrilledTrackOutNum = 5; }

            var foundFirstTrackOutLocation = _panelSiloForkManager.TryFindFirstDrilledLocation(new ConfigParameters
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
                EndLocationCode = wipLocation.Code,
                EndDeviceId = wipLocation.DeviceId,
                EndScheduleId = wipLocation.ScheduleId,
                EndSchedule = wipLocation.ScheduleCode,
                TransferBehavior = SiloTransferBehavior.FIRST_DRILLED_FROM_FORK_TO_WIP,
                TransferDesc = $"FIRST_DRILLED_FROM_FORK_TO_WIP track out {firstItemCode} from {firstTrackOutLocation.Code} to {wipLocation.Code}",
            };

            transTask.InternalLotNo = firstItemCode;
            transTask.ClinkerCount = firstTrackOutLocation.FirstPanelsCount;

            if (await _transferPlanManager.TryAddTransferJob(transTask))
            {
                firstTrackOutLocation.Appoint(transTask.TransferDesc);
                wipLocation.Appoint(transTask.TransferDesc);
                _logger.LogInformation($"TryGenerateTransferJob: {transTask.TransferDesc}");

                return transTask;
            }

            return null;
        }
        finally
        {
            _logger.LogInformation($"FirstFromForkToWipCalculator end");
            _autoResetEvent.Set();
        }
    }
}
