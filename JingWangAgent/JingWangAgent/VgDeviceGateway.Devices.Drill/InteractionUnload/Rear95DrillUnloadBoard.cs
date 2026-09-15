// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

/********************************************************************************************
 *                         Suzhou Vega Technology Co., Ltd                                  *
 ********************************************************************************************
 * Copyright    : Copyright(c) Suzhou Vega Technology Co., Ltd                              *
 *                All rights reserved.                                                      *
 *                                                                                          *
 * DateTime       Author          Comment                                                   *
 * 2024.05.01    Li Haiyan        New                                                       *
 * 2024.06.01    Zhu Shipeng      Re-implementation of CNC84                                *
 ********************************************************************************************/

using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Drill.InteractionUnload
{
    public class Rear95DrillUnloadBoard : DeviceShare<DefaultDrill>, IDrillUnloadInteraction
    {
        private readonly ILogger<Rear95DrillUnloadBoard> logger;
        public readonly byte slaveID;

        public Rear95DrillUnloadBoard(ILogger<Rear95DrillUnloadBoard> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
            slaveID = (byte)device.DeviceDescriptor.Extra["SlaveID"].ToInt();
        }

        public async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterialLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, int position)
        {
            if (deviceServiceInvokeRequest.Params != null && deviceServiceInvokeRequest.Params.ContainsKey("IsLastStep") && deviceServiceInvokeRequest.Params["IsLastStep"].ToBool())
            {
                if (InteractingDevice.isCallAgvUnload)
                {
                    logger.LogDebug($"CompleteUnloadMaterial 最后一步   只下熟料 ");
                    InteractingDevice.allowAllAgv = true;
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvUnLoadingAndLoading"].ToUshort(), 0);
                    logger.LogDebug($"CompleteUnloadMaterial 机器【{InteractingDevice.DeviceDescriptor.DeviceName}】 单个agv 只下料动作结束");
                    return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, $" 单个agv 只下料动作结束95->{DeviceDescriptor.DeviceId}", deviceServiceInvokeRequest.Params);
                }
                else
                {
                    InteractingDevice.allowAllAgv = true;
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvUnLoadingAndLoading"].ToUshort(), 0);
                    logger.LogDebug($"CompleteUnloadMaterial 机器【{InteractingDevice.DeviceDescriptor.DeviceName}】 单个agv个上下料动作结束");
                    return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS);
                }
            }
            else
            {
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 0);
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferReceiveClinkerOnPlc"].ToUshort(), 1);//  512
                var isfinished = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferUnloadOKOnPlc"].ToUshort(), 1);// 409
                if (isfinished[0] != 1)
                {
                    return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"buffer下料未完成95->{DeviceDescriptor.DeviceId}", deviceServiceInvokeRequest.Params);
                }

                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferIsLoadAndUnloadOnPlc"].ToUshort(), 0);
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferUnloadOKOnPlc"].ToUshort(), 0);// 409 BufferUnloadOKOnPlc
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvArrivalEnd"].ToUshort(), 0);
                logger.LogDebug("CompleteUnloadMaterial 下料完成");
                logger.LogDebug($"CompleteUnloadMaterial 下料完成未修改的 PayloadPanels {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)}");
                var panelInfo = InteractingDevice.PayloadPanels[position - 1 + 2 * InteractingDevice.spindleNum];
                panelInfo.ProductStatus = ProductStatus.Finished_DRILL;
                if (panelInfo.IsNull() && string.IsNullOrWhiteSpace(panelInfo!.PanelCode))
                {
                    panelInfo!.PanelCode = "mockinf" + Guid.NewGuid();
                }

                deviceServiceInvokeRequest!.Params["UnloadingPanel"] = JsonSerializer.Serialize(new SwapPanel() { SpindleId = position, PanelList = new List<Panel>() { panelInfo } });

                logger.LogDebug($"CompleteUnloadMaterial 下料完成 UnloadingPanel 赋值：{JsonSerializer.Serialize(deviceServiceInvokeRequest!.Params["UnloadingPanel"])}");
                await PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                {
                    var panel = Panel.HasSilo.NoPanelForSingleSpindle("", position, 2, 1)[0];
                    panel.LocationCode = InteractingDevice.DeviceId;
                    panel.SiloCode = InteractingDevice.DeviceId;
                    InteractingDevice.PayloadPanels[position - 1 + 2 * InteractingDevice.spindleNum] = panel;
                }));

                logger.LogDebug($"CompleteUnloadMaterial 下料完成后 PayloadPanels {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)}");
                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, JsonSerializer.Serialize(InteractingDevice.PayloadPanels), deviceServiceInvokeRequest.Params);
            }
        }

        public async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterialLocal(int position)
        {
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferIsLoadAndUnloadOnPlc"].ToUshort(), 0);
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 0);
            SetPosition(position.ToUshort());
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferStartUnLoadingBoard"].ToUshort(), 1);
            logger.LogDebug($"轴{position}开始下料");
            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }

        private void SetPosition(ushort position)
        {
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferReceiveBoardFlag"].ToUshort(), 0);
            InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferStartLoadingBoard"].ToUshort(), new ushort[] { 0x00, 0x00, 0x00, 0x00, 0x00 });
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferUnloadAndLoadSpline"].ToUshort(), position);
        }

        public async Task<DeviceServiceInvokeResponse> PrepareUnloadMaterialLocal(DeviceServiceInvokeRequest request, int position)
        {
            var bufferOnAgvPosition = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferOnAgvPositionFlag"].ToUshort(), 1);
            if (!bufferOnAgvPosition[0])
            {
                logger.LogDebug($"Buffer不在agv对接层不能执行下料");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"Buffer不在agv对接层不能执行下料95->{DeviceDescriptor.DeviceId}", request.Params);
            }
            var work = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferStatusFlag"].ToUshort(), 4);
            var model = work[1] == 1;
            //logger.LogWarning($"手动状态下agv不能给Buffer上料CNC95->{DeviceDescriptor.DeviceId}->{work[1]}->{model}");
            if (!model)
            {
                logger.LogDebug($"手动状态下agv不能给Buffer下料");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"手动状态下agv不能给Buffer下料95->{DeviceDescriptor.DeviceId}", request.Params);
            }
            var clinker = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferClinkerStatusOnPlc"].ToUshort(), 1); //145
            if ((clinker[0] & 1 << position - 1) == 0)
            {
                var errorMessage = $"轴{position}无熟料不能下料95->{DeviceDescriptor.DeviceId}";
                logger.LogError(errorMessage);
                request.Params["DeviceIsException"] = true;
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["WarningInfoWriteOnPlc"].ToUshort(), (ushort)(100 + position));
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, errorMessage, request.Params);
            }
            var bufferOnWorking = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferOnWorking"].ToUshort(), 3);
            if (bufferOnWorking[0] > 1 || bufferOnWorking[2] > 1)
            {
                logger.LogDebug($"Buffer正在给钻机上下料：上料 {bufferOnWorking[0] > 1} 下料 {bufferOnWorking[2] > 1}");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"Buffer正在给钻机上下料：上料 {bufferOnWorking[0] > 1} 下料 {bufferOnWorking[2] > 1}->95->{DeviceDescriptor.DeviceId}", request.Params);
            }

            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferIsLoadAndUnloadOnPlc"].ToUshort(), 0);
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 0);
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvUnLoadingAndLoading"].ToUshort(), 1);

            var bufferOnWorkingSingleSplindle = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferOnWorkingSingleSplindle"].ToUshort(), 3);
            logger.LogDebug($"agv正在给buffer 单个轴上下料：上料 {bufferOnWorkingSingleSplindle[0] > 1} 下料 {bufferOnWorkingSingleSplindle[2] > 1}");
            if (bufferOnWorkingSingleSplindle[0] <= 1 && bufferOnWorkingSingleSplindle[2] <= 1)
            {
                logger.LogDebug($"不在单个轴 上下过程中  将要赋值509---506---507");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvArrivalEnd"].ToUshort(), 1);
                await Task.Delay(InteractingDevice.DeviceDescriptor.Extra["WritePlcDelay"].ToInt());
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferLoadRequestOnPlc"].ToUshort(), 0); // 506
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferUnLoadRequestOnPlc"].ToUshort(), 0); // 507
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferUnLoadRequestOnPlc"].ToUshort(), 1); // 507
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferUnloadAndLoadSpline"].ToUshort(), position.ToUshort());// 514 轴号
            }

            var bufferPosition = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvPositionEnd"].ToUshort(), 1);
            var ready = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferReadyFlag"].ToUshort(), 1);
            var readyFlag = ready[0] == 1;
            var isPower = work[0] == 1;
            var isNoError = work[2] == 0;
            var isNoFalt = work[3] == 0;
            var hasBoard = (clinker[0] & 1 << position - 1) == 1 << position - 1;
            var isPosition = bufferPosition[0] == 10 + position;
            var result = readyFlag && isPower && model && isNoError && isNoFalt && isPosition;
            return await InteractingDevice.Response(result ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL,
                              result ? string.Empty : $"机器尚未准备好:buffer准备状态 {readyFlag},上电状态：{isPower}, 自动模式:{model} 非报警状态:{isNoFalt} 非急停状态:{isNoFalt},buffer熟料层 轴：{position}有板子状态：{hasBoard}  buffer是否定位完成{isPosition}->95->{DeviceDescriptor.DeviceId}");
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

            if (deviceServiceInvokeRequest.Params == null || !deviceServiceInvokeRequest.Params.ContainsKey("Position"))
            {
                eventId = Events.Drill.DRILL_PARAMETER_NOT_CONTAIN_POSITION_EVENT;
                eventName = Events.Drill.DRILL_PARAMETER_NOT_CONTAIN_POSITION_EVENT_NAME;
                eventMessage = $"{methodName} 传入的参数不包含位置信息";
                return await this.InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"{methodName} 传入的参数不包含位置信息95->{DeviceDescriptor.DeviceId}");
            }

            ushort position = deviceServiceInvokeRequest.Params["Position"].ToUshort();
            if (!(position > 0 && position <= this.InteractingDevice.spindleNum))
            {
                eventId = Events.Drill.DRILL_PARAMETER_POSITION_FALT_EVENT;
                eventName = Events.Drill.DRILL_PARAMETER_POSITION_FALT_EVENT_NAME;
                eventMessage = $"{methodName} 下发Position参数值{position}不在1和{this.InteractingDevice.spindleNum}之间，请下发正确的上料轴信息95->{DeviceDescriptor.DeviceId}";
                logger.LogError(eventMessage);
                return await this.InteractingDevice.Response(ErrorCodes.Sys.FAIL, eventMessage);
            }
            return await this.InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }
    }
}
