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
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Drill.InteractionLoad
{
    public class Rear95DrillLoadBoard : DeviceShare<DefaultDrill>, IDrillLoadInteraction
    {
        private readonly ILogger<Rear95DrillLoadBoard> logger;
        private readonly byte slaveID;
        private readonly bool codeReaderOnOff;
        private readonly bool boardLengthWriteToPlcOnOff;

        public Rear95DrillLoadBoard(ILogger<Rear95DrillLoadBoard> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
            slaveID = (byte)device.DeviceDescriptor.Extra["SlaveID"].ToInt();
            codeReaderOnOff = device.DeviceDescriptor.Extra["CodeReaderOnOff"].ToBool();
            boardLengthWriteToPlcOnOff = device.DeviceDescriptor.Extra["BoardLengthWriteToPlcOnOff"].ToBool();
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
                    logger.LogDebug($"Buffer正在给钻机上下料：轴{position} 是否是既上生料又下熟料-->95 {isLoadAndUnload == 1}  原始数据{isLoadAndUnload}");
                }
                logger.LogDebug($"是否执行优化ct 95  isLoadAndUnload ={isLoadAndUnload}");
                if (isLoadAndUnload == 1)
                {
                    logger.LogDebug($"进入优化ct  95分支");
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 0);
                    //判断该轴有无板子
                    var raw = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["ExistBoardOnPlc"].ToUshort(), 1);
                    if ((raw[0] & 1 << position - 1) == 0)
                    {
                        logger.LogDebug($"ct  95 上料完成 轴{position} 未收到生料");
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"ct 95 上料完成 轴{position} 未收到生料95->{DeviceDescriptor.DeviceId}", deviceServiceInvokeRequest.Params);
                    }
                    var ushorts = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferReceiveBoardFlag"].ToUshort(), 1);
                    logger.LogDebug($"ct  95  --{InteractingDevice.DeviceId} 上料动作未完成 寄存器的值 {ushorts[0]} ");
                    if (ushorts[0] == 0)
                    {
                        logger.LogDebug("ct CompleteLoadMaterial 上料动作未完成");
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"ct 95 CompleteLoadMaterial 上料动作未完成95->{DeviceDescriptor.DeviceId}");
                    }

                    if (deviceServiceInvokeRequest.Params != null && !deviceServiceInvokeRequest.Params.ContainsKey("LoadingPanel"))
                    {
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"ct 95 上料完成中未包含LoadingPanel参数95->{DeviceDescriptor.DeviceId}");
                    }
                    logger.LogDebug($"ct  95 上生料获取的LoadingPanel的信息：{JsonSerializer.Serialize(deviceServiceInvokeRequest.Params["LoadingPanel"])}");
                    var operationEntity = JsonSerializer.Deserialize<SwapPanel>(JsonSerializer.Serialize(deviceServiceInvokeRequest.Params["LoadingPanel"])
                      , new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    logger.LogDebug($"ct  95 CompleteLoadMaterial 上料完成未修改 InteractingDevice.PayloadPanels {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)}");
                    logger.LogDebug($"ct  95 上生料获取的板材信息：{JsonSerializer.Serialize(operationEntity)}");
                    if (operationEntity != null && operationEntity.PanelList.Count == 1)
                    {
                        var panel = operationEntity.PanelList[0];
                        panel.Layer = 0;
                        panel.Position = position;
                        panel.ProductStatus = ProductStatus.WaitingForDrill;
                        panel.LocationCode = InteractingDevice.DeviceId;
                        panel.SiloCode = InteractingDevice.DeviceId;
                        if (codeReaderOnOff)
                        {
                            logger.LogDebug($"优化ct 钻机代理读码功能开开 赋值本地的二维码信息");
                            panel.Barcode = InteractingDevice.PayloadPanels[position - 1].Barcode;
                        }
                        try
                        {
                            panel.TaskCode = deviceServiceInvokeRequest.Params.ContainsKey(ScheduleConstants.PARAMS_TASK_ID) ? deviceServiceInvokeRequest.Params[ScheduleConstants.PARAMS_TASK_ID].ToStr() : "";

                        }
                        catch (Exception)
                        {


                        }

                        await PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                       {
                           InteractingDevice.PayloadPanels[position - 1] = panel;
                       }));
                    }
                    else
                    {
                        logger.LogDebug($"ct 95上生料获取的板材信息为null 或者是多个");
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"ct 95 上生料获取的板材信息为null 或者是多个当前板材信息：{JsonSerializer.Serialize(InteractingDevice.PayloadPanels)}->95->{DeviceDescriptor.DeviceId}");
                    }

                    logger.LogDebug($"ct 95 CompleteLoadMaterial 上料完成后 InteractingDevice.PayloadPanels {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)}");
                    return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, JsonSerializer.Serialize(InteractingDevice.PayloadPanels));
                }
                else
                {
                    logger.LogDebug($"进入非优化ct  95分支");
                    var raw = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["ExistBoardOnPlc"].ToUshort(), 1);
                    if ((raw[0] & 1 << position - 1) == 0)
                    {
                        logger.LogDebug($"95 上料完成 轴{position} 未收到生料");
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"95 上料完成 轴{position} 未收到生料95->{DeviceDescriptor.DeviceId}", deviceServiceInvokeRequest.Params);
                    }
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 0);

                    var ushorts = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferReceiveBoardFlag"].ToUshort(), 1);
                    logger.LogDebug($" 95  --{InteractingDevice.DeviceId} 上料动作未完成 寄存器的值 {ushorts[0]} ");
                    if (ushorts[0] == 0)
                    {
                        logger.LogDebug("95 CompleteLoadMaterial 上料动作未完成");
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"95 CompleteLoadMaterial 上料动作未完成95->{DeviceDescriptor.DeviceId}");
                    }

                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferStartLoadingBoard"].ToUshort(), 0);
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvArrivalEnd"].ToUshort(), 0);
                    logger.LogDebug($"上生料结束");

                    if (deviceServiceInvokeRequest.Params != null && !deviceServiceInvokeRequest.Params.ContainsKey("LoadingPanel"))
                    {
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"95 上料完成中未包含LoadingPanel参数95->{DeviceDescriptor.DeviceId}");
                    }
                    logger.LogDebug($"95 上生料获取的LoadingPanel的信息：{JsonSerializer.Serialize(deviceServiceInvokeRequest.Params["LoadingPanel"])}");
                    var operationEntity = JsonSerializer.Deserialize<SwapPanel>(JsonSerializer.Serialize(deviceServiceInvokeRequest.Params["LoadingPanel"])
                      , new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    logger.LogDebug($"95 CompleteLoadMaterial 上料完成未修改 InteractingDevice.PayloadPanels {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)}");
                    logger.LogDebug($"95 上生料获取的板材信息：{JsonSerializer.Serialize(operationEntity)}");
                    if (operationEntity != null && operationEntity.PanelList.Count == 1)
                    {
                        var panel = operationEntity.PanelList[0];
                        panel.Layer = 0;
                        panel.Position = position;
                        panel.ProductStatus = ProductStatus.WaitingForDrill;
                        panel.LocationCode = InteractingDevice.DeviceId;
                        panel.SiloCode = InteractingDevice.DeviceId;
                        if (codeReaderOnOff)
                        {
                            logger.LogDebug($"钻机代理读码功能开开 赋值本地的二维码信息");
                            panel.Barcode = InteractingDevice.PayloadPanels[position - 1].Barcode;
                        }
                        try
                        {
                            panel.TaskCode = deviceServiceInvokeRequest.Params.ContainsKey(ScheduleConstants.PARAMS_TASK_ID) ? deviceServiceInvokeRequest.Params[ScheduleConstants.PARAMS_TASK_ID].ToStr() : "";

                        }
                        catch (Exception)
                        {


                        }

                        await PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                        {
                            InteractingDevice.PayloadPanels[position - 1] = panel;
                        }));
                    }
                    else
                    {
                        logger.LogDebug($"95 上生料获取的板材信息为null 或者是多个");
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"上生料获取的板材信息为null 或者是多个当前板材信息：{JsonSerializer.Serialize(InteractingDevice.PayloadPanels)}->95->{DeviceDescriptor.DeviceId}");
                    }

                    logger.LogDebug($"95 CompleteLoadMaterial 上料完成后 InteractingDevice.PayloadPanels {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)}");
                    return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, JsonSerializer.Serialize(InteractingDevice.PayloadPanels));
                }
            }
        }

        public async Task<DeviceServiceInvokeResponse> InvokeLoadMaterialLocal(int position)
        {
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 0);
            SetPosition(position.ToUshort());
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferStartLoadingBoard"].ToUshort(), 1);
            try
            {
                if (codeReaderOnOff)
                {
                    logger.LogWarning($"自动触发读码 线程即将开启读码");
                    _ = Task.Factory.StartNew((index) =>
                    {
                        logger.LogWarning($"自动触发读码 线程开启读码");
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
                            logger.LogInformation($"轴{codeIndex + 1}  自动触发读码 线程结束");
                        }
                        catch (Exception ee)
                        {

                            logger.LogDebug($"轴{position}开始上料 多次读码器异常 {ee.Message}");
                        }

                    }, position - 1);

                }
            }
            catch (Exception e)
            {
                logger.LogDebug($"轴{position}开始上料读码器异常 {e.Message}");

            }

            logger.LogDebug($"轴{position}开始上料");
            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }

        public async Task<DeviceServiceInvokeResponse> PrepareLoadMaterialLocal(DeviceServiceInvokeRequest request, int position)
        {
            var bufferOnAgvPosition = InteractingDevice.modbusIpMaster.ReadCoils(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferOnAgvPositionFlag"].ToUshort(), 1);
            if (!bufferOnAgvPosition[0])
            {
                logger.LogDebug($"Buffer不在agv对接层不能执行上料");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"Buffer不在agv对接层不能执行上料95->{DeviceDescriptor.DeviceId}", request.Params);
            }

            var work = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferStatusFlag"].ToUshort(), 4);
            var model = work[1] == 1;
            //logger.LogWarning($"手动状态下agv不能给Buffer上料CNC95->{DeviceDescriptor.DeviceId}->{work[1] }->{ model}");
            if (!model)
            {
                logger.LogDebug($"手动状态下agv不能给Buffer上料");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"手动状态下agv不能给Buffer上料95->{DeviceDescriptor.DeviceId}", request.Params);
            }

            var raw = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["ExistBoardOnPlc"].ToUshort(), 1);
            if ((raw[0] & 1 << position - 1) != 0)
            {
                request.Params["DeviceIsException"] = true;
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["WarningInfoWriteOnPlc"].ToUshort(), (ushort)(90 + position));
                logger.LogDebug($"轴{position} 有生料不能上料");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"轴{position} 有生料不能上料95->{DeviceDescriptor.DeviceId}", request.Params);
            }

            var bufferOnWorking = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferOnWorking"].ToUshort(), 3);
            if (bufferOnWorking[0] > 1 || bufferOnWorking[2] > 1)
            {
                logger.LogDebug($"Buffer正在给钻机上下料：上料 {bufferOnWorking[0] > 1} 下料 {bufferOnWorking[2] > 1}");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"Buffer正在给钻机上下料：上料 {bufferOnWorking[0] > 1} 下料 {bufferOnWorking[2] > 1}->95->{DeviceDescriptor.DeviceId}", request.Params);
            }
            //检查板长信息
            logger.LogDebug($"是否写板长信息到plc开关是否开了 ：{boardLengthWriteToPlcOnOff}");
            if (boardLengthWriteToPlcOnOff)
            {
                try
                {
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BoardLengthInfOnPlc"].ToUshort(), 0);

                }
                catch (Exception ee)
                {
                    logger.LogDebug($"清空板长信息   {ee.Message}");
                }
                if (request.Params == null || !request.Params.ContainsKey("PanelPropertiesToDrill"))
                {
                    request.Params["DeviceIsException"] = true;
                    logger.LogDebug($"检查板长字段的时候  {JsonSerializer.Serialize(request)} 未包含 PanelPropertiesToDrill 参数");
                    return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"检查板长字段的时候 没有 PanelPropertiesToDrill 参数->95->{DeviceDescriptor.DeviceId}", request.Params);
                }
                logger.LogDebug($"  95 上生料获取的PanelPropertiesToDrill的信息：{JsonSerializer.Serialize(request.Params["PanelPropertiesToDrill"])}");

                var operationEntity = JsonSerializer.Deserialize<SwapPanel>(JsonSerializer.Serialize(request.Params["PanelPropertiesToDrill"])
                  , new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                logger.LogDebug($"  95 上生料获取的PanelPropertiesToDrill的信息 反序列化 ");
                if (operationEntity != null && operationEntity.PanelList.Count == 1)
                {
                    //获取板材信息

                    var panel = operationEntity.PanelList[0];
                    var boardLength = panel.PanelLength;
                    logger.LogDebug($"获取的板长信息  {boardLength}");
                    if (boardLength <= 0 || boardLength > ushort.MaxValue)
                    {
                        request.Params["DeviceIsException"] = true;
                        logger.LogDebug($"获取的板长信息 {boardLength} 小于等于0");
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"获取的板长信息 {boardLength} 小于等于0->95->{DeviceDescriptor.DeviceId}", request.Params);

                    }
                    try
                    {
                        ushort length = (ushort)Math.Floor(boardLength);
                        logger.LogDebug($"写入导plc的板长是  {length}");
                        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BoardLengthInfOnPlc"].ToUshort(), length);


                    }
                    catch (Exception ee)
                    {
                        request.Params["DeviceIsException"] = true;
                        logger.LogDebug($"转化板长信息 或者写入plc失败   {ee.Message}");
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"转化板长信息 或者写入plc失败->95->{DeviceDescriptor.DeviceId}", request.Params);

                    }

                }


            }

            //boardLengthWriteToPlcOnOff
            //获取板长
            //写到PLC


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
                              result ? string.Empty : $"机器尚未准备好:buffer准备状态 {readyFlag},上电状态：{isPower}, 自动模式:{model} 非报警状态:{isNoFalt} 非急停状态:{isNoFalt},buffer生料层 轴{position}无板子状态：{hasNoBoard} buffer是否定位完成{isPosition}->95->{DeviceDescriptor.DeviceId}");
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
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"{methodName} 传入的参数不包含位置信息95->{DeviceDescriptor.DeviceId}");
            }

            ushort position = deviceServiceInvokeRequest.Params["Position"].ToUshort();
            if (!(position > 0 && position <= InteractingDevice.spindleNum))
            {
                eventId = Events.Drill.DRILL_PARAMETER_POSITION_FALT_EVENT;
                eventName = Events.Drill.DRILL_PARAMETER_POSITION_FALT_EVENT_NAME;
                eventMessage = $"{methodName} 下发Position参数值{position}不在1和{InteractingDevice.spindleNum}之间，请下发正确的上料轴信息95->{DeviceDescriptor.DeviceId}";
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
