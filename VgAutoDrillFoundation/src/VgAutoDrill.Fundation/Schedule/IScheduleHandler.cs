using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Alarm;

public interface IScheduleHandler : IDeviceShare
{
    /// <summary>
    /// 中控通知设备调度任务取消
    /// </summary>
    /// <param name="deviceServiceInvokeRequest"></param>
    /// <returns></returns>
    public abstract Task<DeviceServiceInvokeResponse> CancelSchedule(DeviceServiceInvokeRequest deviceServiceInvokeRequest);
    /// <summary>
    /// 中控通知设备调度任务完成
    /// </summary>
    /// <param name="deviceServiceInvokeRequest"></param>
    /// <returns></returns>
    public abstract Task<DeviceServiceInvokeResponse> CompleteSchedule(DeviceServiceInvokeRequest deviceServiceInvokeRequest);

    /// <summary>
    /// 中控下发给设备一个新的调度任务
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    public abstract Task<DeviceServiceInvokeResponse> ScheduleTask(DeviceServiceInvokeRequest deviceServiceInvokeRequest);
}
