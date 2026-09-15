using VegaIot.External.AgvEntity.STD;
using VgAutoDrill.Central.Core.Manager;
using static VegaIot.External.Agv.ErrorCodes;

namespace VegaIot.External.StdAgv.Validator;

internal class DeviceValidator
{
    private readonly IDeviceManager _deviceHolder;

    public DeviceValidator(IDeviceManager deviceHolder)
    {
        _deviceHolder = deviceHolder;
    }

    public StdArrivedResponseEntityV2 Validate(string? deviceCode)
    {
        if (string.IsNullOrEmpty(deviceCode))
        {
            return new StdArrivedResponseEntityV2
            {
                Code = ERR_CODE,
                Message = TRANSFER_JOB_START_DEVICE_IS_NOT_EMPTY + $":{deviceCode}"
            };
        }

        var startDevice = _deviceHolder.GetOnlineDevice(deviceCode);
        if (startDevice == null)
        {
            return new StdArrivedResponseEntityV2
            {
                Code = ERR_CODE,
                Message = NOT_FIND_DEVICE + $":{deviceCode}"
            };
        }

        return new StdArrivedResponseEntityV2
        {
            Code = SUCCESS,
        };
    }
}
