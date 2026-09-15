using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Command;

/// <summary>
/// 指令函数
/// </summary>
/// <param name="deviceServiceInvokeRequest">传入参数</param>
/// <returns>返回DeviceServiceInvokeResponse</returns>

public delegate Task<DeviceServiceInvokeResponse> CommandFunction(DeviceServiceInvokeRequest deviceServiceInvokeRequest);
