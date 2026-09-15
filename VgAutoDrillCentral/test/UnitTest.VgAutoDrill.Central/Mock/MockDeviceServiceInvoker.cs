using VgAutoDrill.Central.Core;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace UnitTest.VgAutoDrill.Central.Mock;

internal class MockDeviceServiceInvoker : IDeviceServiceInvoker
{
    public Task<DeviceServiceInvokeResponse> InvokeService(DeviceServiceInvokeRequest request, int retryCount = 10)
    {
        return Task.FromResult(new DeviceServiceInvokeResponse()
        {
            Code = ErrorCodes.Sys.SUCCESS
        });
    }
}
