using Microsoft.Extensions.Logging;
using MQTTnet.Server;
using VgAutoDrill.Central.Core.Handler.Rate;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Infrastructure;

namespace VgAutoDrill.Central.Core.Reporter;

public class DeviceLoginReporter : IDeviceLoginReporter
{
    private readonly IDeviceManager _deviceManager;
    private readonly IDeviceProxyFactory _deviceProxyFactory;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly IObjectFactory _objectFactory;

    public DeviceLoginReporter(IDeviceManager deviceManager,
            IDeviceProxyFactory deviceProxyFactory,
            IScheduleTaskManager scheduleTaskManager,
            IObjectFactory objectFactory,
            ILoggerFactory loggerFactory)
    {
        _deviceManager = deviceManager;
        _deviceProxyFactory = deviceProxyFactory;
        _scheduleTaskManager = scheduleTaskManager;
        _objectFactory = objectFactory;
    }

    public async Task<OnlineResponse> Report(OnlineRequest? onlineRequest, InterceptingPacketEventArgs arg)
    {
        if (onlineRequest != null && onlineRequest.Connected)
        {
            return await RegisterDevice(onlineRequest, arg);
        }

        return new OnlineResponse()
        {
            Code = ErrorCodes.Sys.FAIL
        };
    }

    private async Task<OnlineResponse> RegisterDevice(OnlineRequest onlineRequest, InterceptingPacketEventArgs arg)
    {
        if (!_deviceManager.TryGetLocalDevice(onlineRequest.Descriptor.DeviceId, out DeviceProxy? deviceProxy)
            || deviceProxy == null)
        {
            deviceProxy = _deviceProxyFactory.Create(onlineRequest.Descriptor);
        }

        deviceProxy.Descriptor = onlineRequest.Descriptor;
        deviceProxy.ClientId = arg.ClientId;
        deviceProxy.ClientIp = arg.Endpoint;

        if (deviceProxy.Status == DeviceStatus.Offline
        || deviceProxy.Status == DeviceStatus.Unknown)
        {
            deviceProxy.LogInTime = DateTime.Now;
            deviceProxy.Status = DeviceStatus.Online;
            await _deviceManager.AddOrUpdateDevice(deviceProxy);
        }

        if (!deviceProxy.HasStatusChanging)
        {
            var handler = _objectFactory.GetOrCreate<DeviceStatusChangingRateFactorHandler>();
            deviceProxy.StatusChanging += args => handler.Handle(args);
        }

        var response = new OnlineResponse
        {
            ProductId = deviceProxy.Descriptor.ProductId,
            DeviceId = deviceProxy.Descriptor.DeviceId,
            ClientId = arg.ClientId,
            Code = ErrorCodes.Sys.SUCCESS
        };

        return response;
    }
}
