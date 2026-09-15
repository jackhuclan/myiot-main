// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using Microsoft.Extensions.Logging;
using Quartz;

namespace VgDeviceGateway.Devices.Drill.Other.Jobs
{
    public class StatisticsJobSchedule
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly ILogger<ClearJobSchedule> _logger;

        public StatisticsJobSchedule(ISchedulerFactory schedulerFactory, ILogger<ClearJobSchedule> logger)
        {
            _schedulerFactory = schedulerFactory;
            _logger = logger;
        }

        public async Task StartStatisticsJob(DefaultDrill device, CancellationToken cancellationToken)
        {
            this._logger.LogInformation($"{this.GetType().Name}  StatisticsJob...");
            DateTimeOffset parsedDateTimeOffsetWithFormat = DateTimeOffset.ParseExact($"2024-11-30 00:00:59", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
            await ScheduleJobAsync<StatisticsJob>(device, parsedDateTimeOffsetWithFormat,  $"{device.DeviceId}_Group", cancellationToken);
        }

        protected async Task ScheduleJobAsync<T>(DefaultDrill device, DateTimeOffset startTime,  string groupName, CancellationToken cancellationToken)
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
                //.StartAt(startTime)
                .StartNow()
                .WithSimpleSchedule(x => x
                .WithInterval(TimeSpan.FromSeconds(20))
                .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger, cancellationToken);
        }
    }
}
