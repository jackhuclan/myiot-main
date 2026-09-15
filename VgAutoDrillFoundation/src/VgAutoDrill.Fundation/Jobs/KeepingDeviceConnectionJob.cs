using Microsoft.Extensions.Logging;
using Quartz;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Fundation.Jobs;

public class KeepingDeviceConnectionJob : IJob
{
    private readonly IDeviceProvider deviceProvider;
    private readonly ILogger<KeepingDeviceConnectionJob> logger;

    public KeepingDeviceConnectionJob(IDeviceProvider deviceProvider,
        ILoggerFactory loggerFactory)
    {
        this.deviceProvider = deviceProvider;
        logger = loggerFactory.CreateLogger<KeepingDeviceConnectionJob>();
    }

    public async Task Execute(IJobExecutionContext context)
    {
        string? deviceId = context.JobDetail.JobDataMap.GetString("DeviceId");
        if (deviceId == null)
        {
            ThrowHelper.ThrowArgumentNullException("DeviceId is null in KeepingPlcConnectionJob");
        }

        var device = deviceProvider.GetDevice(deviceId);
        if (device != null)
        {
            if (!device.Connector.IsConnected)
            {
                logger.LogInformation($"[{DateTime.Now.ToLongTimeString()}]: KeepingPlcConnectionJob is reconnecting...");
                await device.Connector.Connect();
            }

            await device.Connector.HeartBeatFunc.Invoke();
        }
    }
}
