// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using Microsoft.Extensions.Logging;
using Quartz;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Drill.Other.Jobs
{
    public class ClearJobSchedule
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly ILogger<ClearJobSchedule> _logger;

        public ClearJobSchedule(ISchedulerFactory schedulerFactory, ILogger<ClearJobSchedule> logger)
        {
            _schedulerFactory = schedulerFactory;
            _logger = logger;
        }

        public async Task StartClearJob(DefaultDrill device, CancellationToken cancellationToken)
        {
            this._logger.LogInformation($"{this.GetType().Name}  StartClearJob...");
            DateTimeOffset parsedDateTimeOffsetWithFormat = DateTimeOffset.ParseExact($"2024-10-09 {device.DeviceDescriptor.Extra["StartHour"].ToString().PadLeft(2, '0')}:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
            await ScheduleJobAsync<ClearJob>(device, parsedDateTimeOffsetWithFormat, TimeSpan.FromHours(device.DeviceDescriptor.Extra["IntervalHour"].ToDouble()), $"{device.DeviceId}_Group", cancellationToken);
        }

        protected async Task ScheduleJobAsync<T>(DefaultDrill device, DateTimeOffset startTime, TimeSpan frequency, string groupName, CancellationToken cancellationToken)
          where T : IJob
        {
            var scheduler = await this._schedulerFactory.GetScheduler();
            var jobName = $"{device.DeviceId}_{typeof(T).Name}_Job";
            var triggerName = $"{device.DeviceId}_{typeof(T).Name}_Trigger";
            var job = JobBuilder.Create<T>()
                                .WithIdentity(jobName, groupName)
                                .UsingJobData("DeviceId", device.DeviceId)
                                .UsingJobData("ClearFlag", device.DeviceDescriptor.Extra["ClearFlag"].ToString())
                                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity(triggerName, groupName)
                .StartAt(startTime)
                .WithSimpleSchedule(x => x
                .WithInterval(frequency)
                .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger, cancellationToken);
        }
    }
}
