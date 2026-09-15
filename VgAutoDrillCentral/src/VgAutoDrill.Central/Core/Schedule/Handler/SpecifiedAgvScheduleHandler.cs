using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Schedule.Deliver;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Schedule.Handler;

/// <summary>
/// 强叫（主叫） 指定一个Agv来执行上下料仓行为。
/// </summary>
internal class SpecifiedAgvScheduleHandler : BaseScheduleTaskHandler
{
    private readonly ILogger<SpecifiedAgvScheduleHandler> _logger;
    private readonly IDeviceManager _deviceHolder;
    private readonly IScheduleTaskAdapter _scheduleTaskAdapter;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly IScheduleTaskDeliverPolicyFactory _scheduleTaskDeliverPolicyFactory;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly IScheduleService _scheduleService;
    private readonly ILoggerFactory _loggerFactory;

    public SpecifiedAgvScheduleHandler(IServiceProvider serviceProvider)
    : base(serviceProvider)
    {
        _scheduleService = serviceProvider.GetRequiredService<IScheduleService>();
        _loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = _loggerFactory.CreateLogger<SpecifiedAgvScheduleHandler>();
        _deviceHolder = serviceProvider.GetRequiredService<IDeviceManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _scheduleTaskAdapter = serviceProvider.GetRequiredService<IScheduleTaskAdapter>();
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _scheduleTaskDeliverPolicyFactory = serviceProvider.GetRequiredService<IScheduleTaskDeliverPolicyFactory>();
    }

    public override async Task Handle()
    {
        if (!CentralFlags.SystemPreloadCompleted)
        {
            return;
        }

        if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN, Admin.Model.Enum.SysConfigCategoryEnum.None, false))
        {
            _logger.LogWarning($"系统即将维护，暂停分配新任务，请稍候.");
            return;
        }

        var onlineDevices = _deviceHolder.Agvs;

        var allForceCall = await _scheduleTaskAdapter.GetNotStartedSpecifiedAgvSchedule();

        foreach (var scheduleTask in allForceCall)
        {
            if (string.IsNullOrEmpty(scheduleTask.AllocatedAgv) || string.IsNullOrEmpty(scheduleTask.CallerDeviceId))
            {
                _logger.LogWarning($"Invalid schedule RequireDeviceId or SourceDeviceId:{scheduleTask.Code}, there is no RequireDeviceId assigned for master auxiliary request");
                await _scheduleTaskManager.CancelSingleSchedule(scheduleTask.Code, true, "主叫AGV取消");
                continue;
            }

            var agv = onlineDevices.FirstOrDefault(d => d.DeviceId.ToLower() == scheduleTask.AllocatedAgv.ToLower());
            if (agv == null)
            {
                _logger.LogWarning($"Agv {scheduleTask.AllocatedAgv} is offline.");
                continue;
            }

            if (agv.Status != DeviceStatus.Ready)
            {
                _logger.LogWarning($"Agv {scheduleTask.AllocatedAgv} is not ready now.");
                continue;
            }

            if (scheduleTask.InteractionSequence == InteractionSequence.LoadOnly
                        && agv.PayloadPanels.IsEmptyPayload
                ||
                scheduleTask.InteractionSequence == InteractionSequence.UnloadOnly
                    && agv.PayloadPanels.Any(p => p.ProductStatus != ProductStatus.EmptyPayload))
            {
                _logger.LogWarning($"Invalid schedule :{scheduleTask.Code}, agv do not match the request now.");
                continue;
            }

            await Task.Delay(30);
            await HandleMasterSiloWithAssignedAgv(scheduleTask, agv);
        }
    }

    private async Task<AgvAllocationResult> HandleMasterSiloWithAssignedAgv(ScheduleTaskWithRequest scheduleTask, Agv agv)
    {
        //系统维护，不影响主叫功能的执行

        var callerDevice = _deviceHolder.GetOnlineDevice(scheduleTask.CallerDeviceId);
        if (callerDevice == null)
        {
            _logger.LogWarning($"HandleMasterSiloWithAssignedAgv, CallerOffline,{scheduleTask.CallerDeviceId}.");
            return new AgvAllocationResult(AgvAllocationResultCode.Failed, AgvAllocationFailedReason.CallerOffline);
        }

        return await DeliverScheduleTask(new DeliverScheduleTaskRequirement
        {
            CallerDevice = callerDevice,
            AgvDevice = agv,
            ScheduleTask = scheduleTask
        });
    }
}
