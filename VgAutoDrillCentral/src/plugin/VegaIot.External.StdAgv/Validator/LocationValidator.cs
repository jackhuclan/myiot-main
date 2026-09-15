using VegaIot.External.AgvEntity.STD;

using VgAutoDrill.Central.Core.Manager;
using static VegaIot.External.Agv.ErrorCodes;

namespace VegaIot.External.StdAgv.Validator;

internal class LocationValidator
{
    private readonly ILocationManager _locationManager;

    public LocationValidator(ILocationManager locationManager)
    {
        _locationManager = locationManager;
    }

    public StdArrivedResponseEntityV2 Validate(string? locationCode)
    {
        if (string.IsNullOrEmpty(locationCode)
            || !_locationManager.TryGetLocation(locationCode, out var startLocation)
            || startLocation == null)
        {
            return new StdArrivedResponseEntityV2
            {
                Code = ERR_CODE,
                Message = NOT_FIND_DEVICE_LOCATION + $":{locationCode}"
            };
        }

        return new StdArrivedResponseEntityV2
        {
            Code = SUCCESS,
        };
    }
}
