using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Schedule.Handler;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VegaIot.External.Bomin;

internal class BominUnpinScheduleHandler : IUnpinScheduleHandler
{
    private readonly ILogger<BominUnpinScheduleHandler> _logger;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> _wipManager;
    private readonly IPartitionManager _partitionManager;
    private readonly ILocationManager _locationManager;

    public BominUnpinScheduleHandler(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<BominUnpinScheduleHandler>>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _wipManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>>();
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
    }

    public async Task Handle()
    {
        if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN))
        {
            _logger.LogWarning($"系统即将维护，暂停分配新任务，请稍候.");
            return;
        }

        var queryUnpinSchedules = _scheduleTaskManager.NotStartedUnpinSchedules;
        var orderedUnpinSchedules = queryUnpinSchedules.OrderBy(s => s.Id);
        if (orderedUnpinSchedules.Count() <= 0)
        {
            //_logger.LogInformation($"BominUnpinScheduleHandler,【Unpin】暂未发起呼叫记录.\r\n");
            return;
        }

        int minEmptyPayloadFork = await _sysConfigManager.GetIntValue("WipCountOfEmptyForkAtLeast");
        _logger.LogInformation($"BominUnpinScheduleHandler,线边仓设定最少空位数:【{minEmptyPayloadFork}】.\r\n");

        var emptyBoxPartition = _partitionManager.PublicEmptySiloPartitions.FirstOrDefault();
        _logger.LogInformation($"BominUnpinScheduleHandler,空仓线边仓区:【{emptyBoxPartition?.PartCode}】.\r\n");

        var clinkerPartition = _partitionManager.PublicClinkerPartitions.FirstOrDefault();
        _logger.LogInformation($"BominUnpinScheduleHandler,熟料线边仓区:【{clinkerPartition?.PartCode}】.\r\n");

        int wipMinDrilledTrackOutNum = await _sysConfigManager.GetIntValue("WipMinDrilledTrackOutNum");
        _logger.LogInformation($"BominUnpinScheduleHandler,线边仓熟料最少自动转出数量:【{wipMinDrilledTrackOutNum}】.\r\n");

        foreach (var schedule in orderedUnpinSchedules)
        {
            _logger.LogInformation($"BominUnpinScheduleHandler,工位:【{schedule.LocationCode}】,schedule:【{schedule.Id}】,发起呼叫:【{schedule.InteractionSequence}】记录.\r\n");

            //1. unpin track out to wip
            if (schedule.Appointed == false
                && schedule.InteractionSequence == InteractionSequence.UnloadOnly
                && emptyBoxPartition != null
                && emptyBoxPartition.PartCode != null
                && schedule.LocationCode != null
                && _locationManager.TryGetLocation(schedule.LocationCode, out var outScheduleLocation)
                && outScheduleLocation != null
                && _wipManager.TryFindEmptyPayloadLocation(minEmptyPayloadFork, emptyBoxPartition.PartCode, out var inboundingLocation)
                && inboundingLocation != null
                && schedule.EventRequest != null
                && inboundingLocation.Schedule != null)
            {
                if (inboundingLocation.TryLock())
                {
                    try
                    {
                        var tranferJob = new TransferJob()
                        {
                            IsUrgent = 0,
                            InteractionSequence = InteractionSequence.LoadOnly,
                            ScheduledTaskStatus = ScheduledTaskStatus.Created,
                            TransportationKind = TransportationKind.EmptySilo,
                            PartitionCode = inboundingLocation.PartitionCode,
                            ForkCode = inboundingLocation.Code,
                            TransferDesc = $"空料仓:从退PIN【{schedule.LocationCode}】转运到空仓区【{inboundingLocation.Code}】",
                            ClinkerCount = 0,
                            RawCount = 0,
                            SiloCode = schedule.EventRequest.PayloadPanels.SiloCode,
                            RelateDeviceCode = schedule.LocationCode,
                            MasterDeviceKind = DeviceKind.UnPin,
                            AgvKind = DeviceKind.ShelfSiloAgv,
                            ForkLocationScheduleId = inboundingLocation.Schedule.Code,
                            DeviceLocationScheduleId = schedule.Code,
                            StartLocationCode = schedule.LocationCode,
                            StartDeviceId = schedule.EventRequest.DeviceId,
                            StartScheduleId = schedule.Id,
                            StartSchedule = schedule.Code,
                            EndLocationCode = inboundingLocation.Code,
                            EndDeviceId = inboundingLocation.DeviceId,
                            EndScheduleId = inboundingLocation.Schedule.Id,
                            EndSchedule = inboundingLocation.ScheduleCode,
                            TransferBehavior = SiloTransferBehavior.EMPTY_BOX_FROM_UNPIN_TO_WIP
                        };

                        if (await _transferPlanManager.TryAddTransferJob(tranferJob)
                               && inboundingLocation.Schedule != null)
                        {
                            inboundingLocation.Schedule.Appointed = true;
                            inboundingLocation.Schedule.AppointedMessage = tranferJob.TransferDesc;
                            schedule.Appointed = true;
                            schedule.AppointedMessage = tranferJob.TransferDesc;

                            _logger.LogInformation($"BominUnpinScheduleHandler,工位:【{schedule.LocationCode}】,schedule:【{schedule.Id}】,交互方式:【{schedule.InteractionSequence}】,已生成料仓任务.\r\n");
                        }
                    }
                    finally
                    {
                        inboundingLocation.ReleaseLock();
                    }
                }
                else
                {
                    _logger.LogInformation($"BominUnpinScheduleHandler - UnloadOnly,【{schedule.Code}】or【{inboundingLocation.Code}】location is locked.\r\n");
                }
            }

            //2. wip track out to unpin
            if (schedule.Appointed == false
                && schedule.InteractionSequence == InteractionSequence.LoadOnly
                && clinkerPartition != null
                && clinkerPartition.PartCode != null
                && schedule.LocationCode != null
                && _locationManager.TryGetLocation(schedule.LocationCode, out var inScheduleLocation)
                && inScheduleLocation != null
                && _wipManager.TryFindFullDrilledLocation(new ConfigParameters { MinDrilledTrackOutNum = wipMinDrilledTrackOutNum }, clinkerPartition.PartCode, out var outboundingLocation)
                && outboundingLocation != null
                && outboundingLocation.Schedule != null
                && schedule.EventRequest != null)
            {
                if (outboundingLocation.TryLock())
                {
                    try
                    {
                        var ItemCode = outboundingLocation.DrilledItemCodes.FirstOrDefault() ?? "";

                        var tranferJob = new TransferJob()
                        {
                            IsUrgent = 0,
                            InteractionSequence = InteractionSequence.UnloadOnly,
                            ScheduledTaskStatus = ScheduledTaskStatus.Created,
                            TransportationKind = TransportationKind.Clinker,
                            InternalLotNo = ItemCode,
                            PartitionCode = outboundingLocation.PartitionCode,
                            ForkCode = outboundingLocation.Code,
                            TransferDesc = $"熟料:【{ItemCode}】:从熟料仓区【{outboundingLocation.Code}】转运到退PIN【{schedule.LocationCode}】",
                            ClinkerCount = outboundingLocation.DrilledPanelsCount,
                            RawCount = outboundingLocation.UndrilledPanelsCount,
                            SiloCode = outboundingLocation.SiloCode,
                            MasterDeviceKind = DeviceKind.UnPin,
                            AgvKind = DeviceKind.ShelfSiloAgv,
                            ForkLocationScheduleId = outboundingLocation.Schedule.Code,
                            DeviceLocationScheduleId = schedule.Code,
                            StartLocationCode = outboundingLocation.Code,
                            StartDeviceId = outboundingLocation.DeviceId,
                            StartScheduleId = outboundingLocation.ScheduleId,
                            StartSchedule = outboundingLocation.ScheduleCode,
                            EndLocationCode = schedule.LocationCode,
                            EndDeviceId = schedule.EventRequest.DeviceId,
                            EndScheduleId = schedule.Id,
                            EndSchedule = schedule.Code,
                            TransferBehavior = SiloTransferBehavior.DRILLED_FROM_WIP_TO_UNPIN
                        };

                        if (await _transferPlanManager.TryAddTransferJob(tranferJob)
                               && outboundingLocation.Schedule != null)
                        {
                            outboundingLocation.Schedule.Appointed = true;
                            outboundingLocation.Schedule.AppointedMessage = tranferJob.TransferDesc;
                            schedule.Appointed = true;
                            schedule.AppointedMessage = tranferJob.TransferDesc;

                            _logger.LogInformation($"BominUnpinScheduleHandler,工位:【{schedule.LocationCode}】,schedule:【{schedule.Id}】,交互方式:【{schedule.InteractionSequence}】,已生成料仓任务.\r\n");
                        }
                    }
                    finally
                    {
                        outboundingLocation.ReleaseLock();
                    }
                }
                else
                {
                    _logger.LogInformation($"BominUnpinScheduleHandler - LoadOnly,【{inScheduleLocation.Code}】or【{outboundingLocation.Code}】location is locked.\r\n");
                }
            }
        }
    }
}
