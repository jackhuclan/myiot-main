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
    public class FrontToolAgv : DeviceShare<DefaultAgv>, IAgvDevice
    {
        private readonly ILogger<FrontToolAgv> logger;
        public readonly byte slaveId;

        public FrontToolAgv(ILogger<FrontToolAgv> logger, IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
        {
            this.logger = logger;
        }

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

                var resopnse = await WorkDetail(deviceServiceInvokeRequest);
                var message = $"\r\n Work结果：Code：{resopnse.Code}，Message：{resopnse.Message}，CurrentEventTraceId：{InteractingDevice.currentEventTraceId}\r\n";
                logger.LogDebug(message + $"_CurrentEventTraceId：{InteractingDevice.currentEventTraceId}");
                await InteractingDevice.ReportingProcess(message, resopnse.Code != ErrorCodes.Sys.SUCCESS ? AlarmLevel.Severe : AlarmLevel.Information, resopnse.Code != ErrorCodes.Sys.SUCCESS ? "AES10001" : "");
                if (resopnse.Code == ErrorCodes.Sys.SUCCESS)
                {
                    var completeScheduleTaskResponse = await InteractingDevice.ReportComplete(deviceServiceInvokeRequest);
                    logger.LogDebug($"Work完成上报：{DateTime.Now.ToLongTimeString()} - {InteractingDevice.ProductId} - {InteractingDevice.DeviceName} - {InteractingDevice.DeviceId} !Response:{JsonSerializer.Serialize(completeScheduleTaskResponse)}");
                    await InteractingDevice.ReportingProcess($"Work完成上报：{completeScheduleTaskResponse.Code}_{completeScheduleTaskResponse.Message}",
                        completeScheduleTaskResponse.Code != ErrorCodes.Sys.SUCCESS ? AlarmLevel.Severe : AlarmLevel.Information, resopnse.Code != ErrorCodes.Sys.SUCCESS ? "AES10002" : ""
                        );
                }
                else if (resopnse.Code == "ReportStartError")
                {
                    await InteractingDevice.ResetDeviceStatus();
                    InteractingDevice.agvIsReady = true;
                    logger.LogDebug($"Work开始上报,中控返回false：{resopnse.Code}，初始化设备，等待下一次分配");
                    await InteractingDevice.ReportingProcess($"Work开始上报,中控返回false：{resopnse.Code}，初始化设备，等待下一次分配");
                }
                else
                {
                    InteractingDevice.SchedulingTasks.Clear();
                    InteractingDevice.isAgvWorkFail = true;
                    var reportFailResponse = await InteractingDevice.ReportFail(deviceServiceInvokeRequest);
                    logger.LogDebug($"Work失败上报：{DateTime.Now.ToLongTimeString()} - {InteractingDevice.ProductId} - {InteractingDevice.DeviceName} - {InteractingDevice.DeviceId} !Response:{JsonSerializer.Serialize(reportFailResponse)}");
                    await InteractingDevice.ReportingProcess($"Work失败上报：{reportFailResponse.Code}_{reportFailResponse.Message}", reportFailResponse.Code != ErrorCodes.Sys.SUCCESS ? AlarmLevel.Severe : AlarmLevel.Information, resopnse.Code != ErrorCodes.Sys.SUCCESS ? "AES10003" : "");
                }
                return resopnse;
            }
            catch (Exception ex)
            {
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
                await InteractingDevice.ReportingProcess(pmessage);
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
                    await InteractingDevice.ReportingProcess(msg, AlarmLevel.Severe, "AES10004");
                    return await Response(ErrorCodes.AGV.AGV_Work_FinishedWork_Code, ErrorCodes.AGV.AGV_Work_FinishedWork_MESSAGE);
                }

                var message = $"Work开始上报···";
                logger.LogDebug(message);
                await InteractingDevice.ReportingProcess(message);

                var reportstartResponse = await InteractingDevice.ReportRunStatusStart(deviceServiceInvokeRequest);
                message = $"Work开始，上报结果：Code：{reportstartResponse.Code}，Message：{reportstartResponse.Message}";
                logger.LogDebug(message);
                await InteractingDevice.ReportingProcess(message, reportstartResponse.Code != ErrorCodes.Sys.SUCCESS ? AlarmLevel.Severe : AlarmLevel.Information,
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

                await InteractingDevice.ReportingProcess($"等待AGV机械结构准备就绪···");
                logger.LogDebug($"\r\n  等待AGV机械结构准备就绪··· \r\n");
                var mSignal = await InteractingDevice.MonitoringSignal(3505, "AGV机械结构准备就绪3505");
                if (!mSignal.Item1)
                {
                    await InteractingDevice.ReportingProcess($"等待AGV机械结构准备就绪3505超时", AlarmLevel.Severe, "AEP10001");
                    InteractingDevice.Status = DeviceStatus.Exception;
                    return await Response(ErrorCodes.Sys.FAIL, mSignal.Item2);
                }

                InteractingDevice.MoveActionStatus = AgvMoveActionStatus.ToDo;
                InteractingDevice.isWorking = true;
                this.WatchingProperties.Property("IsWorking").SetValue(true);
                InteractingDevice.Status = DeviceStatus.Working;
                InteractingDevice.spindlePosition = "";
                InteractingDevice.loadAndUnLoadLayer = -1;
                deviceServiceInvokeRequest.Params["TemporaryStorage"] = null;
                deviceServiceInvokeRequest.Params["TemporaryBelongTo"] = 0;
                deviceServiceInvokeRequest.Params["CheckMovePoint"] = InteractingDevice.configExtra["CheckMovePoint"];
                InItReSet();
                #endregion

                #region 生成动作详情即OperationList
                var operationList = GetOperationList(deviceServiceInvokeRequest, workPreCheck.Item4, workPreCheck.Item5);
                message = $"生成 OperationList，数量：{operationList.Count}";
                logger.LogDebug(message);
                await InteractingDevice.ReportingProcess(message);
                if (operationList.Count < 1)
                {
                    var operationListMsg = $"\r\n operationList  数量不正确：operationList：{operationList}，targetPoslist：{workPreCheck.Item4}，spindleBehaviorlist：{workPreCheck.Item5}\r\n";
                    logger.LogDebug(operationListMsg);
                    await InteractingDevice.ReportingProcess(operationListMsg, AlarmLevel.Severe, "AES10006");
                    return await Response(ErrorCodes.Sys.FAIL, "operationList 数量不正确");
                }
                #endregion

                var lastPosition = "";
                var deviceOperationResponse = new DeviceServiceInvokeResponse();
                var errormsg = "";
                int step = 0;
                int count = 0;
                string taskCode = "";
                string reqCode = "";
                #region 预准备动作
                if (InteractingDevice.materialType == MaterialKind.Cutter)
                {
                    #region 校验料仓二维码
                    await InteractingDevice.ReportingProcess($"料仓二维码校验开始...");

                    var checkBarcode = await CheckSiloBarcode();
                    await InteractingDevice.ReportingProcess($"料仓二维码校验：{checkBarcode.Item1}：{checkBarcode.Item2}");
                    if (!checkBarcode.Item1)
                    {
                        return await Response(ErrorCodes.Sys.FAIL, $"CheckSiloBarcode: {checkBarcode.Item2}");
                    }

                   
                    #endregion

                    logger.LogDebug($"\r\n {InteractingDevice.materialType}设置PLC {operationList[0].SpindleId} 轴预备动作 \r\n");

                    var setresult = SetPlcSingleSpindle(deviceServiceInvokeRequest, operationList[0]);

                    if (!setresult)
                    {
                        logger.LogDebug($"\r\n 设置取放料层数失败 \r\n");
                        await InteractingDevice.ReportingProcess($"\r\n 设置取放料层数失败 \r\n", AlarmLevel.Severe, "AES10007");
                        return await Response(ErrorCodes.Sys.FAIL, "设置取放料层数失败！！！！！！！！！");
                    }

                    if (InteractingDevice.materialType == MaterialKind.Cutter)
                    {
                        logger.LogDebug("\r\n 开始预准备动作 3007\r\n");
                        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3007, 1);
                        await InteractingDevice.ReportingProcess("开始预准备动作 3007");
                    }
                    else
                    {
                        logger.LogDebug($"\r\n 非Cutter无预准备动作，materialType：{InteractingDevice.materialType}\r\n");
                    }
                }
                #endregion

                #region 海康任务组模式主任务
                if (InteractingDevice.configExtra["AgvChassisSupplier"].ToString() == "HikRobot")
                {
                    taskCode = "VegaMove" + InteractingDevice.DeviceId + DateTime.Now.ToString("yyyyMMddHHmmss");
                    reqCode = taskCode + "_" + count.ToString();
                    deviceServiceInvokeRequest.Params["taskCode"] = taskCode;
                    deviceServiceInvokeRequest.Params["count"] = 0;//是否给小车下的第一个任务

                    List<string> AllTargetPos = new List<string>();
                    if (InteractingDevice.materialType == MaterialKind.CutterSilo)
                    {
                        AllTargetPos.Add(operationList[0].AgvPosition);
                        AllTargetPos.Add(deviceServiceInvokeRequest.Params["ShelfInnerPos"].ToStr().Trim());
                        AllTargetPos.Add(operationList[0].AgvPosition);
                    }
                    else if (InteractingDevice.materialType == MaterialKind.Cutter)
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

                    logger.LogDebug("\r\n 请求PLC小车需要移动 3080 \r\n");
                    await InteractingDevice.ReportingProcess("请求PLC小车需要移动 3080");
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 3080, 1);//通知PLC小车需要移动
                    var agvMoveCan = await InteractingDevice.MonitoringSignal(3580, "底盘允许移动");
                    if (!agvMoveCan.Item1)
                    {
                        await InteractingDevice.ReportingProcess("底盘允许移动：3580 超时", AlarmLevel.Severe, "AEP20004");
                        return await Response(ErrorCodes.Sys.FAIL, agvMoveCan.Item2);
                    }
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 3580, 0);
                    await InteractingDevice.ReportingProcess("收到允许移动并重置 3580");


                    //给海康小车部署任务
                    logger.LogDebug($"\r\n AGV移动··· {operationList[0].AgvPosition}\r\n");
                    await InteractingDevice.ReportingProcess($"AGV移动··· {operationList[0].AgvPosition}");
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
                    await InteractingDevice.ReportingProcess(errormsg);
                    if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                    {
                        logger.LogDebug("\r\n 等待车辆到位超时或异常··· \r\n");
                        isAllowedNextOperation = false;
                        InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_ArrivedNoSuccess", "ArrivedNoSuccess", errormsg);
                        return deviceOperationResponse;
                    }
                }
                #endregion

                #region  逐条执行operationList
                foreach (var item in operationList)
                {
                    if (InteractingDevice.CanProceedNextStep(!isAllowedNextOperation))
                    {
                        logger.LogDebug("\r\n 结束任务··· \r\n");
                        return await Response(ErrorCodes.Sys.FAIL, "结束任务:" + item.SpindleId);
                    }

                    await InteractingDevice.ReportingProcess($"上下物料类型：{InteractingDevice.materialType}");

                    InteractingDevice.moveTimeout = InteractingDevice.configExtra["MoveTimeout"].ToInt();
                    step++;
                    if (step > 1)
                    {
                        InteractingDevice.loadAndUnLoadLayer = -1;
                    }

                    reqCode = taskCode + "_" + item.SpindleId.ToString();

                    lastPosition = WatchingProperties.Property("CarCurrentPos").NewValue.ToStr();
                    var yidongmsg = "";
                    var yidong = false;
                    ////
                    //if (lastPosition != "" && lastPosition != item.AgvPosition)
                    //{
                    //    yidong = true;
                    //    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3011, 1);
                    //    await Task.Delay(1000);
                    //    logger.LogDebug("\r\n 发完机械臂回原点，等待1S\r\n");
                    //}
                    yidongmsg = $"\r\n 需要移动点位:{yidong}: CarCurrentPos：{lastPosition}，AgvPosition：{item.AgvPosition}\r\n";
                    logger.LogDebug(yidongmsg);
                    deviceServiceInvokeRequest.Params["LastPos"] = lastPosition;
                    deviceServiceInvokeRequest.Params["SpindlePosition"] = item.SpindleId;
                    deviceServiceInvokeRequest.Params["IsFirstStep"] = step == 1;
                    deviceServiceInvokeRequest.Params["Position"] = item.SpindleId;
                    deviceServiceInvokeRequest.Params["SiloPosition"] = item.SpindleId;
                    deviceServiceInvokeRequest.Params["CarCurrentPos"] = item.AgvPosition;
                    deviceServiceInvokeRequest.Params["Region"] = item.Region;
                    InteractingDevice.spindlePosition = item.SpindleId.ToStr();

                    //InteractingDevice.ReportingProcess($"开始第{item.SpindleId}轴操作/共{operationList.Count}条任务， 等待AGV机械结构准备就绪3505···");
                    //logger.LogDebug($"\r\n 开始第{item.SpindleId}轴操作/{operationList.Count}条任务， 等待AGV机械结构准备就绪3505··· \r\n");
                    //var mSignal = await InteractingDevice.MonitoringSignal(3505, "AGV机械结构准备就绪");
                    //if (!mSignal.Item1)
                    //{
                    //    return await Response(ErrorCodes.Sys.FAIL, mSignal.Item2);
                    //}

                    if (InteractingDevice.CanProceedNextStep(!isAllowedNextOperation))
                    {
                        return await Response(ErrorCodes.Sys.FAIL, "结束任务：轴预备动作之前");
                    }

                    if (InteractingDevice.materialType == MaterialKind.Cutter)
                    {   //左侧右侧上料
                        InteractingDevice.loadAndUnloadDirection = "";
                        var interactivePosition = deviceServiceInvokeRequest.Params["InteractivePosition"].ToStr();
                        var messagep = $"\r\n AGV选择左侧右侧上料3004: {interactivePosition}\r\n";
                        logger.LogDebug(messagep);
                        await InteractingDevice.ReportingProcess(messagep);
                        if (interactivePosition.ToLower() == "left")
                        {
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3004, 2);
                            InteractingDevice.loadAndUnloadDirection = "20";
                        }
                        else if (interactivePosition.ToLower() == "right")
                        {
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3004, 1);
                            InteractingDevice.loadAndUnloadDirection = "10";
                        }
                        if (step > 1)
                        {
                            logger.LogDebug($"\r\n {item.InteractionSequence}开始{InteractingDevice.materialType}设置PLC {item.SpindleId} 轴预备动作 \r\n");
                            await InteractingDevice.ReportingProcess($"\r\n {item.InteractionSequence}开始{InteractingDevice.materialType}设置PLC {item.SpindleId} 轴预备动作 \r\n");
                            var setresult = SetPlcSingleSpindle(deviceServiceInvokeRequest, item);
                            if (!setresult)
                            {
                                logger.LogDebug($"\r\n {item.InteractionSequence},位置配置失败！！！！！！！！！！ \r\n");
                                return await Response(ErrorCodes.Sys.FAIL, $"{item.InteractionSequence},位置配置失败！！！！！！！！！！");
                            }
                        }
                    }
                    if (InteractingDevice.configExtra["AgvChassisSupplier"].ToStr() == "HikRobot")
                    {
                        count++;
                        if (step > 1)
                        {
                            logger.LogDebug("\r\n HK通知PLC小车需要移动 3080 \r\n");
                            await InteractingDevice.ReportingProcess("HK通知PLC小车需要移动 3080");
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 3080, 1);//通知PLC小车需要移动
                            var agvMoveCan = await InteractingDevice.MonitoringSignal(3580, "底盘允许移动");
                            if (!agvMoveCan.Item1)
                            {
                                await InteractingDevice.ReportingProcess("底盘允许移动：3580 超时", AlarmLevel.Severe, "AEP20004");
                                return await Response(ErrorCodes.Sys.FAIL, agvMoveCan.Item2);
                            }
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 3580, 0);
                            await InteractingDevice.ReportingProcess("收到允许移动，并重置 3580");
                        }
                    }
                    else
                    {
                        logger.LogDebug("\r\n 通知PLC小车需要移动 3080 \r\n");
                        InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 3080, 1);
                        var agvMoveCan = await InteractingDevice.MonitoringSignal(3508, "底盘允许移动");
                        if (!agvMoveCan.Item1)
                        {
                            await InteractingDevice.ReportingProcess("底盘允许移动：3508 超时", AlarmLevel.Severe, "AEP20004");
                            return await Response(ErrorCodes.Sys.FAIL, agvMoveCan.Item2);
                        }
                        InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 3508, 0);
                        await InteractingDevice.ReportingProcess("收到允许移动，并重置 3508");
                    }

                    #region 串行模式移动

                    //deviceServiceInvokeRequest.Params["MoveTargetPos"] = item.AgvPosition;
                    //logger.LogDebug($"\r\n AGV移动··· {item.AgvPosition}\r\n");
                    //await InteractingDevice.ReportingProcess($"AGV移动··· {item.AgvPosition}");
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
                    //if (InteractingDevice.materialType == MaterialKind.Cutter && step == 1)
                    //{
                    //    var belongTo = deviceServiceInvokeRequest.Params["TemporaryBelongTo"].ToInt() * 10;
                    //    logger.LogDebug($"\r\n 预准备动作,层数（{belongTo}） 3008\r\n");
                    //    InteractingDevice.ReportingProcess($"\r\n 预准备动作,层数（{belongTo}） 3008\r\n");
                    //    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3008, (ushort)belongTo);

                    //    logger.LogDebug("\r\n 开始预准备动作3007\r\n");
                    //    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3007, 1);
                    //    InteractingDevice.ReportingProcess("开始预准备动作3007");
                    //}

                    //var moveId = "";
                    //if (deviceOperationResponse.Params.ContainsKey("AgvReturnTaskId"))
                    //{
                    //    moveId = deviceOperationResponse.Params["AgvReturnTaskId"].ToStr();
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

                    //if (InteractingDevice.CanProceedNextStep(!isAllowedNextOperation))
                    //{
                    //    return await Response(ErrorCodes.Sys.FAIL, "结束任务：车辆到位之后");
                    //}
                    #endregion

                    if (!InteractingDevice.configExtra.ContainsKey("UseNewModel"))
                    {
                        await InteractingDevice.ReportingProcess("没有UseNewModel参数，执行旧模式");
                        var moveresult = await OldModelMoveTask(deviceServiceInvokeRequest, item, taskCode);
                        await InteractingDevice.ReportingProcess($"没有UseNewModel参数，执行旧模式{moveresult.Code}_{moveresult.Message}");
                        if (moveresult.Code != ErrorCodes.Sys.SUCCESS)
                        {
                            return await Response(ErrorCodes.Sys.FAIL, $"没有UseNewModel参数，执行旧模式Move：false_{moveresult.Message}");
                        }
                    }
                    else
                    {
                        await InteractingDevice.ReportingProcess($"执行新模式_{InteractingDevice.configExtra["UseNewModel"].ToInt()}：pre和move并行");
                        if (InteractingDevice.configExtra["UseNewModel"].ToInt() == 1)
                        {
                            InteractingDevice.checkmoveArrived = false;
                            InteractingDevice.checkPrepareSuccess = false;
                            deviceServiceInvokeRequest.Params["IsPassPre"] = 1;
                            await MoveTask(deviceServiceInvokeRequest, item, taskCode);
                            await PreTask(deviceServiceInvokeRequest, item);
                            InteractingDevice.sendCancelBeforeArrivedDevice = false;

                            var checkresultstr = "等待线程移动和钻机Pre结果";
                            logger.LogDebug(checkresultstr);
                            await InteractingDevice.ReportingProcess(checkresultstr);
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
                            await InteractingDevice.ReportingProcess(checkresultstr);
                            if (!checkresult)
                            {
                                return await Response(ErrorCodes.Sys.FAIL, checkresultstr);
                            }
                        }
                        else
                        {
                            await InteractingDevice.ReportingProcess("UseNewModel 参数不为1");
                            return await Response(ErrorCodes.Sys.FAIL, "UseNewModel 参数不为1");
                        }
                    }

                    logger.LogDebug("\r\n ActionStatus  改为Doing\r\n");
                    InteractingDevice.MoveActionStatus = AgvMoveActionStatus.Doing;

                    deviceServiceInvokeRequest.Params["OperationListItem"] = JsonSerializer.Serialize(item);
                    if (step == 1)
                    {
                        var wait3508 = await InteractingDevice.MonitoringSignal(3508, $"step：{step}预准备动作执行完成3508");
                        if (!wait3508.Item1)
                        {
                            logger.LogDebug($"\r\n step:{step}_预准备等待：{wait3508.Item1} \r\n");
                            return await Response(ErrorCodes.Sys.FAIL, wait3508.Item2);
                        }
                    }

                    if (InteractingDevice.materialType == MaterialKind.Cutter)
                    {
                        logger.LogDebug($"告诉PLC当前第{item.SpindleId}轴");

                        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3013, (ushort)item.SpindleId);
                        //HIK小车锁车信号
                        if (InteractingDevice.configExtra["AgvChassisSupplier"].ToString() == "HikRobot"
                            || InteractingDevice.configExtra["AgvChassisSupplier"].ToString() == "StdRobot")
                        {
                            logger.LogDebug("\r\n 通知PLC锁车 3084\r\n");
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3084, 1);
                        }

                        switch (item.InteractionSequence)
                        {
                            case InteractionSequence.LoadOnly:

                                await InteractingDevice.ReportingProcess("Panel开始上料 LoadMaterialBehavior");
                                deviceOperationResponse = await InteractingDevice.agvToDeviceAgvLoadPolicy.LoadMaterialBehavior(deviceServiceInvokeRequest);
                                errormsg = string.Format($"Panel上料结束_LoadMaterialBehavior：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                                logger.LogDebug(errormsg);
                                await InteractingDevice.ReportingProcess(errormsg);
                                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                                {
                                    isAllowedNextOperation = false;
                                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_LoadMaterialBehaviorNoSuccess", "LoadMaterialBehaviorNoSuccess", errormsg);
                                    break;
                                }
                                break;

                            case InteractionSequence.UnloadOnly:

                                logger.LogDebug($"\r\n 开始下料，Request.PayloadCutterTrays:{JsonSerializer.Serialize(deviceServiceInvokeRequest.PayloadCutterTrays)} \r\n");

                                await InteractingDevice.ReportingProcess("开始下料 UnloadMaterialBehavior");
                                deviceOperationResponse = await InteractingDevice.agvToDeviceAgvUnloadPolicy.UnloadMaterialBehavior(deviceServiceInvokeRequest);
                                errormsg = string.Format($"下料结束_UnloadMaterialBehavior：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                                logger.LogInformation(errormsg);
                                await InteractingDevice.ReportingProcess(errormsg);
                                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                                {
                                    isAllowedNextOperation = false;
                                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_UnloadMaterialBehaviorNoSuccess", "UnloadMaterialBehaviorNoSuccess", errormsg);
                                    break;
                                }
                                break;

                            case InteractionSequence.UnloadThenLoad:

                                await InteractingDevice.ReportingProcess($" 开始下料 UnloadMaterialBehavior");
                                deviceOperationResponse = await InteractingDevice.agvToDeviceAgvUnloadPolicy.UnloadMaterialBehavior(deviceServiceInvokeRequest);
                                errormsg = string.Format($"下料完成_UnloadMaterialBehavior：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                                logger.LogInformation(errormsg);
                                await InteractingDevice.ReportingProcess(errormsg);
                                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                                {
                                    isAllowedNextOperation = false;
                                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_UnloadMaterialBehaviorNoSuccess", "UnloadMaterialBehaviorNoSuccess", errormsg);
                                    break;
                                }

                                //左侧右侧上料
                                var interactivePosition = deviceServiceInvokeRequest.Params["InteractivePosition"].ToStr();
                                logger.LogDebug($"\r\n 第二段选择左侧右侧上料: {interactivePosition}\r\n");
                                if (interactivePosition.ToLower() == "left")
                                {
                                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3004, 2);
                                }
                                else if (interactivePosition.ToLower() == "right")
                                {
                                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3004, 1);
                                }

                                logger.LogDebug($"\r\n {InteractingDevice.materialType}第二段设置PLC {item.SpindleId} 轴预备动作 \r\n");
                                var setresulttwo = SetPlcSingleSpindle(deviceServiceInvokeRequest, item, 2);//1PLC上料2代表PLC下料
                                if (!setresulttwo)
                                {
                                    return await Response(ErrorCodes.Sys.FAIL, $"第二段设置PLC： {item.SpindleId} 轴预备动作失败");
                                }

                                if (InteractingDevice.CanProceedNextStep(!isAllowedNextOperation))
                                {
                                    return await Response(ErrorCodes.Sys.FAIL, "结束任务：UnloadMaterialBehavior 之前");
                                }
                                logger.LogDebug($"\r\n 第二步骤，开始上料，PayloadCutterTrays:{JsonSerializer.Serialize(deviceServiceInvokeRequest.PayloadCutterTrays)} \r\n");

                                await InteractingDevice.ReportingProcess("开始上料 LoadMaterialBehavior");
                                deviceOperationResponse = await InteractingDevice.agvToDeviceAgvLoadPolicy.LoadMaterialBehavior(deviceServiceInvokeRequest);
                                errormsg = string.Format($"上料结束_LoadMaterialBehavior：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                                logger.LogDebug(errormsg);
                                await InteractingDevice.ReportingProcess(errormsg);
                                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                                {
                                    isAllowedNextOperation = false;
                                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_LoadMaterialBehaviorNoSuccess", "LoadMaterialBehaviorNoSuccess", errormsg);
                                    break;
                                }

                                break;

                            default:
                                isAllowedNextOperation = false;
                                deviceOperationResponse.Code = ErrorCodes.Sys.FAIL;
                                errormsg = "InteractionSequence：交互类型参数错误：" + item.InteractionSequence;
                                logger.LogDebug(errormsg);
                                break;
                        }
                        //HIK小车重置锁车信号
                        if (InteractingDevice.configExtra["AgvChassisSupplier"].ToString() == "HikRobot"
                            || InteractingDevice.configExtra["AgvChassisSupplier"].ToString() == "StdRobot")
                        {
                            logger.LogDebug("\r\n 通知PLC解除锁车 3084\r\n");
                            await InteractingDevice.ReportingProcess(" 通知PLC解除锁车 3084\r\n");
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3084, 0);
                        }
                    }
                    else if (InteractingDevice.materialType == MaterialKind.CutterSilo)
                    {
                        switch (item.InteractionSequence)
                        {
                            case InteractionSequence.LoadOnly:
                                await InteractingDevice.ReportingProcess("Silo开始上料 LoadMaterialBehavior");

                                deviceOperationResponse = await InteractingDevice.agvToDeviceAgvLoadPolicy.LoadMaterialBehavior(deviceServiceInvokeRequest);
                                errormsg = string.Format($"Silo上料结束_LoadMaterialBehavior：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                                logger.LogDebug(errormsg);
                                await InteractingDevice.ReportingProcess(errormsg);
                                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                                {
                                    isAllowedNextOperation = false;
                                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_LoadMaterialBehaviorNoSuccess", "LoadMaterialBehaviorNoSuccess", errormsg);
                                    break;
                                }
                                break;

                            case InteractionSequence.UnloadOnly:
                                logger.LogDebug($"\r\n 开始下料，Request.PayloadCutterTrays:{JsonSerializer.Serialize(deviceServiceInvokeRequest.PayloadCutterTrays)} \r\n");
                                await InteractingDevice.ReportingProcess("开始下料 UnloadMaterialBehavior");
                                deviceOperationResponse = await InteractingDevice.agvToDeviceAgvUnloadPolicy.UnloadMaterialBehavior(deviceServiceInvokeRequest);
                                errormsg = string.Format($"下料结束_UnloadMaterialBehavior：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                                logger.LogInformation(errormsg);
                                await InteractingDevice.ReportingProcess(errormsg);
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
                #endregion

                #region 所有轴动作完成解锁小车
                if (InteractingDevice.materialType == MaterialKind.Cutter)
                {
                    if (InteractingDevice.configExtra["AgvChassisSupplier"].ToString() == "HikRobot")
                    {
                        #region 海康小车解锁逻辑
                        taskCode = DateTime.Now.ToString("yyyyMMddHHmmss");
                        deviceServiceInvokeRequest.Params["Unlock"] = true;
                        deviceServiceInvokeRequest.Params["taskCode"] = taskCode;
                        deviceServiceInvokeRequest.Params["reqCode"] = taskCode + "_" + "Unlock";
                        deviceServiceInvokeRequest.Params["MoveTargetPos"] = operationList[operationList.Count() - 1].AgvPosition;
                        logger.LogDebug("\r\n Operations运行结束，解锁小车");
                        InteractingDevice.ReportingProcess($"运行结束，解锁小车");

                        DeviceServiceInvokeResponse response = await MoveLocal(deviceServiceInvokeRequest);
                        if (response.Code != ErrorCodes.Sys.SUCCESS)
                        {
                            isAllowedNextOperation = false;
                            logger.LogDebug($"小车解锁失败，Code: {response.Code}, Message: {response.Message}");
                            InteractingDevice.ReportingProcess($"小车解锁失败，Code: {response.Code}, Message: {response.Message}", AlarmLevel.Severe, "AES10015");
                            return await Response(response.Code, response.Message);
                        }
                        #endregion
                    }
                }
                #endregion

                #region 结束通知结束信号
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
                        return await Response(end.Code, "");
                    }

                    InteractingDevice.ReportingProcess("钻机任务结束，并且回原点3011,3012");
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3011, 1);
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3012, 1);

                    var isExit = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveId, 3516, 1);
                    if (isExit[0] == 1)
                    {
                        InteractingDevice.ReportingProcess("暂存位开始放回料仓");
                        var tsSignal = await InteractingDevice.MonitoringSignal(3518, "暂存位开始放回料仓");
                        if (!tsSignal.Item1)
                        {
                            return await Response(ErrorCodes.Sys.FAIL, tsSignal.Item2);
                        }
                        var siloPosition = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveId, 3519, 1);
                        InteractingDevice.ReportingProcess($"暂存位刀盒放在{siloPosition}个");
                        logger.LogDebug($"\r\n 暂存位刀盒放在{siloPosition}个\r\n");

                        var operationEntity = deviceServiceInvokeRequest.Params["TemporaryStorage"] as SwapTray;
                        if (operationEntity == null)
                        {
                            return await Response(ErrorCodes.Sys.FAIL, "暂存位刀盒数据异常");
                        }
                        var ceng = (int)(siloPosition.ToInt() / 10);
                        var index = siloPosition.ToInt() % 10;

                        logger.LogDebug($"\r\n 转换后的序号：{(ceng - 1) * 10 + index}个\r\n");
                        var tray = InteractingDevice.PayloadCutterTrays.FirstOrDefault(x => x != null && x.Z == (ceng - 1) && x.IndexOnLayer == index);
                        if (tray == null) return await Response(ErrorCodes.Sys.FAIL, $"最后一步，获取实体失败null，层：{ceng}，序号：{index}");
                        tray.Status = CutterTrayStatus.Old;
                        tray.TrayCode = operationEntity.TrayList[0].TrayCode;
                        tray.ItemCode = operationEntity.TrayList[0].ItemCode;
                    }

                    var drillSignal = await InteractingDevice.MonitoringSignal(3511, "PLC钻机任务结束");
                    if (!drillSignal.Item1)
                    {
                        return await Response(ErrorCodes.Sys.FAIL, drillSignal.Item2);
                    }

                    deviceServiceInvokeRequest.Params["TemporaryStorage"] = null;
                    logger.LogDebug("\r\n IsWorking 改为false \r\n");
                    WatchingProperties.Property("IsWorking").SetValue(false);
                    InteractingDevice.isWorking = false;
                    InteractingDevice.Status = DeviceStatus.Ready;
                }
                #endregion

                #region 重置信号
                InItReSet();
                //InteractingDevice.ReportingProcess("开始结束重置信号");
                //logger.LogDebug($"\r\n 结束重置:3002-3012 \r\n");
                //var splinedOperation3002_3012 = new ushort[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveId, 3002, splinedOperation3002_3012);
                //logger.LogDebug($"\r\n 结束重置:3020-3028 \r\n");
                //var jieshu3020_3028 = new ushort[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveId, 3020, jieshu3020_3028);
                //InteractingDevice.ReportingProcess("结束重置信号");
                #endregion
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
            InteractingDevice.ReportingProcess("重置信号");
            logger.LogDebug($"\r\n 重置:3002-3012 \r\n");
            var splinedOperation3002_3012 = new ushort[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveId, 3002, splinedOperation3002_3012);
            logger.LogDebug($"\r\n 重置:3020-3028 \r\n");
            var jieshu3020_3028 = new ushort[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveId, 3020, jieshu3020_3028);
            logger.LogDebug($"\r\n 重置:3506-3519 \r\n");
            var reset3506_3519 = new ushort[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveId, 3506, reset3506_3519);

            logger.LogDebug($"\r\n 重置:3080-3084 \r\n");
            var reset3080_3084 = new ushort[] { 0, 0, 0, 0, 0 };
            InteractingDevice.modbusIpMaster.WriteMultipleRegisters(slaveId, 3506, reset3080_3084);

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
            if (targetPoslist.Length * 3 != spindleBehaviorlist.Length)//每个轴三个区域
            {
                message = $"\r\n Spindles 和 SpindleBehavior 数量不一致\r\n";
                logger.LogDebug(message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_spindleBehaviorError", "spindleBehaviorError", "Spindles 和 SpindleBehavior 数量不一致");
                return new Tuple<bool, string, string, string[], string[]>(false, ErrorCodes.Sys.FAIL, message, targetPoslist, spindleBehaviorlist);
            }
            return new Tuple<bool, string, string, string[], string[]>(true, ErrorCodes.Sys.SUCCESS, message, targetPoslist, spindleBehaviorlist);
        }

        private List<SwapTray> GetOperationList(DeviceServiceInvokeRequest deviceServiceInvokeRequest, string[] targetPoslist, string[] spindleBehaviorlist)
        {
            var convertedBehavior = (InteractionBehavior)deviceServiceInvokeRequest.CallerRequestInteractionBehavior;
            var operationList = new List<SwapTray>();
            var splideIndex = 0;
            logger.LogDebug($"\r\n targetPoslist :{string.Join(',', targetPoslist)}\r\n");
            logger.LogDebug($"\r\n spindleBehaviorlist :{string.Join(',', spindleBehaviorlist)}\r\n");
            for (int i = 0; i < spindleBehaviorlist.Length; i++)
            {
                splideIndex = (int)(i / 3) + 1;
                var targetPos = targetPoslist[splideIndex - 1];
                var behaviorNum = spindleBehaviorlist[i];
                var region = (i + 1) % 3;
                if (targetPos.ToLower() == "null" || targetPos == "0")
                {
                    continue;
                }
                if (behaviorNum.ToLower() == "-1")
                {
                    continue;
                }

                #region setbehavior

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
                    //behavior = InteractionSequence.LoadThenUnload;
                    behavior = InteractionSequence.UnloadThenLoad;
                }
                else if (behaviorNum == "3")
                {
                    behavior = InteractionSequence.UnloadThenLoad;
                }

                #endregion setbehavior

                #region SetdrillPosition

                var cutterTrayPosition = "";
                if (splideIndex == 2 || splideIndex == 4 || splideIndex == 6)
                {
                    cutterTrayPosition = (region + 3).ToStr();
                }
                else
                {
                    cutterTrayPosition = region.ToStr();
                }

                #endregion SetdrillPosition

                var swapTray = new SwapTray();
                swapTray.SpindleId = splideIndex;
                swapTray.AgvPosition = targetPos.Trim();
                swapTray.InteractionSequence = behavior;
                swapTray.CutterTrayPosition = cutterTrayPosition;
                swapTray.Region = region;

                InteractingDevice.materialType = convertedBehavior.MaterialKind;

                switch (InteractingDevice.materialType)
                {
                    case MaterialKind.Cutter:
                        var drilledPanel = deviceServiceInvokeRequest.PayloadCutterTrays.FirstOrDefault(p => p != null && p.Status == CutterTrayStatus.Old && p.IndexOnLayer == i);
                        if (drilledPanel != null)
                        {
                            swapTray.TrayList.Add(drilledPanel);
                        }
                        break;

                    case MaterialKind.PanelSilo:
                        swapTray.TrayList = deviceServiceInvokeRequest.PayloadCutterTrays;
                        break;

                    default:
                        break;
                }
                operationList.Add(swapTray);
            }

            return operationList;
        }

        private bool SetPlcSingleSpindle(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapTray item, int plcNextbehavior = 0)
        {
            var setResult = false;
            var behavior = 0;
            var ceng = 0;
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
            if (plcNextbehavior > 0)
            {
                behavior = plcNextbehavior;
            }

            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3002, behavior.ToUshort());
            var setmsg = $"\r\n 设置PLC {item.SpindleId} 轴动作详情behavior:{behavior}_{item.InteractionSequence}_TemporaryBelongTo:{deviceServiceInvokeRequest.Params["TemporaryBelongTo"].ToInt()} \r\n";
            logger.LogDebug(setmsg);
            InteractingDevice.ReportingProcess(setmsg);
            if (behavior == 1)
            {
                var itemcode = deviceServiceInvokeRequest.Params[ScheduleConstants.PARAMS_TASK_ITEM_CODE].ToStr().ToLower().Trim();
                logger.LogDebug($"交互刀具编码：{itemcode}");
                logger.LogDebug($"上料PayloadCutterTrays：{JsonSerializer.Serialize(InteractingDevice.PayloadCutterTrays)}");

                var tray = new CutterTray();
                if (deviceServiceInvokeRequest.Params["TemporaryBelongTo"].ToInt() <= 0)
                {
                    tray = InteractingDevice.PayloadCutterTrays.FirstOrDefault(x => x != null && x.Status == CutterTrayStatus.New);
                    //&& x.ItemCode.ToLower().Trim() == itemcode);
                }
                else
                {
                    tray = InteractingDevice.PayloadCutterTrays.FirstOrDefault(x => x.Z == deviceServiceInvokeRequest.Params["TemporaryBelongTo"].ToInt() - 1 && x != null && x.Status == CutterTrayStatus.New);
                    //&& x.ItemCode.ToLower().Trim() == itemcode);
                }
                if (tray != null)
                {
                    InteractingDevice.loadAndUnLoadLayer = int.Parse((tray.Z + 1).ToStr() + tray.IndexOnLayer.ToStr());
                    ceng = tray.Z + 1;
                    deviceServiceInvokeRequest.Params["TemporaryBelongTo"] = ceng;
                    var cutterTray = int.Parse(InteractingDevice.loadAndUnloadDirection + item.CutterTrayPosition);
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3003, (ushort)InteractingDevice.loadAndUnLoadLayer);
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3005, (ushort)cutterTray);
                    logger.LogDebug($"\r\n 上生设置抓取 {InteractingDevice.loadAndUnLoadLayer} 层生料3003，放到钻机 {cutterTray} 处3005\r\n");
                    InteractingDevice.ReportingProcess($"\r\n 上生设置抓取 {InteractingDevice.loadAndUnLoadLayer} 层生料3003，放到钻机 {cutterTray} 处3005\r\n");
                    setResult = true;
                }
                else
                {
                    setResult = false;
                    logger.LogDebug($"上生未能找到生料刀具TemporaryBelongTo{deviceServiceInvokeRequest.Params["TemporaryBelongTo"].ToInt()},itemcode{itemcode}, Tray null");
                    InteractingDevice.ReportingProcess($"上生未能找到生料刀具TemporaryBelongTo{deviceServiceInvokeRequest.Params["TemporaryBelongTo"].ToInt()},itemcode{itemcode}, Tray null");
                }
            }
            else if (behavior == 2)
            {
                setResult = false;
                var itemcode = deviceServiceInvokeRequest.Params[ScheduleConstants.PARAMS_TASK_ITEM_CODE].ToStr().ToLower().Trim();
                var tray = new CutterTray();
                var cutterTray = int.Parse(InteractingDevice.loadAndUnloadDirection + item.CutterTrayPosition);

                if (deviceServiceInvokeRequest.Params["TemporaryBelongTo"].ToInt() <= 0)
                {
                    tray = InteractingDevice.PayloadCutterTrays.FirstOrDefault(x => x != null && x.Status == CutterTrayStatus.New);
                    //&& x.ItemCode.ToLower().Trim() == itemcode);
                    if (tray != null)
                    {
                        ceng = tray.Z + 1;
                        var downPosition = 0;
                        var trayList = InteractingDevice.PayloadCutterTrays.Where(x => x != null && x.Z == tray.Z && x.Status == CutterTrayStatus.EmptyTray).ToList();
                        if (trayList.Count == 0)
                        {
                            downPosition = ceng * 10 + 7;
                        }
                        else
                        {
                            downPosition = (trayList[0].Z + 1) * 10 + trayList[0].IndexOnLayer;
                        }

                        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3003, (ushort)downPosition);
                        InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3005, (ushort)cutterTray);
                        var msg = $"\r\n 下熟逻辑1设置抓取钻机 {cutterTray} 熟料3005，放到 {downPosition} 处3003\r\n";
                        logger.LogDebug(msg);
                        InteractingDevice.ReportingProcess(msg);
                        deviceServiceInvokeRequest.Params["TemporaryBelongTo"] = ceng;
                        return true;
                    }
                    for (int i = 0; i < InteractingDevice.PayloadCutterTrays.Count; i++)
                    {
                        if (InteractingDevice.PayloadCutterTrays[i] != null && InteractingDevice.PayloadCutterTrays[i].Status == CutterTrayStatus.EmptyTray)
                        {
                            ceng = InteractingDevice.PayloadCutterTrays[i].Z + 1;
                            var downPosition = ceng * 10 + 7;
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3003, (ushort)downPosition);
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3005, (ushort)cutterTray);
                            var msg = $"\r\n 下熟逻辑2设置抓取钻机 {cutterTray} 熟料3005，放到暂存位 {downPosition} 处3003\r\n";
                            logger.LogDebug(msg);
                            InteractingDevice.ReportingProcess(msg);
                            deviceServiceInvokeRequest.Params["TemporaryBelongTo"] = ceng;
                            return true;
                        }
                    }
                }
                else if (deviceServiceInvokeRequest.Params["TemporaryBelongTo"].ToInt() > 0)
                {
                    for (int i = 0; i < InteractingDevice.PayloadCutterTrays.Count; i++)
                    {
                        if (InteractingDevice.PayloadCutterTrays[i] != null && InteractingDevice.PayloadCutterTrays[i].Z == deviceServiceInvokeRequest.Params["TemporaryBelongTo"].ToInt() - 1 && InteractingDevice.PayloadCutterTrays[i].Status == CutterTrayStatus.EmptyTray)
                        {
                            ceng = InteractingDevice.PayloadCutterTrays[i].Z + 1;
                            var downPosition = ceng * 10 + InteractingDevice.PayloadCutterTrays[i].IndexOnLayer;
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3003, (ushort)downPosition);
                            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3005, (ushort)cutterTray);
                            var msg = $"\r\n 下熟逻辑3设置抓取钻机 {cutterTray} 熟料3005，放到暂存位 {downPosition} 处3003\r\n";
                            logger.LogDebug(msg);
                            InteractingDevice.ReportingProcess(msg);
                            deviceServiceInvokeRequest.Params["TemporaryBelongTo"] = ceng;
                            return true;
                        }
                    }
                }
            }
            return setResult;
        }

        public Tuple<bool, string> ReadMaterialCodeLocal()
        {
            ushort[] ushortstr = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(InteractingDevice.slaveId, 3027, InteractingDevice.readCodeLength.ToUshort());//todo 是否读取编码
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
            var plcHeart = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveId, 3500, 1);
            var isauto = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveId, 3502, 1);
            var iserror = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveId, 3506, 1);

            logger.LogInformation($"给PLC的心跳信号{ticks},PLC心跳值：{plcHeart[0]}，程序心跳：{InteractingDevice.heartValue}，是否自动：{isauto[0]}，是否报错：{iserror[0]}");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3000, plcHeart[0]);
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
            var carCanMoveAndFinished = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveId, 3505, 1);

            if (carCanMoveAndFinished == null)
            {
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_ReadPlcError", "ReadPlcError", "ReadPlcError:3505");
                return new Tuple<bool, string>(false, "ReadPlcError:3505");
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
                return new Tuple<bool, string>(false, "请输入物料代码");
            }
            if (agvPageEntity.LoadMaterialType == 0)
            {
                agvPageEntity.MaterialCode = "";
            }

            if (agvPageEntity.StartLayer + agvPageEntity.LoadLayerCount > InteractingDevice.DeviceDescriptor.LayerLimit)
            {
                return new Tuple<bool, string>(false, $"层数+加载数量不能大于{InteractingDevice.DeviceDescriptor.LayerLimit}层");
            }

            List<string> trayCodes = Enumerable.Repeat<string>(string.Empty, agvPageEntity.LoadLayerCount).ToList();
            if (agvPageEntity.LoadMaterialType > 0)
            {
                //如果原状态是0-没有料仓的状态，调整为1-空料仓
                InteractingDevice.PayloadPanels.Where(x => x.ProductStatus == ProductStatus.EmptyPayload).ToList()
                    .ForEach(p => p.ProductStatus = ProductStatus.EmptySiloBox);

                //获取板料编号
                trayCodes = Enumerable.Repeat<string>(Guid.NewGuid().ToString(), agvPageEntity.LoadLayerCount).ToList();
            }

            var productStatus = CutterTrayStatus.EmptyTray;
            if (agvPageEntity.LoadMaterialType == 1)
            {
                productStatus = CutterTrayStatus.New;
            }
            else if (agvPageEntity.LoadMaterialType == 2)
            {
                productStatus = CutterTrayStatus.Old;
            }
            else
            {
                return new Tuple<bool, string>(false, "板料类型不正确");
            }
            for (var i = 0; i < agvPageEntity.LoadLayerCount && trayCodes != null && trayCodes.Count == agvPageEntity.LoadLayerCount; i++)
            {
                // 料仓号
                InteractingDevice.PayloadCutterTrays[i + agvPageEntity.StartLayer].SiloCode = agvPageEntity.SiloCode;
                InteractingDevice.PayloadCutterTrays[i + agvPageEntity.StartLayer].TrayCode = trayCodes[i];
                InteractingDevice.PayloadCutterTrays[i + agvPageEntity.StartLayer].ItemCode = agvPageEntity.MaterialCode;
                InteractingDevice.PayloadCutterTrays[i + agvPageEntity.StartLayer].Status = productStatus;
            }
            await InteractingDevice.PayloadPanels.RaiseCollectionChangedEvent(InteractingDevice.DeviceId);
            return new Tuple<bool, string>(true, "");
        }

        public string QueryExternalMaterialLocal()
        {
            var allstr = "";
            for (int i = 0; i < InteractingDevice.PayloadCutterTrays.Count; i++)
            {
                allstr += $"第{i + 1}个：{JsonSerializer.Serialize(InteractingDevice.PayloadCutterTrays[i])}" + Environment.NewLine;
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
            //int layer, int rows, int columns
            InteractingDevice.PayloadCutterTrays.Clear();
            InteractingDevice.PayloadCutterTrays.AddRange(CutterTray.InitializeCutterSilo(DeviceDescriptor.Extra["Layer"].ToInt(), DeviceDescriptor.Extra["Rows"].ToInt(), DeviceDescriptor.Extra["Columns"].ToInt()));
        }

        public void InitNoSiloLocal()
        {
            //PayloadCutterTrays.Clear();
            //PayloadCutterTrays.AddRange(CutterTray.no .NoSiloSpindleFirst(DeviceDescriptor.SpindleNum.ToInt() * DeviceDescriptor.LayerLimit.ToInt(), DeviceDescriptor.LayerLimit.ToInt()));
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
            throw new NotImplementedException();
        }

        public Task<DeviceServiceInvokeResponse> PlcOpertaionLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest) => throw new NotImplementedException();
        public Tuple<bool, string> ReadMaterialCodeLocal(int startIndex) => throw new NotImplementedException();
        public string ScannigConvertL(string Scannignumber) => throw new NotImplementedException();
        private async Task<ValueTuple<bool, string>> CheckSiloBarcode()
        {
            var enableCheck = await InteractingDevice.modbusIpMaster.ReadHoldingRegistersAsync(slaveId, 3581, 1);
            if (enableCheck[0] == 0)
            {
                return new ValueTuple<bool, string>(true, "料仓二维码校验没有开启，无需校验！");
            }

            await InteractingDevice.modbusIpMaster.WriteSingleRegisterAsync(slaveId, 3083, 1);

            await InteractingDevice.ReportingProcess($"等待可以读取二维码信号... \r\n");

            var waitReadBarcode = await InteractingDevice.MonitoringSignal(3584, "等待可以读取料仓二维码信号");

            if (!waitReadBarcode.Item1)
            {
                return new ValueTuple<bool, string>(false, "等待可以读取料仓二维码信号异常!");
            }

            string barcodeString = string.Empty;
            string sysBarcode = InteractingDevice.PayloadPanels?.FirstOrDefault()?.SiloCode!;
            var barcode = (await InteractingDevice.modbusIpMaster.ReadHoldingRegistersAsync(slaveId, 3350, 5)).ConvertUshortArrayToStr();

            if (barcode != null && barcode.Length > 0)
            {
                barcodeString = string.Join("", barcode.Select(x => x.ToString()));
                if (barcodeString.Length > 0)
                {

                    if (barcodeString.Equals(sysBarcode))
                    {
                        await InteractingDevice.modbusIpMaster.WriteSingleRegisterAsync(slaveId, 3586, 1);
                        return new ValueTuple<bool, string>(true, $"料仓二维码校验成功，料仓二维码为：{barcodeString},程序料仓码：{sysBarcode}");
                    }
                    else
                    {
                        await InteractingDevice.modbusIpMaster.WriteSingleRegisterAsync(slaveId, 3586, 2);
                    }
                }
            }

            return new ValueTuple<bool, string>(false, $"料仓二维码校验失败,读取到的料仓二维码为：{barcodeString},程序料仓码：{sysBarcode}!");
        }
        private async Task<DeviceServiceInvokeResponse> OldModelMoveTask(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapTray item, string taskCode)
        {
            deviceServiceInvokeRequest.Params["MoveTargetPos"] = item.AgvPosition;
            logger.LogDebug($"\r\n AGV移动··· {item.AgvPosition}\r\n");
            await InteractingDevice.ReportingProcess($"AGV移动··· {item.AgvPosition}");
            var deviceOperationResponse = await MoveLocal(deviceServiceInvokeRequest);
            var errormsg = string.Format($"Move：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
            logger.LogDebug(errormsg);
            if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
            {
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_MoveNoSuccess", "MoveNoSuccess", errormsg);
                return deviceOperationResponse;
            }

            if (InteractingDevice.materialType == MaterialKind.Cutter)
            {
                logger.LogDebug("\r\n 开始预准备动作 3008\r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveId, 3008, 1);
                await InteractingDevice.ReportingProcess("开始预准备动作 3008");
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
            await InteractingDevice.ReportingProcess(errormsg);
            if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
            {
                logger.LogDebug("\r\n 等待车辆到位超时或异常··· \r\n");
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_ArrivedNoSuccess", "ArrivedNoSuccess", errormsg);
                return deviceOperationResponse;
            }
            return deviceOperationResponse;
        }
        private async Task MoveTask(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapTray item, string taskCode)
        {
            Task.Factory.StartNew(async () =>
            {
                deviceServiceInvokeRequest.Params["MoveTargetPos"] = item.AgvPosition;
                logger.LogDebug($"\r\n AGV移动··· {item.AgvPosition}\r\n");
                await InteractingDevice.ReportingProcess($"AGV移动··· {item.AgvPosition}");
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
                    await InteractingDevice.ReportingProcess("开始预准备动作5013");
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

                logger.LogDebug(errormsg);
                await InteractingDevice.ReportingProcess(errormsg);
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    logger.LogDebug("\r\n 等待车辆到位超时或异常··· \r\n");
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_ArrivedNoSuccess", "ArrivedNoSuccess", errormsg);
                }

                InteractingDevice.checkmoveArrived = true;
            });
        }


        private async Task PreTask(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapTray item)
        {
            Task.Factory.StartNew(async () =>
            {
                DateTime startTime = DateTime.Now;
                var postAndGetTimeout = InteractingDevice.configExtra["PostAndGetTimeout"].ToInt();
                var presucess = false;

                if (InteractingDevice.IsTimeout(startTime, postAndGetTimeout))
                {
                    var automsg = "Task调用Pre超时";
                    logger.LogDebug(automsg);
                    await InteractingDevice.ReportingProcess(automsg);
                    presucess = false;
                }
                if (item.InteractionSequence == InteractionSequence.LoadOnly || item.InteractionSequence == InteractionSequence.LoadThenUnload)
                {
                    var loadpre = await InteractingDevice.TargetDeviceOperation(deviceServiceInvokeRequest, DeviceOperationType.PrepareLoadMaterial);
                    var automsg = $"\r\n 程序调用 AutoModelTargetDeviceOperation loadpre结果：{loadpre.Code}_{loadpre.Message}\r\n ";
                    logger.LogDebug(automsg);
                    await InteractingDevice.ReportingProcess(automsg);
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
                    await InteractingDevice.ReportingProcess(automsg);
                    if (unloadpre.Code == ErrorCodes.Sys.SUCCESS)
                    {
                        presucess = true;
                    }
                }

                InteractingDevice.checkPrepareSuccess = presucess;
            });
        }
    }
}
