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

namespace VgAutoDrill.Central.Core.Calculator.ForkOutside;

/// <summary>
/// 空料仓从中转位到外部
/// </summary>
internal class EmptyBoxFromForkToOutsideCalculator
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;
    private readonly ILogger<EmptyBoxFromForkToOutsideCalculator> _logger;
    private readonly IPartitionManager _partitionManager;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private static int _cycle = 0;
    private readonly ICalcutorUseAPI _calcutorUseAPI;
    public EmptyBoxFromForkToOutsideCalculator(IServiceProvider serviceProvider)
    {
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _panelSiloForkManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<EmptyBoxFromForkToOutsideCalculator>();
        _calcutorUseAPI = serviceProvider.GetRequiredService<ICalcutorUseAPI>();
    }

    public async Task<TransferJob?> TryGenerateTransferJob(string partitionCode)
    {
        try
        {
            _autoResetEvent.WaitOne();
            _logger.LogInformation($"EmptyBoxFromForkToOutsideCalculator begin...");

            if (_cycle >= int.MaxValue) _cycle = 0;
            _cycle++;

            _logger.LogInformation($"EmptyBoxFromForkToOutsideCalculator_{_cycle} for {partitionCode}...");
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN, Admin.Model.Enum.SysConfigCategoryEnum.None, false))
            {
                _logger.LogWarning($"系统维护，暂停料仓转运的任务，请稍候.");
                return null;
            }

            if (!_panelSiloForkManager.PartitionAvailableLocations(partitionCode).Any())
            {
                _logger.LogInformation($"EmptyBoxFromForkToOutsideCalculator_{_cycle}: can't find AvailableLocations in {partitionCode} fork");
                return null;
            }

            if (!_partitionManager.TryGetPartition(partitionCode, out var partition)
                 || partition == null)
            {
                _logger.LogInformation($"EmptyBoxFromForkToOutsideCalculator_{_cycle}: can't find partition with partitionCode={partitionCode}");
                return null;
            }

            var maxPartitionEmptySiloNum = await _sysConfigManager.GetIntValue("MaxPartitionEmptySiloNum");
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_STAND_ALONE_PARTITION_SETTINGS))
            {
                maxPartitionEmptySiloNum = partition.MaxEmptyBoxNum.ToInt();
            }
            _logger.LogInformation($"EmptyBoxFromForkToOutsideCalculator_{_cycle},中转位最多空料仓的数量:【{maxPartitionEmptySiloNum}】.\r\n");

            var partitionEmptySiloBoxNowCount = _panelSiloForkManager.PartitionEmptySiloBoxNowCount(partition.PartCode);
            if (partitionEmptySiloBoxNowCount <= maxPartitionEmptySiloNum)
            {
                _logger.LogInformation($"EmptyBoxFromForkToOutsideCalculator_{_cycle}: partitionCode={partitionCode}, {partitionEmptySiloBoxNowCount}<={maxPartitionEmptySiloNum}");
                return null;
            }

            var foundEmptyBoxLocation = _panelSiloForkManager.TryFindEmptySiloBoxLocation(partition.PartCode, out var emptyboxLocation);
            if (!foundEmptyBoxLocation
               || emptyboxLocation == null)
            {
                _logger.LogInformation($"EmptyBoxFromForkToOutsideCalculator_{_cycle}: can't find EmptySiloBox from partition with partitionCode={partitionCode}");
                return null;
            }

            // 检查是否有空库位
            if (_calcutorUseAPI != null && (await _sysConfigManager.GetBoolValue("EnableCheckEmptyLocationFromLineSide")))
            {
                _logger.LogInformation($"EmptyBoxFromForkToOutsideCalculator_{_cycle}: {partitionCode}: IsHaveEmptyLocation: 检查待退pin 区是否有空位!");
                if (!(await _calcutorUseAPI.IsHaveEmptyLocation(TransportationKind.EmptySilo)))
                {
                    _logger.LogWarning($"EmptyBoxFromForkToOutsideCalculator_{_cycle}: {partitionCode}: 空仓区没有空位!");
                    return null;
                }
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
                TransferBehavior = SiloTransferBehavior.EMPTY_BOX_FROM_FORK_TO_OUTSIDE,
                TransferDesc = $"EMPTY_BOX_FROM_FORK_TO_OUTSIDE from {emptyboxLocation.Code} to outside",
            };

            if (emptyboxLocation.Appointed)
            {
                _logger.LogWarning($"EmptyBoxFromForkToOutsideCalculator_{_cycle}:  {emptyboxLocation.Code} has been Appointed Fail");
                return null;
            }

            if (emptyboxLocation.TryLock())
            {
                try
                {
                    if (await _transferPlanManager.TryAddTransferJob(transTask))
                    {
                        emptyboxLocation.Appoint(transTask.TransferDesc);
                        _logger.LogInformation($"EmptyBoxFromForkToOutsideCalculator_{_cycle}: {emptyboxLocation.Code} Appointed Success, {transTask.TransferDesc}");

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
        finally
        {
            _logger.LogInformation($"EmptyBoxFromForkToOutsideCalculator end");
            _autoResetEvent.Set();
        }
    }
}
