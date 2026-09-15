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
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Calculator.ForkWip;

/// <summary>
/// 空料仓从线边仓到中转位
/// </summary>
internal class EmptyBoxFromWipToForkCalculator
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPartitionManager _partitionManager;
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;
    private readonly IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> _wipManager;
    private readonly ILogger<EmptyBoxFromWipToForkCalculator> _logger;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private static int _cycle = 0;

    public EmptyBoxFromWipToForkCalculator(IServiceProvider serviceProvider)
    {
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _panelSiloForkManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();
        _wipManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<EmptyBoxFromWipToForkCalculator>();
    }

    public async Task<TransferJob?> TryGenerateTransferJob(string partitionCode)
    {
        try
        {
            _autoResetEvent.WaitOne();
            _logger.LogInformation($"EmptyBoxFromWipToForkCalculator begin...");

            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN))
            {
                _logger.LogWarning($"系统维护，暂停料仓转运的任务，请稍候.");
                return null;
            }

            if (_cycle >= int.MaxValue) _cycle = 0;
            _cycle++;

            _logger.LogInformation($"EmptyBoxFromWipToForkCalculator_{_cycle},TryGenerateTransferJob for 【{partitionCode}】...");

            var emptyBoxPartition = _partitionManager.PublicEmptySiloPartitions.FirstOrDefault();
            _logger.LogInformation($"EmptyBoxFromWipToForkCalculator_{_cycle},空料仓线边区:【{emptyBoxPartition?.PartCode}】.\r\n");

            if (emptyBoxPartition != null && emptyBoxPartition.PartCode != null)
            {
                if (!_partitionManager.TryGetPartition(partitionCode, out var partition)
                    || partition == null)
                {
                    _logger.LogInformation($"EmptyBoxFromWipToForkCalculator_{_cycle},can't find partition with partitionCode=【{partitionCode}】");
                    return null;
                }

                var foundWip = _wipManager.TryFindEmptySiloBoxLocation(emptyBoxPartition.PartCode, out var wipLocation);
                if (!foundWip || wipLocation == null)
                {
                    _logger.LogInformation($"EmptyBoxFromWipToForkCalculator_{_cycle},can't find EmptySiloBox in wip.\r\n");
                    return null;
                }
                _logger.LogInformation($"EmptyBoxFromWipToForkCalculator_{_cycle},find a wip location:【{wipLocation.Code}】.\r\n");

                if (_transferPlanManager.HasNotReadyPartitionFork(partition.PartCode))
                {
                    _logger.LogInformation($"EmptyBoxFromWipToForkCalculator_{_cycle},TryGenerateTransferJob: 【{partition.PartCode}】exist NotReadyFork");
                    return null;
                }

                int minEmptyPayloadFork = await _sysConfigManager.GetIntValue("CountOfEmptyForkAtLeast");
                if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_STAND_ALONE_PARTITION_SETTINGS))
                {
                    minEmptyPayloadFork = partition.MinEmptyLocationNum.ToInt();
                }
                _logger.LogInformation($"EmptyBoxFromWipToForkCalculator_{_cycle},中转区:【{partitionCode}】,最少空位数:【{minEmptyPayloadFork}】.\r\n");

                var countEmptyPayloadForkNow = _panelSiloForkManager.PartitionEmptyPayloadNowCount(partition.PartCode);
                _logger.LogInformation($"EmptyBoxFromWipToForkCalculator_{_cycle},中转区:【{partitionCode}】,现有空位数:【{countEmptyPayloadForkNow}】.\r\n");

                if (countEmptyPayloadForkNow <= minEmptyPayloadFork)
                {
                    _logger.LogWarning($"EmptyBoxFromWipToForkCalculator_{_cycle},当前剩余空库位，少于或等于系统限定的最少空库位，【{countEmptyPayloadForkNow}】 <= 【{minEmptyPayloadFork}】 不能移入空料仓");
                    return null;
                }

                var countOfEmptySilo = _panelSiloForkManager.PartitionEmptySiloBoxNowCount(partition.PartCode);
                _logger.LogInformation($"EmptyBoxFromWipToForkCalculator_{_cycle},现有空仓数=【{countOfEmptySilo}】");

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
                _logger.LogInformation($"EmptyBoxFromWipToForkCalculator_{_cycle},已预约的空仓数=【{countOfToBeImportedEmptySilo}】");

                int minPartitionEmptySiloNum = await _sysConfigManager.GetIntValue("MinPartitionEmptySiloNum");
                if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_STAND_ALONE_PARTITION_SETTINGS))
                {
                    minPartitionEmptySiloNum = partition.MinEmptyBoxNum.ToInt();
                }
                _logger.LogInformation($"EmptyBoxFromWipToForkCalculator_{_cycle},中转区:【{partitionCode}】,系统设置最小空仓数=【{minPartitionEmptySiloNum}】");

                if (countOfEmptySilo + countOfToBeImportedEmptySilo >= minPartitionEmptySiloNum)
                {
                    _logger.LogInformation($"EmptyBoxFromWipToForkCalculator_{_cycle},中转区:【{partitionCode}】,现有空仓数【{countOfEmptySilo}】+已预约的空仓数【{countOfToBeImportedEmptySilo}】 >= 系统设置最小空仓数【{minPartitionEmptySiloNum}】, 不能转入空仓");
                    return null;
                }
                _logger.LogInformation($"EmptyBoxFromWipToForkCalculator_{_cycle},中转区:【{partitionCode}】,现有空仓数【{countOfEmptySilo}】+已预约的空仓数【{countOfToBeImportedEmptySilo}】<【{minPartitionEmptySiloNum}】,满足转运条件.\r\n");

                var foundEmptyPayloadLocation = _panelSiloForkManager.TryFindEmptyPayloadLocation(minEmptyPayloadFork, partition.PartCode, out var emptyPayloadForkLocation);
                if (!foundEmptyPayloadLocation || emptyPayloadForkLocation == null)
                {
                    _logger.LogInformation($"EmptyBoxFromWipToForkCalculator_{_cycle},can't find EmptyPayload from partition 【{partition.PartCode}】");
                    return null;
                }
                _logger.LogInformation($"EmptyBoxFromWipToForkCalculator_{_cycle},find a empty fork location:{emptyPayloadForkLocation.Code}.\r\n");

                if (wipLocation.TryLock())
                {
                    try
                    {
                        var transTask = new TransferJob
                        {
                            IsUrgent = 0,
                            InteractionSequence = InteractionSequence.LoadOnly,
                            ScheduledTaskStatus = ScheduledTaskStatus.Created,
                            TransportationKind = TransportationKind.EmptySilo,
                            PartitionCode = partition.PartCode,
                            ForkCode = emptyPayloadForkLocation.Code,
                            TransferDesc = $"空料仓:从空仓区【{wipLocation.Code}】转运到中转区【{emptyPayloadForkLocation.Code}】",
                            ClinkerCount = 0,
                            RawCount = 0,
                            SiloCode = wipLocation.SiloCode,
                            MasterRouteCode = emptyPayloadForkLocation.RouteCodes.FirstOrDefault(),
                            MasterDeviceKind = DeviceKind.PublicPanelSiloWIP,
                            AgvKind = DeviceKind.ShelfSiloAgv,
                            ForkLocationScheduleId = emptyPayloadForkLocation.Schedule?.Code,
                            DeviceLocationScheduleId = wipLocation.Schedule?.Code,
                            StartLocationCode = wipLocation.Code,
                            StartDeviceId = wipLocation.DeviceId,
                            StartScheduleId = wipLocation.ScheduleId,
                            StartSchedule = wipLocation.ScheduleCode,
                            EndLocationCode = emptyPayloadForkLocation.Code,
                            EndDeviceId = emptyPayloadForkLocation.DeviceId,
                            EndScheduleId = emptyPayloadForkLocation.ScheduleId,
                            EndSchedule = emptyPayloadForkLocation.ScheduleCode,
                            TransferBehavior = SiloTransferBehavior.EMPTY_BOX_FROM_WIP_TO_FORK
                        };

                        if (await _transferPlanManager.TryAddTransferJob(transTask))
                        {
                            emptyPayloadForkLocation.Appoint(transTask.TransferDesc);
                            wipLocation.Appoint(transTask.TransferDesc);

                            _logger.LogInformation($"EmptyBoxFromWipToForkCalculator_{_cycle},已生成料仓任务:【{transTask.TransferDesc}】.\r\n");

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
                    _logger.LogInformation($"EmptyBoxFromWipToForkCalculator_{_cycle},【{wipLocation.Code}】 or 【{emptyPayloadForkLocation.Code}】 location is locked.\r\n");
                }
            }
            else
            {
                _logger.LogInformation($"EmptyBoxFromWipToForkCalculator_{_cycle},can't find partition function set emptyBoxPartition.\r\n");
            }

            return null;
        }
        finally
        {
            _logger.LogInformation($"EmptyBoxFromWipToForkCalculator end");
            _autoResetEvent.Set();
        }
    }
}
