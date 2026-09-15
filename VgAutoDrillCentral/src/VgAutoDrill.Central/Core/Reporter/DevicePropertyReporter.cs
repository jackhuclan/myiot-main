using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Reporter;

public class DevicePropertyReporter : IDevicePropertyReporter
{
    private readonly IDeviceManager _deviceHolder;

    public DevicePropertyReporter(IDeviceManager deviceHolder)
    {
        _deviceHolder = deviceHolder;
    }

    public Task<DevicePropertiesReportResponse> Report(DevicePropertiesReportRequest? request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);

        DevicePropertiesReportResponse response;

        var device = _deviceHolder.GetOnlineDevice(request.DeviceId);
        if (device == null)
        {
            response = new DevicePropertiesReportResponse
            {
                Code = ErrorCodes.Sys.WRONG_DEVICE_CODE,
                Message = ErrorCodes.Sys.WRONG_DEVICE_MESSAGE
            };
        }
        else
        {
            _ = device.RefreshProperties(request.Params);
        }

        response = new DevicePropertiesReportResponse
        {
            Code = ErrorCodes.Sys.SUCCESS
        };

        return Task.FromResult(response);
    }
}
