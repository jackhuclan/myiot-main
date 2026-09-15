using VgAutoDrill.Fundation.Alarm;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Schedule;

public abstract class AbstractScheduleHandler<TDevice> : DeviceShare<TDevice>, IScheduleHandler
    where TDevice : Device
{
    public AbstractScheduleHandler(IServiceProvider serviceProvider, TDevice device) : base(serviceProvider, device) { }

    /// <inheritdoc/>
    public virtual async Task<DeviceServiceInvokeResponse> CancelSchedule(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await ResponseSuccess();
    }

    /// <inheritdoc/>
    public virtual async Task<DeviceServiceInvokeResponse> CompleteSchedule(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await ResponseSuccess();
    }

    /// <inheritdoc/>
    public virtual async Task<DeviceServiceInvokeResponse> ScheduleTask(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await ResponseSuccess();
    }

}
