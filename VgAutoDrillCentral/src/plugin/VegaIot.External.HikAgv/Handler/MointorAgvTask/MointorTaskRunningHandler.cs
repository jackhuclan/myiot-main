using Microsoft.Extensions.Logging;
using VegaIot.External.AgvEntity.Hik;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VegaIot.External.HikAgv.Handler;

public class MointorTaskRunningHandler
{
    private readonly ILogger<MointorTaskRunningHandler> _logger;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ITransferPlanManager _transferPlanManager;

    public MointorTaskRunningHandler(ILogger<MointorTaskRunningHandler> logger,
        IScheduleTaskManager scheduleTaskManager,
        ITransferPlanManager transferPlanManager)
    {
        _logger = logger;
        _scheduleTaskManager = scheduleTaskManager;
        _transferPlanManager = transferPlanManager;
    }

    public async Task<HikArrivedResponseEntity> Handle(TransferJob agvTask,
        QueryTaskStatusData hikAgvData)
    {
        var response = new HikArrivedResponseEntity();

        if (agvTask.StartScheduleId.HasValue
            && _scheduleTaskManager.TryGetScheduleTaskById(agvTask.StartScheduleId.Value, out var startSchedule)
            && startSchedule != null
            && agvTask.StartDeviceId != null
            && startSchedule.Code != null)
        {
            _logger.LogInformation($"HikAgvTaskMonitor:Excute MointorTaskRunningHandler...start...first schedule...agvTask:{agvTask.Id},startSchedule:{agvTask.StartScheduleId}");

            //更新调度记录界面--调度设备
            startSchedule.AllocatedAgv = hikAgvData.agvCode ?? "";
            await _scheduleTaskManager.UpdateAllocatedAgv(startSchedule);

            _logger.LogInformation($"HikAgvTaskMonitor:Excute MointorTaskRunningHandler...end...first schedule...agvTask:{agvTask.Id},startSchedule:{agvTask.StartScheduleId}");
        }

        if (agvTask.EndScheduleId.HasValue
            && _scheduleTaskManager.TryGetScheduleTaskById(agvTask.EndScheduleId.Value, out var endSchedule)
            && endSchedule != null
            && agvTask.EndDeviceId != null
            && endSchedule.Code != null)
        {
            _logger.LogInformation($"HikAgvTaskMonitor:Excute MointorTaskRunningHandler...start...second schedule ...agvTask:{agvTask.Id},endSchedule:{agvTask.EndScheduleId}");

            //更新调度记录界面--调度设备
            endSchedule.AllocatedAgv = hikAgvData.agvCode ?? "";
            await _scheduleTaskManager.UpdateAllocatedAgv(endSchedule);

            _logger.LogInformation($"HikAgvTaskMonitor:Excute MointorTaskRunningHandler...end...second schedule ...agvTask:{agvTask.Id},endSchedule:{agvTask.EndScheduleId}");
        }

        agvTask.ScheduledTaskStatus = ScheduledTaskStatus.Running;
        agvTask.AllocatedAgv = hikAgvData.agvCode ?? "";
        await _transferPlanManager.TryUpdateTransferJob(agvTask);

        return response;
    }
}
