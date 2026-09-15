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
    public class ShelfSiloAgv : DeviceShare<DefaultAgv>, IAgvDevice
    {
        private readonly ILogger<ShelfSiloAgv> logger;
        public readonly byte slaveId;

        public ShelfSiloAgv(ILogger<ShelfSiloAgv> logger, IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
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
            InteractingDevice.sendCancelBeforeArrivedDevice = true;

            var resopnse = await WorkDetail(deviceServiceInvokeRequest);
            var message = $"\r\n Work结果：Code：{resopnse.Code}，Message：{resopnse.Message}，CurrentEventTraceId：{InteractingDevice.currentEventTraceId}\r\n";
            logger.LogDebug(message);
            InteractingDevice.ReportingProcess(message);
            if (resopnse.Code == ErrorCodes.Sys.SUCCESS)
            {
                var completeScheduleTaskResponse = await InteractingDevice.ReportComplete(deviceServiceInvokeRequest);
                string successMessage = $"Work完成上报：{DateTime.Now.ToLongTimeString()} - {InteractingDevice.ProductId} - {InteractingDevice.DeviceName} - {InteractingDevice.DeviceId} !Response:{JsonSerializer.Serialize(completeScheduleTaskResponse)}";
                InteractingDevice.ReportingProcess(successMessage);
                logger.LogDebug(successMessage);
            }
            else
            {
                InteractingDevice.isAgvWorkFail = true;
                var completeScheduleTaskResponse = await InteractingDevice.ReportFail(deviceServiceInvokeRequest);
                string failMessage = $"Work失败上报：{DateTime.Now.ToLongTimeString()} - {InteractingDevice.ProductId} - {InteractingDevice.DeviceName} - {InteractingDevice.DeviceId} !Response:{JsonSerializer.Serialize(completeScheduleTaskResponse)}";
                InteractingDevice.ReportingProcess(failMessage);
                logger.LogDebug(failMessage);
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
                InteractingDevice.isWorking = true;
                this.WatchingProperties.Property("IsWorking").SetValue(true);
                InteractingDevice.Status = DeviceStatus.Working;
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
                    if (item.InteractionSequence != InteractionSequence.LoadOnly && item.InteractionSequence != InteractionSequence.UnloadOnly)
                    {
                        logger.LogDebug($"\r\n 运料AGV 动作类型不正确:{item.InteractionSequence}··· \r\n");
                        return await Response(ErrorCodes.Sys.FAIL, $"\r\n 运料AGV 动作类型不正确:{item.InteractionSequence}··· \r\n");
                    }

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

                    deviceServiceInvokeRequest.Params["MoveTargetPos"] = item.AgvPosition;
                    logger.LogDebug($"\r\n AGV移动··· {item.AgvPosition}\r\n");

                    var agvOperation = 0;
                    if (item.InteractionSequence == InteractionSequence.LoadOnly)
                    {
                        agvOperation = (int)AgvOperationType.ToTop;
                    }
                    else if (item.InteractionSequence == InteractionSequence.UnloadOnly)
                    {
                        agvOperation = (int)AgvOperationType.ToBottom;
                    }
                    InteractingDevice.ReportingProcess($"AGV移动··· {item.AgvPosition},动作：{agvOperation}");
                    deviceOperationResponse = await MoveOperationLocal(deviceServiceInvokeRequest, agvOperation);
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

                    var moveId = InteractingDevice.publicMoveId;

                    logger.LogDebug($"\r\n 等待车辆到位···: {moveId}\r\n");
                    deviceOperationResponse = await InteractingDevice.defaultAgvChassis.ArrivedLocal(InteractingDevice, deviceServiceInvokeRequest, moveId);
                    errormsg = string.Format($"AGV到达{moveId}：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                    InteractingDevice.sendCancelBeforeArrivedDevice = false;
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

                    deviceServiceInvokeRequest.Params["OperationListItem"] = JsonSerializer.Serialize(item);

                    if (InteractingDevice.materialType == MaterialKind.PanelSilo)
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
                    InteractingDevice.Status = DeviceStatus.Ready;
                }
                InteractingDevice.ReportingProcess("开始结束重置信号");
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
        }

        public Tuple<bool, string, string, string[], string[]> WorkPreCheck(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            var message = "";

            //获取钻机在用的轴位，停用是用null表示
            var targetPoslist = deviceServiceInvokeRequest.Params["TransSpindles"].ToStr().Split(',');
            if (targetPoslist == null || targetPoslist.Length <= 0)
            {
                message = $"\r\n TransSpindles 数量为O\r\n";
                logger.LogDebug(message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_RequestParamsError", "RequestParamsError", "没有TransSpindles信息");
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
                message = $"\r\n TransSpindles 和 SpindleBehavior 数量不一致\r\n";
                logger.LogDebug(message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_spindleBehaviorError", "spindleBehaviorError", "TransSpindles 和 SpindleBehavior 数量不一致");
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
            return true;
        }

        public Tuple<bool, string> ReadMaterialCodeLocal()
        {
            return new Tuple<bool, string>(true, "");
        }

        public void SetPlcHeartLocal()
        {
        }

        public Tuple<bool, string> CheckCanMoveLocal()
        {
            return new Tuple<bool, string>(true, "");
        }

        public async Task<Tuple<bool, string>> LoadExternalMaterialLocal(AgvPageEntity agvPageEntity)
        {
            if (string.IsNullOrEmpty(agvPageEntity.MaterialCode)
                           && agvPageEntity.LoadMaterialType != 0)
            {
                return new Tuple<bool, string>(false, $"请输入物料代码:{agvPageEntity.MaterialCode}_Type:{agvPageEntity.LoadMaterialType}");
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

            List<Panel> response = new List<Panel>();
            List<string> panelCodes = Enumerable.Repeat<string>(string.Empty, agvPageEntity.LoadLayerCount).ToList(); ;
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
                    PanelWidth = agvPageEntity.PanelWidth,
                    PinOffset = agvPageEntity.PinOffset,
                    ProductStatus = productStatus,
                    SiloCode = agvPageEntity.SiloCode,
                    Position = 1,
                    BatchCode = "",
                    LotId = "",
                };
                //获取板料编号
                response = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<GetNextPanelRequest, List<Panel>>(InteractingDevice.DeviceDescriptor.Extra["GetNextPanelNumberV2"].ToStr(), request);
            }

            await InteractingDevice.PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
            {
                for (var i = 0; i < agvPageEntity.LoadLayerCount && (response != null && response.Count == agvPageEntity.LoadLayerCount || agvPageEntity.LoadMaterialType == 0); i++)
                {
                    InteractingDevice.PayloadPanels[i + agvPageEntity.StartLayer] = new Panel
                    {
                        PanelCode = response.Count > 0 ? response[i].PanelCode : string.Empty,
                        ItemCode = agvPageEntity.MaterialCode,
                        PanelWidth = agvPageEntity.PanelWidth,
                        PinOffset = agvPageEntity.PinOffset,
                        ProductStatus = productStatus,
                        SiloCode = "",
                        Layer = i + agvPageEntity.StartLayer,
                        Position = 1,//单个料仓 AGV，1
                        BatchCode = "",
                        LotId = "",
                    };
                }
            }));
            return new Tuple<bool, string>(true, "");
        }

        public string QueryExternalMaterialLocal()
        {
            var allstr = "";
            for (int i = 0; i < InteractingDevice.PayloadPanels.Count; i++)
            {
                allstr += $"第{i + 1}层：{JsonSerializer.Serialize(InteractingDevice.PayloadPanels[i])}" + Environment.NewLine;
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
                InteractingDevice.PayloadPanels.Clear();
                InteractingDevice.PayloadPanels.AddRange(Panel.HasSilo.NoPanelForSpindleFirst("", DeviceDescriptor.SpindleNum.ToInt() * DeviceDescriptor.LayerLimit.ToInt(), DeviceDescriptor.LayerLimit.ToInt()));
            }));
        }

        public void InitNoSiloLocal()
        {
            PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
            {
                PayloadPanels.Clear();
                PayloadPanels.AddRange(Panel.NoSilo.PanelForSpindleFirst(DeviceDescriptor.SpindleNum.ToInt() * DeviceDescriptor.LayerLimit.ToInt(), DeviceDescriptor.LayerLimit.ToInt()));
            }));
        }

        public async Task<DeviceServiceInvokeResponse> MoveOperationLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, int operation)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to Move!");
            try
            {
                var newmoveresult = await InteractingDevice.defaultAgvChassis.MoveOperationLocal(InteractingDevice, deviceServiceInvokeRequest, operation);
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
