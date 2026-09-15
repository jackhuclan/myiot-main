using VegaIot.External.AgvEntity.Hik;
using VgAutoDrill.Central.Core.Manager;
using static VegaIot.External.Agv.ErrorCodes;

namespace VegaIot.External.HikAgv.Validator;

internal class DeviceValidator
{
    private readonly IDeviceManager _deviceHolder;

    public DeviceValidator(IDeviceManager deviceHolder)
    {
        _deviceHolder = deviceHolder;
    }

    public HikArrivedResponseEntity Validate(string? deviceCode)
    {
        if (string.IsNullOrEmpty(deviceCode))
        {
            return new HikArrivedResponseEntity
            {
                code = ERR_CODE,
                message = TRANSFER_JOB_START_DEVICE_IS_NOT_EMPTY + $":{deviceCode}"
            };
        }

        var startDevice = _deviceHolder.GetOnlineDevice(deviceCode);
        if (startDevice == null)
        {
            return new HikArrivedResponseEntity
            {
                code = ERR_CODE,
                message = NOT_FIND_DEVICE + $":{deviceCode}"
            };
        }

        return new HikArrivedResponseEntity
        {
            code = SUCCESS,
        };
    }
}
