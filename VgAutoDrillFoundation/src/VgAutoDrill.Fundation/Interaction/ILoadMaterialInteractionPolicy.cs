using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Interaction;

public interface ILoadMaterialInteractionPolicy : IDeviceShare
{
    /// <summary>
    /// 设备准备上料
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    Task<DeviceServiceInvokeResponse> PrepareLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest);

    /// <summary>
    /// 设备执行上料
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    Task<DeviceServiceInvokeResponse> InvokeLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest);

    /// <summary>
    /// 设备完成上料
    /// </summary>
    /// <param name="deviceServiceInvokeRequest">包含服务调用的所有信息</param>
    /// <returns></returns>
    Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest);
}
