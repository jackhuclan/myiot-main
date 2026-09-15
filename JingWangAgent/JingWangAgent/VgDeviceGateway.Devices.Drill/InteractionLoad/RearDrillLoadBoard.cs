// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Drill.InteractionLoad
{
    public class RearDrillLoadBoard : DeviceShare<DefaultDrill>, IDrillLoadInteraction
    {
        private readonly ILogger<RearDrillLoadBoard> logger;
        private readonly byte slaveID;
        private readonly bool codeReaderOnOff;
        public RearDrillLoadBoard(ILogger<RearDrillLoadBoard> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
            slaveID = (byte)device.DeviceDescriptor.Extra["SlaveID"].ToInt();
            codeReaderOnOff = device.DeviceDescriptor.Extra["CodeReaderOnOff"].ToBool();
        }

        public async Task<DeviceServiceInvokeResponse> CompleteLoadMaterialLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, int position)
        {
            if (deviceServiceInvokeRequest.Params != null && deviceServiceInvokeRequest.Params.ContainsKey("IsLastStep") && deviceServiceInvokeRequest.Params["IsLastStep"].ToBool())
            {
                InteractingDevice.allowAllAgv = true;
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvUnLoadingAndLoading"].ToUshort(), 0);
                logger.LogDebug($"CompleteLoadMaterial 机器【{InteractingDevice.DeviceDescriptor.DeviceName}】  单个agv 上下料动作结束");
                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS);
            }
            else
            {
                var isLoadAndUnload = 0;
                //不包含IsLoadAndUnload 表示不优化ct
                if (deviceServiceInvokeRequest.Params != null && deviceServiceInvokeRequest.Params.ContainsKey("IsLoadAndUnload"))
                {
                    isLoadAndUnload = deviceServiceInvokeRequest.Params["IsLoadAndUnload"].ToUshort();
                    logger.LogDebug($"Buffer正在给钻机上下料：轴{position} 是否是既上生料又下熟料 {isLoadAndUnload == 1}  原始数据{isLoadAndUnload}");
                }
                logger.LogDebug($"是否执行优化ct  84 isLoadAndUnload ={isLoadAndUnload}");
                if (isLoadAndUnload == 1)
                {
                    logger.LogDebug($"进入优化ct  84分支");
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 0);
                    //判断该轴有无板子
                    var raw = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["ExistBoardOnPlc"].ToUshort(), 1);
                    if ((raw[0] & 1 << position - 1) == 0)
                    {
                        logger.LogDebug($"ct 84 轴{position} 未收到生料");
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"ct  84 轴{position} 未收到生料84->{DeviceDescriptor.DeviceId}", deviceServiceInvokeRequest.Params);
                    }
                    var ushorts = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferReceiveBoardFlag"].ToUshort(), 1);
                    logger.LogDebug($"ct 84  --{InteractingDevice.DeviceId} 上料动作未完成 寄存器的值 {ushorts[0]} ");
                    if (ushorts[0] == 0)
                    {
                        logger.LogDebug("ct CompleteLoadMaterial 上料动作未完成");
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"ct 84  CompleteLoadMaterial 上料动作未完成84->{DeviceDescriptor.DeviceId}");
                    }

                    if (deviceServiceInvokeRequest.Params != null && !deviceServiceInvokeRequest.Params.ContainsKey("LoadingPanel"))
                    {
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, "ct 84 上料完成中未包含LoadingPanel参数");
                    }
                    logger.LogDebug($"ct 84 上生料获取的LoadingPanel的信息：{JsonSerializer.Serialize(deviceServiceInvokeRequest.Params["LoadingPanel"])}");
                    var operationEntity = JsonSerializer.Deserialize<SwapPanel>(JsonSerializer.Serialize(deviceServiceInvokeRequest.Params["LoadingPanel"])
                      , new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    logger.LogWarning($"ct 84 CompleteLoadMaterial 上料完成未修改 【 PayloadPanels 】 {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)}");
                    logger.LogWarning($"ct 84 上生料获取的板材信息：{JsonSerializer.Serialize(operationEntity)}");
                    if (operationEntity != null && operationEntity.PanelList.Count == 1)
                    {
                        var panel = operationEntity.PanelList[0];
                        panel.Layer = 0;
                        panel.Position = position;
                        panel.ProductStatus = ProductStatus.WaitingForDrill;
                        panel.LocationCode = InteractingDevice.DeviceId;
                        panel.SiloCode = InteractingDevice.DeviceId;
                        try
                        {
                            panel.TaskCode = deviceServiceInvokeRequest.Params.ContainsKey(ScheduleConstants.PARAMS_TASK_ID) ? deviceServiceInvokeRequest.Params[ScheduleConstants.PARAMS_TASK_ID].ToStr() : "";

                        }
                        catch (Exception ee)
                        {
                            logger.LogWarning($"ct 84   panel.TaskCode {ee.Message}");

                        }


                        await PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                        {
                            InteractingDevice.PayloadPanels[position - 1] = panel;
                        }));
                    }
                    else
                    {
                        logger.LogWarning($"ct 84 上生料获取的板材信息为null 或者是多个");
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"ct 84 上生料获取的板材信息为null 或者是多个当前板材信息：{JsonSerializer.Serialize(InteractingDevice.PayloadPanels)}");
                    }

                    logger.LogWarning($"ct 84 CompleteLoadMaterial 上料完成后 【 PayloadPanels 】 {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)}");
                    return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, JsonSerializer.Serialize(InteractingDevice.PayloadPanels));
                }
                else
                {
                    logger.LogDebug($"进入非优化ct  84分支");
                    var raw = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["ExistBoardOnPlc"].ToUshort(), 1);
                    if ((raw[0] & 1 << position - 1) == 0)
                    {
                        logger.LogDebug($"84 上料完成 轴{position} 未收到生料");
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"上料完成 轴{position} 未收到生料84->{DeviceDescriptor.DeviceId}", deviceServiceInvokeRequest.Params);
                    }
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 0);

                    var ushorts = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferReceiveBoardFlag"].ToUshort(), 1);
                    logger.LogDebug($" 84 --{InteractingDevice.DeviceId} 上料动作未完成 寄存器的值 {ushorts[0]} ");
                    if (ushorts[0] == 0)
                    {
                        logger.LogDebug("84 CompleteLoadMaterial 上料动作未完成");
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, "84 CompleteLoadMaterial 上料动作未完成");
                    }

                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferStartLoadingBoard"].ToUshort(), 0);
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvArrivalEnd"].ToUshort(), 0);
                    logger.LogDebug($"84 上生料结束");

                    if (deviceServiceInvokeRequest.Params != null && !deviceServiceInvokeRequest.Params.ContainsKey("LoadingPanel"))
                    {
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, "84 上料完成中未包含LoadingPanel参数");
                    }
                    logger.LogDebug($"84 上生料获取的LoadingPanel的信息：{JsonSerializer.Serialize(deviceServiceInvokeRequest.Params["LoadingPanel"])}");
                    var operationEntity = JsonSerializer.Deserialize<SwapPanel>(JsonSerializer.Serialize(deviceServiceInvokeRequest.Params["LoadingPanel"])
                      , new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    logger.LogWarning($"84 CompleteLoadMaterial 上料完成未修改 【 PayloadPanels 】 {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)}");
                    logger.LogWarning($"84 上生料获取的板材信息：{JsonSerializer.Serialize(operationEntity)}");
                    if (operationEntity != null && operationEntity.PanelList.Count == 1)
                    {
                        var panel = operationEntity.PanelList[0];
                        panel.Layer = 0;
                        panel.Position = position;
                        panel.ProductStatus = ProductStatus.WaitingForDrill;
                        panel.LocationCode = InteractingDevice.DeviceId;
                        panel.SiloCode = InteractingDevice.DeviceId;

                        try
                        {
                            panel.TaskCode = deviceServiceInvokeRequest.Params.ContainsKey(ScheduleConstants.PARAMS_TASK_ID) ? deviceServiceInvokeRequest.Params[ScheduleConstants.PARAMS_TASK_ID].ToStr() : "";

                        }
                        catch (Exception ee)
                        {
                            logger.LogWarning($"ct 84   panel.TaskCode {ee.Message}");

                        }

                        await PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                        {
                            InteractingDevice.PayloadPanels[position - 1] = panel;
                        }));
                    }
                    else
                    {
                        logger.LogWarning($"84 上生料获取的板材信息为null 或者是多个");
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"84 上生料获取的板材信息为null 或者是多个当前板材信息：{JsonSerializer.Serialize(InteractingDevice.PayloadPanels)}");
                    }

                    logger.LogWarning($"84 CompleteLoadMaterial 上料完成后 【 PayloadPanels 】 {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)}");
                    return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, JsonSerializer.Serialize(InteractingDevice.PayloadPanels));
                }
            }
        }

        public async Task<DeviceServiceInvokeResponse> InvokeLoadMaterialLocal(int position)
        {
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 0);
            SetPosition(position.ToUshort());
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferStartLoadingBoard"].ToUshort(), 1);
            logger.LogDebug($"84 轴{position}开始上料");
            try
            {
                if (codeReaderOnOff)
                {
                    Task.Factory.StartNew((index) =>
                    {

                        int codeIndex = (int)index;
                        InteractingDevice.codeReaderBaseSetting.SendScanMesssageToSignal(codeIndex, "1", true);
                        logger.LogInformation($"轴{codeIndex + 1} 自动触发读码 线程开始");
                        Task.Delay(2000).Wait();
                        try
                        {
                            do
                            {
                                ushort curTrigger = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, (ushort)(InteractingDevice.DeviceDescriptor.Extra["BufferStartLoadingBoard"].ToUshort()), 1)[0];
                                var codeReaderNum = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra[$"Spline{codeIndex + 1}CodeReaderCodeNum"].ToUshort(), 1)[0];
                                if (curTrigger == 0 || codeReaderNum != 0)
                                {
                                    break;
                                }
                                InteractingDevice.codeReaderBaseSetting.SendScanMesssageToSignal(codeIndex, "1");
                                Task.Delay(3000).Wait();

                            } while (true);
                        }
                        catch (Exception ee)
                        {

                            logger.LogInformation($"84 触发读码 停止异常{ee.Message}");
                        }

                        logger.LogInformation($"84 轴{codeIndex + 1}  自动触发读码 线程结束");
                    }, position - 1);

                }
            }
            catch (Exception ee)
            {

                logger.LogInformation($"84 触发读码异常{ee.Message}");
            }

            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }

        public async Task<DeviceServiceInvokeResponse> PrepareLoadMaterialLocal(DeviceServiceInvokeRequest request, int position)
        {
            var bufferOnAgvPosition = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferOnAgvPositionFlag"].ToUshort(), 1);
            if (!bufferOnAgvPosition[0])
            {
                logger.LogDebug($"Buffer不在agv对接层不能执行上料");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"Buffer不在agv对接层不能执行上料84->{DeviceDescriptor.DeviceId}", request.Params);
            }

            var work = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferStatusFlag"].ToUshort(), 4);
            var model = work[1] == 1;
            if (!model)
            {
                logger.LogDebug($"手动状态下agv不能给Buffer上料");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"手动状态下agv不能给Buffer上料84->{DeviceDescriptor.DeviceId}", request.Params);
            }

            var raw = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["ExistBoardOnPlc"].ToUshort(), 1);
            if ((raw[0] & 1 << position - 1) != 0)
            {
                request.Params["DeviceIsException"] = true;
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["WarningInfoWriteOnPlc"].ToUshort(), (ushort)(90 + position));
                logger.LogDebug($"轴{position} 有生料不能上料");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"轴{position} 有生料不能上料84->{DeviceDescriptor.DeviceId}", request.Params);
            }

            var bufferOnWorking = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferOnWorking"].ToUshort(), 3);
            if (bufferOnWorking[0] > 1 || bufferOnWorking[2] > 1)
            {
                logger.LogDebug($"Buffer正在给钻机上下料：上料 {bufferOnWorking[0] > 1} 下料 {bufferOnWorking[2] > 1}");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"Buffer正在给钻机上下料：上料 {bufferOnWorking[0] > 1} 下料 {bufferOnWorking[2] > 1}->84->{DeviceDescriptor.DeviceId}", request.Params);
            }

            //不包含IsLoadAndUnload 表示不优化ct
            if (request.Params == null || !request.Params.ContainsKey("IsLoadAndUnload"))
            {
                logger.LogDebug($"Buffer正在给钻机上下料：轴{position} 是否是既上生料又下熟料 {JsonSerializer.Serialize(request)} 未包含 IsLoadAndUnload 参数");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferIsLoadAndUnloadOnPlc"].ToUshort(), 0);
            }
            else
            {
                var isLoadAndUnload = request.Params["IsLoadAndUnload"].ToUshort();
                logger.LogDebug($"Buffer正在给钻机上下料：轴{position} 是否是既上生料又下熟料 {isLoadAndUnload == 1}  原始数据{isLoadAndUnload}");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferIsLoadAndUnloadOnPlc"].ToUshort(), isLoadAndUnload);
            }

            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 0);
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvUnLoadingAndLoading"].ToUshort(), 1);//  508

            var bufferOnWorkingSingleSplindle = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferOnWorkingSingleSplindle"].ToUshort(), 3);
            logger.LogDebug($"agv正在给buffer 单个轴上下料：上料 {bufferOnWorkingSingleSplindle[0] > 1} 下料 {bufferOnWorkingSingleSplindle[2] > 1}");
            if (bufferOnWorkingSingleSplindle[0] <= 1 && bufferOnWorkingSingleSplindle[2] <= 1)
            {
                logger.LogDebug($"不在单个轴 上下过程中  将要赋值509---506---507");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvArrivalEnd"].ToUshort(), 1);//  509
                await Task.Delay(InteractingDevice.DeviceDescriptor.Extra["WritePlcDelay"].ToInt());
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferUnLoadRequestOnPlc"].ToUshort(), 0);//  507
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferLoadRequestOnPlc"].ToUshort(), 0);//  506
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferLoadRequestOnPlc"].ToUshort(), 1);//  506
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferUnloadAndLoadSpline"].ToUshort(), position.ToUshort());//  514 轴号
            }
            var ready = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferReadyFlag"].ToUshort(), 1);

            var bufferPosition = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvPositionEnd"].ToUshort(), 1);

            var readyFlag = ready[0] == 1;
            var isPower = work[0] == 1;

            var isNoError = work[2] == 0;
            var isNoFalt = work[3] == 0;
            var hasNoBoard = (raw[0] & 1 << position - 1) == 0;
            var isPosition = bufferPosition[0] == position;
            var result = readyFlag && isPower && model && isNoError && isNoFalt && isPosition;
            return await InteractingDevice.Response(result ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL,
                              result ? string.Empty : $"机器尚未准备好:buffer准备状态 {readyFlag},上电状态：{isPower}, 自动模式:{model} 非报警状态:{isNoFalt} 非急停状态:{isNoFalt},buffer生料层 轴{position}无板子状态：{hasNoBoard} buffer是否定位完成{isPosition}->84->{DeviceDescriptor.DeviceId}");
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
                eventMessage = $"{methodName} 传入的参数不包含位置信息";
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"{methodName} 传入的参数不包含位置信息84->{DeviceDescriptor.DeviceId}");
            }

            ushort position = deviceServiceInvokeRequest.Params["Position"].ToUshort();
            if (!(position > 0 && position <= InteractingDevice.spindleNum))
            {
                eventId = Events.Drill.DRILL_PARAMETER_POSITION_FALT_EVENT;
                eventName = Events.Drill.DRILL_PARAMETER_POSITION_FALT_EVENT_NAME;
                eventMessage = $"{methodName} 下发Position参数值{position}不在1和{InteractingDevice.spindleNum}之间，请下发正确的上料轴信息84->{DeviceDescriptor.DeviceId}";
                logger.LogError(eventMessage);
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, eventMessage);
            }

            return await this.InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }

        private void SetPosition(ushort position)
        {
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferReceiveBoardFlag"].ToUshort(), 0);
            InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferStartLoadingBoard"].ToUshort(), new ushort[] { 0x00, 0x00, 0x00, 0x00, 0x00 });
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferUnloadAndLoadSpline"].ToUshort(), position);
        }
    }
}
