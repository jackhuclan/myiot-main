using Microsoft.Extensions.Logging;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Reporter;

public class DeviceServiceReporter : IDeviceServiceReporter
{
    private readonly ILogger<DeviceServiceReporter> _logger;
    private readonly IDeviceManager _deviceManager;

    public DeviceServiceReporter(IDeviceManager deviceManager,
        ILoggerFactory loggerFactory)
    {
        _deviceManager = deviceManager;
        _logger = loggerFactory.CreateLogger<DeviceServiceReporter>();
    }

    public async Task<DeviceServiceInvokeResponse> Report(DeviceServiceInvokeRequest? request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ServiceId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.TargetProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.TargetDeviceId);
        _logger.LogInformation($"service/invoke {request.DeviceId}");

        var response = new DeviceServiceInvokeResponse()
        {
            Code = ErrorCodes.Sys.UNKNOWN_CODE,
            Message = $"中控-{ErrorCodes.Sys.UNKNOWN_MESSAGE}"
        };

        //var caller = _deviceManager.GetOnlineDevice(request.DeviceId);
        //if (caller == null)
        //{
        //    _logger.LogInformation(string.Format(ErrorCodes.Sys.WRONG_DEVICE_MESSAGE, request.DeviceId));
        //    response = new DeviceServiceInvokeResponse
        //    {
        //        Code = ErrorCodes.Sys.WRONG_DEVICE_CODE,
        //        Message = $"中控-{string.Format(ErrorCodes.Sys.WRONG_DEVICE_MESSAGE, request.DeviceId)}"
        //    };

        //    return response;
        //}

        var target = _deviceManager.GetOnlineDevice(request.TargetDeviceId);
        if (target == null)
        {
            response = new DeviceServiceInvokeResponse
            {
                Code = ErrorCodes.Sys.WRONG_DEVICE_CODE,
                Message = $"中控-{string.Format(ErrorCodes.Sys.WRONG_DEVICE_MESSAGE, request.TargetDeviceId)}"
            };

            return response;
        }
        else
        {
            request.TargetClientId = target.ClientId;

            switch (request.ServiceId)
            {
                case Topics.Services.COMPLETE_SCHEDULE_SERVICE_ID:
                case Topics.Services.SCHEDULE_TASK_SERVICE_ID:
                case Topics.Services.PROPERTIES_READ_SERVICE_ID:
                case Topics.Services.PROPERTIES_WRITE_SERVICE_ID:
                case Topics.Services.WORK_SERVICE_ID:
                case Topics.Services.AGV_MOVE_SERVICE_ID:
                case Topics.Services.AGV_CHARGE_SERVICE_ID:
                case Topics.Services.STANDBY_SERVICE_ID:
                case Topics.Services.SHUTDOWN_SERVICE_ID:
                case Topics.Services.MAINTAIN_SERVICE_ID:
                case Topics.Services.PREPARE_LOAD_MATERIAL_SERVICE_ID:
                case Topics.Services.INVOKE_LOAD_MATERIAL_SERVICE_ID:
                case Topics.Services.COMPLETE_LOAD_MATERIAL_SERVICE_ID:
                case Topics.Services.PREPARE_UNLOAD_MATERIAL_SERVICE_ID:
                case Topics.Services.INVOKE_UNLOAD_MATERIAL_SERVICE_ID:
                case Topics.Services.COMPLETE_UNLOAD_MATERIAL_SERVICE_ID:
                case Topics.Services.REMOTE_COMMAND_SERVICE_ID:

                    response = await target.InvokeService(request);
                    return response;
            }

            return new DeviceServiceInvokeResponse
            {
                Code = ErrorCodes.Sys.WRONG_UNSUPPORTED_SERVICE,
                Message = $"中控-不支持此服务：{request.ServiceId}"
            };
        }
    }
}
