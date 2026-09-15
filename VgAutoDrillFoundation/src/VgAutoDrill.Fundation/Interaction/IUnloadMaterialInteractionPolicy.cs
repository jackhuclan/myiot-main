using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Interaction;

public interface IUnloadMaterialInteractionPolicy : IDeviceShare
{
    /// <summary>
    /// 设备准备下料
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    Task<DeviceServiceInvokeResponse> PrepareUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest);

    /// <summary>
    /// 设备执行下料
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    Task<DeviceServiceInvokeResponse> InvokeUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest);

    /// <summary>
    /// 设备完成下料
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest);

}
