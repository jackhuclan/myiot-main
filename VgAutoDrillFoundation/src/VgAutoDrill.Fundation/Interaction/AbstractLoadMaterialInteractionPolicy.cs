using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Interaction;

/// <summary>
/// 抽象上料策略，具体的上料策略均应该继承于此类
/// </summary>
/// <typeparam name="TDevice">具体的设备类</typeparam>
public abstract class AbstractLoadMaterialInteractionPolicy<TDevice> : DeviceShare<TDevice>, ILoadMaterialInteractionPolicy
    where TDevice : Device
{
    public AbstractLoadMaterialInteractionPolicy(IServiceProvider serviceProvider, TDevice device) : base(serviceProvider, device) { }

    /// <inheritdoc/>
    public abstract Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest);
    /// <inheritdoc/>
    public abstract Task<DeviceServiceInvokeResponse> InvokeLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest);
    /// <inheritdoc/>
    public abstract Task<DeviceServiceInvokeResponse> PrepareLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest);
}
