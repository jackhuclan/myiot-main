using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;
using VgDeviceGateway.Devices.Common.Agv;

namespace VgDeviceGateway.Devices.Agv.InteractionUnload
{
    public class ShelfSiloAgvUnloadSilo : DeviceShare<DefaultAgv>, IAgvUnloadInteraction
    {
        private readonly ILogger<ShelfSiloAgvUnloadSilo> logger;

        public ShelfSiloAgvUnloadSilo(ILogger<ShelfSiloAgvUnloadSilo> logger, IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
        {
            this.logger = logger;
        }

        public async Task<DeviceServiceInvokeResponse> AgvUnloadMaterialSelfLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
        {
            if (deviceOperation == DeviceOperationType.PrepareUnloadMaterial)
            {
                return await PrepareUnloadMaterialSelf(deviceServiceInvokeRequest);
            }
            else if (deviceOperation == DeviceOperationType.InvokeUnloadMaterial)
            {
                return await InvokeUnloadMaterialSelf(deviceServiceInvokeRequest);
            }
            else if (deviceOperation == DeviceOperationType.CompleteUnloadMaterial)
            {
                return await CompleteUnloadMaterialSelf(deviceServiceInvokeRequest);
            }
            return await Response(ErrorCodes.Sys.FAIL, $"AgvUnloadMaterialSelfLocal 参数类型不正确deviceOperation：{deviceOperation}");
        }

        public async Task<DeviceServiceInvokeResponse> CheckDeviceStatusLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
        {
            var checkresult = CheckStatusSilo(deviceServiceInvokeRequest, deviceOperation);
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

        private async Task<DeviceServiceInvokeResponse> PrepareUnloadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            return await PrepareUnloadForSilo(request);
        }

        private async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            return await InvokeUnloadForSilo(request);
        }

        private async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            return await CompleteUnloadForSilo(request);
        }

        private async Task<DeviceServiceInvokeResponse> PrepareUnloadForSilo(DeviceServiceInvokeRequest request)
        {
            try
            {
                if (InteractingDevice.configExtra["AgvChassisSupplier"].ToStr() == "AjwRobot")
                {
                    if (!request.Params.ContainsKey("TransActionPos"))
                    {
                        return await InteractingDevice.ResponseFail("参数传值不正确：TransActionPos");
                    }
                    var actionPos = request.Params["TransActionPos"].ToStr().Trim();
                    logger.LogDebug($"AGV移动到插齿区动作点:{actionPos}，下降");//move 到插齿区内点
                    request.Params["MoveTargetPos"] = actionPos;
                    var actionPosResponse = await InteractingDevice.defaultAgvChassis.MoveOperationLocal(InteractingDevice, request, (int)AgvOperationType.ToBottom);
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
                var deviceOperationResponse = await InteractingDevice.defaultAgvChassis.MoveOperationLocal(InteractingDevice, request, (int)AgvOperationType.ToTop);
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

                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_PrepareUnloadForShelf_Exception", "Exception", ex.Message);
                return await InteractingDevice.ResponseFail(ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        private async Task<DeviceServiceInvokeResponse> InvokeUnloadForSilo(DeviceServiceInvokeRequest request)
        {
            try
            {
                var item = JsonSerializer.Deserialize<SwapPanel>(request.Params["OperationListItem"].ToStr());
                if (item == null)
                {
                    logger.LogDebug("\r\n InvokeUnloadForShelf_OperationListItemNull\r\n");
                    return await InteractingDevice.ResponseFail("InvokeUnloadForShelf_OperationListItem_Null ");
                }
                logger.LogDebug($"\r\n 设置下料的料仓信息:{JsonSerializer.Serialize(item)} \r\n");
                SetUnloadingSilo(request, item);

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
                //    InteractingDevice.Status = DeviceStatus.Exception;
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
                //    InteractingDevice.Status = DeviceStatus.Exception;
                //    return deviceOperationResponse;
                //}

                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_InvokeUnloadForShelf_Exception", "Exception", ex.Message);
                return await InteractingDevice.ResponseFail(ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        private async Task<DeviceServiceInvokeResponse> CompleteUnloadForSilo(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:CompleteUnloadForShelf" + request.DeviceId);

                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_CompleteUnloadForShelf_Exception", "Exception", ex.Message);
                return await InteractingDevice.ResponseFail(ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        private static void SetUnloadingSilo(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapPanel item)
        {
            deviceServiceInvokeRequest.Params["UnloadingPanel"] = item;
        }

        public async Task<DeviceServiceInvokeResponse> TargetDeviceOperationLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
        {
            return await InteractingDevice.TargetDeviceOperation(deviceServiceInvokeRequest, deviceOperation);
        }

        public void ResetSingleStart(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            UnloadSiloResetSingleStart();
        }

        public void ResetSingleFinished(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            UnloadSiloResetSingleFinished();
        }

        private void UnloadSiloResetSingleStart()
        {
        }

        private void UnloadSiloResetSingleFinished()
        {
        }

        public async void UpdateSiloInfoForUnload(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            try
            {
                var operationEntity = deviceServiceInvokeRequest.Params["UnloadingPanel"] as SwapPanel;
                logger.LogDebug($"\r\n UpdateUnloadingSiloInfo_operationEntity：{JsonSerializer.Serialize(operationEntity)}\r\n ");
                if (operationEntity == null || operationEntity.PanelList == null)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_UpdateUnloadingSiloInfo_UnExistUnloadingPanel", "UnExistUnloadingPanel", "operationEntity:null");
                    return;
                }

                logger.LogDebug($"\r\n UpdateUnloadingSiloInfo_item.InteractionBehavior：{operationEntity.InteractionSequence}\r\n ");
                PayloadPanels.Clear();
                for (int i = 0; i < operationEntity.PanelList.Count; i++)
                {
                    var status = operationEntity.PanelList[i].ProductStatus;
                    if (operationEntity.PanelList[i].ProductStatus == ProductStatus.Finished_POST_BUFFER)
                    {
                        status = ProductStatus.PRE_UNPIN_TRANSFER_AGV_OUTPUT_1;
                    }
                    else if (operationEntity.PanelList[i].ProductStatus == ProductStatus.Finished_PIN)
                    {
                        status = ProductStatus.PRE_PRE_BUFFER_TRANSFER_AGV_OUTPUT_1;
                    }
                    operationEntity.PanelList[i].ProductStatus = status;
                    operationEntity.PanelList[i].Layer = i;
                    operationEntity.PanelList[i].Position = 1;
                }

                PayloadPanels.AddRange(operationEntity.PanelList);
                await PayloadPanels.RaiseCollectionChangedEvent(InteractingDevice.DeviceId);
                logger.LogDebug($"\r\n 料架下料时整体料仓信息PayloadPanels：{JsonSerializer.Serialize(PayloadPanels)}\r\n ");
            }
            catch (Exception ex)
            {
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_UpdateUnloadingSiloInfo_Exception", "Exception", ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }
    }
}
