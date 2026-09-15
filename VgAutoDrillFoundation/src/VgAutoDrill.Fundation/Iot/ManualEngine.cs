using Microsoft.Extensions.Logging;
using Quartz;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Jobs;

namespace VgAutoDrill.Fundation.Iot;

public class ManualEngine : IDeviceEngine
{
    public IDeviceConnector DeviceConnector { get; private set; }
    private readonly ILogger<ManualEngine> _logger;
    private readonly ISchedulerFactory _schedulerFactory;

    public ManualEngine(IDeviceConnectorProvider deviceConnectorProvider,
        ISchedulerFactory schedulerFactory,
        ILoggerFactory loggerFactory,
        DeviceDescriptor deviceDescriptor)
    {
        this.DeviceConnector = deviceConnectorProvider.GetDeviceConnector(deviceDescriptor);
        _schedulerFactory = schedulerFactory;
        _logger = loggerFactory.CreateLogger<ManualEngine>();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        this.DeviceConnector.Dispose();
    }

    public virtual async Task Fire(Device device, CancellationToken cancellationToken)
    {
        _logger.LogInformation("ManualEngine fired");
        await ConnectDevice(device, cancellationToken);
        await StartDataCollectingJob(device, cancellationToken);
    }

    public Task Shutdown(CancellationToken cancellationToken)
    {
        Dispose();
        return Task.CompletedTask;
    }

    protected virtual async Task ConnectDevice(Device device, CancellationToken cancellationToken)
    {
        this._logger.LogInformation($"{this.GetType().Name} ConnectPLC...");
        await DeviceConnector.Connect(cancellationToken);
        await StartKeepingDeviceConnectionJob(device, cancellationToken);
    }

    protected async Task StartDataCollectingJob(Device device, CancellationToken cancellationToken)
    {
        this._logger.LogInformation($"{this.GetType().Name}  StartDataCollectingJob...");
        await ScheduleJobAsync<DataCollectingJob>(device, TimeSpan.FromSeconds(device.DeviceDescriptor.DataCollectingPerSeconds), $"{device.DeviceId}_Group", cancellationToken);
    }

    protected async Task StartKeepingDeviceConnectionJob(Device device, CancellationToken cancellationToken)
    {
        this._logger.LogInformation($"{this.GetType().Name} StartKeepingDeviceConnectionJob...");

        await ScheduleJobAsync<KeepingDeviceConnectionJob>(device, TimeSpan.FromMilliseconds(device.DeviceDescriptor.KeepingPlcConnectionPerMilliSeconds), $"{device.DeviceId}_Group", cancellationToken);
    }

    /// <summary>
    /// Quarz任务调度
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="device"></param>
    /// <param name="frequency"></param>
    /// <param name="groupName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected async Task ScheduleJobAsync<T>(Device device, TimeSpan frequency, string groupName, CancellationToken cancellationToken)
        where T : IJob
    {
        var scheduler = await this._schedulerFactory.GetScheduler();
        var jobName = $"{device.DeviceId}_{typeof(T).Name}_Job";
        var triggerName = $"{device.DeviceId}_{typeof(T).Name}_Trigger";
        var job = JobBuilder.Create<T>()
                            .WithIdentity(jobName, groupName)
                            .UsingJobData("DeviceId", device.DeviceId)
                            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity(triggerName, groupName)
            .StartNow()
            .WithSimpleSchedule(x => x
            .WithInterval(frequency)
            .RepeatForever())
            .Build();

        await scheduler.ScheduleJob(job, trigger, cancellationToken);
    }
}
