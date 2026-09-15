using VegaIot.External.AgvEntity.Hik;
using VgAutoDrill.Central.Core.Manager;
using static VegaIot.External.Agv.ErrorCodes;

namespace VegaIot.External.HikAgv.Validator;

internal class LocationValidator
{
    private readonly ILocationManager _locationManager;

    public LocationValidator(ILocationManager locationManager)
    {
        _locationManager = locationManager;
    }

    public HikArrivedResponseEntity Validate(string? locationCode)
    {
        if (string.IsNullOrEmpty(locationCode)
            || !_locationManager.TryGetLocation(locationCode, out var location)
            || location == null)
        {
            return new HikArrivedResponseEntity
            {
                code = ERR_CODE,
                message = NOT_FIND_DEVICE_LOCATION + $":{locationCode}"
            };
        }

        return new HikArrivedResponseEntity
        {
            code = SUCCESS,
        };
    }
}
