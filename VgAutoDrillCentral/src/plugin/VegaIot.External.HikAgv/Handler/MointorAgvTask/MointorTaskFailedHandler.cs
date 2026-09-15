using Microsoft.Extensions.Logging;
using VegaIot.External.AgvEntity.Hik;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VegaIot.External.HikAgv.Handler;

public class MointorTaskFailedHandler
{
    private readonly ILogger<MointorTaskFailedHandler> _logger;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ITransferPlanManager _transferPlanManager;

    public MointorTaskFailedHandler(ILogger<MointorTaskFailedHandler> logger,
        IScheduleTaskManager scheduleTaskManager,
        ITransferPlanManager transferPlanManager)
    {
        _logger = logger;
        _scheduleTaskManager = scheduleTaskManager;
        _transferPlanManager = transferPlanManager;
    }

    public async Task<HikArrivedResponseEntity> Handle(TransferJob agvTask,
        Location startLocation,
        Location endLocation,
        DeviceProxy startDevice,
        DeviceProxy endDevice,
        QueryTaskStatusData hikAgvData)
    {
        var response = new HikArrivedResponseEntity();

        if (agvTask.StartScheduleId.HasValue
        && _scheduleTaskManager.TryGetScheduleTaskById(agvTask.StartScheduleId.Value, out var startSchedule)
        && startSchedule != null
        && agvTask.StartDeviceId != null
        && startSchedule.Code != null)
        {
            _logger.LogInformation($"HikAgvTaskMonitor:Excute MointorTaskFailedHandler...start...first schedule...agvTask:{agvTask.Id},startSchedule:{agvTask.StartScheduleId}");

            startSchedule.AllocatedAgv = hikAgvData.agvCode ?? "";
            await _scheduleTaskManager.SetScheduleAsException(new FailScheduleTaskRequest
            {
                DeviceId = agvTask.StartDeviceId,
                TraceId = startSchedule.Code,
                Params = { { "AllocatedAgv", startSchedule.AllocatedAgv } }
            });

            _logger.LogInformation($"HikAgvTaskMonitor:Excute MointorTaskFailedHandler...end...first schedule...agvTask:{agvTask.Id},startSchedule:{agvTask.StartScheduleId}");
        }

        if (agvTask.EndScheduleId.HasValue
            && _scheduleTaskManager.TryGetScheduleTaskById(agvTask.EndScheduleId.Value, out var endSchedule)
            && endSchedule != null
            && agvTask.EndDeviceId != null
            && endSchedule.Code != null)
        {
            _logger.LogInformation($"HikAgvTaskMonitor:Excute MointorTaskFailedHandler...start...second schedule...agvTask:{agvTask.Id},endSchedule:{agvTask.EndScheduleId}");

            endSchedule.AllocatedAgv = hikAgvData.agvCode ?? "";
            await _scheduleTaskManager.SetScheduleAsException(new FailScheduleTaskRequest
            {
                DeviceId = agvTask.EndDeviceId,
                TraceId = endSchedule.Code,
                Params = { { "AllocatedAgv", endSchedule.AllocatedAgv } }
            });

            _logger.LogInformation($"HikAgvTaskMonitor:Excute MointorTaskFailedHandler...end...second schedule...agvTask:{agvTask.Id},endSchedule:{agvTask.EndScheduleId}");
        }

        agvTask.ScheduledTaskStatus = ScheduledTaskStatus.Failed;
        agvTask.AllocatedAgv = hikAgvData.agvCode ?? "";
        await _transferPlanManager.TryUpdateTransferJob(agvTask);

        return response;
    }
}
