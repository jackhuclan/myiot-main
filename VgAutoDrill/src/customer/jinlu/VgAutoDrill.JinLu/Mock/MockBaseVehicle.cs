using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Mqtt;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.JinLu.Mock
{
    public abstract class MockBaseVehicle<TMock> : MockBaseDevice<TMock>, IVehicle
    {
        private readonly ILogger<TMock> logger;
        private readonly IMqttClientWrapper mqttClientWrapper;
        private readonly ILoggerFactory loggerFactory;

        protected MockBaseVehicle(IHttpRequestInvoker httpRequestInvoker,
            IDeviceEngine deviceEngine,
            IMqttClientWrapper mqttClientWrapper,
            DeviceDescriptor deviceDescriptor,
            IOptions<CentralWebOptions> options,
            ILoggerFactory loggerFactory)
            : base(httpRequestInvoker, deviceEngine, mqttClientWrapper, deviceDescriptor, options, loggerFactory)
        {
            this.mqttClientWrapper = mqttClientWrapper;
            this.loggerFactory = loggerFactory;
            logger = loggerFactory.CreateLogger<TMock>();
        }

        public async virtual Task<DeviceServiceInvokeResponse> Charge(DeviceServiceInvokeRequest request)
        {
            logger.LogInformation($"{DeviceName}-{DeviceId} is Charge!");

            return await DeviceServiceInvokeResponseBuilder
                .Create(mqttClientWrapper, loggerFactory)
                .Build(ErrorCodes.Sys.SUCCESS, string.Empty, null, request.ReplyTopic);
        }

        public virtual async Task<DeviceServiceInvokeResponse> Move(DeviceServiceInvokeRequest request)
        {
            logger.LogInformation($"{DeviceName}-{DeviceId} is Moving!");
            return await DeviceServiceInvokeResponseBuilder
                .Create(mqttClientWrapper, loggerFactory)
                .Build(ErrorCodes.Sys.SUCCESS, string.Empty, null, request.ReplyTopic);
        }

        private void ReverseCallerAndTarget(DeviceServiceInvokeRequest request)
        {
            var callerProductId = request.ProductId;
            var callerDeviceId = request.DeviceId;
            var callerClientId = request.ClientId;
            request.ProductId = this.ProductId;
            request.DeviceId = this.DeviceId;
            request.ClientId = this.ClientId;
            request.TargetProductId = callerProductId;
            request.TargetDeviceId = callerDeviceId;
            request.TargetClientId = callerClientId;
        }

        public override async Task<DeviceServiceInvokeResponse> ScheduleTask(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            ReverseCallerAndTarget(deviceServiceInvokeRequest);

            string? replyTopic = RefreshReplyTopicByUsingLatestClientId(deviceServiceInvokeRequest);

            if (SchedulingTasks.Count > 0)
            {
                if (SchedulingTasks.TryPeek(out var _, out var nextTaskOrder))
                {
                    var agvAction = deviceServiceInvokeRequest.Params[Tasks.PARAMS_TASK_ACTION]?.ToString();
                    if (agvAction == Tasks.PARAMS_TASK_ACTION_CHARGE)
                    {
                        deviceServiceInvokeRequest.Params[Tasks.PARAMS_TASK_ORDER] = nextTaskOrder - 1;
                        SchedulingTasks.Enqueue(deviceServiceInvokeRequest, deviceServiceInvokeRequest.Params[Tasks.PARAMS_TASK_ORDER].ToInt());
                    }
                    else
                    {
                        deviceServiceInvokeRequest.Params[Tasks.PARAMS_TASK_ORDER] = nextTaskOrder + 1;
                        SchedulingTasks.Enqueue(deviceServiceInvokeRequest, deviceServiceInvokeRequest.Params[Tasks.PARAMS_TASK_ORDER].ToInt());
                    }
                }
            }
            else
            {
                var agvAction = deviceServiceInvokeRequest.Params[Tasks.PARAMS_TASK_ACTION]?.ToString();
                switch (agvAction)
                {
                    case Tasks.PARAMS_TASK_ACTION_WORK:
                    case Tasks.PARAMS_TASK_ACTION_CHARGE:
                        deviceServiceInvokeRequest.Params[Tasks.PARAMS_TASK_ORDER] = DEFAULT_MAX_TASKS_LIMIT / 2;
                        SchedulingTasks.Enqueue(deviceServiceInvokeRequest, deviceServiceInvokeRequest.Params[Tasks.PARAMS_TASK_ORDER].ToInt());
                        break;
                    default:
                        break;
                }
            }

            return await DeviceServiceInvokeResponseBuilder
                .Create(this.mqttClientWrapper, this.loggerFactory)
                .Build(ErrorCodes.Sys.SUCCESS, string.Empty, replyTopic);
        }

        public virtual Task<DeviceStatusReportResponse> ReportStatus()
        {
            throw new NotImplementedException();
        }

        private string? RefreshReplyTopicByUsingLatestClientId(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            string? replyTopic = deviceServiceInvokeRequest.ReplyTopic;
            if (!string.IsNullOrEmpty(replyTopic))
            {
                var slices = replyTopic.Split('/');
                slices[slices.Length - 2] = this.ClientId;
                replyTopic = string.Join('/', slices);
            }

            return replyTopic;
        }
    }
}
