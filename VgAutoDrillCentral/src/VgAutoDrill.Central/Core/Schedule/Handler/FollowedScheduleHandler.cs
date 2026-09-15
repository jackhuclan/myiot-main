using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Schedule.Deliver;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Central.Core.Schedule.Handler;

/// <summary>
/// 承接上下料仓的下半个任务
/// </summary>
internal class FollowedScheduleHandler : BaseScheduleTaskHandler
{
    private readonly ILogger<FollowedScheduleHandler> _logger;
    private readonly IDeviceManager _deviceManager;
    private readonly IScheduleTaskAdapter _scheduleTaskAdapter;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ISysConfigManager _sysConfigManager;

    public FollowedScheduleHandler(IServiceProvider serviceProvider)
    : base(serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<FollowedScheduleHandler>>();
        _deviceManager = serviceProvider.GetRequiredService<IDeviceManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _scheduleTaskAdapter = serviceProvider.GetRequiredService<IScheduleTaskAdapter>();
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
    }

    public override async Task Handle()
    {
        var schedulesIsAuxiliary = await _scheduleTaskAdapter.GetNotStartedFollowedSchedule();

        foreach (var schedule in schedulesIsAuxiliary)
        {
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN, Admin.Model.Enum.SysConfigCategoryEnum.None, false))
            {
                _logger.LogWarning($"系统即将维护，暂停分配新任务，请稍候.");
                return;
            }
            await Task.Delay(30);

            var master = await _scheduleTaskAdapter.FindMasterSchedule(schedule);

            if (master == null
                || master.ScheduledTaskStatus == ScheduledTaskStatus.Canceled
                || master.ScheduledTaskStatus == ScheduledTaskStatus.Failed)
            {
                _ = await _scheduleTaskManager.CancelSingleSchedule(schedule.Code, false, "master schedule is canceled or failed, or not exists.");
                continue;
            }

            if (master.ScheduledTaskStatus == ScheduledTaskStatus.Completed)
            {
                schedule.ScheduledTaskStatus = ScheduledTaskStatus.Allocated;
                var allocateResult = await DoSecondlyTask(schedule);
                if (allocateResult.Code == AgvAllocationResultCode.Failed && allocateResult.Reason == AgvAllocationFailedReason.CentralSystemMaintain)
                {
                    break;
                }
            }
        }
    }

    private async Task<AgvAllocationResult> DoSecondlyTask(ScheduleTaskWithRequest schedule)
    {
        _logger.LogDebug($"DoSecondlyTask 正在处理调度请求：{schedule.Id}, {schedule.CallerDeviceId}");

        if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN, Admin.Model.Enum.SysConfigCategoryEnum.None, false))
        {
            _logger.LogWarning($"系统即将维护，暂停分配新任务，请稍候.");
            return new AgvAllocationResult(AgvAllocationResultCode.Failed, AgvAllocationFailedReason.CentralSystemMaintain);
        }

        var callerDevice = _deviceManager.GetOnlineDevice(schedule.CallerDeviceId);
        if (callerDevice == null)
        {
            _logger.LogWarning($"CallerOffline,{schedule.CallerDeviceId}.");
            return new AgvAllocationResult(AgvAllocationResultCode.Failed, AgvAllocationFailedReason.CallerOffline);
        }

        var allocatedAgv = _deviceManager.GetOnlineDevice(schedule.AllocatedAgv) as Agv;

        if (allocatedAgv == null)
        {
            _logger.LogError($"DoSecondlyTask,scheduleId:{schedule.Id}, schedule.CallerDeviceId:{schedule.CallerDeviceId},{schedule.AllocatedAgv} AGV当前不在线");
            return new AgvAllocationResult(AgvAllocationResultCode.Failed, AgvAllocationFailedReason.NoAvailableAGV);
        }

        if (allocatedAgv.Status != DeviceStatus.Ready)
        {
            _logger.LogWarning($"DoSecondlyTask,scheduleId:{schedule.Id}, schedule.CallerDeviceId:{schedule.CallerDeviceId},{schedule.AllocatedAgv} is not ready now.");
            return new AgvAllocationResult(AgvAllocationResultCode.Failed, AgvAllocationFailedReason.NoAvailableAGV);
        }

        return await DeliverScheduleTask(new DeliverScheduleTaskRequirement
        {
            CallerDevice = callerDevice,
            AgvDevice = allocatedAgv,
            ScheduleTask = schedule
        });
    }
}
