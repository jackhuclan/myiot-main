using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Drill.InteractionUnload
{
    public class HalfRearDrillUnloadBoard : DeviceShare<DefaultDrill>, IDrillUnloadInteraction
    {
        private readonly ILogger<HalfRearDrillUnloadBoard> logger;
        public readonly byte slaveID;
        private readonly bool bufferAutoUnloadOutOnOff;

        public HalfRearDrillUnloadBoard(ILogger<HalfRearDrillUnloadBoard> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
            slaveID = (byte)device.DeviceDescriptor.Extra["SlaveID"].ToInt();
            bufferAutoUnloadOutOnOff = device.DeviceDescriptor.Extra["BufferAutoUnloadOutOnOff"].ToBool();
        }

        public async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterialLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, int position)
        {
            InteractingDevice.allowAllAgv = true;
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvUnLoadingAndLoading"].ToUshort(), 0);
            logger.LogDebug($"CompleteUnloadMaterial 机器【{InteractingDevice.DeviceDescriptor.DeviceName}】 人工 上下料动作结束");
            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS);
        }

        public async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterialLocal(int position)
        {
            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }

        public async Task<DeviceServiceInvokeResponse> PrepareUnloadMaterialLocal(DeviceServiceInvokeRequest request, int position)
        {
            var bufferOnAgvPosition = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferOnAgvPositionFlag"].ToUshort(), 1);
            if (!bufferOnAgvPosition[0])
            {
                logger.LogDebug($"Buffer不在agv对接层不能执行下料");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"Buffer不在agv对接层不能执行下料84->{DeviceDescriptor.DeviceId}", request.Params);
            }
            var work = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferStatusFlag"].ToUshort(), 4);
            var model = work[1] == 1;
            if (!model)
            {
                logger.LogDebug($"手动状态下agv不能给Buffer下料");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"手动状态下agv不能给Buffer下料84->{DeviceDescriptor.DeviceId}", request.Params);
            }
            var bufferOnWorking = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferOnWorking"].ToUshort(), 3);
            if (bufferOnWorking[0] > 1 || bufferOnWorking[2] > 1)
            {
                logger.LogDebug($"Buffer正在给钻机上下料：上料 {bufferOnWorking[0] > 1} 下料 {bufferOnWorking[2] > 1}");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"Buffer正在给钻机上下料：上料 {bufferOnWorking[0] > 1} 下料 {bufferOnWorking[2] > 1}->84->{DeviceDescriptor.DeviceId}", request.Params);
            }

            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 0);
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvUnLoadingAndLoading"].ToUshort(), 1);
            logger.LogDebug($"Buffer下熟料自动退出方便人员下板开关 {bufferAutoUnloadOutOnOff}");
            if (bufferAutoUnloadOutOnOff)
            {
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAutoUnloadOutOnPlc"].ToUshort(), 0);
                await Task.Delay(10);
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAutoUnloadOutOnPlc"].ToUshort(), 1);
                await Task.Delay(10);
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAutoUnloadOutOnPlc"].ToUshort(), 0);
            }

            var ready = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferReadyFlag"].ToUshort(), 1);
            var readyFlag = ready[0] == 1;
            var isPower = work[0] == 1;
            var isNoError = work[2] == 0;
            var isNoFalt = work[3] == 0;

            var result = readyFlag && isPower && model && isNoError && isNoFalt;
            return await InteractingDevice.Response(result ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL,
                              result ? string.Empty : $"机器尚未准备好:buffer准备状态 {readyFlag},上电状态：{isPower}, 自动模式:{model} 非报警状态:{isNoFalt} 非急停状态:{isNoFalt}");
        }

        public async Task<DeviceServiceInvokeResponse> CanExecuteDeviceServiceInvokeLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest,
                                                                                 DefaultDrill InteractingDevice,
                                                                                 string eventId = "",
                                                                                 string eventName = "",
                                                                                 string eventMessage = "",
                                                                                 string methodName = "")
        {
            if (this.InteractingDevice.modbusIpMaster == null)
            {
                eventId = Events.Drill.BUFFER_UNCONNECT_EVENT;
                eventName = Events.Drill.BUFFER_UNCONNECT_EVENT_NAME;
                eventMessage = ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE;
                return await this.InteractingDevice.Response(ErrorCodes.Sys.PLC_UNCONNECT_CODE, ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE);
            }

            return await this.InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }
    }
}
