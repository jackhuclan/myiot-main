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

namespace VgAutoDrill.Central.Core.Schedule.Handler;

/// <summary>
/// Unpin的调度申请处理
/// </summary>
internal class UnpinScheduleHandler : IUnpinScheduleHandler
{
    private readonly ILogger<UnpinScheduleHandler> _logger;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;

    public UnpinScheduleHandler(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<UnpinScheduleHandler>>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _panelSiloForkManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();
    }

    public async Task Handle()
    {
        if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN, Admin.Model.Enum.SysConfigCategoryEnum.None, false))
        {
            _logger.LogWarning($"系统即将维护，暂停分配新任务，请稍候.");
            return;
        }
        int minEmptyPayloadFork = await _sysConfigManager.GetIntValue("CountOfEmptyForkAtLeast", Admin.Model.Enum.SysConfigCategoryEnum.None, false);//最少空位数

        var queryUnpinSchedules = _scheduleTaskManager.NotStartedUnpinSchedules;

        //排序调度记录
        var orderedUnpinSchedules = queryUnpinSchedules.OrderBy(s => s.Id);

        foreach (var schedule in orderedUnpinSchedules)
        {
            //1. unpin track out
            if (schedule.Appointed == false
            && schedule.InteractionSequence == InteractionSequence.UnloadOnly
            && _panelSiloForkManager.TryFindEmptyPayloadLocation(minEmptyPayloadFork, out var inboundingLocation)
            && inboundingLocation != null
            && schedule.EventRequest != null
            && inboundingLocation.Schedule != null)
            {
                var tranferJob = new TransferJob()
                {
                    IsUrgent = 0,
                    InteractionSequence = InteractionSequence.LoadOnly,
                    PartitionCode = inboundingLocation.PartitionCode,
                    TransportationKind = TransportationKind.EmptySilo,
                    ForkCode = inboundingLocation.Code,
                    SiloCode = inboundingLocation.SiloCode,
                    RelateDeviceCode = schedule.LocationCode,
                    MasterDeviceKind = DeviceKind.UnPin,
                    AgvKind = DeviceKind.ShelfSiloAgv,
                    ScheduledTaskStatus = ScheduledTaskStatus.Created,
                    ForkLocationScheduleId = inboundingLocation.Schedule.Code,
                    DeviceLocationScheduleId = schedule.Code,
                    MasterScheduleId = schedule.Id,
                    TransferBehavior = SiloTransferBehavior.EMPTY_BOX_FROM_UNPIN_TO_FORK,
                    TransferDesc = $"EMPTY_BOX_FROM_UNPIN_TO_FORK from {schedule.LocationCode} to {inboundingLocation.Code}",
                    StartLocationCode = schedule.LocationCode,
                    StartDeviceId = schedule.EventRequest.DeviceId,
                    StartScheduleId = schedule.Id,
                    StartSchedule = schedule.Code,
                    EndLocationCode = inboundingLocation.Code,
                    EndDeviceId = inboundingLocation.DeviceId,
                    EndScheduleId = inboundingLocation.ScheduleId,
                    EndSchedule = inboundingLocation.ScheduleCode,
                };

                if (await _transferPlanManager.TryAddTransferJob(tranferJob)
                       && inboundingLocation.Schedule != null)
                {
                    inboundingLocation.Schedule.Appointed = true;
                    inboundingLocation.Schedule.AppointedMessage = tranferJob.TransferDesc;
                    schedule.Appointed = true;
                    schedule.AppointedMessage = tranferJob.TransferDesc;
                }
            }

            //2. unpin track in
            if (schedule.Appointed == false
                && schedule.InteractionSequence == InteractionSequence.LoadOnly
                && _panelSiloForkManager.TryFindFullDrilledLocation(new ConfigParameters { }, out var outboundingLocation)
                && outboundingLocation != null
                && outboundingLocation.Schedule != null
                && schedule.EventRequest != null)
            {
                var tranferJob = new TransferJob()
                {
                    IsUrgent = 0,
                    InteractionSequence = InteractionSequence.UnloadOnly,
                    PartitionCode = outboundingLocation.PartitionCode,
                    TransportationKind = TransportationKind.Clinker,
                    ForkCode = outboundingLocation.Code,
                    SiloCode = outboundingLocation.SiloCode,
                    ClinkerCount = schedule.EventRequest.PayloadPanels.DrilledPanelCount(),
                    RelateDeviceCode = schedule.LocationCode,
                    MasterDeviceKind = DeviceKind.UnPin,
                    AgvKind = DeviceKind.ShelfSiloAgv,
                    ScheduledTaskStatus = ScheduledTaskStatus.Created,
                    ForkLocationScheduleId = outboundingLocation.Schedule.Code,
                    DeviceLocationScheduleId = schedule.Code,
                    MasterScheduleId = schedule.Id,
                    TransferBehavior = SiloTransferBehavior.DRILLED_FROM_FORK_TO_UNPIN,
                    TransferDesc = $"DRILLED_FROM_FORK_TO_UNPIN from {outboundingLocation.Code} to {schedule.LocationCode}",
                    StartLocationCode = outboundingLocation.Code,
                    StartDeviceId = outboundingLocation.DeviceId,
                    StartScheduleId = outboundingLocation.Schedule.Id,
                    StartSchedule = outboundingLocation.ScheduleCode,
                    EndLocationCode = schedule.LocationCode,
                    EndDeviceId = schedule.EventRequest.DeviceId,
                    EndScheduleId = schedule.Id,
                    EndSchedule = schedule.Code,
                };

                if (await _transferPlanManager.TryAddTransferJob(tranferJob)
                       && outboundingLocation.Schedule != null)
                {
                    outboundingLocation.Schedule.Appointed = true;
                    outboundingLocation.Schedule.AppointedMessage = tranferJob.TransferDesc;
                    schedule.Appointed = true;
                    schedule.AppointedMessage = tranferJob.TransferDesc;
                }
            }
        }
    }
}
