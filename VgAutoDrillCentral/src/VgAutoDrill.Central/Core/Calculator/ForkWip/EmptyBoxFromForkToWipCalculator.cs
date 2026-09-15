using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Central.Core.Calculator.ForkWip;

/// <summary>
/// 空料仓从中转位到线边仓
/// </summary>
internal class EmptyBoxFromForkToWipCalculator
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;
    private readonly IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> _wipManager;
    private readonly ILogger<EmptyBoxFromForkToWipCalculator> _logger;
    private readonly IPartitionManager _partitionManager;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private static int _cycle = 0;

    public EmptyBoxFromForkToWipCalculator(IServiceProvider serviceProvider)
    {
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _panelSiloForkManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();
        _wipManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<EmptyBoxFromForkToWipCalculator>();
    }

    public async Task<TransferJob?> TryGenerateTransferJob(string partitionCode)
    {
        try
        {
            _autoResetEvent.WaitOne();
            _logger.LogInformation($"EmptyBoxFromForkToWipCalculator begin...");

            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN))
            {
                _logger.LogWarning($"系统维护，暂停料仓转运的任务，请稍候.");
                return null;
            }

            if (_cycle >= int.MaxValue) _cycle = 0;
            _cycle++;

            _logger.LogInformation($"EmptyBoxFromForkToWipCalculator_{_cycle},TryGenerateTransferJob for 【{partitionCode}】...");

            var emptyBoxPartition = _partitionManager.PublicEmptySiloPartitions.FirstOrDefault();
            _logger.LogInformation($"EmptyBoxFromForkToWipCalculator_{_cycle},空仓线边仓区:【{emptyBoxPartition?.PartCode}】.\r\n");

            if (emptyBoxPartition != null && emptyBoxPartition.PartCode != null)
            {
                if (!_partitionManager.TryGetPartition(partitionCode, out var partition)
                    || partition == null)
                {
                    _logger.LogInformation($"EmptyBoxFromForkToWipCalculator_{_cycle},can't find partition with partitionCode=【{partitionCode}】.\r\n");
                    return null;
                }

                if (!_partitionManager.TryGetPartition(emptyBoxPartition.PartCode, out var wipPartition)
                    || wipPartition == null)
                {
                    _logger.LogInformation($"DrilledFromForkToWipCalculator_{_cycle},can't find wip partition with partitionCode=【{emptyBoxPartition.PartCode}】.\r\n");
                    return null;
                }

                int minEmptyPayloadFork = await _sysConfigManager.GetIntValue("WipCountOfEmptyForkAtLeast");
                if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_STAND_ALONE_PARTITION_SETTINGS))
                {
                    minEmptyPayloadFork = wipPartition.MinEmptyLocationNum.ToInt();
                }
                _logger.LogInformation($"EmptyBoxFromForkToWipCalculator_{_cycle},线边仓设定最少空位数:【{minEmptyPayloadFork}】.\r\n");

                var foundWip = _wipManager.TryFindEmptyPayloadLocation(minEmptyPayloadFork, emptyBoxPartition.PartCode, out var wipLocation);
                if (!foundWip || wipLocation == null)
                {
                    _logger.LogInformation($"EmptyBoxFromForkToWipCalculator_{_cycle},can't find EmptyPayload in wip.minEmptyPayloadFork is 【{minEmptyPayloadFork}】.\r\n");
                    return null;
                }
                _logger.LogInformation($"EmptyBoxFromForkToWipCalculator_{_cycle},find a wipLocation:【{wipLocation.Code}】.\r\n");

                var maxPartitionEmptySiloNum = await _sysConfigManager.GetIntValue("MaxPartitionEmptySiloNum");
                if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_STAND_ALONE_PARTITION_SETTINGS))
                {
                    maxPartitionEmptySiloNum = partition.MaxEmptyBoxNum.ToInt();
                }
                _logger.LogInformation($"EmptyBoxFromForkToWipCalculator_{_cycle},中转位最多空料仓的数量:【{maxPartitionEmptySiloNum}】.\r\n");

                var partitionEmptySiloBoxNowCount = _panelSiloForkManager.PartitionEmptySiloBoxNowCount(partition.PartCode);
                _logger.LogInformation($"EmptyBoxFromForkToWipCalculator_{_cycle},中转区现存空料仓数量:【{partitionEmptySiloBoxNowCount}】.\r\n");

                if (partitionEmptySiloBoxNowCount <= maxPartitionEmptySiloNum)
                {
                    _logger.LogInformation($"EmptyBoxFromForkToWipCalculator_{_cycle},partitionCode=【{partitionCode}】, partitionEmptySiloBoxNowCount<=【{maxPartitionEmptySiloNum}】.\r\n");
                    return null;
                }
                _logger.LogInformation($"EmptyBoxFromForkToWipCalculator_{_cycle},中转区现存空料仓数量 > 中转位最多空料仓的数量,触发转运料仓任务.\r\n");

                var foundEmptyBoxLocation = _panelSiloForkManager.TryFindEmptySiloBoxLocation(partition.PartCode, out var emptyboxLocation);
                if (!foundEmptyBoxLocation || emptyboxLocation == null)
                {
                    _logger.LogInformation($"EmptyBoxFromForkToWipCalculator_{_cycle},can't find EmptySiloBox from partition with partitionCode=【{partitionCode}】.\r\n");
                    return null;
                }
                _logger.LogInformation($"EmptyBoxFromForkToWipCalculator_{_cycle},find a fork emptyBox location:【{emptyboxLocation.Code}】.\r\n");

                if (wipLocation.TryLock())
                {
                    try
                    {
                        var transTask = new TransferJob
                        {
                            IsUrgent = 0,
                            InteractionSequence = InteractionSequence.UnloadOnly,
                            ScheduledTaskStatus = ScheduledTaskStatus.Created,
                            TransportationKind = TransportationKind.EmptySilo,
                            PartitionCode = partition.PartCode,
                            ForkCode = emptyboxLocation.Code,
                            TransferDesc = $"空料仓:从中转区【{emptyboxLocation.Code}】转运到空仓区【{wipLocation.Code}】",
                            ClinkerCount = 0,
                            RawCount = 0,
                            SiloCode = emptyboxLocation.SiloCode,
                            MasterRouteCode = emptyboxLocation.RouteCodes.FirstOrDefault(),
                            MasterDeviceKind = DeviceKind.PublicPanelSiloWIP,
                            AgvKind = DeviceKind.ShelfSiloAgv,
                            ForkLocationScheduleId = emptyboxLocation.Schedule?.Code,
                            DeviceLocationScheduleId = wipLocation.Schedule?.Code,
                            StartLocationCode = emptyboxLocation.Code,
                            StartDeviceId = emptyboxLocation.DeviceId,
                            StartScheduleId = emptyboxLocation.ScheduleId,
                            StartSchedule = emptyboxLocation.ScheduleCode,
                            EndLocationCode = wipLocation.Code,
                            EndDeviceId = wipLocation.DeviceId,
                            EndScheduleId = wipLocation.ScheduleId,
                            EndSchedule = wipLocation.ScheduleCode,
                            TransferBehavior = SiloTransferBehavior.EMPTY_BOX_FROM_FORK_TO_WIP
                        };

                        if (await _transferPlanManager.TryAddTransferJob(transTask))
                        {
                            emptyboxLocation.Appoint(transTask.TransferDesc);
                            wipLocation.Appoint(transTask.TransferDesc);

                            _logger.LogInformation($"EmptyBoxFromForkToWipCalculator_{_cycle},已生成料仓任务:【{transTask.TransferDesc}】.\r\n");

                            return transTask;
                        }
                    }
                    finally
                    {
                        wipLocation.ReleaseLock();
                    }
                }
                else
                {
                    _logger.LogInformation($"EmptyBoxFromForkToWipCalculator_{_cycle},【{wipLocation.Code}】 location is locked.\r\n");
                }
            }
            else
            {
                _logger.LogInformation($"EmptyBoxFromForkToWipCalculator_{_cycle},can't find partition function set emptyBoxPartition.\r\n");
            }

            return null;
        }
        finally
        {
            _logger.LogInformation($"EmptyBoxFromForkToWipCalculator end");
            _autoResetEvent.Set();
        }
    }
}
