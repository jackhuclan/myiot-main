using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgDeviceGateway.Devices.Agv.Chassis;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Agv;

public class DefaultAgvChassis : DeviceShare<DefaultAgv>, IAgvChassis
{
    private readonly IServiceProvider serviceProvider;
    private IAgvChassis agvChassis;

    public DefaultAgvChassis(IServiceProvider serviceProvider,
        IObjectFactory objectFactory,
      DefaultAgv defaultAgv)
      : base(serviceProvider, defaultAgv)
    {
        this.serviceProvider = serviceProvider;
        agvChassis = InteractionAgvFactory.CreatetAgvChassis(objectFactory, InteractingDevice.DeviceDescriptor.Extra["AgvChassisSupplier"].ToStr());
    }

    public async Task<DeviceServiceInvokeResponse> ChargeLocal(DefaultAgv InteractingDevice)
    {
        return await agvChassis.ChargeLocal(InteractingDevice);
    }

    public async Task<DeviceServiceInvokeResponse> GetAgvInfoLocal(DefaultAgv InteractingDevice)
    {
        return await agvChassis.GetAgvInfoLocal(InteractingDevice);
    }

    public async Task<DeviceServiceInvokeResponse> MoveLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        var moveresult = await agvChassis.MoveLocal(InteractingDevice, deviceServiceInvokeRequest);
        // InteractingDevice.ReportingProcess($"Move：{moveresult.Code}_{moveresult.Message}", moveresult.Code != ErrorCodes.Sys.SUCCESS ? AlarmLevel.Severe : AlarmLevel.Information, "AES10008");
        return moveresult;
    }

    public async Task<DeviceServiceInvokeResponse> MoveCheckLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await agvChassis.MoveCheckLocal(InteractingDevice, deviceServiceInvokeRequest);
    }

    public Task<bool> IsLowBatteryLocal(DefaultAgv InteractingDevice)
    {
        return agvChassis.IsLowBatteryLocal(InteractingDevice);
    }

    public Task<bool> CanDispatchLocal(DefaultAgv InteractingDevice)
    {
        return agvChassis.CanDispatchLocal(InteractingDevice);
    }

    public Task<bool> CheckIsArrivedLocal(DefaultAgv InteractingDevice)
    {
        return agvChassis.CheckIsArrivedLocal(InteractingDevice);
    }

    public async Task<DeviceServiceInvokeResponse> ArrivedLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest, string taskId)
    {
        var arrivedFail = await agvChassis.ArrivedLocal(InteractingDevice, deviceServiceInvokeRequest, taskId);
        InteractingDevice.logger.LogDebug($"车辆到达:{arrivedFail.Code}_Message：{arrivedFail.Message}");
        await InteractingDevice.ReportingProcess($"车辆到达:{arrivedFail.Code}_Message：{arrivedFail.Message}",
             arrivedFail.Code != ErrorCodes.Sys.SUCCESS ? AlarmLevel.Severe : AlarmLevel.Information,
             arrivedFail.Code != ErrorCodes.Sys.SUCCESS ? "AES10020" : "");

        return arrivedFail;
    }

    public async Task<DefaultCallBackEntity> ArrivedInfoLocal(DefaultAgv InteractingDevice)
    {
        return await agvChassis.ArrivedInfoLocal(InteractingDevice);
    }

    public async Task<DeviceServiceInvokeResponse> MoveOperationLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest, int operationType)
    {
        return await agvChassis.MoveOperationLocal(InteractingDevice, deviceServiceInvokeRequest, operationType);
    }

    public async Task<DeviceServiceInvokeResponse> CancelLocal(DefaultAgv interactingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await agvChassis.CancelLocal(InteractingDevice, deviceServiceInvokeRequest);
    }

    public async Task<DeviceServiceInvokeResponse> ReleaseLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        var releasesult = await agvChassis.ReleaseLocal(InteractingDevice, deviceServiceInvokeRequest);
        return releasesult;
    }
}
