// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
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
    public class BackPanelAgv : DeviceShare<DefaultAgv>, IAgvDevice
    {
        private readonly ILogger<BackPanelAgv> logger;
        public readonly byte slaveId;

        public BackPanelAgv(ILogger<BackPanelAgv> logger, IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
        {
            this.logger = logger;
            slaveId = (byte)device.DeviceDescriptor.Extra["ModbusTcpSlaveId"].ToInt();
        }
        //protected Dictionary<string, float> ItemPanelInfoWhenCompleteLoadMaterial = new Dictionary<string, float>();
        public Task InitializeLocal()
        {
            throw new NotImplementedException();
        }

        public async Task<DeviceServiceInvokeResponse> WorkLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            try
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
                InteractingDevice.sendCancelBeforeArrivedDevice = true;

                //查找生料的板长信息
                //ItemPanelInfoWhenCompleteLoadMaterial.Clear();
                //正常也走不到这个逻辑
                var undrillItems = PayloadPanels.UndrilledPanels.Where(p => p.PanelLength <= 0);
                if (undrillItems.Any())
                {
                    var itemCodes = PayloadPanels.UndrilledItemCodes;
                    foreach (var itemCode in itemCodes)
                    {
                        var panelLength = await GetPanthLength(itemCode);
                        foreach (var panel in undrillItems.Where(x => x.ItemCode.ToLower().Trim() == itemCode.ToLower().Trim()))
                        {
                            panel.PanelLength = panelLength;
                        }
                    }
                }

                var resopnse = await WorkDetail(deviceServiceInvokeRequest);
                var message = $"\r\n Work结果：Code：{resopnse.Code}，Message：{resopnse.Message}，CurrentEventTraceId：{InteractingDevice.currentEventTraceId}\r\n";
                logger.LogDebug(message + $"_CurrentEventTraceId：{InteractingDevice.currentEventTraceId}");
                _ = InteractingDevice.ReportingProcess(message, resopnse.Code != ErrorCodes.Sys.SUCCESS ? AlarmLevel.Severe : AlarmLevel.Information, resopnse.Code != ErrorCodes.Sys.SUCCESS ? "AES10001" : "");
                if (resopnse.Code == ErrorCodes.Sys.SUCCESS)
                {
                    var completeScheduleTaskResponse = await InteractingDevice.ReportComplete(deviceServiceInvokeRequest);
                    logger.LogDebug($"Work完成上报：{DateTime.Now.ToLongTimeString()} - {InteractingDevice.ProductId} - {InteractingDevice.DeviceName} - {InteractingDevice.DeviceId} !Response:{JsonSerializer.Serialize(completeScheduleTaskResponse)}");
                    _ = InteractingDevice.ReportingProcess($"Work完成上报：{completeScheduleTaskResponse.Code}_{completeScheduleTaskResponse.Message}",
                        completeScheduleTaskResponse.Code != ErrorCodes.Sys.SUCCESS ? AlarmLevel.Severe : AlarmLevel.Information, resopnse.Code != ErrorCodes.Sys.SUCCESS ? "AES10002" : ""
                        );
                }
                else if (resopnse.Code == "ReportStartError")
                {
                    await InteractingDevice.ResetDeviceStatus();
                    InteractingDevice.agvIsReady = true;
                    logger.LogDebug($"Work开始上报,中控返回false：{resopnse.Code}，初始化设备，等待下一次分配");
                    _ = InteractingDevice.ReportingProcess($"Work开始上报,中控返回false：{resopnse.Code}，初始化设备，等待下一次分配");
                }
                else
                {
                    InteractingDevice.SchedulingTasks.Clear();
                    InteractingDevice.isAgvWorkFail = true;
                    this.WatchingProperties.Property("IsAgvWorkFail").SetValue(true);
                    var reportFailResponse = await InteractingDevice.ReportFail(deviceServiceInvokeRequest);
                    logger.LogDebug($"Work失败上报：{DateTime.Now.ToLongTimeString()} - {InteractingDevice.ProductId} - {InteractingDevice.DeviceName} - {InteractingDevice.DeviceId} !Response:{JsonSerializer.Serialize(reportFailResponse)}");
                    _ = InteractingDevice.ReportingProcess($"Work失败上报：{reportFailResponse.Code}_{reportFailResponse.Message}", reportFailResponse.Code != ErrorCodes.Sys.SUCCESS ? AlarmLevel.Severe : AlarmLevel.Information, resopnse.Code != ErrorCodes.Sys.SUCCESS ? "AES10003" : "");
                }
                return resopnse;
            }
            catch (Exception ex)
            {
                logger.LogDebug($"程序捕捉到异常：{ex.Message}");
                await InteractingDevice.ReportingProcess($"程序捕捉到异常，请前去代理工控机查看详情");
                return await Response(ErrorCodes.Sys.FAIL, ex.Message);
            }
            finally
            {
                InteractingDevice.moveTimeout = InteractingDevice.configExtra["MoveTimeout"].ToInt();
                logger.LogDebug($"还原移动超时时间：{InteractingDevice.moveTimeout}秒");
            }
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
            InteractingDevice.currentStepMsg = "";
            InteractingDevice.errorsMsg = "";
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to Work!");
            var isAllowedNextOperation = true;
            try
            {
                #region 验证状态及上报开始

                var pmessage = $"\r\n 开始程序，进入work CurrentEventTraceId：{InteractingDevice.currentEventTraceId}，CurrentRoutingKey：{InteractingDevice.CurrentRoutingKey}，TargetProductId：{InteractingDevice.targetProductId}，TargetDeviceId：{InteractingDevice.targetDeviceId} \r\n";
                logger.LogDebug(pmessage);
                _ = InteractingDevice.ReportingProcess(pmessage);
                if (InteractingDevice.CanProceedNextStep(!isAllowedNextOperation))
                {
                    logger.LogDebug($"\r\n 程序进入work 异常：isAllowedNextOperation：{isAllowedNextOperation} \r\n");
                    return await Response(ErrorCodes.AGV.AGV_Work_FinishedWork_Code, ErrorCodes.AGV.AGV_Work_FinishedWork_MESSAGE);
                }
                var workPreCheck = WorkPreCheck(deviceServiceInvokeRequest);
                if (!workPreCheck.Item1)
                {
                    var msg = $"\r\n WorkPreCheck 验证失败：{workPreCheck.Item1}_Code:{workPreCheck.Item2}_Message:{workPreCheck.Item3} \r\n";
                    logger.LogDebug(msg);
                    _ = InteractingDevice.ReportingProcess(msg, AlarmLevel.Severe, "AES10004");
                    return await Response(ErrorCodes.AGV.AGV_Work_FinishedWork_Code, ErrorCodes.AGV.AGV_Work_FinishedWork_MESSAGE);
                }

                var message = $"Work开始上报···";
                logger.LogDebug(message);
                _ = InteractingDevice.ReportingProcess(message);

                var reportstartResponse = await InteractingDevice.ReportRunStatusStart(deviceServiceInvokeRequest);
                message = $"Work开始，上报结果：Code：{reportstartResponse.Code}，Message：{reportstartResponse.Message}";
                logger.LogDebug(message);
                _ = InteractingDevice.ReportingProcess(message, reportstartResponse.Code != ErrorCodes.Sys.SUCCESS ? AlarmLevel.Severe : AlarmLevel.Information,
                    reportstartResponse.Code != ErrorCodes.Sys.SUCCESS ? "AES10005" : "");

                if (reportstartResponse.Code == ErrorCodes.Sys.HTTPREQUEST_TIMEOUT_CODE)
                {
                    InteractingDevice.Status = DeviceStatus.Exception;//超时异常
                    return await Response(reportstartResponse.Code, reportstartResponse.Message);
                }
                else if (reportstartResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    logger.LogDebug($"Work开始上报,中控返回false");
                    return await Response("ReportStartError", reportstartResponse.Message);
                }

                _ = InteractingDevice.ReportingProcess($"等待AGV机械结构准备就绪···");
                logger.LogDebug($"\r\n  等待AGV机械结构准备就绪··· \r\n");
                var mSignal = await InteractingDevice.MonitoringSignal(4019, "AGV机械结构准备就绪4019");
                if (!mSignal.Item1)
                {
                    _ = InteractingDevice.ReportingProcess($"等待AGV机械结构准备就绪4019超时", AlarmLevel.Severe, "AEP10001");
                    InteractingDevice.Status = DeviceStatus.Exception;
                    return await Response(ErrorCodes.Sys.FAIL, mSignal.Item2);
                }

                InteractingDevice.MoveActionStatus = AgvMoveActionStatus.ToDo;
                InteractingDevice.isWorking = true;
                this.WatchingProperties.Property("IsWorking").SetValue(true);
                InteractingDevice.Status = DeviceStatus.Working;
                InteractingDevice.spindlePosition = "";
                InteractingDevice.loadAndUnLoadLayer = -1;
                deviceServiceInvokeRequest.Params["CheckMovePoint"] = InteractingDevice.configExtra["CheckMovePoint"];
                ResetPlcSingnal();

                #endregion 验证状态及上报开始

                #region 生成动作详情即OperationList

                var operationList = GetOperationList(deviceServiceInvokeRequest, workPreCheck.Item4, workPreCheck.Item5);
                message = $"生成 OperationList，数量：{operationList.Count}";
                logger.LogDebug(message);
                _ = InteractingDevice.ReportingProcess(message);
                if (operationList.Count < 1)
                {
                    var operationListMsg = $"\r\n operationList  数量不正确：operationList：{operationList}，targetPoslist：{workPreCheck.Item4}，spindleBehaviorlist：{workPreCheck.Item5}\r\n";
                    logger.LogDebug(operationListMsg);
                    _ = InteractingDevice.ReportingProcess(operationListMsg, AlarmLevel.Severe, "AES10006");
                    return await Response(ErrorCodes.Sys.FAIL, "operationList 数量不正确");
                }

                #endregion 生成动作详情即OperationList

                var lastPosition = "";
                var deviceOperationResponse = new DeviceServiceInvokeResponse();
                var errormsg = "";
                int step = 0;
                int count = 0;
                string taskCode = "";
                string reqCode = "";

                #region 预准备动作

                if (InteractingDevice.materialType == MaterialKind.Panel)
                {
                    #region 校验料仓二维码

                    var checkBarcode = await CheckSiloBarcode();
                    _ = InteractingDevice.ReportingProcess($"料仓二维码校验结果：{checkBarcode.Item1}：{checkBarcode.Item2}");
                    if (!checkBarcode.Item1)
                    {
                        return await Response(ErrorCodes.Sys.FAIL, $"CheckSiloBarcode: {checkBarcode.Item2}");
                    }

                    #endregion 校验料仓二维码

                    logger.LogDebug($"\r\n {InteractingDevice.materialType}设置PLC {operationList[0].SpindleId} 轴预备动作 \r\n");

                    var setresult = await SetPlcSingleSpindleAsync(deviceServiceInvokeRequest, operationList[0]);

                    if (!setresult)
                    {
                        logger.LogDebug($"\r\n 设置取放料层数失败 \r\n");
                        _ = InteractingDevice.ReportingProcess($"\r\n 设置取放料层数失败 \r\n", AlarmLevel.Severe, "AES10007");
                        return await Response(ErrorCodes.Sys.FAIL, "设置取放料层数失败！！！！！！！！！");
                    }

                    if (InteractingDevice.materialType == MaterialKind.Panel)
                    {
                        logger.LogDebug("\r\n 开始预准备动作5013\r\n");
                        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);
                        _ = InteractingDevice.ReportingProcess("开始预准备动作5013");
                    }
                    else
                    {
                        logger.LogDebug($"\r\n 非Panel无预准备动作，materialType：{InteractingDevice.materialType}\r\n");
                    }
                }

                #endregion 预准备动作

                #region 海康任务组模式主任务

                if (InteractingDevice.configExtra["AgvChassisSupplier"].ToString() == "HikRobot")
                {
                    taskCode = "VegaMove" + InteractingDevice.DeviceId + DateTime.Now.ToString("yyyyMMddHHmmss");
                    reqCode = taskCode + "_" + count.ToString();
                    deviceServiceInvokeRequest.Params["taskCode"] = taskCode;
                    deviceServiceInvokeRequest.Params["count"] = 0;//是否给小车下的第一个任务

                    List<string> AllTargetPos = new List<string>();
                    if (InteractingDevice.materialType == MaterialKind.PanelSilo)
                    {
                        AllTargetPos.Add(operationList[0].AgvPosition);
                        AllTargetPos.Add(deviceServiceInvokeRequest.Params["ShelfInnerPos"].ToStr().Trim());
                        AllTargetPos.Add(operationList[0].AgvPosition);
                    }
                    else if (InteractingDevice.materialType == MaterialKind.Panel)
                    {
                        for (int i = 0; i < operationList.Count(); i++)
                        {
                            AllTargetPos.Add(operationList[i].AgvPosition);
                        }
                    }

                    deviceServiceInvokeRequest.Params["AllTargetPos"] = AllTargetPos;
                    deviceServiceInvokeRequest.Params["count"] = 0;
                    deviceServiceInvokeRequest.Params["MoveTargetPos"] = operationList[0].AgvPosition;
                    deviceServiceInvokeRequest.Params["reqCode"] = reqCode;
                    deviceServiceInvokeRequest.Params["method"] = "end";
                    //最后一个步骤
                    deviceServiceInvokeRequest.Params["Unlock"] = false;

                    logger.LogDebug("\r\n 请求PLC小车需要移动5055 \r\n");
                    _ = InteractingDevice.ReportingProcess("请求PLC小车需要移动5055");
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5055, 1);//通知PLC小车需要移动
                    var agvMoveCan = await InteractingDevice.MonitoringSignal(4055, "底盘允许移动");
                    if (!agvMoveCan.Item1)
                    {
                        _ = InteractingDevice.ReportingProcess("底盘允许移动：4055 超时", AlarmLevel.Severe, "AEP20004");
                        return await Response(ErrorCodes.Sys.FAIL, agvMoveCan.Item2);
                    }
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4055, 0);
                    _ = InteractingDevice.ReportingProcess("收到允许移动并重置4055");

                    //给海康小车部署任务
                    logger.LogDebug($"\r\n AGV移动··· {operationList[0].AgvPosition}\r\n");
                    _ = InteractingDevice.ReportingProcess($"AGV移动··· {operationList[0].AgvPosition}");
                    deviceOperationResponse = await MoveLocal(deviceServiceInvokeRequest);
                    errormsg = string.Format($"Move：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                    logger.LogDebug(errormsg);
                    if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                    {
                        isAllowedNextOperation = false;
                        return deviceOperationResponse;
                    }
                    InteractingDevice.failTaskCode = taskCode;

                    logger.LogDebug($"\r\n 等待AGV到达 {operationList[0].AgvPosition}···\r\n");
                    deviceOperationResponse = await InteractingDevice.defaultAgvChassis.ArrivedLocal(InteractingDevice, deviceServiceInvokeRequest, taskCode);
                    errormsg = string.Format($"AGV到达{taskCode}：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                    InteractingDevice.sendCancelBeforeArrivedDevice = false;
                    logger.LogDebug(errormsg);
                    _ = InteractingDevice.ReportingProcess($"{errormsg}_下发判断参数：{InteractingDevice.sendCancelBeforeArrivedDevice}");
                    if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                    {
                        logger.LogDebug("\r\n 等待车辆到位超时或异常··· \r\n");
                        isAllowedNextOperation = false;
                        InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_ArrivedNoSuccess", "ArrivedNoSuccess", errormsg);
                        return deviceOperationResponse;
                    }
                }

                #endregion 海康任务组模式主任务

                InteractingDevice.StdGroupNo = $"{DeviceDescriptor.DeviceName}_{DateTime.Now.Ticks}";

                #region 逐条执行operationList

                foreach (var item in operationList)
                {
                    if (InteractingDevice.CanProceedNextStep(!isAllowedNextOperation))
                    {
                        logger.LogDebug("\r\n 结束任务··· \r\n");
                        return await Response(ErrorCodes.Sys.FAIL, "结束任务:" + item.SpindleId);
                    }
                    _ = InteractingDevice.ReportingProcess($"上下物料类型：{InteractingDevice.materialType}");

                    InteractingDevice.moveTimeout = InteractingDevice.configExtra["MoveTimeout"].ToInt();
                    step++;
                    if (step > 1)
                    {
                        InteractingDevice.loadAndUnLoadLayer = -1;
                    }

                    reqCode = taskCode + "_" + item.SpindleId.ToString();

                    if (InteractingDevice.configExtra.ContainsKey("SpindleMoveTimeout"))
                    {
                        InteractingDevice.moveTimeout = InteractingDevice.configExtra["SpindleMoveTimeout"].ToInt();
                        logger.LogDebug($"\r\n 监控到在钻机轴之间移动, 移动超时时间更正为{InteractingDevice.moveTimeout}秒\r\n");
                    }
                    else
                    {
                        logger.LogDebug($"\r\n 配置文件里没有 SpindleMoveTimeout这个参数\r\n");
                    }

                    logger.LogDebug($"\r\n  移动超时时间为{InteractingDevice.moveTimeout}秒\r\n");

                    lastPosition = WatchingProperties.Property("CarCurrentPos").NewValue.ToStr();
                    deviceServiceInvokeRequest.Params["LastPos"] = lastPosition;
                    deviceServiceInvokeRequest.Params["SpindlePosition"] = item.SpindleId;
                    deviceServiceInvokeRequest.Params["IsFirstStep"] = step == 1;
                    deviceServiceInvokeRequest.Params["Position"] = item.SpindleId;
                    deviceServiceInvokeRequest.Params["SiloPosition"] = item.SpindleId;
                    deviceServiceInvokeRequest.Params["CarCurrentPos"] = item.AgvPosition;
                    InteractingDevice.spindlePosition = item.SpindleId.ToStr();
                    InteractingDevice.currentStepMsg += $"【目标设备ID：{deviceServiceInvokeRequest.TargetDeviceId}】【轴号：{item.SpindleId}】【动作：{item.InteractionSequence}】";

                    if (InteractingDevice.CanProceedNextStep(!isAllowedNextOperation))
                    {
                        return await Response(ErrorCodes.Sys.FAIL, "结束任务：轴预备动作之前");
                    }

                    if (InteractingDevice.configExtra["AgvChassisSupplier"].ToStr() == "HikRobot")
                    {
                        count++;
                        if (step > 1)
                        {
                            logger.LogDebug("\r\n HK通知PLC小车需要移动5055 \r\n");
                            _ = InteractingDevice.ReportingProcess("HK通知PLC小车需要移动5055");
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5055, 1);//通知PLC小车需要移动
                            var agvMoveCan = await InteractingDevice.MonitoringSignal(4055, "底盘允许移动");
                            if (!agvMoveCan.Item1)
                            {
                                _ = InteractingDevice.ReportingProcess("底盘允许移动：4055 超时", AlarmLevel.Severe, "AEP20004");
                                return await Response(ErrorCodes.Sys.FAIL, agvMoveCan.Item2);
                            }
                            logger.LogDebug("\r\n 清除4055信号 \r\n");
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4055, 0);
                            _ = InteractingDevice.ReportingProcess("收到允许移动，并重置4055");
                        }
                    }
                    else
                    {
                        logger.LogDebug("\r\n 通知PLC小车需要移动5055 \r\n");
                        InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5055, 1);//通知PLC小车需要移动
                        var agvMoveCan = await InteractingDevice.MonitoringSignal(4055, "底盘允许移动");
                        if (!agvMoveCan.Item1)
                        {
                            _ = InteractingDevice.ReportingProcess("底盘允许移动：4055 超时", AlarmLevel.Severe, "AEP20004");
                            return await Response(ErrorCodes.Sys.FAIL, agvMoveCan.Item2);
                        }
                        logger.LogDebug("\r\n 清除4055信号 \r\n");
                        InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4055, 0);
                        _ = InteractingDevice.ReportingProcess("收到允许移动，并重置4055");
                    }

                    _ = InteractingDevice.ReportingProcess($"\r\n 下一步，根据物料类型_{InteractingDevice.materialType}，给PLC赋值左右侧，以及轴预备动作 \r\n");
                    if (InteractingDevice.materialType == MaterialKind.Panel)
                    {
                        //左侧右侧上料
                        var interactivePosition = deviceServiceInvokeRequest.Params["InteractivePosition"].ToStr();
                        var messagep = $"\r\n AGV选择左侧右侧上料5010: {interactivePosition}\r\n";
                        logger.LogDebug(messagep);
                        _ = InteractingDevice.ReportingProcess(messagep);
                        if (interactivePosition.ToLower() == "left")
                        {
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5010, 2);
                        }
                        else if (interactivePosition.ToLower() == "right")
                        {
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5010, 1);
                        }
                        if (step > 1)
                        {
                            logger.LogDebug($"\r\n {InteractingDevice.materialType}设置PLC {item.SpindleId} 轴预备动作 \r\n");

                            var setresult = await SetPlcSingleSpindleAsync(deviceServiceInvokeRequest, item);

                            if (!setresult)
                            {
                                logger.LogDebug($"\r\n 设置取放料层数失败 \r\n");
                                return await Response(ErrorCodes.Sys.FAIL, "设置取放料层数失败！！！！！！！！！");
                            }
                        }
                    }

                    deviceServiceInvokeRequest.Params["count"] = item.SpindleId;
                    deviceServiceInvokeRequest.Params["taskCode"] = taskCode;
                    deviceServiceInvokeRequest.Params["method"] = "end" + count;

                    taskCode = deviceServiceInvokeRequest.Params["taskCode"].ToStr();
                    InteractingDevice.publicMoveId = taskCode;

                    deviceServiceInvokeRequest.Params["reqCode"] = taskCode + "_" + item.SpindleId;
                    deviceServiceInvokeRequest.Params["IsLoadAndUnload"] = item.InteractionSequence == InteractionSequence.LoadThenUnload ? 1 : 0;
                    deviceServiceInvokeRequest.Params["OperationListItem"] = JsonSerializer.Serialize(item);
                    deviceServiceInvokeRequest.Params["IsPassPre"] = 0;

                    #region 串行模式移动

                    //deviceServiceInvokeRequest.Params["MoveTargetPos"] = item.AgvPosition;
                    //logger.LogDebug($"\r\n AGV移动··· {item.AgvPosition}\r\n");
                    //InteractingDevice.ReportingProcess($"AGV移动··· {item.AgvPosition}");
                    //deviceOperationResponse = await MoveLocal(deviceServiceInvokeRequest);
                    //errormsg = string.Format($"Move：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                    //logger.LogDebug(errormsg);
                    //if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                    //{
                    //    isAllowedNextOperation = false;
                    //    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_MoveNoSuccess", "MoveNoSuccess", errormsg);
                    //    return deviceOperationResponse;
                    //}

                    //if (InteractingDevice.CanProceedNextStep(!isAllowedNextOperation))
                    //{
                    //    return await Response(ErrorCodes.Sys.FAIL, "结束任务：开始预准备动作之前");
                    //}

                    //var moveId = InteractingDevice.publicMoveId;
                    //if (InteractingDevice.configExtra["AgvChassisSupplier"].ToStr() == "HikRobot")
                    //{
                    //    logger.LogDebug($"\r\n 海康小车任务特殊逻辑：moveId：{taskCode}\r\n");
                    //    moveId = taskCode;
                    //}

                    //logger.LogDebug($"\r\n 等待车辆到位···: {moveId}\r\n");
                    //deviceOperationResponse = await InteractingDevice.defaultAgvChassis.ArrivedLocal(InteractingDevice, deviceServiceInvokeRequest, moveId);
                    //errormsg = string.Format($"AGV到达{moveId}：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                    //InteractingDevice.sendCancelBeforeArrivedDevice = false;
                    //logger.LogDebug(errormsg);
                    //InteractingDevice.ReportingProcess(errormsg);
                    //if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                    //{
                    //    logger.LogDebug("\r\n 等待车辆到位超时或异常··· \r\n");
                    //    isAllowedNextOperation = false;
                    //    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_ArrivedNoSuccess", "ArrivedNoSuccess", errormsg);
                    //    return deviceOperationResponse;
                    //}

                    #endregion 串行模式移动

                    InteractingDevice.AgvCanLeave = false;
                    InteractingDevice.AgvCanLeaveCallBack = false;
                    if (!InteractingDevice.configExtra.ContainsKey("UseNewModel"))
                    {
                        _ = InteractingDevice.ReportingProcess("没有UseNewModel参数，执行旧模式");
                        var moveresult = await OldModelMoveTask(deviceServiceInvokeRequest, item, taskCode);
                        _ = InteractingDevice.ReportingProcess($"没有UseNewModel参数，执行旧模式{moveresult.Code}_{moveresult.Message}");
                        if (moveresult.Code != ErrorCodes.Sys.SUCCESS)
                        {
                            return await Response(ErrorCodes.Sys.FAIL, $"没有UseNewModel参数，执行旧模式Move：false_{moveresult.Message}");
                        }
                    }
                    else
                    {
                        _ = InteractingDevice.ReportingProcess($"执行新模式_{InteractingDevice.configExtra["UseNewModel"].ToInt()}：pre和move并行");
                        if (InteractingDevice.configExtra["UseNewModel"].ToInt() == 1)
                        {
                            InteractingDevice.checkmoveArrived = false;
                            InteractingDevice.checkPrepareSuccess = false;
                            deviceServiceInvokeRequest.Params["IsPassPre"] = 1;
                            await MoveTask(deviceServiceInvokeRequest, item, taskCode);
                            await PreTask(deviceServiceInvokeRequest, item);
                            //中控下发逻辑
                            InteractingDevice.sendCancelBeforeArrivedDevice = false;

                            var checkresultstr = "等待线程移动和钻机Pre结果";
                            logger.LogDebug(checkresultstr);
                            _ = InteractingDevice.ReportingProcess(checkresultstr);
                            DateTime startTime = DateTime.Now;
                            var checkresult = false;
                            var istimeout = false;
                            while (true)
                            {
                                if (InteractingDevice.checkPrepareSuccess && InteractingDevice.checkmoveArrived)
                                {
                                    checkresult = true;
                                    break;
                                }
                                istimeout = InteractingDevice.IsTimeout(startTime, InteractingDevice.moveTimeout);
                                if (istimeout)
                                {
                                    break;
                                }
                                if (InteractingDevice.CanProceedNextStep())
                                {
                                    logger.LogDebug($"\r\n 等待线程结果时：程序异常跳出循环 \r\n");
                                    break;
                                }
                                await Task.Delay(1000);
                            }
                            checkresultstr = $"\r\n 收到线程移动结果：{InteractingDevice.checkPrepareSuccess}_线程Pre：{InteractingDevice.checkPrepareSuccess}_是否超时：{istimeout}\r\n ";
                            logger.LogDebug(checkresultstr);
                            _ = InteractingDevice.ReportingProcess(checkresultstr);
                            if (!checkresult)
                            {
                                return await Response(ErrorCodes.Sys.FAIL, checkresultstr);
                            }
                        }
                        else
                        {
                            _ = InteractingDevice.ReportingProcess("UseNewModel 参数不为1");
                            return await Response(ErrorCodes.Sys.FAIL, "UseNewModel 参数不为1");
                        }
                    }

                    logger.LogDebug("\r\n ActionStatus  改为Doing\r\n");
                    InteractingDevice.MoveActionStatus = AgvMoveActionStatus.Doing;

                    deviceServiceInvokeRequest.Params["OperationListItem"] = JsonSerializer.Serialize(item);

                    if (InteractingDevice.materialType == MaterialKind.Panel)
                    {
                        //HIK小车锁车信号
                        if (InteractingDevice.configExtra["AgvChassisSupplier"].ToString() == "HikRobot"
                            || InteractingDevice.configExtra["AgvChassisSupplier"].ToString() == "StdRobot")
                        {
                            logger.LogDebug("\r\n 通知PLC锁车5057\r\n");
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5057, 1);
                        }

                        switch (item.InteractionSequence)
                        {
                            case InteractionSequence.LoadOnly:
                                deviceServiceInvokeRequest.Params["IsLoadAndUnload"] = 0;
                                _ = InteractingDevice.ReportingProcess("Panel开始上料 LoadMaterialBehavior");
                                deviceOperationResponse = await InteractingDevice.agvToDeviceAgvLoadPolicy.LoadMaterialBehavior(deviceServiceInvokeRequest);
                                errormsg = string.Format($"Panel上料_LoadMaterialBehavior：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                                logger.LogDebug(errormsg);
                                _ = InteractingDevice.ReportingProcess(errormsg, deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS ? AlarmLevel.Severe : AlarmLevel.Information,
                                    deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS ? "AES10009" : "");
                                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                                {
                                    isAllowedNextOperation = false;
                                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_LoadMaterialBehaviorNoSuccess", "LoadMaterialBehaviorNoSuccess", errormsg);
                                    break;
                                }
                                break;

                            case InteractionSequence.UnloadOnly:
                                deviceServiceInvokeRequest.Params["IsLoadAndUnload"] = 0;
                                logger.LogDebug($"\r\n 开始下料，Request.PayloadPanels:{JsonSerializer.Serialize(deviceServiceInvokeRequest.PayloadPanels)} \r\n");

                                _ = InteractingDevice.ReportingProcess("开始下料 UnloadMaterialBehavior");
                                deviceOperationResponse = await InteractingDevice.agvToDeviceAgvUnloadPolicy.UnloadMaterialBehavior(deviceServiceInvokeRequest);
                                errormsg = string.Format($"Panel下料_UnloadMaterialBehavior：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                                logger.LogInformation(errormsg);
                                _ = InteractingDevice.ReportingProcess(errormsg, deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS ? AlarmLevel.Severe : AlarmLevel.Information,
                                    deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS ? "AES10010" : "");
                                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                                {
                                    isAllowedNextOperation = false;
                                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_UnloadMaterialBehaviorNoSuccess", "UnloadMaterialBehaviorNoSuccess", errormsg);
                                    break;
                                }
                                break;

                            case InteractionSequence.LoadThenUnload:
                                deviceServiceInvokeRequest.Params["IsLoadAndUnload"] = 1;
                                _ = InteractingDevice.ReportingProcess("开始上料 LoadMaterialBehavior");
                                deviceOperationResponse = await InteractingDevice.agvToDeviceAgvLoadPolicy.LoadMaterialBehavior(deviceServiceInvokeRequest);
                                errormsg = string.Format($"上料结束_LoadMaterialBehavior：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                                logger.LogDebug(errormsg);
                                _ = InteractingDevice.ReportingProcess(errormsg, deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS ? AlarmLevel.Severe : AlarmLevel.Information,
                                   deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS ? "AES10009" : "");
                                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                                {
                                    isAllowedNextOperation = false;
                                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_LoadMaterialBehaviorNoSuccess", "LoadMaterialBehaviorNoSuccess", errormsg);
                                    break;
                                }

                                InteractingDevice.currentStepMsg = "";
                                InteractingDevice.currentStepMsg += $"【目标设备ID：{deviceServiceInvokeRequest.TargetDeviceId}】【轴号：{item.SpindleId}】【动作：{item.InteractionSequence}】";
                                //左侧右侧上料
                                var interactivePosition = deviceServiceInvokeRequest.Params["InteractivePosition"].ToStr();
                                logger.LogDebug($"\r\n 选择左侧右侧上料: {interactivePosition}\r\n");
                                deviceServiceInvokeRequest.Params["IsPassPre"] = 0;
                                if (interactivePosition.ToLower() == "left")
                                {
                                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5010, 2);
                                }
                                else if (interactivePosition.ToLower() == "right")
                                {
                                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5010, 1);
                                }

                                logger.LogDebug($"\r\n {InteractingDevice.materialType}设置PLC {item.SpindleId} 轴预备动作 \r\n");
                                var setresult = await SetPlcSingleSpindleAsync(deviceServiceInvokeRequest, item, 2);//1PLC上料2代表PLC下料
                                if (!setresult)
                                {
                                    return await Response(ErrorCodes.Sys.FAIL, $"设置PLC： {item.SpindleId} 轴预备动作失败");
                                }

                                logger.LogDebug("\r\n 开始预准备动作\r\n");
                                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);

                                if (InteractingDevice.CanProceedNextStep(!isAllowedNextOperation))
                                {
                                    return await Response(ErrorCodes.Sys.FAIL, "结束任务：UnloadMaterialBehavior 之前");
                                }
                                logger.LogDebug($"\r\n 第二步骤，开始下料，PayloadPanels:{JsonSerializer.Serialize(deviceServiceInvokeRequest.PayloadPanels)} \r\n");

                                _ = InteractingDevice.ReportingProcess($" 第二步骤，开始下料 UnloadMaterialBehavior");
                                deviceOperationResponse = await InteractingDevice.agvToDeviceAgvUnloadPolicy.UnloadMaterialBehavior(deviceServiceInvokeRequest);
                                errormsg = string.Format($"第二步骤下料完成_UnloadMaterialBehavior：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                                logger.LogDebug(errormsg);
                                _ = InteractingDevice.ReportingProcess(errormsg, deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS ? AlarmLevel.Severe : AlarmLevel.Information,
                                      deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS ? "AES10010" : "");
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
                        //HIK小车重置锁车信号
                        if (InteractingDevice.configExtra["AgvChassisSupplier"].ToString() == "HikRobot"
                            || InteractingDevice.configExtra["AgvChassisSupplier"].ToString() == "StdRobot")
                        {
                            logger.LogDebug("\r\n 通知PLC解除锁车5057\r\n");
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5057, 0);
                        }
                    }
                    else if (InteractingDevice.materialType == MaterialKind.PanelSilo)
                    {
                        switch (item.InteractionSequence)
                        {
                            case InteractionSequence.LoadOnly:
                                if (PayloadPanels[0].ProductStatus == 0)
                                {
                                    logger.LogDebug("检测到上料仓动作，AGV数据没有料仓");
                                    _ = InteractingDevice.ReportingProcess("检测到上料仓动作，AGV数据没有料仓", AlarmLevel.Severe, "AES10011");
                                    break;
                                }
                                _ = InteractingDevice.ReportingProcess("Silo开始上料 LoadMaterialBehavior");
                                deviceOperationResponse = await InteractingDevice.agvToDeviceAgvLoadPolicy.LoadMaterialBehavior(deviceServiceInvokeRequest);
                                errormsg = string.Format($"Silo上料结束_LoadMaterialBehavior：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                                logger.LogDebug(errormsg);
                                _ = InteractingDevice.ReportingProcess(errormsg, deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS ? AlarmLevel.Severe : AlarmLevel.Information,
                                     deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS ? "AES10013" : "");
                                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                                {
                                    isAllowedNextOperation = false;
                                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_LoadMaterialBehaviorNoSuccess", "LoadMaterialBehaviorNoSuccess", errormsg);
                                    break;
                                }
                                break;

                            case InteractionSequence.UnloadOnly:
                                if (PayloadPanels[0].ProductStatus != 0)
                                {
                                    logger.LogDebug("检测到下料仓动作，AGV数据已经有料仓");
                                    _ = InteractingDevice.ReportingProcess("检测到下料仓动作，AGV数据已经有料仓", AlarmLevel.Severe, "AES10012");
                                    break;
                                }
                                logger.LogDebug($"\r\n 开始下料，Request.PayloadPanels:{JsonSerializer.Serialize(deviceServiceInvokeRequest.PayloadPanels)} \r\n");
                                _ = InteractingDevice.ReportingProcess("开始下料 UnloadMaterialBehavior");
                                deviceOperationResponse = await InteractingDevice.agvToDeviceAgvUnloadPolicy.UnloadMaterialBehavior(deviceServiceInvokeRequest);
                                errormsg = string.Format($"Silo下料结束_UnloadMaterialBehavior：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                                logger.LogInformation(errormsg);
                                _ = InteractingDevice.ReportingProcess(errormsg, deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS ? AlarmLevel.Severe : AlarmLevel.Information,
                                     deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS ? "AES10014" : "");
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
                    InteractingDevice.AgvCanLeave = true;
                    //STD 车需要等待AskLeave 执行完
                    if (InteractingDevice.configExtra["AgvChassisSupplier"].ToString() == "StdRobot")
                    {
                        logger.LogDebug("\r\n CheckCanExcuteNextTask 检查 斯坦德 是否已回调AskLeave 成功\r\n");
                        if (!await InteractingDevice.CheckCanExcuteNextTask())
                        {
                            return await Response(ErrorCodes.Sys.FAIL, "Std AskLeave 接口超时...");
                        }
                    }
                }

                #endregion 逐条执行operationList

                #region 所有轴动作完成解锁小车

                var response = await ReleaseAgv(deviceServiceInvokeRequest);
                if (response.Code != ErrorCodes.Sys.SUCCESS)
                {
                    isAllowedNextOperation = false;
                    logger.LogDebug($"小车解锁失败，Code: {response.Code}, Message: {response.Message}");
                    _ = InteractingDevice.ReportingProcess($"小车解锁失败，Code: {response.Code}, Message: {response.Message}", AlarmLevel.Severe, "AES10015");
                    return await Response(response.Code, response.Message);
                }
                //if (InteractingDevice.configExtra["AgvChassisSupplier"].ToString() == "HikRobot")
                //{
                //    if (InteractingDevice.materialType == MaterialKind.Panel)
                //    {
                //        #region 海康小车解锁逻辑
                //        var response =await ReleaseAgv( deviceServiceInvokeRequest);
                //        if (response.Code != ErrorCodes.Sys.SUCCESS)
                //        {
                //            isAllowedNextOperation = false;
                //            logger.LogDebug($"小车解锁失败，Code: {response.Code}, Message: {response.Message}");
                //            InteractingDevice.ReportingProcess($"小车解锁失败，Code: {response.Code}, Message: {response.Message}", AlarmLevel.Severe, "AES10015");
                //            return await Response(response.Code, response.Message);
                //        }
                //        #endregion
                //    }
                //}
                //else if (InteractingDevice.configExtra["AgvChassisSupplier"].ToString() == "StdRobot")
                //{
                //    var finishResult = await InteractingDevice.defaultAgvChassis.CompleteGroup(InteractingDevice, deviceServiceInvokeRequest);
                //    if (finishResult.Code != ErrorCodes.Sys.SUCCESS)
                //    {
                //        isAllowedNextOperation = false;
                //        logger.LogDebug($"斯坦德完成组任务接口失败: {finishResult?.Message}");
                //        _ = InteractingDevice.ReportingProcess($"斯坦德完成组任务调用接口失败:{finishResult?.Message}");

                //        return finishResult;
                //    }
                //    else
                //    {
                //        _ = InteractingDevice.ReportingProcess($"斯坦德完成组任务调用接口成功!");
                //    }
                //}

                #endregion 所有轴动作完成解锁小车

                #region 结束通知结束信号

                if (step == operationList.Count)
                {
                    _ = InteractingDevice.ReportingProcess("调用设备IsLastStep");
                    logger.LogDebug("\r\n调用设备IsLastStep\r\n");
                    deviceServiceInvokeRequest.Params["IsLastStep"] = true;
                    var end = await InteractingDevice.TargetDeviceOperation(deviceServiceInvokeRequest, DeviceOperationType.CompleteLoadMaterial);
                    var messagelast = $"\r\n 调用设备IsLastStep 结果：{end.Code}_{end.Message}\r\n";
                    logger.LogDebug(messagelast);
                    _ = InteractingDevice.ReportingProcess(messagelast, end.Code != ErrorCodes.Sys.SUCCESS ? AlarmLevel.Severe : AlarmLevel.Information, "AES10016");
                    if (end.Code != ErrorCodes.Sys.SUCCESS)
                    {
                        isAllowedNextOperation = false;
                        return await Response(end.Code, end.Message);
                    }

                    logger.LogDebug("\r\n IsWorking 改为false \r\n");
                    WatchingProperties.Property("IsWorking").SetValue(false);
                    InteractingDevice.isWorking = false;
                    InteractingDevice.Status = DeviceStatus.Ready;
                }

                #endregion 结束通知结束信号

                #region 重置信号

                ResetPlcSingnal();

                #endregion 重置信号

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

        private async Task<ValueTuple<bool, string>> CheckSiloBarcode()
        {
            if (InteractingDevice.configExtra.GetConfig("CheckSiloBarcode").ToBool())
            {
                await InteractingDevice.modbusIpMaster.WriteSingleRegisterAsync(slaveId, 5068, 1);
            }
            else
            {
                await InteractingDevice.modbusIpMaster.WriteSingleRegisterAsync(slaveId, 5068, 0);
                return new ValueTuple<bool, string>(true, "料仓二维码校验没有开启，无需校验！");
            }
            //var enableCheck = await InteractingDevice.modbusIpMaster.ReadHoldingRegistersAsync(slaveId, 5068, 1);
            //if (enableCheck[0] == 0)
            //{
            //    return new ValueTuple<bool, string>(true, "料仓二维码校验没有开启，无需校验！");
            //}

            await InteractingDevice.modbusIpMaster.WriteSingleRegisterAsync(slaveId, 5067, 1);

            _ = InteractingDevice.ReportingProcess($"等待可以读取二维码信号... \r\n");

            var waitReadBarcode = await InteractingDevice.MonitoringSignal(4029, "等待可以读取二维码信号");

            if (!waitReadBarcode.Item1)
            {
                return new ValueTuple<bool, string>(false, "等待可以读取二维码信号异常!");
            }

            string barcodeString = string.Empty;
            string sysBarcode = InteractingDevice.PayloadPanels?.FirstOrDefault()?.SiloCode!;
            var barcode = (await InteractingDevice.modbusIpMaster.ReadHoldingRegistersAsync(slaveId, 4450, 5)).ConvertUshortArrayToStr();

            if (barcode != null && barcode.Length > 0)
            {
                barcodeString = string.Join("", barcode.Select(x => x.ToString()));
                if (barcodeString.Length > 0)
                {
                    if (barcodeString.Equals(sysBarcode))
                    {
                        await InteractingDevice.modbusIpMaster.WriteSingleRegisterAsync(slaveId, 5064, 1);
                        return new ValueTuple<bool, string>(true, $"料仓二维码校验成功，料仓二维码为：{barcodeString},程序料仓码：{sysBarcode}");
                    }
                    else
                    {
                        await InteractingDevice.modbusIpMaster.WriteSingleRegisterAsync(slaveId, 5064, 2);
                    }
                }
            }

            return new ValueTuple<bool, string>(false, $"料仓二维码校验失败,读取到的料仓二维码为：{barcodeString},程序料仓码：{sysBarcode}!");
        }

        private void ResetPlcSingnal()
        {
            _ = InteractingDevice.ReportingProcess("初始化重置信号");

            logger.LogDebug($"\r\n 初始化重置:5010-5020 \r\n");
            var splinedOperation5010_20 = new ushort[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveId, 5010, splinedOperation5010_20);

            logger.LogDebug($"\r\n 初始化重置:5027-5029 \r\n");
            var splinedOperation272829 = new ushort[] { 0, 0, 0 };
            InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveId, 5027, splinedOperation272829);

            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5057, 0);

            logger.LogDebug($"\r\n 初始化重置:5060-5066 \r\n");
            var splinedOperation = new ushort[] { 0, 0, 0, 0, 0, 0, 0 };
            InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveId, 5060, splinedOperation);

            _ = InteractingDevice.ReportingProcess("\r\n 初始化重置信号：4029，4060，4062 \r\n");

            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 4029, 0);

            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 4060, 0);
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 4062, 0);

            _ = InteractingDevice.ReportingProcess("初始化重置信号完成···");
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
                message = $"\r\n Work验证： Spindles 数量为O\r\n";
                logger.LogDebug(message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_RequestParamsError", "RequestParamsError", "没有Spindles信息");
                return new Tuple<bool, string, string, string[], string[]>(false, ErrorCodes.Sys.FAIL, message, null, null);
            }
            var spindleBehaviorlist = deviceServiceInvokeRequest.Params["SpindleBehavior"].ToStr().Split(',');
            if (spindleBehaviorlist == null || spindleBehaviorlist.Length <= 0)
            {
                message = $"\r\n Work验证：SpindleBehavior 数量为O\r\n";
                logger.LogDebug(message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_spindleBehaviorError", "spindleBehaviorError", "SpindleBehavior 数量为O");
                return new Tuple<bool, string, string, string[], string[]>(false, ErrorCodes.Sys.FAIL, message, targetPoslist, null);
            }
            if (targetPoslist.Length != spindleBehaviorlist.Length)
            {
                message = $"\r\n Work验证：Spindles 和 SpindleBehavior 数量不一致\r\n";
                logger.LogDebug(message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_spindleBehaviorError", "spindleBehaviorError", "Spindles 和 SpindleBehavior 数量不一致");
                return new Tuple<bool, string, string, string[], string[]>(false, ErrorCodes.Sys.FAIL, message, targetPoslist, spindleBehaviorlist);
            }
            ushort[] PLCDetectsSiloSensor = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(InteractingDevice.slaveId, 4066, 1);
            string sensorValuesString = PLCDetectsSiloSensor != null ? string.Join(", ", PLCDetectsSiloSensor) : "null";
            var SiloCodeYN = InteractingDevice.PayloadPanels.Where(x => x.SiloCode != "").FirstOrDefault();
            string siloCodeYnString = SiloCodeYN?.ToString() ?? "null";
            //    InteractingDevice.errorInfo = new Tuple<string, string, string>("PLC检测料仓传感器的值：", sensorValuesString, siloCodeYnString);
            if ((siloCodeYnString == null && sensorValuesString[0] == 1) || (siloCodeYnString != null && sensorValuesString[0] == 0))
            {
                message = $"\r\n Work验证：PLC检测料仓传感器\r\n";
                logger.LogDebug(message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("PLC检测料仓传感器的值：", sensorValuesString, siloCodeYnString);
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

        private async Task<bool> SetPlcSingleSpindleAsync(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapPanel item, int plcnextbehavior = 0)
        {
            var setResult = false;
            var behavior = 0;
            var newbehavior = 0;
            if (item.InteractionSequence == InteractionSequence.LoadOnly)
            {
                behavior = 1;
                newbehavior = 1;
            }
            else if (item.InteractionSequence == InteractionSequence.UnloadOnly)
            {
                behavior = 2;
                newbehavior = 2;
            }
            else if (item.InteractionSequence == InteractionSequence.UnloadThenLoad)
            {
                behavior = 2;
                if (InteractingDevice.configExtra.ContainsKey("UseNewModel") && InteractingDevice.configExtra["UseNewModel"].ToInt() == 1)
                {
                    newbehavior = 4;
                }
            }
            else if (item.InteractionSequence == InteractionSequence.LoadThenUnload)
            {
                behavior = 1;
                if (InteractingDevice.configExtra.ContainsKey("UseNewModel") && InteractingDevice.configExtra["UseNewModel"].ToInt() == 1)
                {
                    newbehavior = 3;
                }
            }
            if (plcnextbehavior > 0)
            {
                behavior = plcnextbehavior;
            }
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5070, newbehavior.ToUshort());
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5028, behavior.ToUshort());
            logger.LogDebug($"\r\n 设置PLC {item.SpindleId} 轴动作详情:{behavior}_{item.InteractionSequence} \r\n");
            _ = InteractingDevice.ReportingProcess($"\r\n 设置PLC {item.SpindleId} 轴动作详情:{behavior}_{item.InteractionSequence} \r\n");
            if (behavior == 1)
            {
                var taskCode = deviceServiceInvokeRequest.Params[ScheduleConstants.PARAMS_TASK_ID].ToStr().Trim();
                var itemcode = deviceServiceInvokeRequest.Params[ScheduleConstants.PARAMS_TASK_ITEM_CODE].ToStr().ToLower().Trim();
                logger.LogDebug($"交互板料编码：{itemcode}");
                logger.LogDebug($"上料PayloadPanels：{JsonSerializer.Serialize(PayloadPanels)}");
                var panel = PayloadPanels.LastOrDefault(x => x != null && x.ProductStatus == ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1 && x.ItemCode.ToLower().Trim() == itemcode
                );
                if (panel != null)
                {
                    InteractingDevice.loadAndUnLoadLayer = panel.Layer;
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5029, (ushort)(InteractingDevice.loadAndUnLoadLayer + 1));
                    logger.LogDebug($"\r\n 设置抓取 {InteractingDevice.loadAndUnLoadLayer + 1} 层生料 \r\n");
                    _ = InteractingDevice.ReportingProcess($"\r\n 设置抓取 {InteractingDevice.loadAndUnLoadLayer + 1} 层生料 \r\n");
                    InteractingDevice.currentStepMsg += $"【设置抓取 {InteractingDevice.loadAndUnLoadLayer + 1} 层生料";

                    // 从中控获取板长
                    if (panel.PanelLength <= 0)
                    {
                        panel.PanelLength = await GetPanthLength(panel, itemcode);
                    }

                    //上生料时，赋值任务代号
                    panel.TaskCode = taskCode;

                    _ = InteractingDevice.ReportingProcess($"从中控获取板长最终结果：{panel.PanelLength}");
                    var loadingPanel = new SwapPanel()
                    {
                        AgvPosition = item.AgvPosition,
                        SpindleId = item.SpindleId,

                        PanelList = new List<Panel>() { panel }
                    };
                    deviceServiceInvokeRequest.Params["PanelPropertiesToDrill"] = loadingPanel;
                    setResult = true;
                }
                else
                {
                    setResult = false;
                    logger.LogDebug("生料 设置板料 panel null");
                }
            }
            else if (behavior == 2)
            {
                logger.LogDebug($"\r\n 设置下料层数时 InteractionSequence:{item.InteractionSequence}_loadAndUnLoadLayer:{InteractingDevice.loadAndUnLoadLayer} \r\n");
                if (item.InteractionSequence == InteractionSequence.LoadThenUnload || InteractingDevice.loadAndUnLoadLayer > -1)
                {
                    logger.LogDebug($"即上又下的时候放原生料层：{item.InteractionSequence}，熟料层数不变：{InteractingDevice.loadAndUnLoadLayer + 1}");
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5029, (ushort)(InteractingDevice.loadAndUnLoadLayer + 1));
                    logger.LogDebug($"\r\n 设置放置 {InteractingDevice.loadAndUnLoadLayer + 1} 层熟料 \r\n");
                    setResult = true;
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
                            _ = InteractingDevice.ReportingProcess($"\r\n 设置放置 {InteractingDevice.loadAndUnLoadLayer + 1} 层熟料 \r\n");
                            InteractingDevice.currentStepMsg += $"【设置放置 {InteractingDevice.loadAndUnLoadLayer + 1} 层熟料";

                            setResult = true;
                            break;
                        }
                    }
                }
            }
            return setResult;
        }

        //读取料码
        public Tuple<bool, string> ReadMaterialCodeLocal(int startIndex)
        {
            ushort[] ushortstr = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(InteractingDevice.slaveId, (ushort)startIndex, InteractingDevice.readCodeLength.ToUshort());
            var code = string.Join("", ushortstr);
            if (string.IsNullOrEmpty(code))
            {
                return new Tuple<bool, string>(false, code);
            }
            return new Tuple<bool, string>(true, code);
        }

        //扫码结果转换
        public string ScannigConvertL(string ScannigData)
        {
            // 用于存储结果的StringBuilder
            StringBuilder resultBuilder = new StringBuilder();
            ushort num = Convert.ToUInt16(ScannigData);
            // foreach (ushort num in segments)
            // {
            // 提取低字节（最低有效8位）
            byte lowByte = (byte)(num & 0xFF);
            // 提取高字节（最高有效8位）
            byte highByte = (byte)((num >> 8) & 0xFF);

            // 将低位字节转换为ASCII字符（先添加）
            char lowChar = Convert.ToChar(lowByte);
            // 将高位字节转换为ASCII字符（后添加）
            char highChar = Convert.ToChar(highByte);

            // 添加交换后的字符：低位在前，高位在后
            resultBuilder.Append(lowChar);
            resultBuilder.Append(highChar);
            // }
            string finalString = resultBuilder.ToString();
            return finalString;
        }

        public void SetPlcHeartLocal()
        {
            var ticks = DateTime.Now.Ticks;
            var plcHeart = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveId, 4000, 1);
            var isauto = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveId, 4002, 1);
            var iserror = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveId, 4003, 1);

            //  logger.LogInformation($"给PLC的心跳信号{ticks},PLC心跳值：{plcHeart[0]}，程序心跳：{InteractingDevice.heartValue}，是否自动：{isauto[0]}，是否报错：{iserror[0]}");
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
            var carCanMoveAndFinished = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveId, 4019, 1);

            if (carCanMoveAndFinished == null)
            {
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_ReadPlcError", "ReadPlcError", "ReadPlcError:4019");
                return new Tuple<bool, string>(false, "ReadPlcError:4019");
            }
            if (carCanMoveAndFinished[0] != 1)
            {
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_carCanMoveAndFinishedError", "carCanMoveAndFinishedError", $"carCanMoveAndFinishedError:{carCanMoveAndFinished[0]}");
                return new Tuple<bool, string>(false, $"carCanMoveAndFinishedError:{carCanMoveAndFinished[0]}");
            }
            return new Tuple<bool, string>(true, "");
        }

        public async Task<Tuple<bool, string>> LoadExternalMaterialLocal(AgvPageEntity agvPageEntity)
        {
            if (string.IsNullOrEmpty(agvPageEntity.MaterialCode)
                && agvPageEntity.LoadMaterialType != 0)
            {
                return new Tuple<bool, string>(false, $"请输入物料代码:{agvPageEntity.MaterialCode}_Type:{agvPageEntity.LoadMaterialType}");
            }
            //if (agvPageEntity.PanelPcs <= 0 && agvPageEntity.LoadMaterialType != 0)
            //{
            //    return new Tuple<bool, string>(false, $"请输入板料每叠片数:{agvPageEntity.PanelPcs}_Type:{agvPageEntity.LoadMaterialType}");
            //}

            if (string.IsNullOrEmpty(agvPageEntity.SiloCode))
            {
                return new Tuple<bool, string>(false, $"请选择料仓:{agvPageEntity.SiloCode}");
            }

            if (agvPageEntity.LoadMaterialType == 0)
            {
                agvPageEntity.MaterialCode = "";
            }

            if (agvPageEntity.StartLayer + agvPageEntity.LoadLayerCount > InteractingDevice.DeviceDescriptor.LayerLimit)
            {
                return new Tuple<bool, string>(false, $"层数+加载数量不能大于{InteractingDevice.DeviceDescriptor.LayerLimit}层");
            }

            var productStatus = ProductStatus.EmptySiloBox;

            if (agvPageEntity.LoadMaterialType == 0)
            {
                productStatus = ProductStatus.EmptySiloBox;
            }
            else if (agvPageEntity.LoadMaterialType == 1)
            {
                productStatus = ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1;
            }
            else if (agvPageEntity.LoadMaterialType == 2)
            {
                productStatus = ProductStatus.PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1;
            }
            else if (agvPageEntity.LoadMaterialType == 3)
            {
                //料仓到达运料agv 已上pin
                productStatus = ProductStatus.PRE_PRE_BUFFER_TRANSFER_AGV_OUTPUT_1;
            }
            else if (agvPageEntity.LoadMaterialType == 4)
            {
                //料仓到达运料agv 待下pin，unpin
                productStatus = ProductStatus.PRE_UNPIN_TRANSFER_AGV_OUTPUT_1;
            }
            else
            {
                return new Tuple<bool, string>(false, $"板料类型不正确：{agvPageEntity.LoadMaterialType}");
            }

            //List<string> panelCodes = Enumerable.Repeat<string>(string.Empty, agvPageEntity.LoadLayerCount).ToList();
            List<Panel> response = new List<Panel>();
            if (agvPageEntity.LoadMaterialType > 0)
            {
                //如果原状态是0-没有料仓的状态，调整为1-空料仓
                InteractingDevice.PayloadPanels.Where(x => x.ProductStatus == ProductStatus.EmptyPayload).ToList()
                    .ForEach(p => p.ProductStatus = ProductStatus.EmptySiloBox);
                GetNextPanelRequest request = new GetNextPanelRequest()
                {
                    BeginLayer = agvPageEntity.StartLayer,
                    Count = agvPageEntity.LoadLayerCount,
                    ItemCode = agvPageEntity.MaterialCode,
                    //PanelWidth = agvPageEntity.PanelWidth,
                    PinOffset = agvPageEntity.PinOffset,
                    ProductStatus = productStatus,
                    SiloCode = agvPageEntity.SiloCode,
                    Position = 1,
                    BatchCode = "",
                    LotId = "",
                };

                response = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<GetNextPanelRequest, List<Panel>>(InteractingDevice.DeviceDescriptor.Extra["GetNextPanelNumberV2"].ToStr(), request);
                if (response == null)
                {
                    return new Tuple<bool, string>(false, $"未从服务器获取到板料信息");
                }
            }

            await InteractingDevice.PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
            {
                for (int i = 0; i < InteractingDevice.PayloadPanels.Count; i++)
                {
                    InteractingDevice.PayloadPanels[i].SiloCode = agvPageEntity.SiloCode;
                    InteractingDevice.PayloadPanels[i].LocationCode = DeviceDescriptor.DeviceId;
                }

                for (var i = 0; i < agvPageEntity.LoadLayerCount && (response != null && response.Count == agvPageEntity.LoadLayerCount || agvPageEntity.LoadMaterialType == 0); i++)
                {
                    InteractingDevice.PayloadPanels[i + agvPageEntity.StartLayer] = new Panel
                    {
                        PanelCode = response.Count > 0 ? response[i].PanelCode : string.Empty,
                        ItemCode = agvPageEntity.MaterialCode,
                        PanelWidth = response.Count > 0 ? response[i].PanelWidth : 0,
                        PanelLength = response.Count > 0 ? response[i].PanelLength : 0,
                        PinOffset = agvPageEntity.PinOffset,
                        ProductStatus = productStatus,
                        SiloCode = agvPageEntity.SiloCode,
                        Layer = i + agvPageEntity.StartLayer,
                        Position = 1,//单个料仓 AGV，1
                        BatchCode = "",
                        LotId = "",
                        LocationCode = DeviceDescriptor.DeviceId,
                        Pcs = response.Count > 0 ? response[i].Pcs : 0,
                    };
                }
            }));
            logger.LogDebug($"人工操作加载数据：{JsonSerializer.Serialize(InteractingDevice.PayloadPanels)}");
            return new Tuple<bool, string>(true, "");
        }

        public string QueryExternalMaterialLocal()
        {
            var allstr = "";
            for (int i = 0; i < InteractingDevice.PayloadPanels.Count; i++)
            {
                string status = FormatPanelStatus.GetPanelOrSiloStatus(InteractingDevice.PayloadPanels[i].ProductStatus);

                if (InteractingDevice.PayloadPanels.Any(t => t.ProductStatus != ProductStatus.EmptySiloBox || t.ProductStatus != ProductStatus.EmptyPayload)
                    && InteractingDevice.PayloadPanels[i].ProductStatus == ProductStatus.EmptySiloBox) status = "空层";

                if (InteractingDevice.PayloadPanels.All(t => t.ProductStatus == ProductStatus.EmptySiloBox)) status = "空料仓";

                allstr += $"第{i + 1}层:"
                     + $"【板料状态】:<span style='font-weight: bold;'>{status}</span>,"
                       + $"【料仓号】:<span style='font-weight: bold;color:sandybrown'>{InteractingDevice.PayloadPanels[i].SiloCode ?? ""}</span>，"
                       + $"【料号】:<span style='font-weight: bold;color:blue'>{InteractingDevice.PayloadPanels[i].ItemCode ?? ""}</span>，"
                       + $"【板料码】:{InteractingDevice.PayloadPanels[i].PanelCode ?? ""},"
                       + $"【板料二维码】:<span style='font-weight: bold;color:magenta'>{InteractingDevice.PayloadPanels[i].Barcode ?? ""}</span>,"
                       + $"【板长】:{InteractingDevice.PayloadPanels[i].PanelLength}，"
                       + $"【板宽】:{InteractingDevice.PayloadPanels[i].PanelWidth}，"
                       + $"【板厚】:{InteractingDevice.PayloadPanels[i].PanelThickness}，"
                       + $"【位置】:{InteractingDevice.PayloadPanels[i].LocationCode ?? ""}，"
                       + $"【片数】:{(int)InteractingDevice.PayloadPanels[i].Pcs}</br>";

                //allstr += $"第{i + 1}层：{JsonSerializer.Serialize(InteractingDevice.PayloadPanels[i])}" + Environment.NewLine;
            }
            return allstr;
        }

        public string ClearExternalMaterialLocal()
        {
            InitSiloNoMaterialLocal();
            InteractingDevice.isFullSilo = false;
            return JsonSerializer.Serialize(InteractingDevice.PayloadPanels);
        }

        public void InitSiloNoMaterialLocal()
        {
            PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
            {
                var silocode = PayloadPanels.SiloCode;
                InteractingDevice.PayloadPanels.Clear();
                InteractingDevice.PayloadPanels.AddRange(Panel.HasSilo.NoPanelForSpindleFirst(silocode, DeviceDescriptor.SpindleNum.ToInt() * DeviceDescriptor.LayerLimit.ToInt(), DeviceDescriptor.LayerLimit.ToInt()));
                PayloadPanels.SetLocationCode(InteractingDevice.DeviceId);
                PayloadPanels.SetSiloCode(silocode);
            }));
        }

        public void InitNoSiloLocal()
        {
            PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
            {
                PayloadPanels.Clear();
                PayloadPanels.AddRange(Panel.NoSilo.PanelForSpindleFirst(DeviceDescriptor.SpindleNum.ToInt() * DeviceDescriptor.LayerLimit.ToInt(), DeviceDescriptor.LayerLimit.ToInt()));
                PayloadPanels.SetLocationCode(InteractingDevice.DeviceId);
                PayloadPanels.SetSiloCode("");
            }));
        }

        public Task<DeviceServiceInvokeResponse> MoveOperationLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, int operation)
        {
            return Response(ErrorCodes.Sys.EXCEPTION_CODE, "没有移动并且动作");
        }

        public Tuple<bool, string> AgvToReadyLocal()
        {
            if (InteractingDevice.Status == DeviceStatus.Working)
            {
                return new Tuple<bool, string>(false, "正在运行中，别瞎点");
            }
            else
            {
                //InteractingDevice.agvIsReady = true;
                //InteractingDevice.Status = DeviceStatus.Ready;
                //return new Tuple<bool, string>(true, $"");

                var isExistSilo = InteractingDevice.WatchingProperties.Property("IsExistSilo").NewValue.ToBool();
                var toreadyStr = "";
                if (isExistSilo)
                {
                    toreadyStr = "有料仓ToReady";
                    var panel = InteractingDevice.PayloadPanels.Where(x => x.SiloCode != "").FirstOrDefault();
                    if (panel != null)
                    {
                        var ExistSilo = panel.ProductStatus != ProductStatus.EmptyPayload;
                        if (isExistSilo == ExistSilo)
                        {
                            InteractingDevice.agvIsReady = true;
                            InteractingDevice.Status = DeviceStatus.Ready;
                            logger.LogDebug("人工操作准备就绪：有料仓");
                            return new Tuple<bool, string>(true, $"{toreadyStr}：{true}_{InteractingDevice.Status}");
                        }
                    }
                }
                else
                {
                    toreadyStr = "无料仓ToReady";
                    var panel = InteractingDevice.PayloadPanels.Where(x => x.SiloCode != "").FirstOrDefault();
                    if (panel == null)
                    {
                        InteractingDevice.agvIsReady = true;
                        InteractingDevice.Status = DeviceStatus.Ready;
                        logger.LogDebug("人工操作准备就绪：无料仓");
                        return new Tuple<bool, string>(true, $"{toreadyStr}：{true}_{InteractingDevice.Status}");
                    }
                    toreadyStr += $",数据料仓{panel.SiloCode}";
                }
                return new Tuple<bool, string>(false, $"{toreadyStr}，IsExistSilo与实际有无料仓不一致");
            }
        }

        public List<Panel> InitPayloadPanels()
        {
            return Panel.NoSilo.PanelForLayerFirst(DeviceDescriptor.SpindleNum.ToInt(), DeviceDescriptor.LayerLimit.ToInt(), ProductStatus.EmptyPayload);
        }

        /// <summary>
        /// 取消HIK小车未做完的任务、任务组的子任务
        /// </summary>
        /// <param name="interactingDevice"></param>
        /// <param name="deviceServiceInvokeRequest"></param>
        /// <returns></returns>
        private async Task<DeviceServiceInvokeResponse> CancelTask(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            var newmoveresult = await InteractingDevice.defaultAgvChassis.CancelLocal(InteractingDevice, deviceServiceInvokeRequest);
            string taskId = deviceServiceInvokeRequest.Params["taskCode"].ToString();
            if (newmoveresult.Code != ErrorCodes.Sys.SUCCESS)
            {
                _ = InteractingDevice.ReportingProcess($"取消任务{taskId}失败");
                return await ResponseFail(newmoveresult.Message);
            }
            else
            {
                _ = InteractingDevice.ReportingProcess($"取消任务{taskId}成功");
                return await ResponseSuccess(newmoveresult.Message);
            }
        }

        private async Task MoveTask(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapPanel item, string taskCode)
        {
            await Task.Factory.StartNew(async () =>
            {
                deviceServiceInvokeRequest.Params["MoveTargetPos"] = item.AgvPosition;
                logger.LogDebug($"\r\n AGV移动··· {item.AgvPosition}\r\n");
                _ = InteractingDevice.ReportingProcess($"AGV移动··· {item.AgvPosition}");
                var deviceOperationResponse = await MoveLocal(deviceServiceInvokeRequest);
                var errormsg = string.Format($"Move：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                logger.LogDebug(errormsg);
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_MoveNoSuccess", "MoveNoSuccess", errormsg);
                }

                if (InteractingDevice.materialType == MaterialKind.Panel)
                {
                    logger.LogDebug("\r\n 开始预准备动作5013\r\n");
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);
                    _ = InteractingDevice.ReportingProcess("开始预准备动作5013");
                }
                else
                {
                    logger.LogDebug($"\r\n 非Panel无预准备动作，materialType：{InteractingDevice.materialType}\r\n");
                }

                var moveId = InteractingDevice.publicMoveId;
                if (InteractingDevice.configExtra["AgvChassisSupplier"].ToStr() == "HikRobot")
                {
                    logger.LogDebug($"\r\n 海康小车任务特殊逻辑：moveId：{taskCode}\r\n");
                    moveId = taskCode;
                }

                logger.LogDebug($"\r\n 等待车辆到位···: {moveId}\r\n");
                deviceOperationResponse = await InteractingDevice.defaultAgvChassis.ArrivedLocal(InteractingDevice, deviceServiceInvokeRequest, moveId);
                errormsg = string.Format($"AGV到达{moveId}：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                InteractingDevice.sendCancelBeforeArrivedDevice = false;
                logger.LogDebug(errormsg);
                _ = InteractingDevice.ReportingProcess(errormsg);
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    logger.LogDebug("\r\n 等待车辆到位超时或异常··· \r\n");
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_ArrivedNoSuccess", "ArrivedNoSuccess", errormsg);
                }

                InteractingDevice.checkmoveArrived = true;
            });
        }

        private async Task PreTask(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapPanel item)
        {
            await Task.Factory.StartNew(async () =>
            {
                DateTime startTime = DateTime.Now;
                var postAndGetTimeout = InteractingDevice.configExtra["PostAndGetTimeout"].ToInt();
                var presucess = false;

                if (InteractingDevice.IsTimeout(startTime, postAndGetTimeout))
                {
                    var automsg = "Task调用Pre超时";
                    logger.LogDebug(automsg);
                    _ = InteractingDevice.ReportingProcess(automsg);
                    presucess = false;
                }
                if (item.InteractionSequence == InteractionSequence.LoadOnly || item.InteractionSequence == InteractionSequence.LoadThenUnload)
                {
                    var loadpre = await InteractingDevice.TargetDeviceOperation(deviceServiceInvokeRequest, DeviceOperationType.PrepareLoadMaterial);
                    var automsg = $"\r\n 程序调用 AutoModelTargetDeviceOperation loadpre结果：{loadpre.Code}_{loadpre.Message}\r\n ";
                    logger.LogDebug(automsg);
                    _ = InteractingDevice.ReportingProcess(automsg);
                    if (loadpre.Code == ErrorCodes.Sys.SUCCESS)
                    {
                        presucess = true;
                    }
                }
                if (item.InteractionSequence == InteractionSequence.UnloadOnly || item.InteractionSequence == InteractionSequence.UnloadThenLoad)
                {
                    var unloadpre = await InteractingDevice.TargetDeviceOperation(deviceServiceInvokeRequest, DeviceOperationType.PrepareUnloadMaterial);
                    var automsg = $"\r\n 程序调用 AutoModelTargetDeviceOperation unloadpre结果：{unloadpre.Code}_{unloadpre.Message}\r\n ";
                    logger.LogDebug(automsg);
                    _ = InteractingDevice.ReportingProcess(automsg);
                    if (unloadpre.Code == ErrorCodes.Sys.SUCCESS)
                    {
                        presucess = true;
                    }
                }

                InteractingDevice.checkPrepareSuccess = presucess;
            });
        }

        private async Task<DeviceServiceInvokeResponse> OldModelMoveTask(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapPanel item, string taskCode)
        {
            deviceServiceInvokeRequest.Params["MoveTargetPos"] = item.AgvPosition;
            logger.LogDebug($"\r\n AGV移动··· {item.AgvPosition}\r\n");
            _ = InteractingDevice.ReportingProcess($"AGV移动··· {item.AgvPosition}");
            var deviceOperationResponse = await MoveLocal(deviceServiceInvokeRequest);
            var errormsg = string.Format($"Move：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
            logger.LogDebug(errormsg);
            if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
            {
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_MoveNoSuccess", "MoveNoSuccess", errormsg);
                return deviceOperationResponse;
            }

            if (InteractingDevice.materialType == MaterialKind.Panel)
            {
                logger.LogDebug("\r\n 开始预准备动作5013\r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);
                _ = InteractingDevice.ReportingProcess("开始预准备动作5013");
            }
            else
            {
                logger.LogDebug($"\r\n 非Panel无预准备动作，materialType：{InteractingDevice.materialType}\r\n");
            }

            var moveId = InteractingDevice.publicMoveId;
            if (InteractingDevice.configExtra["AgvChassisSupplier"].ToStr() == "HikRobot")
            {
                logger.LogDebug($"\r\n 海康小车任务特殊逻辑：moveId：{taskCode}\r\n");
                moveId = taskCode;
            }

            logger.LogDebug($"\r\n 等待车辆到位···: {moveId}\r\n");
            deviceOperationResponse = await InteractingDevice.defaultAgvChassis.ArrivedLocal(InteractingDevice, deviceServiceInvokeRequest, moveId);
            errormsg = string.Format($"AGV到达{moveId}：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
            InteractingDevice.sendCancelBeforeArrivedDevice = false;
            logger.LogDebug(errormsg);
            _ = InteractingDevice.ReportingProcess(errormsg);
            if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
            {
                logger.LogDebug("\r\n 等待车辆到位超时或异常··· \r\n");
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_ArrivedNoSuccess", "ArrivedNoSuccess", errormsg);
                return deviceOperationResponse;
            }
            return deviceOperationResponse;
        }

        public async Task<DeviceServiceInvokeResponse> PlcOpertaionLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            if (!deviceServiceInvokeRequest.Params.ContainsKey("PlcCommand"))
            {
                return await ResponseFail("未发现参数PlcCommand");
            }
            //提升到固定高度
            if (deviceServiceInvokeRequest.Params["PlcCommand"].ToStr() == "adjust_height")
            {
                //清楚信号
                //ResetPlcSingnal();

                logger.LogDebug("\r\n PLC动作类型\r\n");
                //InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);
                logger.LogDebug("\r\n 提升到高度\r\n");
                //InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);
                logger.LogDebug("\r\n 开始提升\r\n");
                //InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);

                var agvMoveCan = await InteractingDevice.MonitoringSignal(4055, "等待AGV允许移动");
                if (!agvMoveCan.Item1)
                {
                    _ = InteractingDevice.ReportingProcess("底盘允许移动：4055 超时", AlarmLevel.Severe, "AEP20004");
                    return await ResponseFail(agvMoveCan.Item2);
                }
                return await ResponseSuccess("成功提升到制定位置");
            }
            //加载料箱（提升高度）
            if (deviceServiceInvokeRequest.Params["PlcCommand"].ToStr() == "load_silo")
            {
                logger.LogDebug("\r\n PLC动作类型\r\n");
                //InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);
                //是否在允许提升的位置
                var plcHeart = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveId, 4000, 1);

                logger.LogDebug("\r\n 提升到高度\r\n");
                //InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);
                logger.LogDebug("\r\n 开始提升\r\n");
                //InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);

                var agvMoveCan = await InteractingDevice.MonitoringSignal(4055, "等到AGV允许移动");
                if (!agvMoveCan.Item1)
                {
                    _ = InteractingDevice.ReportingProcess("底盘允许移动：4055 超时", AlarmLevel.Severe, "AEP20004");
                    return await ResponseFail(agvMoveCan.Item2);
                }
                return await ResponseSuccess("成功提升到制定位置");
            }
            //提升到固定层数
            if (deviceServiceInvokeRequest.Params["PlcCommand"].ToStr() == "up_silo_floor")
            {
                //清楚信号
                //ResetPlcSingnal();

                logger.LogDebug("\r\n PLC动作类型\r\n");
                //InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);
                logger.LogDebug("\r\n 提升到高度\r\n");
                //InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);
                logger.LogDebug("\r\n 开始提升\r\n");
                //InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);

                var agvMoveCan = await InteractingDevice.MonitoringSignal(4055, "等待提升到位");
                if (!agvMoveCan.Item1)
                {
                    _ = InteractingDevice.ReportingProcess("底盘允许移动：4055 超时", AlarmLevel.Severe, "AEP20004");
                    return await ResponseFail(agvMoveCan.Item2);
                }
                return await ResponseSuccess("成功提升到制定位置");
            }
            //开始推板子进buffer
            if (deviceServiceInvokeRequest.Params["PlcCommand"].ToStr() == "push_panel")
            {
                logger.LogDebug("\r\n PLC动作类型\r\n");
                //InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 6666, 4);
                //是否在允许提升的位置
                var plcHeart = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveId, 4000, 1);

                logger.LogDebug("\r\n 提升到高度\r\n");
                //InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);
                logger.LogDebug("\r\n 开始提升\r\n");
                //InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);

                var agvMoveCan = await InteractingDevice.MonitoringSignal(4055, "等待提升到位");
                if (!agvMoveCan.Item1)
                {
                    _ = InteractingDevice.ReportingProcess("底盘允许移动：4055 超时", AlarmLevel.Severe, "AEP20004");
                    return await ResponseFail(agvMoveCan.Item2);
                }
                return await ResponseSuccess("成功提升到制定位置");
            }
            //开始收板子进料仓
            if (deviceServiceInvokeRequest.Params["PlcCommand"].ToStr() == "receive_panel")
            {
                logger.LogDebug("\r\n PLC动作类型\r\n");
                //InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);
                //是否在允许提升的位置
                var plcHeart = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveId, 4000, 1);

                logger.LogDebug("\r\n 提升到高度\r\n");
                //InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);
                logger.LogDebug("\r\n 开始提升\r\n");
                //InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);

                var agvMoveCan = await InteractingDevice.MonitoringSignal(4055, "等待提升到位");
                if (!agvMoveCan.Item1)
                {
                    _ = InteractingDevice.ReportingProcess("底盘允许移动：4055 超时", AlarmLevel.Severe, "AEP20004");
                    return await ResponseFail(agvMoveCan.Item2);
                }
                return await ResponseSuccess("成功提升到制定位置");
            }
            //放下料仓
            if (deviceServiceInvokeRequest.Params["PlcCommand"].ToStr() == "unload_silo")
            {
                //清楚信号
                //ResetPlcSingnal();

                logger.LogDebug("\r\n PLC动作类型\r\n");
                //InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);
                logger.LogDebug("\r\n 提升到高度\r\n");
                //InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);
                logger.LogDebug("\r\n 开始提升\r\n");
                //InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 5013, 1);

                var agvMoveCan = await InteractingDevice.MonitoringSignal(4055, "等待提升到位");
                if (!agvMoveCan.Item1)
                {
                    _ = InteractingDevice.ReportingProcess("底盘允许移动：4055 超时", AlarmLevel.Severe, "AEP20004");
                    return await ResponseFail(agvMoveCan.Item2);
                }
                return await ResponseSuccess("成功提升到制定位置");
            }
            return await ResponseFail($"请确认参数PlcCommand是否正确：{deviceServiceInvokeRequest.Params["PlcCommand"].ToStr()}");
        }

        public async Task<DeviceServiceInvokeResponse> ReleaseAgv(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to Move!");

            if (InteractingDevice.configExtra["AgvChassisSupplier"].ToString() == "HikRobot")
            {
                if (InteractingDevice.materialType == MaterialKind.Panel)
                {
                    #region 海康小车解锁逻辑

                    var response = await InteractingDevice.defaultAgvChassis.ReleaseLocal(InteractingDevice, deviceServiceInvokeRequest);
                    if (response.Code != ErrorCodes.Sys.SUCCESS)
                    {
                        logger.LogDebug($"Hik释放或解锁调用接口，Code: {response.Code}, Message: {response.Message}");
                        _ = InteractingDevice.ReportingProcess($"小车解锁失败，Code: {response.Code}, Message: {response.Message}", AlarmLevel.Severe, "AES10015");
                        return await Response(response.Code, response.Message);
                    }
                    _ = InteractingDevice.ReportingProcess($"Hik释放或解锁调用接口成功!");

                    #endregion 海康小车解锁逻辑
                }
            }
            else if (InteractingDevice.configExtra["AgvChassisSupplier"].ToString() == "StdRobot")
            {
                var finishResult = await InteractingDevice.defaultAgvChassis.ReleaseLocal(InteractingDevice, deviceServiceInvokeRequest);
                if (finishResult.Code != ErrorCodes.Sys.SUCCESS)
                {
                    logger.LogDebug($"Std释放或解锁失败: {finishResult?.Message}");
                    _ = InteractingDevice.ReportingProcess($"Std释放或解锁失败:{finishResult?.Message}");
                    return finishResult;
                }
                else
                {
                    _ = InteractingDevice.ReportingProcess($"Std释放或解锁 调用接口成功!");
                }
            }

            return await ResponseSuccess(string.Empty);
        }
        private async Task<float> GetPanthLength(Panel panel,string itemcode)
        {
            float panthLength = 0;
            try
            {
                if (panel.PanelLength <= 0)
                {
                    if (!string.IsNullOrWhiteSpace(itemcode)
                        && DeviceDescriptor.Extra.ContainsKey("GetItemInfo"))
                    {
                        var messagea = $"从中控获取板长条件：物料号：{itemcode},{panel.ItemCode},板长：{panel.PanelLength}，配置GetItemInfo：{DeviceDescriptor.Extra["GetItemInfo"].ToStr()}";
                        logger.LogDebug(messagea);
                        _ = InteractingDevice.ReportingProcess(messagea);

                        var itemInfo = await HttpRequestInvoker.GetFromJsonAsync<MaterialPanelInfo>(string.Format(DeviceDescriptor.Extra["GetItemInfo"].ToStr(), itemcode));
                        if (itemInfo != null)
                        {
                            logger.LogDebug($"中控获取到的板长：{itemInfo.panelLength}");
                            _ = InteractingDevice.ReportingProcess($"中控获取到的板长：{itemInfo.panelLength}");
                            panthLength = itemInfo.panelLength;
                        }
                        else
                        {
                            logger.LogDebug($"中控获取到的板长：接口返回 为null");
                            _ = InteractingDevice.ReportingProcess($"中控获取到的板长：接口返回 为null");
                        }
                    }
                    else
                    {
                        var messageb = $"从中控获取板长条件不满足！！！：物料号：{itemcode},{panel.ItemCode},板长：{panel.PanelLength}，是否存在配置GetItemInfo：{DeviceDescriptor.Extra.ContainsKey("GetItemInfo")}";
                        logger.LogDebug(messageb);
                        _ = InteractingDevice.ReportingProcess(messageb);
                    }
                }
                else {
                    panthLength = panel.PanelLength;
                    _ = InteractingDevice.ReportingProcess($"中控获取到的板长，不为0：{panthLength}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"{DateTime.Now.ToLongTimeString()}--- 获取板长失败，原因：{ex.Message}", ex);
            }
            return panthLength;
        }

        private async Task<float> GetPanthLength(string itemCode)
        {
            float panthLength = 0;
            try
            {
                if (!string.IsNullOrWhiteSpace(itemCode)
                        && DeviceDescriptor.Extra.ContainsKey("GetItemInfo"))
                {
                    string urlGetItemInfo = DeviceDescriptor.Extra["GetItemInfo"].ToStr();
                    var messagea = $"从中控获取板长条件：物料号：{itemCode}，配置GetItemInfo：{urlGetItemInfo}";
                    logger.LogDebug(messagea);
                    _ = InteractingDevice.ReportingProcess(messagea);

                    var itemInfo = await HttpRequestInvoker.GetFromJsonAsync<MaterialPanelInfo>(string.Format(urlGetItemInfo, itemCode));
                    if (itemInfo != null)
                    {
                        logger.LogDebug($"中控获取到的板长：{itemInfo.panelLength}");
                        _ = InteractingDevice.ReportingProcess($"中控获取到的板长：{itemInfo.panelLength}");
                        panthLength = itemInfo.panelLength;
                    }
                    else
                    {
                        logger.LogDebug($"中控获取到的板长：接口返回 为null");
                        _ = InteractingDevice.ReportingProcess($"中控获取到的板长：接口返回 为null");
                    }
                }

            }
            catch (Exception ex)
            {
                logger.LogError($"{DateTime.Now.ToLongTimeString()}--- 获取板长失败，原因：{ex.Message}", ex);
            }
            return panthLength;
        }
    }
}
