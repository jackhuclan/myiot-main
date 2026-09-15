using Microsoft.Extensions.Logging;
using VegaIot.External.AgvEntity.STD;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VegaIot.External.StdAgv.Handler;

public class MointorTaskCancelHandler
{
    private readonly ILogger<MointorTaskCancelHandler> _logger;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ITransferPlanManager _transferPlanManager;

    public MointorTaskCancelHandler(ILogger<MointorTaskCancelHandler> logger,
        IScheduleTaskManager scheduleTaskManager,
        ITransferPlanManager transferPlanManager)
    {
        _logger = logger;
        _scheduleTaskManager = scheduleTaskManager;
        _transferPlanManager = transferPlanManager;
    }

    public async Task<StdArrivedResponseEntityV2> Handle(TransferJob agvTask,
        Location startLocation,
        Location endLocation,
        DeviceProxy startDevice,
        DeviceProxy endDevice,
        QueryTaskStatusData hikAgvData)
    {
        var response = new StdArrivedResponseEntityV2();

        if (agvTask.StartScheduleId.HasValue
        && _scheduleTaskManager.TryGetScheduleTaskById(agvTask.StartScheduleId.Value, out var startSchedule)
        && startSchedule != null
        && startSchedule.Code != null)
        {
            _logger.LogInformation($"StdAgvTaskMonitor:Excute MointorTaskCancelHandler...start...first schedule...agvTask:{agvTask.Id},startSchedule:{agvTask.StartScheduleId}");

            await _scheduleTaskManager.CancelSingleSchedule(startSchedule.Code, true, "the task has timed out, and it has been automatically cancelled.");

            _logger.LogInformation($"StdAgvTaskMonitor:Excute MointorTaskCancelHandler...end...first schedule...agvTask:{agvTask.Id},startSchedule:{agvTask.StartScheduleId}");
        }

        if (agvTask.EndScheduleId.HasValue
            && _scheduleTaskManager.TryGetScheduleTaskById(agvTask.EndScheduleId.Value, out var endSchedule)
            && endSchedule != null
            && endSchedule.Code != null)
        {
            _logger.LogInformation($"StdAgvTaskMonitor:Excute MointorTaskCancelHandler...start...second schedule...agvTask:{agvTask.Id},endSchedule:{agvTask.EndScheduleId}");

            await _scheduleTaskManager.CancelSingleSchedule(endSchedule.Code, false, "the task has timed out, and it has been automatically cancelled.");

            _logger.LogInformation($"StdAgvTaskMonitor:Excute MointorTaskCancelHandler...end...second schedule...agvTask:{agvTask.Id},endSchedule:{agvTask.EndScheduleId}");
        }

        agvTask.ScheduledTaskStatus = ScheduledTaskStatus.Canceled;
        agvTask.AllocatedAgv = hikAgvData.agvCode ?? "";
        await _transferPlanManager.TryUpdateTransferJob(agvTask);

        return response;
    }
}
