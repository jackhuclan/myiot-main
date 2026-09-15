
namespace VgAutoDrill.Central.Core;

public interface IDeviceServiceInvocationLogger
{
    Task OnInvocationFailure(DeviceServiceInvocationArgs arg);
    //Task OnInvocationSuccess(DeviceServiceInvocationArgs arg);
}
