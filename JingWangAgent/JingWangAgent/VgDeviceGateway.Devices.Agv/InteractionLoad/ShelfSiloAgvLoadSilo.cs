using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;
using VgDeviceGateway.Devices.Common.Agv;

namespace VgDeviceGateway.Devices.Agv.InteractionLoad
{
    public class ShelfSiloAgvLoadSilo : DeviceShare<DefaultAgv>, IAgvLoadInteraction
    {
        private readonly ILogger<ShelfSiloAgvLoadSilo> logger;

        public ShelfSiloAgvLoadSilo(ILogger<ShelfSiloAgvLoadSilo> logger, IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
        {
            this.logger = logger;
        }

        public async Task<DeviceServiceInvokeResponse> CheckDeviceStatusLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperationType)
        {
            var checkresult = CheckStatusSilo(deviceServiceInvokeRequest, deviceOperationType);
            if (!checkresult.Item1)
            {
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_PrepareLoadMaterial_CheckStatusError", "CheckStatusError", checkresult.Item3);
            }
            return await InteractingDevice.Response(checkresult.Item1 ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL, checkresult.Item3);
        }

        private Tuple<bool, string, string> CheckStatusSilo(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
        {
            if (!InteractingDevice.Engine.DeviceConnector.IsConnected)
            {
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_CheckStatus_PlcUnConnected", "PlcUnConnected", "PLC未连接");
                return new Tuple<bool, string, string>(false, ErrorCodes.Sys.PLC_UNCONNECT_CODE, ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE);
            }
            var checkresult = true;
            switch (deviceOperation)
            {
                case DeviceOperationType.PrepareLoadMaterial:
                    if (InteractingDevice.Status != DeviceStatus.Working) checkresult = false;
                    break;

                case DeviceOperationType.InvokeLoadMaterial:
                    if (InteractingDevice.Status == DeviceStatus.Exception || InteractingDevice.Status != DeviceStatus.Working) checkresult = false;
                    break;

                case DeviceOperationType.CompleteLoadMaterial:
                    if (InteractingDevice.Status == DeviceStatus.Exception || InteractingDevice.Status != DeviceStatus.Working) checkresult = false;
                    break;

                case DeviceOperationType.PrepareUnloadMaterial:
                    if (InteractingDevice.Status != DeviceStatus.Working) checkresult = false;
                    break;

                case DeviceOperationType.InvokeUnloadMaterial:
                    if (InteractingDevice.Status == DeviceStatus.Exception || InteractingDevice.Status != DeviceStatus.Working) checkresult = false;
                    break;

                case DeviceOperationType.CompleteUnloadMaterial:
                    if (InteractingDevice.Status == DeviceStatus.Exception || InteractingDevice.Status != DeviceStatus.Working) checkresult = false;
                    break;

                default:
                    checkresult = false;
                    break;
            }
            if (!checkresult)
            {
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_CheckStatus_DeviceStatusError", "DeviceStatusError", string.Format($"{deviceOperation},设备状态不正确：{InteractingDevice.Status}"));
                return new Tuple<bool, string, string>(false, ErrorCodes.Sys.FAIL, string.Format($"{deviceOperation},设备状态不正确：{InteractingDevice.Status}"));
            }
            return new Tuple<bool, string, string>(true, ErrorCodes.Sys.SUCCESS, "");
        }

        public async Task<DeviceServiceInvokeResponse> AgvLoadMaterialSelfLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
        {
            if (deviceOperation == DeviceOperationType.PrepareLoadMaterial)
            {
                return await PrepareLoadMaterialSelf(deviceServiceInvokeRequest);
            }
            else if (deviceOperation == DeviceOperationType.InvokeLoadMaterial)
            {
                return await InvokeLoadMaterialSelf(deviceServiceInvokeRequest);
            }
            else if (deviceOperation == DeviceOperationType.CompleteLoadMaterial)
            {
                return await CompleteLoadMaterialSelf(deviceServiceInvokeRequest);
            }
            return await Response(ErrorCodes.Sys.FAIL, $"AgvLoadMaterialSelfLocal 参数类型不正确deviceOperation：{deviceOperation}");
        }

        private async Task<DeviceServiceInvokeResponse> PrepareLoadMaterialSelf(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            return await PrepareLoadForSilo(deviceServiceInvokeRequest);
        }

        private async Task<DeviceServiceInvokeResponse> InvokeLoadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            return await InvokeLoadForSilo(request);
        }

        public async Task<DeviceServiceInvokeResponse> CompleteLoadMaterialSelf(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            return await CompleteLoadForSilo(deviceServiceInvokeRequest);
        }

