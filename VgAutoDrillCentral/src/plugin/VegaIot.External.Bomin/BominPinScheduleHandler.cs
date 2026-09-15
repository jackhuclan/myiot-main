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

public class BominPinScheduleHandler : IPinScheduleHandler
{
    private readonly ILogger<BominPinScheduleHandler> _logger;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask> _wipManager;
    private readonly IPartitionManager _partitionManager;
    private readonly ILocationManager _locationManager;

    public BominPinScheduleHandler(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<BominPinScheduleHandler>>();
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

        IReadOnlyList<ScheduleTaskWithRequest> queryPinSchedules = _scheduleTaskManager.NotStartedPinSchedules;
        var orderedPinSchedules = queryPinSchedules.OrderBy(s => s.Id);
        if (orderedPinSchedules.Count() <= 0)
        {
            //_logger.LogInformation($"BominPinScheduleHandler,【pin】暂未发起呼叫记录.\r\n");
            return;
        }

        int minEmptyPayloadFork = await _sysConfigManager.GetIntValue("WipCountOfEmptyForkAtLeast");
        _logger.LogInformation($"BominPinScheduleHandler,线边仓设定最少空位数:【{minEmptyPayloadFork}】.\r\n");

        var emptyBoxPartition = _partitionManager.PublicEmptySiloPartitions.FirstOrDefault();
        _logger.LogInformation($"BominPinScheduleHandler,空仓线边仓区:【{emptyBoxPartition?.PartCode}】.\r\n");

        var rawPartition = _partitionManager.PublicRawPartitions.FirstOrDefault();
        _logger.LogInformation($"BominPinScheduleHandler,生料线边仓区:【{rawPartition?.PartCode}】.\r\n");

        foreach (var schedule in orderedPinSchedules)
        {
            _logger.LogInformation($"BominPinScheduleHandler,工位:【{schedule.LocationCode}】,schedule:【{schedule.Id}】,发起呼叫:【{schedule.InteractionSequence}】记录.\r\n");

            //1. pin track out to wip
            if (schedule.Appointed == false
                && schedule.InteractionSequence == InteractionSequence.UnloadOnly
                && rawPartition != null
                && rawPartition.PartCode != null
                && schedule.LocationCode != null
                && _locationManager.TryGetLocation(schedule.LocationCode, out var outScheduleLocation)
                && outScheduleLocation != null
                && _wipManager.TryFindEmptyPayloadLocation(minEmptyPayloadFork, rawPartition.PartCode, out var inboundingLocation)
                && inboundingLocation != null
                && schedule.EventRequest != null
                && inboundingLocation.Schedule != null)
            {
                if (inboundingLocation.TryLock())
                {
                    try
                    {
                        var ItemCode = schedule.EventRequest.PayloadPanels.UndrilledItemCodes.FirstOrDefault() ?? "";

                        var tranferJob = new TransferJob()
                        {
                            IsUrgent = 0,
                            InteractionSequence = InteractionSequence.LoadOnly,
                            ScheduledTaskStatus = ScheduledTaskStatus.Created,
                            TransportationKind = TransportationKind.Raw,
                            InternalLotNo = ItemCode,
                            PartitionCode = inboundingLocation.PartitionCode,
                            ForkCode = inboundingLocation.Code,
                            TransferDesc = $"生料【{ItemCode}】:从PIN【{schedule.LocationCode}】转运到生料区【{inboundingLocation.Code}】",
                            ClinkerCount = 0,
                            RawCount = schedule.EventRequest.PayloadPanels.Count(x => ProductStatusConstants.Finished_PIN.Contains(x.ProductStatus)),
                            SiloCode = schedule.EventRequest.PayloadPanels.SiloCode,
                            RelateDeviceCode = schedule.LocationCode,
                            MasterDeviceKind = DeviceKind.Pin,
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
                            EndSchedule = inboundingLocation.Schedule.Code,
                            TransferBehavior = SiloTransferBehavior.UNDRILLED_FROM_PIN_TO_WIP
                        };

                        if (await _transferPlanManager.TryAddTransferJob(tranferJob)
                               && inboundingLocation.Schedule != null)
                        {
                            inboundingLocation.Schedule.Appointed = true;
                            inboundingLocation.Schedule.AppointedMessage = tranferJob.TransferDesc;

                            schedule.Appointed = true;
                            schedule.AppointedMessage = tranferJob.TransferDesc;

                            _logger.LogInformation($"BominPinScheduleHandler,工位:【{schedule.LocationCode}】,schedule:【{schedule.Id}】,交互方式:【{schedule.InteractionSequence}】,已生成料仓任务.\r\n");
                        }
                    }
                    finally
                    {
                        inboundingLocation.ReleaseLock();
                    }
                }
                else
                {
                    _logger.LogInformation($"BominPinScheduleHandler - UnloadOnly,【{schedule.Code}】or【{inboundingLocation.Code}】location is locked.\r\n");
                }
            }

            //2.  wip track out to pin
            if (schedule.Appointed == false
                && schedule.InteractionSequence == InteractionSequence.LoadOnly
                && emptyBoxPartition != null
                && emptyBoxPartition.PartCode != null
                && schedule.LocationCode != null
                && _locationManager.TryGetLocation(schedule.LocationCode, out var inScheduleLocation)
                && inScheduleLocation != null
                && _wipManager.TryFindEmptySiloBoxLocation(emptyBoxPartition.PartCode, out var outboundingLocation)
                && outboundingLocation != null
                && schedule.EventRequest != null
                && outboundingLocation.Schedule != null)
            {
                if (outboundingLocation.TryLock())
                {
                    try
                    {
                        var tranferJob = new TransferJob()
                        {
                            IsUrgent = 0,
                            InteractionSequence = InteractionSequence.UnloadOnly,
                            ScheduledTaskStatus = ScheduledTaskStatus.Created,
                            TransportationKind = TransportationKind.EmptySilo,
                            PartitionCode = outboundingLocation.PartitionCode,
                            ForkCode = outboundingLocation.Code,
                            TransferDesc = $"空料仓:从空仓区【{outboundingLocation.Code}】转运到PIN【{schedule.LocationCode}】",
                            ClinkerCount = outboundingLocation.DrilledPanelsCount,
                            RawCount = outboundingLocation.UndrilledPanelsCount,
                            SiloCode = outboundingLocation.SiloCode,
                            RelateDeviceCode = schedule.LocationCode,
                            MasterDeviceKind = DeviceKind.Pin,
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
                            TransferBehavior = SiloTransferBehavior.EMPTY_BOX_FROM_WIP_TO_PIN
                        };

                        if (await _transferPlanManager.TryAddTransferJob(tranferJob)
                               && outboundingLocation.Schedule != null)
                        {
                            outboundingLocation.Schedule.Appointed = true;
                            outboundingLocation.Schedule.AppointedMessage = tranferJob.TransferDesc;
                            schedule.Appointed = true;
                            schedule.AppointedMessage = tranferJob.TransferDesc;

                            _logger.LogInformation($"BominPinScheduleHandler,工位:【{schedule.LocationCode}】,schedule:【{schedule.Id}】,交互方式:【{schedule.InteractionSequence}】,已生成料仓任务.\r\n");
                        }
                    }
                    finally
                    {
                        outboundingLocation.ReleaseLock();
                    }
                }
                else
                {
                    _logger.LogInformation($"BominPinScheduleHandler - LoadOnly,【{schedule.Code}】or【{outboundingLocation.Code}】location is locked.\r\n");
                }
            }
        }
    }
}
