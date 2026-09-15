using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Drill.InteractionLoad
{
    public class FrontDrillLoadTool : DeviceShare<DefaultDrill>, IDrillLoadInteraction
    {
        private readonly ILogger<FrontDrillLoadTool> logger;
        private readonly byte slaveID;

        public FrontDrillLoadTool(ILogger<FrontDrillLoadTool> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
            slaveID = (byte)device.DeviceDescriptor.Extra["SlaveID"].ToInt();
        }

        public async Task<DeviceServiceInvokeResponse> CompleteLoadMaterialLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, int position)
        {
            //是否最后一个 最后一步置上整个Agv给插齿整个上下刀盘结束
            if (deviceServiceInvokeRequest.Params != null && deviceServiceInvokeRequest.Params.ContainsKey("IsLastStep") && deviceServiceInvokeRequest.Params["IsLastStep"].ToBool())
            {
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["ToolBufferAgvLoadAndUnLoadTray"].ToUshort(), 0);
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["ToolBufferAgvAllUnloadAndLoadEndOnPlc"].ToUshort(), 1);
                logger.LogDebug($"CompleteLoadMaterial ToolBuffer 机器【{InteractingDevice.DeviceDescriptor.DeviceName}】  整个上下刀盘动作结束");
            }
            else
            {
                var positionDrillRegionStatus = GetDeviceToolPositionStatus("ToolBufferStatusOnPlc", position);
                ushort region = deviceServiceInvokeRequest.Params["Region"].ToUshort();
                if (positionDrillRegionStatus[region - 1] != '1')
                {
                    logger.LogDebug($"ToolBuffer  轴{position} --- {region} 没有刀盘 上刀盘动作未完成");
                    return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"ToolBuffer  轴{position} --- {region} 没有刀盘 上刀盘动作未完成", deviceServiceInvokeRequest.Params);
                }
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["ToolBufferAgvLoadAndUnLoadTray"].ToUshort(), 1);
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["ToolBufferAgvAllUnloadAndLoadEndOnPlc"].ToUshort(), 0);

                UpdateDeviceToolPositionRegionStatus("ToolBufferNewOldOnPlc", position, region, false);

                if (deviceServiceInvokeRequest.Params != null && !deviceServiceInvokeRequest.Params.ContainsKey("LoadingTray"))
                {
                    return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, "上新刀盘完成中未包含LoadingTray参数");
                }
                logger.LogDebug($"上新刀盘获取的LoadingTray的信息：{JsonSerializer.Serialize(deviceServiceInvokeRequest.Params["LoadingTray"])}");
                var operationEntity = JsonSerializer.Deserialize<SwapTray>(JsonSerializer.Serialize(deviceServiceInvokeRequest.Params["LoadingTray"])
                  , new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                logger.LogDebug($"CompleteLoadMaterial 上新刀盘完成未修改 InteractingDevice.PayloadCutterTrays {JsonSerializer.Serialize(InteractingDevice.PayloadCutterTrays)}");
                logger.LogDebug($"上生刀盘获取的新刀盘信息：{JsonSerializer.Serialize(operationEntity)}");
                if (operationEntity != null && operationEntity.TrayList.Count == 1)
                {
                    var tray = operationEntity.TrayList[0];
                    int bufferIndex = InteractingDevice.spindleNum * InteractingDevice.region + (position - 1) * InteractingDevice.region + region - 1;
                    InteractingDevice.PayloadCutterTrays[bufferIndex].SiloCode = tray.SiloCode;
                    InteractingDevice.PayloadCutterTrays[bufferIndex].ItemCode = tray.ItemCode;
                    InteractingDevice.PayloadCutterTrays[bufferIndex].Status = CutterTrayStatus.New;
                    InteractingDevice.PayloadCutterTrays[bufferIndex].TrayCode = tray.TrayCode;
                }
                else
                {
                    logger.LogDebug($"上新刀盘获取的新刀信息为null 或者是多个");
                    return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"上新刀获取的刀盘信息为null 或者是多个当前新刀信息：{JsonSerializer.Serialize(InteractingDevice.PayloadCutterTrays)}");
                }
                logger.LogDebug($"CompleteLoadMaterial 上新刀盘完成后 InteractingDevice.PayloadCutterTrays {JsonSerializer.Serialize(InteractingDevice.PayloadCutterTrays)}");

                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, JsonSerializer.Serialize(InteractingDevice.PayloadCutterTrays));
            }

            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }

        public async Task<DeviceServiceInvokeResponse> InvokeLoadMaterialLocal(int position)
        {
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["ToolBufferAgvLoadAndUnLoadTray"].ToUshort(), 1);
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["ToolBufferAgvAllUnloadAndLoadEndOnPlc"].ToUshort(), 0);

            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }

        public async Task<DeviceServiceInvokeResponse> PrepareLoadMaterialLocal(DeviceServiceInvokeRequest request, int position)
        {
            var drillUnLoadingAndLoading = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["ToolBufferDrillUnLoadingAndLoadingOnPlc"].ToUshort(), 1);
            if (drillUnLoadingAndLoading[0] != 0)
            {
                logger.LogDebug($"ToolBuffer 插齿正在和钻机进行上下刀盘，Agv不能执行上刀盘");
                request.Params["DeviceIsException"] = true;
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"ToolBuffer 插齿正在和钻机进行上下刀盘，Agv不能执行上刀盘", request.Params);
            }

            var positionRegionStatus = GetDeviceToolPositionStatus("ToolBufferStatusOnPlc", position);
            ushort region = request.Params["Region"].ToUshort();
            if (positionRegionStatus[region - 1] == '1')
            {
                request.Params["DeviceIsException"] = true;
                logger.LogDebug($"ToolBuffer 插齿 轴{position} --- {region} 有刀盘不能上刀盘");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"插齿 轴{position} --- {region} 有刀盘 不能上刀盘", request.Params);
            }

            var positionDrillRegionStatus = GetDeviceToolPositionStatus("ToolBufferDrillStatusOnPlc", position);
            if (positionDrillRegionStatus[region - 1] == '1')
            {
                request.Params["DeviceIsException"] = true;
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["WarningInfoWriteOnPlc"].ToUshort(), (ushort)(90 + position));
                logger.LogDebug($"ToolBuffer 钻机 轴{position} --- {region} 有刀盘不能上刀盘");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"ToolBuffer 钻机  轴{position} --- {region} 有刀盘不能上刀盘", request.Params);
            }

            if (positionRegionStatus.Count(s => s == '1') >= 2)
            {
                request.Params["DeviceIsException"] = true;
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["WarningInfoWriteOnPlc"].ToUshort(), (ushort)(90 + position));
                logger.LogDebug($"ToolBuffer 插齿 轴{position}  {positionRegionStatus}上干涉不能上刀盘");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"ToolBuffer 插齿 轴{position}  {positionRegionStatus}上干涉不能上刀盘", request.Params);
            }
            else
            {
                var index = positionRegionStatus.IndexOf("1");
                if (index == 1 || (region == 2 && (index == 2 || index == 0)))
                {
                    request.Params["DeviceIsException"] = true;
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["WarningInfoWriteOnPlc"].ToUshort(), (ushort)(90 + position));
                    logger.LogDebug($"ToolBuffer 插齿 轴{position} --- {region}  {positionRegionStatus} 干涉不能上刀盘");
                    return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"ToolBuffer 插齿 轴{position} --- {region}  {positionRegionStatus} 干涉不能上刀盘", request.Params);
                }
            }

            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["ToolBufferAgvLoadAndUnLoadTray"].ToUshort(), 1);
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["ToolBufferAgvAllUnloadAndLoadEndOnPlc"].ToUshort(), 0);

            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }

        public async Task<DeviceServiceInvokeResponse> CanExecuteDeviceServiceInvokeLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest,
                                                                                 DefaultDrill InteractingDevice,
                                                                                 string eventId = "",
                                                                                 string eventName = "",
                                                                                 string eventMessage = "",
                                                                                 string methodName = "")
        {
            if (InteractingDevice.modbusIpMaster == null)
            {
                eventId = Events.Drill.BUFFER_UNCONNECT_EVENT;
                eventName = Events.Drill.BUFFER_UNCONNECT_EVENT_NAME;
                eventMessage = ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE;
                return await InteractingDevice.Response(ErrorCodes.Sys.PLC_UNCONNECT_CODE, ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE);
            }

            if (deviceServiceInvokeRequest.Params == null || !deviceServiceInvokeRequest.Params.ContainsKey("Position"))
            {
                eventId = Events.Drill.DRILL_PARAMETER_NOT_CONTAIN_POSITION_EVENT;
                eventName = Events.Drill.DRILL_PARAMETER_NOT_CONTAIN_POSITION_EVENT_NAME;
                eventMessage = $"ToolBuffer {methodName} 传入的参数不包含位置信息";
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"ToolBuffer {methodName} 传入的参数不包含上刀盘 位置信息");
            }
            ushort position = deviceServiceInvokeRequest.Params["Position"].ToUshort();
            if (!(position > 0 && position <= InteractingDevice.spindleNum))
            {
                eventId = Events.Drill.DRILL_PARAMETER_POSITION_FALT_EVENT;
                eventName = Events.Drill.DRILL_PARAMETER_POSITION_FALT_EVENT_NAME;
                eventMessage = $"ToolBuffer {methodName} 下发Position参数值{position}不在1和{InteractingDevice.spindleNum}之间，请下发正确的上刀盘 轴信息";
                logger.LogError(eventMessage);
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, eventMessage);
            }
            if (deviceServiceInvokeRequest.Params == null || !deviceServiceInvokeRequest.Params.ContainsKey("Region"))
            {
                eventId = Events.Drill.DRILL_PARAMETER_NOT_CONTAIN_POSITION_EVENT;
                eventName = Events.Drill.DRILL_PARAMETER_NOT_CONTAIN_POSITION_EVENT_NAME;
                eventMessage = $"ToolBuffer {methodName} 传入的参数不包含区域信息";
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"ToolBuffer {methodName} 传入的参数不包含上刀盘 区域信息");
            }

            ushort region = deviceServiceInvokeRequest.Params["Region"].ToUshort();
            int maxRegion = InteractingDevice.DeviceDescriptor.Extra["Region"].ToUshort();
            if (!(region > 0 && region <= maxRegion))
            {
                eventId = Events.Drill.DRILL_PARAMETER_POSITION_FALT_EVENT;
                eventName = Events.Drill.DRILL_PARAMETER_POSITION_FALT_EVENT_NAME;
                eventMessage = $"ToolBuffer{methodName} 下发Region参数值{region}不在1和{maxRegion}之间，请下发正确的上刀盘 轴信息";
                logger.LogError(eventMessage);
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, eventMessage);
            }

            return await this.InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }

        private string GetDeviceToolPositionStatus(string plcPosition, int position)
        {
            int regionSize = InteractingDevice.DeviceDescriptor.Extra["Region"].ToUshort();
            var frontSize = InteractingDevice.spindleNum / 2;
            var backSize = InteractingDevice.spindleNum / 2;
            if (InteractingDevice.spindleNum % 2 != 0)
            {
                frontSize += 1;
            }

            var toolBuffer = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra[plcPosition].ToUshort(), 2);
            var front3 = string.Join("", Convert.ToString(toolBuffer[0], 2).PadLeft(16, '0').Reverse().ToArray()).ToString().Substring(0, frontSize * regionSize);
            var back3 = string.Join("", Convert.ToString(toolBuffer[1], 2).PadLeft(16, '0').Reverse().ToArray()).ToString().Substring(0, backSize * regionSize);
            var allToolBuffer = string.Concat(front3, back3);

            return allToolBuffer.Substring((position - 1) * regionSize, regionSize);
        }

        private void UpdateDeviceToolPositionRegionStatus(string plcPosition, int position, int region, bool setFlag = true)
        {
            int regionSize = InteractingDevice.DeviceDescriptor.Extra["Region"].ToUshort();
            var frontSize = InteractingDevice.spindleNum / 2;
            if (InteractingDevice.spindleNum % 2 != 0)
            {
                frontSize += 1;
            }

            var toolBuffer = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, DeviceDescriptor.Extra[plcPosition].ToUshort(), 2);
            if (position <= frontSize)
            {
                toolBuffer[0] = setFlag ? (ushort)(toolBuffer[0] | (ushort)(1 << ((regionSize * (position - 1)) + (region - 1)))) : (ushort)(toolBuffer[0] & ~(1 << ((regionSize * (position - 1)) + (region - 1))));
            }
            else
            {
                var tmpPosition = position - frontSize;
                toolBuffer[1] = setFlag ? (ushort)(toolBuffer[0] | (ushort)(1 << ((regionSize * (tmpPosition - 1)) + (region - 1)))) : (ushort)(toolBuffer[0] & ~(1 << ((regionSize * (tmpPosition - 1)) + (region - 1))));
            }

            InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveID, DeviceDescriptor.Extra[plcPosition].ToUshort(), toolBuffer);
        }
    }
}