        private async Task<DeviceServiceInvokeResponse> PrepareLoadForSilo(DeviceServiceInvokeRequest request)
        {
            try
            {
                if (InteractingDevice.configExtra["AgvChassisSupplier"].ToStr() == "AjwRobot")
                {
                    if (!request.Params.ContainsKey("TransActionPos"))
                    {
                        return await InteractingDevice.ResponseFail("参数没有：TransActionPos");
                    }
                    var actionPos = request.Params["TransActionPos"].ToStr().Trim();
                    logger.LogDebug($"AGV移动到插齿区动作点:{actionPos}");//move 到插齿区内点
                    request.Params["MoveTargetPos"] = actionPos;
                    var actionPosResponse = await InteractingDevice.defaultAgvChassis.MoveOperationLocal(InteractingDevice, request, (int)AgvOperationType.ToTop);
                    var actionPosResponseerrormsg = string.Format($"Move：AGV移动到插齿区动作点{actionPos}_{actionPosResponse.Code}_{actionPosResponse.Message}");
                    logger.LogDebug(actionPosResponseerrormsg);
                    if (actionPosResponse.Code != ErrorCodes.Sys.SUCCESS)
                    {
                        InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_MoveNoSuccess", "MoveNoSuccess", actionPosResponseerrormsg);
                        return actionPosResponse;
                    }

                    var actionPosmoveId = InteractingDevice.publicMoveId;
                    logger.LogDebug($"\r\n 等待车辆到达插齿动作点···:{actionPosmoveId} \r\n");
                    actionPosResponse = await InteractingDevice.defaultAgvChassis.ArrivedLocal(InteractingDevice, request, actionPosmoveId);
                    actionPosResponseerrormsg = string.Format($"Arrived：{actionPosmoveId}_{actionPosResponse.Code}_{actionPosResponse.Message}");
                    if (actionPosResponse.Code != ErrorCodes.Sys.SUCCESS)
                    {
                        InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_ArrivedNoSuccess", "ArrivedNoSuccess", actionPosResponseerrormsg);
                        return actionPosResponse;
                    }
                }
                if (!request.Params.ContainsKey("TransShelfInnerPos"))
                {
                    return await InteractingDevice.ResponseFail("参数传值不正确：没有TransShelfInnerPos");
                }
                var innerPos = request.Params["TransShelfInnerPos"].ToStr().Trim();
                logger.LogDebug($"AGV移动到插齿区内点:{innerPos}");//move 到插齿区内点
                request.Params["MoveTargetPos"] = innerPos;
                var deviceOperationResponse = await InteractingDevice.defaultAgvChassis.MoveOperationLocal(InteractingDevice, request, (int)AgvOperationType.ToBottom);
                var errormsg = string.Format($"Move：AGV移动到插齿区内点{innerPos}_{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                logger.LogDebug(errormsg);
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_MoveNoSuccess", "MoveNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                var moveId = InteractingDevice.publicMoveId;

                logger.LogDebug($"\r\n 等待车辆到达插齿内点···:{moveId} \r\n");
                deviceOperationResponse = await InteractingDevice.defaultAgvChassis.ArrivedLocal(InteractingDevice, request, moveId);
                errormsg = string.Format($"Arrived：{moveId}_{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_ArrivedNoSuccess", "ArrivedNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                //logger.LogDebug("\r\n AGV到达叉齿区告知PLC\r\n");
                //InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5063, 1);

                return await InteractingDevice.ResponseSuccess();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_PrepareLoadForShelf_Exception", "Exception", ex.Message);
                return await InteractingDevice.ResponseFail(ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        private async Task<DeviceServiceInvokeResponse> InvokeLoadForSilo(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogDebug("\r\n 开始 InvokeLoadForShelf \r\n");
                var item = JsonSerializer.Deserialize<SwapPanel>(request.Params["OperationListItem"].ToStr());

                //收到可以离开
                logger.LogDebug("\r\n 收到AGV可以离开插齿 ActionStatus  改为Done \r\n");
                InteractingDevice.MoveActionStatus = AgvMoveActionStatus.Done;

                if (!request.Params.ContainsKey("TransShelfAgvIdlePos"))
                {
                    return await InteractingDevice.ResponseFail("参数传值不正确：没有 TransShelfAgvIdlePos");
                }
                var idlePos = request.Params["TransShelfAgvIdlePos"].ToStr().Trim();
                logger.LogDebug($"AGV移动到空闲点:{idlePos}");
                request.Params["MoveTargetPos"] = idlePos;
                var deviceOperationResponse = await InteractingDevice.defaultAgvChassis.MoveOperationLocal(InteractingDevice, request, (int)AgvOperationType.ToBottom);
                var errormsg = string.Format($"Move：AGV移动到空闲点{idlePos}_{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                logger.LogDebug(errormsg);
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_MoveNoSuccess", "MoveNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                var moveId = InteractingDevice.publicMoveId;

                //等待车辆到外点
                logger.LogDebug($"\r\n 等待车辆到达空闲···{moveId} \r\n");
                deviceOperationResponse = await InteractingDevice.defaultAgvChassis.ArrivedLocal(InteractingDevice, request, moveId);
                errormsg = string.Format($"Arrived：空闲点{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_ArrivedNoSuccess", "ArrivedNoSuccess", errormsg);
                    return deviceOperationResponse;
                }
                //logger.LogDebug($"AGV移动到插齿区外点:{item.AgvPosition}");
                //request.Params["MoveTargetPos"] = item.AgvPosition;
                //var deviceOperationResponse = await InteractingDevice.defaultAgvChassis.MoveOperationLocal(InteractingDevice, request, (int)AgvOperationType.ToBottom);
                //var errormsg = string.Format($"Move：AGV移动到插齿区外点{item.AgvPosition}_{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                //logger.LogDebug(errormsg);
                //if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                //{
                //    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_MoveNoSuccess", "MoveNoSuccess", errormsg);
                //    return deviceOperationResponse;
                //}

                //var moveId = InteractingDevice.publicMoveId;

                ////等待车辆到外点
                //logger.LogDebug($"\r\n 等待车辆到达插齿外点···{moveId} \r\n");
                //deviceOperationResponse = await InteractingDevice.defaultAgvChassis.ArrivedLocal(InteractingDevice, request, moveId);
                //errormsg = string.Format($"Arrived：外点{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                //if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                //{
                //    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_ArrivedNoSuccess", "ArrivedNoSuccess", errormsg);
                //    return deviceOperationResponse;
                //}

                logger.LogDebug("\r\n 设置上料的板料信息\r\n");
                InteractingDevice.ReportingProcess("设置上料的板料信息");
                var result = SetLoadingSilo(request, item);
                if (!result)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_SetLoadingPanel_SetLoadingPanelError", "SetLoadingPanelError", "SetLoadingPanel false");
                    InteractingDevice.ReportingProcess("设置上料的板料信息_异常");
                    return await InteractingDevice.ResponseFail("设置上料的板料信息 异常");
                }
                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                logger.LogDebug($"\r\n  InvokeLoadForShelf异常: {ex.Message}\r\n");
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_InvokeLoadForDrill_Exception", "Exception", ex.Message);
                return await InteractingDevice.ResponseFail(ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
                logger.LogDebug("\r\n 结束 InvokeLoadForShelf \r\n");
            }
        }

        private async Task<DeviceServiceInvokeResponse> CompleteLoadForSilo(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            try
            {
                logger.LogInformation(deviceServiceInvokeRequest.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_CompleteLoadForDrill_Exception", "Exception", ex.Message);
                return await InteractingDevice.ResponseFail(ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "");
        }

        public async Task<DeviceServiceInvokeResponse> TargetDeviceOperationLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
        {
            return await InteractingDevice.TargetDeviceOperation(deviceServiceInvokeRequest, deviceOperation);
        }

        public void ResetSingleStart(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            LoadSiloResetSingleStart();
        }

        public void ResetSingleFinished(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            LoadSiloResetSingleFinished();
        }

        private void LoadSiloResetSingleStart()
        {
        }

        private void LoadSiloResetSingleFinished()
        {
        }

        public void UpdateSiloInfoForLoad(DeviceServiceInvokeRequest request)
        {
            try
            {
                for (int i = 0; i < PayloadPanels.Count; i++)
                {
                    PayloadPanels[i].SetEmpty();
                    PayloadPanels[i].Position = 1;
                    PayloadPanels[i].ProductStatus = ProductStatus.EmptyPayload;
                }
                PayloadPanels.RaiseCollectionChangedEvent(InteractingDevice.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_UpdateLoadingSiloInfo_Exception", "Exception", ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        private bool SetLoadingSilo(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapPanel item)
        {
            bool result = true;

            var loadingPanel = new SwapPanel()
            {
                AgvPosition = item.AgvPosition,
                SpindleId = item.SpindleId,
                PanelList = PayloadPanels
            };
            deviceServiceInvokeRequest.Params["LoadingPanel"] = JsonSerializer.Serialize(loadingPanel);
            logger.LogDebug($"运料AGV上料仓LoadingPanel：{JsonSerializer.Serialize(loadingPanel)}");
            return result;
        }
    }
}
