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
using VgAutoDrill.Fundation.Utils;
using TransferJob = VgAutoDrill.Central.Core.Domain.TransferJob;

namespace VgAutoDrill.Central.Core.Calculator.ForkWip;

/// <summary>
/// 熟料从中转位到线边仓
/// </summary>
internal class DrilledFromForkToWipCalculator
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPartitionManager _partitionManager;
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;
    private readonly IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> _wipManager;
    private readonly ILogger<DrilledFromForkToWipCalculator> _logger;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private static int _cycle = 0;

    public DrilledFromForkToWipCalculator(IServiceProvider serviceProvider)
    {
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _panelSiloForkManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();
        _wipManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<DrilledFromForkToWipCalculator>();
    }

    public async Task<TransferJob?> TryGenerateTransferJob(string partitionCode)
    {
        try
        {
            _autoResetEvent.WaitOne();
            _logger.LogInformation($"DrilledFromForkToWipCalculator begin...");

            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN))
            {
                _logger.LogWarning($"系统维护，暂停料仓转运的任务，请稍候.");
                return null;
            }

            if (_cycle >= int.MaxValue) _cycle = 0;
            _cycle++;

            _logger.LogInformation($"DrilledFromForkToWipCalculator_{_cycle},TryGenerateTransferJob for 【{partitionCode}】...");

            var clinkerPartition = _partitionManager.PublicClinkerPartitions.FirstOrDefault();
            _logger.LogInformation($"DrilledFromForkToWipCalculator_{_cycle},熟料线边仓区:【{clinkerPartition?.PartCode}】.\r\n");

            if (clinkerPartition != null && clinkerPartition.PartCode != null)
            {
                if (!_partitionManager.TryGetPartition(partitionCode, out var partition)
                    || partition == null)
                {
                    _logger.LogInformation($"DrilledFromForkToWipCalculator_{_cycle},未能找到中转区:【{partitionCode}】信息.\r\n");
                    return null;
                }

                if (!_partitionManager.TryGetPartition(clinkerPartition.PartCode, out var wipPartition)
                    || wipPartition == null)
                {
                    _logger.LogInformation($"DrilledFromForkToWipCalculator_{_cycle},未能找到线边仓分区:【{clinkerPartition.PartCode}】信息.\r\n");
                    return null;
                }

                int minEmptyPayloadFork = await _sysConfigManager.GetIntValue("WipCountOfEmptyForkAtLeast");
                if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_STAND_ALONE_PARTITION_SETTINGS))
                {
                    minEmptyPayloadFork = wipPartition.MinEmptyLocationNum.ToInt();
                }
                _logger.LogInformation($"DrilledFromForkToWipCalculator_{_cycle},线边仓设定最少空位数:【{minEmptyPayloadFork}】.\r\n");

                var foundWip = _wipManager.TryFindEmptyPayloadLocation(minEmptyPayloadFork, clinkerPartition.PartCode, out var wipLocation);
                if (!foundWip || wipLocation == null)
                {
                    _logger.LogInformation($"DrilledFromForkToWipCalculator_{_cycle},熟料线边仓:【{clinkerPartition.PartCode}】没有可以转运的空库位.\r\n");
                    return null;
                }
                _logger.LogInformation($"DrilledFromForkToWipCalculator_{_cycle},熟料线边仓找到可用库位:【{wipLocation.Code}】.\r\n");

                //条件1：熟料最小转出数量
                var minDrilledTrackOutNum = await _sysConfigManager.GetIntValue("MinDrilledTrackOutNum");
                if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_STAND_ALONE_PARTITION_SETTINGS))
                {
                    minDrilledTrackOutNum = partition.MinDrilledTrackOutNum.ToInt();
                }
                if (minDrilledTrackOutNum == 0) { minDrilledTrackOutNum = 6; }
                _logger.LogInformation($"DrilledFromForkToWipCalculator_{_cycle},系统设定熟料最小转出数量:【{minDrilledTrackOutNum}】.\r\n");

                //条件2：是否自动转出
                var autoTrackOutSilo = await _sysConfigManager.GetBoolValue("AutoTrackOutSilo");
                if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_STAND_ALONE_PARTITION_SETTINGS))
                {
                    autoTrackOutSilo = partition.IsAutoTrackOutDrilledSilo.ToBool();
                }
                _logger.LogInformation($"DrilledFromForkToWipCalculator_{_cycle},当料仓中没有钻机中相同的料号时是否立即转出:【{autoTrackOutSilo}】.\r\n");

                //条件3：熟料占用超时时间设定
                var drilledItemOccupyLocationIdleTimeout = await _sysConfigManager.GetIntValue("DrilledItemOccupyLocationIdleTimeout");
                if (await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_STAND_ALONE_PARTITION_SETTINGS))
                {
                    drilledItemOccupyLocationIdleTimeout = partition.DrilledTrackOutTimeMinutes.ToInt();
                }
                _logger.LogInformation($"DrilledFromForkToWipCalculator_{_cycle},当前分区:【{partitionCode}】, 熟料转出超时时间设定:【{drilledItemOccupyLocationIdleTimeout} 分钟】.\r\n");

                //条件4：最少空库位数
                var countEmptyPayloadForkNow = _panelSiloForkManager.PartitionEmptyPayloadNowCount(partition.PartCode);//现有空位数
                _logger.LogInformation($"DrilledFromForkToWipCalculator_{_cycle},当前分区:【{partitionCode}】, 空库位数:【{countEmptyPayloadForkNow}】个.\r\n");

                var foundTrackoutLocation = _panelSiloForkManager.TryFindFullDrilledLocation(
                    new ConfigParameters
                    {
                        MinDrilledTrackOutNum = minDrilledTrackOutNum,
                        AutoDrilledTrackOutSilo = autoTrackOutSilo,
                        DrilledItemOccupyLocationIdleTimeout = drilledItemOccupyLocationIdleTimeout,
                        CountEmptyPayloadForkNow = countEmptyPayloadForkNow
                    },
                    partitionCode, out var trackoutLocation
                );
                if (!foundTrackoutLocation || trackoutLocation == null)
                {
                    _logger.LogInformation($"DrilledFromForkToWipCalculator_{_cycle},can't find drilled panel.\r\n");
                    return null;
                }
                _logger.LogInformation($"DrilledFromForkToWipCalculator_{_cycle},find a fork location:【{trackoutLocation.Code}】.\r\n");

                if (wipLocation.TryLock())
                {
                    try
                    {
                        var drilledItemCode = trackoutLocation.DrilledItemCodes.FirstOrDefault();
                        var transTask = new TransferJob
                        {
                            IsUrgent = 0,
                            InteractionSequence = InteractionSequence.UnloadOnly,
                            ScheduledTaskStatus = ScheduledTaskStatus.Created,
                            TransportationKind = TransportationKind.Clinker,
                            InternalLotNo = trackoutLocation.DrilledItemCodes.FirstOrDefault(),
                            PartitionCode = partitionCode,
                            ForkCode = trackoutLocation.Code,
                            TransferDesc = $"熟料:【{drilledItemCode}】,从中转区【{trackoutLocation.Code}】转运到熟料区【{wipLocation.Code}】",
                            ClinkerCount = trackoutLocation.DrilledPanelsCount,
                            RawCount = trackoutLocation.UndrilledPanelsCount,
                            SiloCode = trackoutLocation.SiloCode,
                            MasterRouteCode = trackoutLocation.RouteCodes.FirstOrDefault(),
                            MasterDeviceKind = DeviceKind.PublicPanelSiloWIP,
                            AgvKind = DeviceKind.ShelfSiloAgv,
                            ForkLocationScheduleId = trackoutLocation.Schedule?.Code,
                            DeviceLocationScheduleId = wipLocation.Schedule?.Code,
                            StartLocationCode = trackoutLocation.Code,
                            StartDeviceId = trackoutLocation.DeviceId,
                            StartScheduleId = trackoutLocation.ScheduleId,
                            StartSchedule = trackoutLocation.ScheduleCode,
                            EndLocationCode = wipLocation.Code,
                            EndDeviceId = wipLocation.DeviceId,
                            EndScheduleId = wipLocation.ScheduleId,
                            EndSchedule = wipLocation.ScheduleCode,
                            TransferBehavior = SiloTransferBehavior.DRILLED_FROM_FORK_TO_WIP
                        };

                        transTask.InternalLotNo = drilledItemCode;
                        transTask.ClinkerCount = trackoutLocation.DrilledPanelsCount;

                        if (await _transferPlanManager.TryAddTransferJob(transTask))
                        {
                            trackoutLocation.Appoint(transTask.TransferDesc);
                            wipLocation.Appoint(transTask.TransferDesc);

                            _logger.LogInformation($"DrilledFromForkToWipCalculator_{_cycle},已生成料仓任务:【{transTask.TransferDesc}】.\r\n");

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
                    _logger.LogInformation($"DrilledFromForkToWipCalculator_{_cycle},【{wipLocation.Code}】 location is locked.\r\n");
                }
            }
            else
            {
                _logger.LogInformation($"DrilledFromForkToWipCalculator_{_cycle},can't find partition function set clinkerPartition.\r\n");
            }

            return null;
        }
        finally
        {
            _logger.LogInformation($"DrilledFromForkToWipCalculator end");
            _autoResetEvent.Set();
        }
    }
}
