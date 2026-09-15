using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Reporter;

internal class DeviceCutterReporter : IDeviceCutterReporter
{
    private readonly ILogger<DeviceCutterReporter> _logger;
    private readonly IDeviceManager _deviceHolder;

    public DeviceCutterReporter(IDeviceManager deviceHolder,
        ILoggerFactory loggerFactory)
    {
        _deviceHolder = deviceHolder;
        _logger = loggerFactory.CreateLogger<DeviceCutterReporter>();
    }

    public async Task<DeviceCutterTrayChangedResponse> Report(DeviceCutterTrayChangedRequest? request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        _logger.LogTrace(JsonSerializer.Serialize(request));

        var deviceProxy = _deviceHolder.GetOnlineDevice(request.DeviceId);
        if (deviceProxy == null)
        {
            return new DeviceCutterTrayChangedResponse
            {
                Code = ErrorCodes.Sys.WRONG_DEVICE_CODE,
                Message = ErrorCodes.Sys.WRONG_DEVICE_MESSAGE
            };
        }

        await deviceProxy.SetCutterTrays(request.CutterTrayList);

        return new DeviceCutterTrayChangedResponse()
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = string.Empty
        };
    }
}
