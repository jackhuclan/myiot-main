using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Mqtt;
using VgAutoDrill.Fundation.Mqtt.Client;

namespace VgAutoDrill.JinLu.Mock
{
    public abstract class MockBaseDevice<TMock> : Device
    {
        private readonly ILogger<TMock> logger;
        private readonly DeviceDescriptor deviceDescriptor;
        private readonly IMqttClientWrapper mqttClientWrapper;
        private readonly ILoggerFactory loggerFactory;

        public MockBaseDevice(IHttpRequestInvoker httpRequestInvoker,
            IDeviceEngine deviceEngine,
            IMqttClientWrapper mqttClientWrapper,
            DeviceDescriptor deviceDescriptor,
            IOptions<CentralWebOptions> options,
            ILoggerFactory loggerFactory)
            : base(httpRequestInvoker, deviceEngine, mqttClientWrapper, deviceDescriptor, options, loggerFactory)
        {
            logger = loggerFactory.CreateLogger<TMock>();

            this.Connector.ConnectFunc = (device) =>
            {
                return Task.FromResult(true);
            };

            this.deviceDescriptor = deviceDescriptor;
            this.mqttClientWrapper = mqttClientWrapper;
            this.loggerFactory = loggerFactory;
        }
        protected DeviceStatusReportRequest GetStatusRequest(DeviceStatus newStatus)
        {
            var deviceServiceInvokeRequest = new DeviceStatusReportRequest
            {
                DeviceId = DeviceDescriptor.DeviceId,
                ProductId = DeviceDescriptor.ProductId,
                OldStatus = this.Status,
                NewStatus = newStatus,
                PayloadPanels = PayloadPanels,
            };
            return deviceServiceInvokeRequest;
        }

        protected void ReverseCallerAndTarget(DeviceServiceInvokeRequest request)
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
        public override Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest request)
        {
            logger.LogInformation($"{DeviceName}-{DeviceId}  finish CompleteLoadMaterial!");

            return DeviceServiceInvokeResponseBuilder
                .Create(mqttClientWrapper, loggerFactory)
                .Build(ErrorCodes.Sys.SUCCESS, string.Empty, null, request.ReplyTopic);
        }

        public override Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest request)
        {
            logger.LogInformation($"{DeviceName}-{DeviceId}  finish CompleteUnloadMaterial!");

            return DeviceServiceInvokeResponseBuilder
                .Create(mqttClientWrapper, loggerFactory)
                .Build(ErrorCodes.Sys.SUCCESS, string.Empty, null, request.ReplyTopic);
        }

        public override Task<DeviceServiceInvokeResponse> InvokeLoadMaterial(DeviceServiceInvokeRequest request)
        {
            logger.LogInformation($"{DeviceName}-{DeviceId}  finish InvokeLoadMaterial!");

            return DeviceServiceInvokeResponseBuilder
                .Create(mqttClientWrapper, loggerFactory)
                .Build(ErrorCodes.Sys.SUCCESS, string.Empty, null, request.ReplyTopic);
        }

        public override Task<DeviceServiceInvokeResponse> InvokeUnloadMaterial(DeviceServiceInvokeRequest request)
        {
            logger.LogInformation($"{DeviceName}-{DeviceId}  finish InvokeUnloadMaterial!");

            return DeviceServiceInvokeResponseBuilder
                .Create(mqttClientWrapper, loggerFactory)
                .Build(ErrorCodes.Sys.SUCCESS, string.Empty, null, request.ReplyTopic);
        }

        public override Task<DeviceServiceInvokeResponse> PrepareLoadMaterial(DeviceServiceInvokeRequest request)
        {
            logger.LogInformation($"{DeviceName}-{DeviceId}   finish PrepareLoadMaterial!");

            return DeviceServiceInvokeResponseBuilder
                .Create(mqttClientWrapper, loggerFactory)
                .Build(ErrorCodes.Sys.SUCCESS, string.Empty, null, request.ReplyTopic);
        }

        public override Task<DeviceServiceInvokeResponse> PrepareUnloadMaterial(DeviceServiceInvokeRequest request)
        {
            logger.LogInformation($"{DeviceName}-{DeviceId} finish PrepareUnloadMaterial!");

            return DeviceServiceInvokeResponseBuilder
                .Create(mqttClientWrapper, loggerFactory)
                .Build(ErrorCodes.Sys.SUCCESS, string.Empty, null, request.ReplyTopic);
        }

        public override Task<DeviceServiceInvokeResponse> ReadProperties(DeviceServiceInvokeRequest request)
        {
            logger.LogInformation($"{DeviceName}-{DeviceId}  finish PropertiesRead!");

            return DeviceServiceInvokeResponseBuilder
                .Create(mqttClientWrapper, loggerFactory)
                .Build(ErrorCodes.Sys.SUCCESS, string.Empty, null, request.ReplyTopic);
        }

        public override Task<DeviceServiceInvokeResponse> WriteProperties(DeviceServiceInvokeRequest request)
        {
            logger.LogInformation($"{DeviceName}-{DeviceId} finish PropertiesWrite!");

            return DeviceServiceInvokeResponseBuilder
                .Create(mqttClientWrapper, loggerFactory)
                .Build(ErrorCodes.Sys.SUCCESS, string.Empty, null, request.ReplyTopic);
        }

        public override Task<DeviceServiceInvokeResponse> ScheduleTask(DeviceServiceInvokeRequest request)
        {
            logger.LogInformation($"ScheduleTask is done, {DeviceName}-{DeviceId} is ScheduleTask!");

            return DeviceServiceInvokeResponseBuilder
                .Create(mqttClientWrapper, loggerFactory)
                .Build(ErrorCodes.Sys.SUCCESS, string.Empty, null, request.ReplyTopic);
        }

        public override Task<DeviceServiceInvokeResponse> Shutdown(DeviceServiceInvokeRequest request)
        {
            logger.LogInformation($"{DeviceName}-{DeviceId} finish Shutdown!");

            return DeviceServiceInvokeResponseBuilder
                .Create(mqttClientWrapper, loggerFactory)
                .Build(ErrorCodes.Sys.SUCCESS, string.Empty, null, request.ReplyTopic);
        }

        public override Task<DeviceServiceInvokeResponse> Standby(DeviceServiceInvokeRequest request)
        {
            logger.LogInformation($"{DeviceName}-{DeviceId} finish Standby!");

            return DeviceServiceInvokeResponseBuilder
                .Create(mqttClientWrapper, loggerFactory)
                .Build(ErrorCodes.Sys.SUCCESS, string.Empty, null, request.ReplyTopic);
        }

        public override Task<DeviceServiceInvokeResponse> Work(DeviceServiceInvokeRequest request)
        {
            logger.LogInformation($"{DeviceName} {DeviceId} finish work!");

            return DeviceServiceInvokeResponseBuilder
                .Create(mqttClientWrapper, loggerFactory)
                .Build(ErrorCodes.Sys.SUCCESS, string.Empty, null, request.ReplyTopic);
        }
    }
}
