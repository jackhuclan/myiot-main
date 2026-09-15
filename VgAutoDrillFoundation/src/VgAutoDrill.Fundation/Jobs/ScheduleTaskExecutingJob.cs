using System.Text.Json;
using Microsoft.Extensions.Logging;
using Quartz;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Fundation.Jobs;

public class ScheduleTaskExecutingJob : IJob
{
    private readonly IDeviceProvider deviceProvider;
    private readonly IMqttClientWrapper mqttClientWrapper;
    private readonly ILogger<ScheduleTaskExecutingJob> logger;

    public ScheduleTaskExecutingJob(IDeviceProvider deviceProvider,
        IMqttClientWrapper mqttClientWrapper,
        ILoggerFactory loggerFactory)
    {
        this.deviceProvider = deviceProvider;
        this.mqttClientWrapper = mqttClientWrapper;
        logger = loggerFactory.CreateLogger<ScheduleTaskExecutingJob>();
    }

    public async Task Execute(IJobExecutionContext context)
    {
        DeviceServiceInvokeRequest? deviceServiceInvokeRequest = null;

        if (!mqttClientWrapper.IsConnected) return;

        try
        {
            string? deviceId = context.JobDetail.JobDataMap.GetString("DeviceId");
            ThrowHelper.ThrowArgumentNullException(deviceId, "DeviceId should not be null in ScheduleTaskExecutingJob");

            Device device = deviceProvider.GetDevice(deviceId);
            if (device != null && device.SchedulingTasks.Count > 0 && device.Status == DeviceStatus.Ready)
            {
                logger.LogInformation($"[{DateTime.Now.ToLongTimeString()}]: ScheduleTaskExecutingJob is executing at {deviceId},tasks count : {device.SchedulingTasks.Count}...");

                if (device.SchedulingTasks.TryPeek(out deviceServiceInvokeRequest, out int _))
                {
                    if (!await device.CheckStatus(deviceServiceInvokeRequest))
                    {
                        logger.LogInformation($"目标设备不可用，TargetDeviceId:{deviceServiceInvokeRequest.TargetDeviceId}");
                        return;
                    }

                    if (deviceServiceInvokeRequest.Params.ContainsKey(ScheduleConstants.PARAMS_TASK_SENDED_TIME))
                    {
                        var allocateTimeStr = deviceServiceInvokeRequest.Params[ScheduleConstants.PARAMS_TASK_SENDED_TIME].ToStr();
                        DateTime allocateTime;
                        if (DateTime.TryParse(allocateTimeStr, out allocateTime))
                        {
                            if (DateTime.Now.Subtract(allocateTime).TotalSeconds < device.DeviceDescriptor.ScheduleTaskDelaySeconds)
                            {
                                logger.LogInformation($"DeviceId:{device.DeviceId},任务延时执行。分配时间：{deviceServiceInvokeRequest.Params[ScheduleConstants.PARAMS_TASK_SENDED_TIME].ToStr()}");
                                return;
                            }
                        }
                        else
                        {
                            logger.LogWarning($"DeviceId:{device.DeviceId},任务延时执行。分配时间：{deviceServiceInvokeRequest.Params[ScheduleConstants.PARAMS_TASK_SENDED_TIME].ToStr()}");
                        }
                    }

                    object taskStatus = deviceServiceInvokeRequest.Params[ScheduleConstants.PARAMS_TASK_STATUS];
                    object taskAction = deviceServiceInvokeRequest.Params[ScheduleConstants.PARAMS_TASK_ACTION];
                    if (taskStatus == null
                        || taskAction == null
                        || string.IsNullOrEmpty(taskStatus.ToString())
                        || string.IsNullOrEmpty(taskAction.ToString())
                        || Enum.TryParse(taskStatus.ToString(), true, out ScheduledTaskStatus val) && val == ScheduledTaskStatus.Running)
                        return;

                    deviceServiceInvokeRequest.Params[ScheduleConstants.PARAMS_TASK_STATUS] = ScheduledTaskStatus.Running;

                    var response = new DeviceServiceInvokeResponse();
                    logger.LogInformation($"[ScheduleTaskExecutingJob]:" + JsonSerializer.Serialize(deviceServiceInvokeRequest));
                    switch (taskAction.ToString()?.ToUpper())
                    {
                        case ScheduleConstants.PARAMS_TASK_ACTION_WORK:
                            response = await device.Work(deviceServiceInvokeRequest);
                            break;

                        case ScheduleConstants.PARAMS_TASK_ACTION_CHARGE:
                            if (device is IVehicle vechicle)
                            {
                                device.Status = DeviceStatus.Charging;
                                response = await vechicle.Charge(deviceServiceInvokeRequest);
                            }
                            break;

                        default:
                            break;
                    }

                    if (response != null && response.Code == ErrorCodes.Sys.SUCCESS)
                    {
                        deviceServiceInvokeRequest.Params[ScheduleConstants.PARAMS_TASK_STATUS] = ScheduledTaskStatus.Completed;
                        device.SchedulingTasks.Dequeue();
                    }
                    else
                    {
                        deviceServiceInvokeRequest.Params[ScheduleConstants.PARAMS_TASK_STATUS] = ScheduledTaskStatus.Failed;
                        logger.LogInformation($"[ScheduleTaskExecutingJob] Failed:" + JsonSerializer.Serialize(deviceServiceInvokeRequest));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (deviceServiceInvokeRequest != null)
            {
                deviceServiceInvokeRequest.Params[ScheduleConstants.PARAMS_TASK_STATUS] = ScheduledTaskStatus.Failed;
                logger.LogInformation($"[ScheduleTaskExecutingJob] Failed:" + JsonSerializer.Serialize(deviceServiceInvokeRequest));
            }

            logger.LogError(ex, ex.Message);
        }
        finally
        {
        }
    }
}
