using Microsoft.Extensions.Logging;
using Quartz;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Fundation.Jobs;

public class DataCollectingJob : IJob
{
    private readonly IDeviceProvider deviceProvider;
    private readonly ILogger<DataCollectingJob> logger;

    public DataCollectingJob(IDeviceProvider deviceProvider,
        ILoggerFactory loggerFactory)
    {
        this.deviceProvider = deviceProvider;
        logger = loggerFactory.CreateLogger<DataCollectingJob>();
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await Task.Run(() =>
        {
            string? deviceId = context.JobDetail.JobDataMap.GetString("DeviceId");
            if (deviceId == null)
            {
                ThrowHelper.ThrowArgumentNullException("DeviceId is null in DataCollectingJob");
            }

            logger.LogInformation($"[{DateTime.Now.ToLongTimeString()}]: DataCollectingJob is executing at {deviceId}...");
            var device = deviceProvider.GetDevice(deviceId);
            device.CollectDataFunc?.Invoke(device);
        });
    }
}

