// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Quartz;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Drill.Other.Jobs
{
    public class ClearJob : IJob
    {
        private readonly ILogger<ClearJob> _logger;
        private readonly IDeviceProvider _deviceProvider;

        public ClearJob(ILogger<ClearJob> logger, IDeviceProvider deviceProvider)
        {
            _logger = logger;
            _deviceProvider = deviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() =>
            {
                string? deviceId = context.JobDetail.JobDataMap.GetString("DeviceId");
                if (deviceId == null)
                {
                    ThrowHelper.ThrowArgumentNullException("DeviceId is null in ClearJob");
                }

                string? clearFlag = context.JobDetail.JobDataMap.GetString("ClearFlag");
                if (clearFlag == null)
                {
                    ThrowHelper.ThrowArgumentNullException("ClearFlag is null in ClearJob");
                }

                _logger.LogInformation($"[{DateTime.Now.ToLongTimeString()}]:  ClearJob is executing at {deviceId} {clearFlag}...");
                var device = (DefaultDrill)_deviceProvider.GetDevice(deviceId);
                try
                {
                    device.cnc84Command.WriteCncNode<bool>($"ns=4;s=UI/normalized/custom/{clearFlag}", true);
                    _logger.LogInformation($"[{DateTime.Now.ToLongTimeString()}]:  ClearJob is executing success.");
                }
                catch (Exception e)
                {
                    _logger.LogError($"[{DateTime.Now.ToLongTimeString()}]:  ClearJob is executing fail {e.Message}.");
                }
            });
        }
    }
}
