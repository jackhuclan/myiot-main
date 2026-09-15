using System.Text;
using System.Text.Json;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Polly;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Extensions;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Mqtt.Server;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;

namespace VgAutoDrill.Central.WebApi.Core;

public class MqttServerDeviceServiceInvoker : IDeviceServiceInvoker
{
    private readonly MqttServer _mqttServer;
    private readonly IAsyncTaskWaiter _taskWaiter;
    private readonly ILogger<MqttServerDeviceServiceInvoker> _logger;

    public MqttServerDeviceServiceInvoker(MqttServer mqttServer,
        IAsyncTaskWaiter taskWaiter,
        ILoggerFactory loggerFactory)
    {
        _mqttServer = mqttServer;
        _taskWaiter = taskWaiter;
        _logger = loggerFactory.CreateLogger<MqttServerDeviceServiceInvoker>();
    }

    public async Task<DeviceServiceInvokeResponse> InvokeService(DeviceServiceInvokeRequest request, int retryCount = 10)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ServiceId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.TargetProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.TargetDeviceId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.TargetClientId);

        var requestTopic = request.GetRequestTopic();
        var replyTopic = request.GetReplyTopic();
        request.RequestTopic = requestTopic;
        request.ReplyTopic = replyTopic;

        _logger.LogInformation($"MqttServerInvoker,{request.DeviceId} ->TargetDeviceId:{request.TargetDeviceId},ServiceId:{request.ServiceId},TargetProductId:{request.TargetProductId},TargetClientId:{request.TargetClientId},requestTopic:{requestTopic}.\r\n");
        _logger.LogInformation($"MqttServerInvoker,replyTopic:{replyTopic}.\r\n");

        if (request.ServiceId == Topics.Services.SCHEDULE_TASK_SERVICE_ID)
        {
            request.Params.TryAdd(ScheduleConstants.PARAMS_TASK_ORDER, 0);
            request.Params.TryAdd(ScheduleConstants.PARAMS_TASK_STATUS, ScheduledTaskStatus.Created.ToString());
            request.Params.TryAdd(ScheduleConstants.PARAMS_TASK_ACTION, ScheduleConstants.PARAMS_TASK_ACTION_WORK);
            request.Params.TryAdd(ScheduleConstants.PARAMS_TASK_CREATE_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        var inputRequest = JsonSerializer.Serialize(request);
        var inputBytes = Encoding.UTF8.GetBytes(inputRequest);

        return await Policy.HandleInner<Exception>()
                .WaitAndRetryAsync(retryCount, t => TimeSpan.FromSeconds(t), (outcome, i, ctx) =>
                {
                    _logger.LogInformation($"MqttServerInvoker,ExecuteServiceInvocation is retrying at {i} times...");
                })
                .ExecuteAsync(async () =>
                {
                    var serviceLevel = MqttQualityOfServiceLevel.ExactlyOnce;
                    _logger.LogInformation($"MqttServerInvoker,ExecuteServiceInvocation begin:{request.DeviceId} -> {request.TargetDeviceId} - {request.ServiceId}");
                    var response = await ExecuteServiceInvocation(request, requestTopic, replyTopic, inputBytes, serviceLevel);
                    _logger.LogInformation($"MqttServerInvoker,ExecuteServiceInvocation end:{request.DeviceId} -> {request.TargetDeviceId} - {request.ServiceId}");

                    return response;
                });
    }

    private async Task<DeviceServiceInvokeResponse> ExecuteServiceInvocation(DeviceServiceInvokeRequest request,
        string requestTopic,
        string replyTopic,
        byte[] inputBytes,
        MqttQualityOfServiceLevel serviceLevel)
    {
        var response = new DeviceServiceInvokeResponse()
        {
            Code = ErrorCodes.Sys.FAIL,
            Message = "中控-目标设备调用超时"
        };

        try
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(TimeSpan.FromSeconds(10));
            CancellationToken cancellationToken = tokenSource.Token;

            var invokeResult = await _mqttServer.InvokeServcieAsync(
                                   _taskWaiter,
                                   requestTopic,
                                   replyTopic,
                                   inputBytes,
                                   serviceLevel,
                                   _logger,
                                   cancellationToken);
            if (JsonSerializer.Deserialize<DeviceServiceInvokeResponse>(invokeResult) != null)
            {
                response = JsonSerializer.Deserialize<DeviceServiceInvokeResponse>(invokeResult);
                _logger.LogInformation($"MqttServerInvoker,{request.DeviceId} -> {request.TargetDeviceId} - {request.ServiceId} - {response.Message}");
            }
            else
            {
                _logger.LogInformation($"MqttServerInvoker,中控-调用服务时返回类型不正确");
                response.Code = ErrorCodes.Sys.FAIL;
                response.Message = "中控-调用服务时返回类型不正确";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"MqttServerInvoker,{request.DeviceId} -> {request.TargetDeviceId} - {request.ServiceId} - {ex.Message}");
            response = new DeviceServiceInvokeResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"中控-{ex.Message}"
            };
        }

        return response ?? new DeviceServiceInvokeResponse()
        {
            Code = ErrorCodes.Sys.FAIL,
            Message = $"中控-{ErrorCodes.Sys.UNKNOWN_MESSAGE}"
        };
    }
}
