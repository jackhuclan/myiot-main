// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;
using VgDeviceGateway.Devices.Common.Agv;

namespace VgDeviceGateway.Devices.Agv.AgvDevice
{
    public class FrontPanelAgv : DeviceShare<DefaultAgv>, IAgvDevice
    {
        private readonly ILogger<FrontPanelAgv> logger;
        public readonly byte slaveId;

        public FrontPanelAgv(ILogger<FrontPanelAgv> logger, IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
        {
            this.logger = logger;
        }

        public Task InitializeLocal()
        {
            throw new NotImplementedException();
        }

        public async Task<DeviceServiceInvokeResponse> WorkLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            InteractingDevice.currentEventTraceId = deviceServiceInvokeRequest.Params.ContainsKey(ScheduleConstants.PARAMS_TASK_EVENT_TRACE_ID)
                           ? deviceServiceInvokeRequest.Params[ScheduleConstants.PARAMS_TASK_EVENT_TRACE_ID].ToStr()
                           : string.Empty;
            InteractingDevice.CurrentRoutingKey = deviceServiceInvokeRequest.Params.ContainsKey("routingKey")
                             ? deviceServiceInvokeRequest.Params["routingKey"].ToStr()
                             : string.Empty;
            InteractingDevice.targetProductId = deviceServiceInvokeRequest.TargetProductId;
            InteractingDevice.targetDeviceId = deviceServiceInvokeRequest.TargetDeviceId;

            InteractingDevice.currentLoadedCount = 0;
            InteractingDevice.currentUnloadedCount = 0;
            var resopnse = await WorkDetail(deviceServiceInvokeRequest);
            var message = $"\r\n Work结果：Code：{resopnse.Code}，Message：{resopnse.Message}，CurrentEventTraceId：{InteractingDevice.currentEventTraceId}\r\n";
            logger.LogDebug(message);
            InteractingDevice.ReportingProcess(message);
            if (resopnse.Code == ErrorCodes.Sys.SUCCESS)
            {
                var completeScheduleTaskResponse = await InteractingDevice.ReportComplete(deviceServiceInvokeRequest);
                logger.LogDebug($"Work完成上报：{DateTime.Now.ToLongTimeString()} - {InteractingDevice.ProductId} - {InteractingDevice.DeviceName} - {InteractingDevice.DeviceId} !Response:{JsonSerializer.Serialize(completeScheduleTaskResponse)}");
            }
            else
            {
                InteractingDevice.isAgvWorkFail = true;
                var completeScheduleTaskResponse = await InteractingDevice.ReportFail(deviceServiceInvokeRequest);
                logger.LogDebug($"Work失败上报：{DateTime.Now.ToLongTimeString()} - {InteractingDevice.ProductId} - {InteractingDevice.DeviceName} - {InteractingDevice.DeviceId} !Response:{JsonSerializer.Serialize(completeScheduleTaskResponse)}");
            }
            return resopnse;
        }

        public Task<DeviceServiceInvokeResponse> StandbyLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            throw new NotImplementedException();
        }

