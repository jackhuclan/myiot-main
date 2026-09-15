using Microsoft.Extensions.Logging;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.AgentHub.Mqtt;

internal class HubDeviceServiceInvoker : IDeviceServiceInvoker
{
    private readonly IMqttMessagePublisher _mqttMessagePublisher;
    private ILogger<HubDeviceServiceInvoker> _logger;

    public HubDeviceServiceInvoker(IMqttMessagePublisher mqttMessagePublisher,
        IDeviceServiceInvocationLogger deviceServiceInvocationLogger,
        ILoggerFactory loggerFactory)
    {
        _mqttMessagePublisher = mqttMessagePublisher;
        _logger = loggerFactory.CreateLogger<HubDeviceServiceInvoker>();
    }

    public async Task<DeviceServiceInvokeResponse> InvokeService(DeviceServiceInvokeRequest request, int retryCount = 10)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ServiceId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.TargetProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.TargetDeviceId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.TargetClientId);

        var response = await ExecuteServiceInvocation(request);

        return response;
    }

    private async Task<DeviceServiceInvokeResponse> ExecuteServiceInvocation(DeviceServiceInvokeRequest request)
    {
        var response = new DeviceServiceInvokeResponse();

        try
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(TimeSpan.FromSeconds(5));
            CancellationToken cancellationToken = tokenSource.Token;
            var requestTopic = request.RequestTopic;
            var replyTopic = request.ReplyTopic;
            var invokeResult = await _mqttMessagePublisher.Publish<DeviceServiceInvokeRequest, DeviceServiceInvokeResponse>(requestTopic, replyTopic, request);
            if (invokeResult == null)
            {
                response.Code = ErrorCodes.Sys.FAIL;
                response.Message = "调用服务时返回类型不正确";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"{request.DeviceId} - {request.TargetDeviceId}-{ex.Message}");
            response = new DeviceServiceInvokeResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = ex.Message
            };
        }

        return response ?? new DeviceServiceInvokeResponse()
        {
            Code = ErrorCodes.Sys.FAIL,
            Message = ErrorCodes.Sys.UNKNOWN_MESSAGE
        };
    }
}
