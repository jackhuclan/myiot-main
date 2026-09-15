using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceServiceInvocation;

namespace VgAutoDrill.Central.Core;

public class DeviceServiceInvocationLogger : IDeviceServiceInvocationLogger
{
    private readonly ILogger<DeviceServiceInvocationLogger> _logger;
    private readonly IDeviceServiceInvocationService _deviceServiceInvocationService;

    public DeviceServiceInvocationLogger(ILoggerFactory loggerFactory,
        IDeviceServiceInvocationService deviceServiceInvocationService)
    {
        _logger = loggerFactory.CreateLogger<DeviceServiceInvocationLogger>();
        _deviceServiceInvocationService = deviceServiceInvocationService;
    }

    /// <summary>
    /// 调用失败写入数据库
    /// 1. 如果MessageId存在，更新Retries, Payload, LastInvocationTimestamp,ServiceLevel
    /// 2. 如果MessageId不存在，直接写入一条新记录
    /// </summary>
    /// <param name="arg"></param>
    /// <returns></returns>
    public async Task OnInvocationFailure(DeviceServiceInvocationArgs arg)
    {
        await _deviceServiceInvocationService.AddDeviceServiceInvocationList(new List<AddOrUpdateDeviceServiceInvocationReq>
        {
            new AddOrUpdateDeviceServiceInvocationReq
            {
                MessageId = arg.MessageId,
                Reason = arg.Reason,
                RequestTopic = arg.RequestTopic,
                ResponseTopic = arg.ResponseTopic,
                Retries = 0,
                IsDealed = false,
                IsTimeout = false,
                MqttQualityOfServiceLevel = (int)arg.ServiceLevel,
                Payload = arg.Payload,
                RoutingKey = arg.RoutingKey,
            }
        });
    }

    //public Task OnInvocationSuccess(DeviceServiceInvocationArgs arg)
    //{
    //    return Task.CompletedTask;
    //}
}