        public Task<DeviceServiceInvokeResponse> ShutdownLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<DeviceServiceInvokeResponse> ChargeLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            try
            {
                if (!Engine.DeviceConnector.IsConnected)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Charge_PlcUnConnected", "PlcUnConnected", "PLC未连接");
                    return await Response(ErrorCodes.Sys.PLC_UNCONNECT_CODE, ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Charge_Exception", "Exception", ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }

            return await ResponseSuccess();
        }

        public async Task<DeviceServiceInvokeResponse> MoveLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to Move!");
            try
            {
                var newmoveresult = await InteractingDevice.defaultAgvChassis.MoveLocal(InteractingDevice, deviceServiceInvokeRequest);
                if (newmoveresult.Code != ErrorCodes.Sys.SUCCESS)
                {
                    return await Response(ErrorCodes.Sys.FAIL, newmoveresult.Message);
                }
                var moveResponse = newmoveresult.Params["MoveResponse"] as TaskResponse;
                var taskId = moveResponse?.Result?.TaskId;
                //车辆状态变成移动中···
                var update = new Dictionary<string, object?>
                {
                   { "IsMoving", true },
                   { "IsArrived",false },
                   { "AgvReturnTaskId", taskId }
                };
                WatchingProperties.SetValues(update);
                InteractingDevice.isMoving = true;
                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} end to Move!");
                return await ResponseSuccess(string.Empty, update);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_Exception", "Exception", ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        public async Task<DeviceServiceInvokeResponse> WorkDetail(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to Work!");
            var isAllowedNextOperation = true;
            try
            {
                var pmessage = $"\r\n 开始程序，进入work CurrentEventTraceId：{InteractingDevice.currentEventTraceId}，CurrentRoutingKey：{InteractingDevice.CurrentRoutingKey}，TargetProductId：{InteractingDevice.targetProductId}，TargetDeviceId：{InteractingDevice.targetDeviceId} \r\n";
                logger.LogDebug(pmessage);
                InteractingDevice.ReportingProcess(pmessage);
                if (InteractingDevice.CanProceedNextStep(!isAllowedNextOperation))
                {
                    logger.LogDebug($"\r\n 程序进入work 异常：isAllowedNextOperation：{isAllowedNextOperation} \r\n");
                    return await Response(ErrorCodes.AGV.AGV_Work_FinishedWork_Code, ErrorCodes.AGV.AGV_Work_FinishedWork_MESSAGE);
                }

                var workPreCheck = WorkPreCheck(deviceServiceInvokeRequest);
                if (!workPreCheck.Item1)
                {
                    logger.LogDebug($"\r\n WorkPreCheck 验证失败：{workPreCheck.Item1}_Code:{workPreCheck.Item2}_Message:{workPreCheck.Item3} \r\n");
                    return await Response(ErrorCodes.AGV.AGV_Work_FinishedWork_Code, ErrorCodes.AGV.AGV_Work_FinishedWork_MESSAGE);
                }

                var message = $"Work开始上报···";
                logger.LogDebug(message);
                InteractingDevice.ReportingProcess(message);
                var reportstartResponse = await InteractingDevice.ReportRunStatusStart(deviceServiceInvokeRequest);
                message = $"Work开始，上报结果：Code：{reportstartResponse.Code}，Message：{reportstartResponse.Message}";
                logger.LogDebug(message);
                InteractingDevice.ReportingProcess(message);
                if (reportstartResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.Status = DeviceStatus.Exception;
                    return await Response(reportstartResponse.Code, reportstartResponse.Message);
                }

                InteractingDevice.MoveActionStatus = AgvMoveActionStatus.ToDo;
                this.WatchingProperties.Property("IsWorking").SetValue(true);
                InteractingDevice.Status = DeviceStatus.Working;
                InteractingDevice.isWorking = true;
                InteractingDevice.spindlePosition = "";
                InItReSet();

                var operationList = GetOperationList(deviceServiceInvokeRequest, workPreCheck.Item4, workPreCheck.Item5);
                message = $"生成 OperationList，数量：{operationList.Count}";
                logger.LogDebug(message);
                InteractingDevice.ReportingProcess(message);
                if (operationList.Count < 1)
                {
                    logger.LogDebug($"\r\n operationList  数量不正确：operationList：{operationList}，targetPoslist：{workPreCheck.Item4}，spindleBehaviorlist：{workPreCheck.Item5}\r\n");
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_operationListError", "operationListError", "operationList 数量不正确");
                    return await Response(ErrorCodes.Sys.FAIL, "operationList 数量不正确");
                }

                var lastPosition = "";
                var deviceOperationResponse = new DeviceServiceInvokeResponse();
                var errormsg = "";
                int step = 0;
                foreach (var item in operationList)
                {
                    if (InteractingDevice.CanProceedNextStep(!isAllowedNextOperation))
                    {
                        logger.LogDebug("\r\n 结束任务··· \r\n");
                        return await Response(ErrorCodes.Sys.FAIL, "结束任务:" + item.SpindleId);
                    }
                    InteractingDevice.ReportingProcess($"上下物料类型：{InteractingDevice.materialType}");

                    InteractingDevice.loadAndUnLoadLayer = -1;
                    step++;
                    lastPosition = WatchingProperties.Property("CarCurrentPos").NewValue.ToStr();
                    deviceServiceInvokeRequest.Params["LastPos"] = lastPosition;
                    deviceServiceInvokeRequest.Params["SpindlePosition"] = item.SpindleId;
                    deviceServiceInvokeRequest.Params["IsFirstStep"] = step == 1;
                    deviceServiceInvokeRequest.Params["Position"] = item.SpindleId;
                    deviceServiceInvokeRequest.Params["SiloPosition"] = item.SpindleId;
                    deviceServiceInvokeRequest.Params["CarCurrentPos"] = item.AgvPosition;
                    InteractingDevice.spindlePosition = item.SpindleId.ToStr();

                    InteractingDevice.ReportingProcess($"开始第{item.SpindleId}轴操作/共{operationList.Count}条任务， 等待AGV机械结构准备就绪···");
                    logger.LogDebug($"\r\n 开始第{item.SpindleId}轴操作/{operationList.Count}条任务， 等待AGV机械结构准备就绪··· \r\n");
                    var mSignal = await InteractingDevice.MonitoringSignal(4019, "AGV机械结构准备就绪");
                    if (!mSignal.Item1)
                    {
                        return await Response(ErrorCodes.Sys.FAIL, mSignal.Item2);
                    }

                    if (InteractingDevice.CanProceedNextStep(!isAllowedNextOperation))
                    {
                        return await Response(ErrorCodes.Sys.FAIL, "结束任务：轴预备动作之前");
                    }
                    if (InteractingDevice.materialType == MaterialKind.Panel)
                    {
                        //左侧右侧上料
                        var interactivePosition = deviceServiceInvokeRequest.Params["InteractivePosition"].ToStr();
                        var messagep = $"\r\n AGV选择左侧右侧上料5010: {interactivePosition}\r\n";
                        logger.LogDebug(messagep);
                        InteractingDevice.ReportingProcess(messagep);
                        if (interactivePosition.ToLower() == "left")
                        {
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5010, 2);
                        }
                        else if (interactivePosition.ToLower() == "right")
                        {
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5010, 1);
                        }

                        logger.LogDebug($"\r\n {InteractingDevice.materialType}设置PLC {item.SpindleId} 轴预备动作 \r\n");
                        var setresult = SetPlcSingleSpindle(deviceServiceInvokeRequest, item);
                        if (!setresult)
                        {
                            logger.LogDebug($"\r\n 料仓物无生料！！！！！！！！！！ \r\n");
                            return await Response(ErrorCodes.Sys.FAIL, "料仓物无生料！！！！！！！！！！");
                        }
                    }

