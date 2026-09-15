using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Fundation.Iot;

public abstract class Vehicle : Device, IVehicle
{
    public AgvMoveActionStatus MoveActionStatus { get; set; }
    public bool AgvScanResult { get; set; }

    protected Vehicle(DeviceDescriptor deviceDescriptor,
        IDeviceEngine deviceEngine,
        IServiceProvider serviceProvider)
        : base(deviceDescriptor, deviceEngine, serviceProvider)
    {
    }

    protected override void ConfigureServiceCapabilities()
    {
        base.ConfigureServiceCapabilities();
        AddCommand(Topics.Services.AGV_MOVE_SERVICE_ID, Move);
        AddCommand(Topics.Services.AGV_CHARGE_SERVICE_ID, Charge);
    }

    protected virtual bool CanAcceptScheduleTask(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return true;
    }

    public override Task<DeviceServiceInvokeResponse> ScheduleTask(DeviceServiceInvokeRequest request)
    {
        ReverseCallerAndTarget(request);
        request.Params[ScheduleConstants.PARAMS_TASK_SENDED_TIME] = DateTime.Now;

        if (!CanAcceptScheduleTask(request))
        {
            return Task.FromResult(new DeviceServiceInvokeResponse { Code = ErrorCodes.Sys.FAIL });
        }

        if (SchedulingTasks.Count > 0)
        {
            if (SchedulingTasks.TryPeek(out var _, out var nextTaskOrder))
            {
                var agvAction = request.Params[ScheduleConstants.PARAMS_TASK_ACTION]?.ToString();
                if (agvAction == ScheduleConstants.PARAMS_TASK_ACTION_CHARGE)
                {
                    request.Params[ScheduleConstants.PARAMS_TASK_ORDER] = nextTaskOrder - 1;
                    SchedulingTasks.Enqueue(request, request.Params[ScheduleConstants.PARAMS_TASK_ORDER].ToInt());
                }
                else
                {
                    request.Params[ScheduleConstants.PARAMS_TASK_ORDER] = nextTaskOrder + 1;
                    SchedulingTasks.Enqueue(request, request.Params[ScheduleConstants.PARAMS_TASK_ORDER].ToInt());
                }
            }
        }
        else
        {
            var agvAction = request.Params[ScheduleConstants.PARAMS_TASK_ACTION]?.ToString();
            switch (agvAction)
            {
                case ScheduleConstants.PARAMS_TASK_ACTION_WORK:
                case ScheduleConstants.PARAMS_TASK_ACTION_CHARGE:
                    request.Params[ScheduleConstants.PARAMS_TASK_ORDER] = DEFAULT_MAX_TASKS_LIMIT / 2;
                    SchedulingTasks.Enqueue(request, request.Params[ScheduleConstants.PARAMS_TASK_ORDER].ToInt());
                    break;

                default:
                    break;
            }
        }

        return Task.FromResult(new DeviceServiceInvokeResponse { Code = ErrorCodes.Sys.SUCCESS });
    }

    /// <summary>
    /// 重置调度任务，使其处于可以被再次调度的状态
    /// </summary>
    /// <param name="request">当前request</param>
    /// <returns>是否重置成功</returns>
    public bool ResetScheduleTask(DeviceServiceInvokeRequest request)
    {
        if (SchedulingTasks.Count == 0) return false;
        if (SchedulingTasks.TryPeek(out var first, out var _))
        {
            if (first != null && first.Equals(request))
            {
                first.Params[ScheduleConstants.PARAMS_TASK_STATUS] = ScheduledTaskStatus.Allocated;
                return true;
            }
        }

        return false;
    }

    private void ReverseCallerAndTarget(DeviceServiceInvokeRequest request)
    {
        var callerProductId = request.ProductId;
        var callerDeviceId = request.DeviceId;
        var callerClientId = request.ClientId;
        var callerHostAddress = request.HostAddress;
        request.ProductId = this.ProductId;
        request.DeviceId = this.DeviceId;
        request.ClientId = this.ClientId;
        request.HostAddress = this.DeviceDescriptor.HostAddress;
        request.TargetProductId = callerProductId;
        request.TargetDeviceId = callerDeviceId;
        request.TargetClientId = callerClientId;
        request.TargetHostAddress = callerHostAddress;
    }

    public abstract Task<DeviceServiceInvokeResponse> Move(DeviceServiceInvokeRequest deviceServiceInvokeRequest);

    public abstract Task<DeviceServiceInvokeResponse> Charge(DeviceServiceInvokeRequest deviceServiceInvokeRequest);
}
