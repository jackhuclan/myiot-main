using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Interaction;

/// <summary>
/// 抽象下料策略，具体的下料策略均应该继承于此类
/// </summary>
/// <typeparam name="TDevice">具体的设备类</typeparam>
public abstract class AbstractUnloadMaterialInteractionPolicy<TDevice> : DeviceShare<TDevice>, IUnloadMaterialInteractionPolicy
    where TDevice : Device
{
    public AbstractUnloadMaterialInteractionPolicy(IServiceProvider serviceProvider, TDevice device) : base(serviceProvider, device) { }

    /// <inheritdoc/>
    public abstract Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest);

    /// <inheritdoc/>
    public abstract Task<DeviceServiceInvokeResponse> InvokeUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest);

    /// <inheritdoc/>
    public abstract Task<DeviceServiceInvokeResponse> PrepareUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest);
}