                    deviceServiceInvokeRequest.Params["MoveTargetPos"] = item.AgvPosition;
                    logger.LogDebug($"\r\n AGV移动··· {item.AgvPosition}\r\n");
                    InteractingDevice.ReportingProcess($"AGV移动··· {item.AgvPosition}");
                    deviceOperationResponse = await MoveLocal(deviceServiceInvokeRequest);
                    errormsg = string.Format($"Move：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                    logger.LogDebug(errormsg);
                    if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                    {
                        isAllowedNextOperation = false;
                        InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_MoveNoSuccess", "MoveNoSuccess", errormsg);
                        return deviceOperationResponse;
                    }

                    if (InteractingDevice.CanProceedNextStep(!isAllowedNextOperation))
                    {
                        return await Response(ErrorCodes.Sys.FAIL, "结束任务：开始预准备动作之前");
                    }
                    if (InteractingDevice.materialType == MaterialKind.Panel)
                    {
                        logger.LogDebug("\r\n 开始预准备动作5013\r\n");
                        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);
                        InteractingDevice.ReportingProcess("开始预准备动作5013");
                    }

                    var moveId = "";
                    if (deviceOperationResponse.Params.ContainsKey("AgvReturnTaskId"))
                    {
                        moveId = deviceOperationResponse.Params["AgvReturnTaskId"].ToStr();
                    }
                    logger.LogDebug($"\r\n 等待车辆到位···: {moveId}\r\n");
                    deviceOperationResponse = await InteractingDevice.defaultAgvChassis.ArrivedLocal(InteractingDevice, deviceServiceInvokeRequest, moveId);
                    errormsg = string.Format($"AGV到达{moveId}：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                    logger.LogDebug(errormsg);
                    InteractingDevice.ReportingProcess(errormsg);
                    if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                    {
                        logger.LogDebug("\r\n 等待车辆到位超时或异常··· \r\n");
                        isAllowedNextOperation = false;
                        InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_ArrivedNoSuccess", "ArrivedNoSuccess", errormsg);
                        return deviceOperationResponse;
                    }

                    if (InteractingDevice.CanProceedNextStep(!isAllowedNextOperation))
                    {
                        return await Response(ErrorCodes.Sys.FAIL, "结束任务：车辆到位之后");
                    }

                    logger.LogDebug("\r\n ActionStatus  改为Doing\r\n");
                    InteractingDevice.MoveActionStatus = AgvMoveActionStatus.Doing;

                    deviceServiceInvokeRequest.Params["OperationListItem"] = item;

                    if (InteractingDevice.materialType == MaterialKind.Panel)
                    {
                        switch (item.InteractionSequence)
                        {
                            case InteractionSequence.LoadOnly:
                                InteractingDevice.ReportingProcess("Panel开始上料 LoadMaterialBehavior");
                                deviceOperationResponse = await InteractingDevice.agvToDeviceAgvLoadPolicy.LoadMaterialBehavior(deviceServiceInvokeRequest);
                                errormsg = string.Format($"Panel上料结束_LoadMaterialBehavior：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                                logger.LogDebug(errormsg);
                                InteractingDevice.ReportingProcess(errormsg);
                                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                                {
                                    isAllowedNextOperation = false;
                                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_LoadMaterialBehaviorNoSuccess", "LoadMaterialBehaviorNoSuccess", errormsg);
                                    break;
                                }
                                break;

                            case InteractionSequence.UnloadOnly:
                                logger.LogDebug($"\r\n 开始下料，Request.PayloadPanels:{JsonSerializer.Serialize(deviceServiceInvokeRequest.PayloadPanels)} \r\n");

                                InteractingDevice.ReportingProcess("开始下料 UnloadMaterialBehavior");
                                deviceOperationResponse = await InteractingDevice.agvToDeviceAgvUnloadPolicy.UnloadMaterialBehavior(deviceServiceInvokeRequest);
                                errormsg = string.Format($"下料结束_UnloadMaterialBehavior：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                                logger.LogInformation(errormsg);
                                InteractingDevice.ReportingProcess(errormsg);
                                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                                {
                                    isAllowedNextOperation = false;
                                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_UnloadMaterialBehaviorNoSuccess", "UnloadMaterialBehaviorNoSuccess", errormsg);
                                    break;
                                }
                                break;

                            case InteractionSequence.LoadThenUnload:
                                InteractingDevice.ReportingProcess("开始上料 LoadMaterialBehavior");
                                deviceOperationResponse = await InteractingDevice.agvToDeviceAgvLoadPolicy.LoadMaterialBehavior(deviceServiceInvokeRequest);
                                errormsg = string.Format($"上料结束_LoadMaterialBehavior：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                                logger.LogDebug(errormsg);
                                InteractingDevice.ReportingProcess(errormsg);
                                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                                {
                                    isAllowedNextOperation = false;
                                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_LoadMaterialBehaviorNoSuccess", "LoadMaterialBehaviorNoSuccess", errormsg);
                                    break;
                                }

                                //左侧右侧上料
                                var interactivePosition = deviceServiceInvokeRequest.Params["InteractivePosition"].ToStr();
                                logger.LogDebug($"\r\n 选择左侧右侧上料: {interactivePosition}\r\n");
                                if (interactivePosition.ToLower() == "left")
                                {
                                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5010, 2);
                                }
                                else if (interactivePosition.ToLower() == "right")
                                {
                                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5010, 1);
                                }

                                logger.LogDebug($"\r\n {InteractingDevice.materialType}设置PLC {item.SpindleId} 轴预备动作 \r\n");
                                SetPlcSingleSpindle(deviceServiceInvokeRequest, item, 2);//1PLC上料2代表PLC下料

                                logger.LogDebug("\r\n 开始预准备动作\r\n");
                                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);

                                if (InteractingDevice.CanProceedNextStep(!isAllowedNextOperation))
                                {
                                    return await Response(ErrorCodes.Sys.FAIL, "结束任务：UnloadMaterialBehavior 之前");
                                }
                                logger.LogDebug($"\r\n 第二步骤，开始下料，PayloadPanels:{JsonSerializer.Serialize(deviceServiceInvokeRequest.PayloadPanels)} \r\n");

                                InteractingDevice.ReportingProcess($" 第二步骤，开始下料 UnloadMaterialBehavior");
                                deviceOperationResponse = await InteractingDevice.agvToDeviceAgvUnloadPolicy.UnloadMaterialBehavior(deviceServiceInvokeRequest);
                                errormsg = string.Format($"下料完成_UnloadMaterialBehavior：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                                logger.LogInformation(errormsg);
                                InteractingDevice.ReportingProcess(errormsg);
                                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                                {
                                    isAllowedNextOperation = false;
                                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_UnloadMaterialBehaviorNoSuccess", "UnloadMaterialBehaviorNoSuccess", errormsg);
                                    break;
                                }
                                break;

                            default:
                                isAllowedNextOperation = false;
                                deviceOperationResponse.Code = ErrorCodes.Sys.FAIL;
                                errormsg = "UnknowBehavior：Error," + item.InteractionSequence;
                                logger.LogInformation(errormsg);
                                break;
                        }
                    }
                    else if (InteractingDevice.materialType == MaterialKind.PanelSilo)
                    {
                        switch (item.InteractionSequence)
                        {
                            case InteractionSequence.LoadOnly:
                                InteractingDevice.ReportingProcess("Silo开始上料 LoadMaterialBehavior");
                                deviceOperationResponse = await InteractingDevice.agvToDeviceAgvLoadPolicy.LoadMaterialBehavior(deviceServiceInvokeRequest);
                                errormsg = string.Format($"Silo上料结束_LoadMaterialBehavior：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                                logger.LogDebug(errormsg);
                                InteractingDevice.ReportingProcess(errormsg);
                                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                                {
                                    isAllowedNextOperation = false;
                                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_LoadMaterialBehaviorNoSuccess", "LoadMaterialBehaviorNoSuccess", errormsg);
                                    break;
                                }
                                break;

                            case InteractionSequence.UnloadOnly:
                                logger.LogDebug($"\r\n 开始下料，Request.PayloadPanels:{JsonSerializer.Serialize(deviceServiceInvokeRequest.PayloadPanels)} \r\n");
                                InteractingDevice.ReportingProcess("开始下料 UnloadMaterialBehavior");
                                deviceOperationResponse = await InteractingDevice.agvToDeviceAgvUnloadPolicy.UnloadMaterialBehavior(deviceServiceInvokeRequest);
                                errormsg = string.Format($"下料结束_UnloadMaterialBehavior：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                                logger.LogInformation(errormsg);
                                InteractingDevice.ReportingProcess(errormsg);
                                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                                {
                                    isAllowedNextOperation = false;
                                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_UnloadMaterialBehaviorNoSuccess", "UnloadMaterialBehaviorNoSuccess", errormsg);
                                    break;
                                }
                                break;

                            default:
                                isAllowedNextOperation = false;
                                deviceOperationResponse.Code = ErrorCodes.Sys.FAIL;
                                errormsg = "UnknowBehavior：Error," + item.InteractionSequence;
                                logger.LogInformation(errormsg);
                                break;
                        }
                    }

                    logger.LogDebug($"\r\n Behavior：{item.InteractionSequence}_deviceOperationResponse.Code:{deviceOperationResponse.Code}_Message:{deviceOperationResponse.Message}··· \r\n");
                    if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                    {
                        isAllowedNextOperation = false;
                        return await Response(deviceOperationResponse.Code, deviceOperationResponse.Message);
                    }

                    logger.LogDebug("\r\n ActionStatus改为Done 结束AGV订单任务AGV可以继续执行下个站点任务\r\n");
                    InteractingDevice.MoveActionStatus = AgvMoveActionStatus.Done;
                }

                //因为没有传感器所以需要一个信号用于 结束信号
                if (step == operationList.Count)
                {
                    InteractingDevice.ReportingProcess("调用设备IsLastStep");
                    logger.LogDebug("\r\n调用设备IsLastStep\r\n");
                    deviceServiceInvokeRequest.Params["IsLastStep"] = true;
                    var end = await InteractingDevice.TargetDeviceOperation(deviceServiceInvokeRequest, DeviceOperationType.CompleteLoadMaterial);
                    var messagelast = $"\r\n 调用设备IsLastStep 结果：{end.Code}_{end.Message}\r\n";
                    logger.LogDebug(messagelast);
                    InteractingDevice.ReportingProcess(messagelast);
                    if (end.Code != ErrorCodes.Sys.SUCCESS)
                    {
                        isAllowedNextOperation = false;
                        return await Response(end.Code, end.Message);
                    }

                    logger.LogDebug("\r\n IsWorking 改为false \r\n");
                    WatchingProperties.Property("IsWorking").SetValue(false);
                    InteractingDevice.isWorking = false;
                }
                InteractingDevice.ReportingProcess("开始结束重置信号");

                logger.LogDebug($"\r\n 结束重置:5010-5020 \r\n");
                var splinedOperation5010_20 = new ushort[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveId, 5010, splinedOperation5010_20);
                logger.LogDebug($"\r\n 结束重置:5027-5029 \r\n");
                var jieshu272829 = new ushort[] { 0, 0, 0 };
                InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveId, 5027, jieshu272829);
                logger.LogDebug($"\r\n 结束重置:5060-5066 \r\n");
                var splined50605066 = new ushort[] { 0, 0, 0, 0, 0, 0, 0 };
                InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveId, 5060, splined50605066);
                InteractingDevice.ReportingProcess("结束重置信号");
                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} end to Work!");
                return await ResponseSuccess();
            }
            catch (Exception ex)
            {
                logger.LogDebug($"\r\n 程序发生异常：{ex} \r\n");
                InteractingDevice.Status = DeviceStatus.Exception;
                isAllowedNextOperation = false;
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_Exception", "Exception", ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        private void InItReSet()
        {
            InteractingDevice.ReportingProcess("初始化重置信号");
            logger.LogDebug($"\r\n 初始化重置:5010-5020 \r\n");
            var splinedOperation5010_20 = new ushort[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveId, 5010, splinedOperation5010_20);
            logger.LogDebug($"\r\n 初始化重置:5027-5029 \r\n");
            var splinedOperation272829 = new ushort[] { 0, 0, 0 };
            InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveId, 5027, splinedOperation272829);
            logger.LogDebug($"\r\n 初始化重置:5060-5066 \r\n");
            var splinedOperation = new ushort[] { 0, 0, 0, 0, 0, 0, 0 };
            InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveId, 5060, splinedOperation);
            InteractingDevice.ReportingProcess("初始化重置信号完成···");
        }

        public Tuple<bool, string, string, string[], string[]> WorkPreCheck(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            var message = "";
            if (!Engine.DeviceConnector.IsConnected)
            {
                message = $"\r\n PLC断开：DeviceConnector.IsConnected:{Engine.DeviceConnector.IsConnected} \r\n ";
                logger.LogDebug(message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_PlcUnConnected", "PlcUnConnected", "PLC未连接");
                return new Tuple<bool, string, string, string[], string[]>(false, ErrorCodes.Sys.PLC_UNCONNECT_CODE, message, null, null);
            }

            //获取钻机在用的轴位，停用是用null表示
            var targetPoslist = deviceServiceInvokeRequest.Params["Spindles"].ToStr().Split(',');
            if (targetPoslist == null || targetPoslist.Length <= 0)
            {
                message = $"\r\n Spindles 数量为O\r\n";
                logger.LogDebug(message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_RequestParamsError", "RequestParamsError", "没有Spindles信息");
                return new Tuple<bool, string, string, string[], string[]>(false, ErrorCodes.Sys.FAIL, message, null, null);
            }
            var spindleBehaviorlist = deviceServiceInvokeRequest.Params["SpindleBehavior"].ToStr().Split(',');
            if (spindleBehaviorlist == null || spindleBehaviorlist.Length <= 0)
            {
                message = $"\r\n SpindleBehavior 数量为O\r\n";
                logger.LogDebug(message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_spindleBehaviorError", "spindleBehaviorError", "SpindleBehavior 数量为O");
                return new Tuple<bool, string, string, string[], string[]>(false, ErrorCodes.Sys.FAIL, message, targetPoslist, null);
            }
            if (targetPoslist.Length != spindleBehaviorlist.Length)
            {
                message = $"\r\n Spindles 和 SpindleBehavior 数量不一致\r\n";
                logger.LogDebug(message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_spindleBehaviorError", "spindleBehaviorError", "Spindles 和 SpindleBehavior 数量不一致");
                return new Tuple<bool, string, string, string[], string[]>(false, ErrorCodes.Sys.FAIL, message, targetPoslist, spindleBehaviorlist);
            }
            return new Tuple<bool, string, string, string[], string[]>(true, ErrorCodes.Sys.SUCCESS, message, targetPoslist, spindleBehaviorlist);
        }

        private List<SwapPanel> GetOperationList(DeviceServiceInvokeRequest deviceServiceInvokeRequest, string[] targetPoslist, string[] spindleBehaviorlist)
        {
            var convertedBehavior = (InteractionBehavior)deviceServiceInvokeRequest.CallerRequestInteractionBehavior;
            var operationList = new List<SwapPanel>();
            var splideIndex = 0;
            logger.LogDebug($"\r\n targetPoslist :{string.Join(',', targetPoslist)}\r\n");
            logger.LogDebug($"\r\n spindleBehaviorlist :{string.Join(',', spindleBehaviorlist)}\r\n");
            for (int i = 0; i < targetPoslist.Length; i++)
            {
                splideIndex = i + 1;
                var targetPos = targetPoslist[i];
                var behaviorNum = spindleBehaviorlist[i];
                if (targetPos.ToLower() == "null" || targetPos == "0")
                {
                    continue;
                }
                if (behaviorNum.ToLower() == "-1")
                {
                    continue;
                }
                var behavior = InteractionSequence.LoadOnly;
                if (behaviorNum == "0")
                {
                    behavior = InteractionSequence.LoadOnly;
                }
                else if (behaviorNum == "1")
                {
                    behavior = InteractionSequence.UnloadOnly;
                }
                else if (behaviorNum == "2")
                {
                    behavior = InteractionSequence.LoadThenUnload;
                }
                else if (behaviorNum == "3")
                {
                    behavior = InteractionSequence.UnloadThenLoad;
                }

                var swapPanel = new SwapPanel();
                swapPanel.SpindleId = splideIndex;
                swapPanel.AgvPosition = targetPos.Trim();
                swapPanel.InteractionSequence = behavior;
                InteractingDevice.materialType = convertedBehavior.MaterialKind;
                switch (InteractingDevice.materialType)
                {
                    case MaterialKind.Panel:
                        ////先取下料信息,首次运行时，可能不需要下料
                        var drilledPanel = deviceServiceInvokeRequest.PayloadPanels.FirstOrDefault(p => p != null && p.ProductStatus == ProductStatus.Finished_DRILL && p.Position == splideIndex);
                        if (drilledPanel != null)
                        {
                            swapPanel.PanelList.Add(drilledPanel);
                        }
                        break;

                    case MaterialKind.PanelSilo:
                        swapPanel.PanelList = deviceServiceInvokeRequest.PayloadPanels;
                        break;

                    default:
                        break;
                }
                operationList.Add(swapPanel);
            }

            return operationList;
        }

        private bool SetPlcSingleSpindle(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapPanel item, int plcnextbehavior = 0)
        {
            var setResult = true;
            var behavior = 0;

            if (item.InteractionSequence == InteractionSequence.LoadOnly)
            {
                behavior = 1;
            }
            else if (item.InteractionSequence == InteractionSequence.UnloadOnly)
            {
                behavior = 2;
            }
            else if (item.InteractionSequence == InteractionSequence.UnloadThenLoad)
            {
                behavior = 2;
            }
            else if (item.InteractionSequence == InteractionSequence.LoadThenUnload)
            {
                behavior = 1;
            }
            if (plcnextbehavior > 0)
            {
                behavior = plcnextbehavior;
            }

            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5028, behavior.ToUshort());
            logger.LogDebug($"\r\n 设置PLC {item.SpindleId} 轴动作详情:{behavior}_{item.InteractionSequence} \r\n");

            if (behavior == 1)
            {
                var itemcode = deviceServiceInvokeRequest.Params[ScheduleConstants.PARAMS_TASK_ITEM_CODE].ToStr().ToLower().Trim();
                var panel = PayloadPanels.LastOrDefault(x => x != null && x.ProductStatus == ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1 && x.ItemCode.ToLower().Trim() == itemcode);
                if (panel != null)
                {
                    InteractingDevice.loadAndUnLoadLayer = panel.Layer;
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5029, (ushort)(InteractingDevice.loadAndUnLoadLayer + 1));
                    logger.LogDebug($"\r\n 设置抓取 {InteractingDevice.loadAndUnLoadLayer + 1} 层生料 \r\n");
                }
                else
                {
                    setResult = false;
                    logger.LogDebug("生料 设置板料 panel null");
                }
            }
            else if (behavior == 2)
            {
                if (item.InteractionSequence == InteractionSequence.LoadThenUnload || InteractingDevice.loadAndUnLoadLayer > -1)
                {
                    logger.LogDebug($"即上又下：{item.InteractionSequence}，熟料层数不变：{InteractingDevice.loadAndUnLoadLayer}");
                }
                else
                {
                    for (int i = PayloadPanels.Count - 1; i > -1; i--)
                    {
                        if (PayloadPanels[i] != null && PayloadPanels[i].ProductStatus == ProductStatus.EmptySiloBox)
                        {
                            InteractingDevice.loadAndUnLoadLayer = i;
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5029, (ushort)(InteractingDevice.loadAndUnLoadLayer + 1));
                            logger.LogDebug($"\r\n 设置放置 {InteractingDevice.loadAndUnLoadLayer + 1} 层熟料 \r\n");
                            break;
                        }
                    }
                }
            }
            return setResult;
        }

        public Tuple<bool, string> ReadMaterialCodeLocal()
        {
            ushort[] ushortstr = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(InteractingDevice.slaveId, 4350, InteractingDevice.readCodeLength.ToUshort());
            var code = string.Join("", ushortstr);
            if (string.IsNullOrEmpty(code))
            {
                return new Tuple<bool, string>(false, code);
            }
            return new Tuple<bool, string>(true, code);
        }

        public void SetPlcHeartLocal()
        {
            var ticks = DateTime.Now.Ticks;
            var plcHeart = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveId, 4000, 1);
            var isauto = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveId, 4002, 1);
            var iserror = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveId, 4003, 1);

            logger.LogInformation($"给PLC的心跳信号{ticks},PLC心跳值：{plcHeart[0]}，程序心跳：{InteractingDevice.heartValue}，是否自动：{isauto[0]}，是否报错：{iserror[0]}");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5000, (ushort)InteractingDevice.heartValue);
            if (InteractingDevice.heartValue == 0)
            {
                InteractingDevice.heartValue = 1;
            }
            else
            {
                InteractingDevice.heartValue = 0;
            }
        }

        public Tuple<bool, string> CheckCanMoveLocal()
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<bool, string>> LoadExternalMaterialLocal(AgvPageEntity agvPageEntity)
        {
            throw new NotImplementedException();
        }

        public string QueryExternalMaterialLocal()
        {
            throw new NotImplementedException();
        }

        public string ClearExternalMaterialLocal()
        {
            throw new NotImplementedException();
        }

        public void InitSiloNoMaterialLocal()
        {
            throw new NotImplementedException();
        }

        public void InitNoSiloLocal()
        {
            throw new NotImplementedException();
        }

        public Task<DeviceServiceInvokeResponse> MoveOperationLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, int operation)
        {
            throw new NotImplementedException();
        }

        public Tuple<bool, string> AgvToReadyLocal()
        {
            if (InteractingDevice.Status == DeviceStatus.Working)
            {
                return new Tuple<bool, string>(false, "正在运行中，别瞎点");
            }
            else
            {
                InteractingDevice.agvIsReady = true;
                InteractingDevice.Status = DeviceStatus.Ready;
            }
            return new Tuple<bool, string>(true, $"设备状态：{InteractingDevice.Status}");
        }

        public List<Panel> InitPayloadPanels()
        {
            return Panel.HasSilo.NoPanelForLayerFirst("", DeviceDescriptor.SpindleNum.ToInt(), DeviceDescriptor.LayerLimit.ToInt());
        }

        public Task<DeviceServiceInvokeResponse> PlcOpertaionLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest) => throw new NotImplementedException();
        public Tuple<bool, string> ReadMaterialCodeLocal(int startIndex) => throw new NotImplementedException();
        public string ScannigConvertL(string Scannignumber) => throw new NotImplementedException();
    }
}
