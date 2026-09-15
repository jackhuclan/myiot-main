
using VgAutoDrill.Fundation.Iot.Models;

namespace VgDeviceGateway.Devices.Common;

public static class DeviceServiceInvokeRequestExtensions
{
    public static int GetPosition(this DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        try
        {
            string? shelfIndex = deviceServiceInvokeRequest?.EventId?.Split('#')[1];
            if (string.IsNullOrWhiteSpace(shelfIndex))
                throw new ArgumentNullException(nameof(shelfIndex));

            return int.Parse(shelfIndex);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
