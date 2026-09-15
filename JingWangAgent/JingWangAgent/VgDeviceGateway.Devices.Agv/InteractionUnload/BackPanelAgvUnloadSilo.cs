using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;
using VgDeviceGateway.Devices.Common.Agv;

namespace VgDeviceGateway.Devices.Agv.InteractionUnload
{
    public class BackPanelAgvUnloadSilo : DeviceShare<DefaultAgv>, IAgvUnloadInteraction
    {
        private readonly ILogger<BackPanelAgvUnloadSilo> logger;

        public BackPanelAgvUnloadSilo(ILogger<BackPanelAgvUnloadSilo> logger, IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
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
                //下料 即进AGV
                logger.LogDebug("\r\n 发送下料请求 5060\r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5060, 1);

                var response = await InteractingDevice.MonitoringSignal(4062, $"允许AGV进入插齿区：4062");
                if (!response.Item1)
                {
                    InteractingDevice.ReportingProcess("允许AGV进入插齿区：4062超时", AlarmLevel.Severe, "AEP20003");
                    return await InteractingDevice.ResponseFail(response.Item2);
                }

                if (InteractingDevice.configExtra["AgvChassisSupplier"].ToStr() == "HikRobot")
                {
                    logger.LogDebug("\r\n 通知PLC小车需要移动5055 \r\n");
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5055, 1);//通知PLC小车需要移动
                    var agvMoveCan = await InteractingDevice.MonitoringSignal(4055, "底盘允许移动");
                    if (!agvMoveCan.Item1)
                    {
                        InteractingDevice.ReportingProcess("底盘允许移动：4055 超时", AlarmLevel.Severe, "AEP20004");
                        return await Response(ErrorCodes.Sys.FAIL, agvMoveCan.Item2);
                    }
                    logger.LogDebug("\r\n 重置4055信号 \r\n");
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4055, 0);
                }

                logger.LogDebug("\r\n 收到允许AGV进入插齿区 ActionStatus  改为Done \r\n");
                InteractingDevice.ReportingProcess("\r\n 收到允许AGV进入插齿区 ActionStatus  改为Done \r\n");

                InteractingDevice.MoveActionStatus = AgvMoveActionStatus.Done;
                InteractingDevice.AgvCanLeave = true;

                if (InteractingDevice.configExtra["AgvChassisSupplier"].ToStr() == "StdRobot")
                {
                    if (!await InteractingDevice.CheckCanExcuteNextTask())
                    {
                        InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_MoveNoSuccess", "MoveNoSuccess", "PrepareUnloadForSilo Std AskLeave 接口超时...");
                        return await InteractingDevice.ResponseFail("PrepareUnloadForSilo Std AskLeave 接口超时...");
                    }
                }

                var innerPos = request.Params["ShelfInnerPos"].ToStr().Trim();
                logger.LogDebug($"AGV移动到插齿区内点:{innerPos}");//move 到插齿区内点
                request.Params["MoveTargetPos"] = innerPos;

                if (InteractingDevice.configExtra["AgvChassisSupplier"].ToStr() == "HikRobot")
                {
                    if (InteractingDevice.materialType == MaterialKind.PanelSilo)
                    {
                        //string taskCode = "VegaHik" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        //request.Params["taskCode"] = taskCode;
                        //request.Params["reqCode"] = taskCode + "_1";
                        //InteractingDevice.publicMoveId = taskCode;

                        request.Params["count"] = "1";
                        request.Params["method"] = "end2";
                        request.Params["reqCode"] = "ReqCode" + InteractingDevice.DeviceId + DateTime.Now.ToString("yyyyMMddHHmmss") + "2";
                    }
                }

                InteractingDevice.AgvCanLeave = false;
                var deviceOperationResponse = await InteractingDevice.Move(request);
                var errormsg = string.Format($"Move：AGV移动到插齿区内点{innerPos}_{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                logger.LogDebug(errormsg);

                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_MoveNoSuccess", "MoveNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                var moveId = InteractingDevice.publicMoveId;
                logger.LogDebug($"\r\n 等待车辆到达插齿内点···:{moveId} \r\n");
                InteractingDevice.ReportingProcess($"\r\n 等待车辆到达插齿内点···:{moveId} \r\n");

                deviceOperationResponse = await InteractingDevice.defaultAgvChassis.ArrivedLocal(InteractingDevice, request, moveId);
                errormsg = string.Format($"Arrived：{moveId}_{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                InteractingDevice.ReportingProcess(errormsg);
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_ArrivedNoSuccess", "ArrivedNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                logger.LogDebug("\r\n AGV到达叉齿区告知PLC 5063\r\n");
                InteractingDevice.ReportingProcess("\r\n AGV到达叉齿区告知PLC 5063\r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5063, 1);

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
                    logger.LogDebug("\r\n InvokeUnloadForSilo_OperationListItem_Null\r\n");
                    return await InteractingDevice.ResponseFail("InvokeUnloadForSilo_OperationListItem_Null ");
                }
                logger.LogDebug($"\r\n 设置下料的料仓信息:{JsonSerializer.Serialize(item)} \r\n");
                SetUnloadingPanel(request, item);

                var response = await InteractingDevice.MonitoringSignal(4060, $"AGV可以离开插齿：4060");
                if (!response.Item1)
                {
                    InteractingDevice.ReportingProcess("AGV可以离开插齿：4060 超时", AlarmLevel.Severe, "AEP20008");
                    return await InteractingDevice.ResponseFail(response.Item2);
                }

                logger.LogDebug("\r\n 收到AGV可以离开插齿  4060\r\n");
                InteractingDevice.ReportingProcess("\r\n 收到AGV可以离开插齿 4060\r\n");
                if (InteractingDevice.configExtra["AgvChassisSupplier"].ToStr() == "HikRobot")
                {
                    logger.LogDebug("\r\n 通知PLC小车需要移动5055 \r\n");
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5055, 1);//通知PLC小车需要移动
                    var agvMoveCan = await InteractingDevice.MonitoringSignal(4055, "底盘允许移动");
                    if (!agvMoveCan.Item1)
                    {
                        InteractingDevice.ReportingProcess("底盘允许移动：4055 超时", AlarmLevel.Severe, "AEP20004");
                        return await Response(ErrorCodes.Sys.FAIL, agvMoveCan.Item2);
                    }
                    logger.LogDebug("\r\n 清除4055信号 \r\n");
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4055, 0);
                }
                logger.LogDebug("\r\n 收到底盘允许移动  重置4055   改为Done \r\n");
                InteractingDevice.ReportingProcess("\r\n 收到底盘允许移动  重置4055   改为Done \r\n");
                InteractingDevice.MoveActionStatus = AgvMoveActionStatus.Done;
                InteractingDevice.AgvCanLeave = true;

                if (InteractingDevice.configExtra["AgvChassisSupplier"].ToStr() == "StdRobot")
                {
                    if (!await InteractingDevice.CheckCanExcuteNextTask())
                    {
                        InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_MoveNoSuccess", "MoveNoSuccess", "InvokeUnloadForSilo Std AskLeave 接口超时...");
                        return await InteractingDevice.ResponseFail("InvokeUnloadForSilo Std AskLeave 接口超时...");
                    }
                }

                //if (!request.Params.ContainsKey("ShelfAgvIdlePos"))
                //{
                //    return await InteractingDevice.ResponseFail("参数传值不正确：没有 ShelfAgvIdlePos");
                //}
                //var idlePos = request.Params["ShelfAgvIdlePos"].ToStr().Trim();
                //logger.LogDebug($"AGV移动到空闲点:{idlePos}");
                //request.Params["MoveTargetPos"] = idlePos;
                logger.LogDebug($"AGV移动到外点:{item.AgvPosition}");
                request.Params["MoveTargetPos"] = item.AgvPosition;

                if (InteractingDevice.configExtra["AgvChassisSupplier"].ToString() == "HikRobot")
                {
                    request.Params["count"] = "1";
                    request.Params["method"] = "end3";
                    request.Params["reqCode"] = "ReqCode" + InteractingDevice.DeviceId + DateTime.Now.ToString("yyyyMMddHHmmss") + "3";
                    var deviceOperationResponses = await InteractingDevice.defaultAgvChassis.MoveLocal(InteractingDevice, request);
                    var errormsge = string.Format($"Move：hikAGV移动到外点{item.AgvPosition}_{deviceOperationResponses.Code}_{deviceOperationResponses.Message}");
                    logger.LogDebug(errormsge);
                    InteractingDevice.ReportingProcess(errormsge);
                    if (deviceOperationResponses.Code != ErrorCodes.Sys.SUCCESS)
                    {
                        InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_MoveNoSuccess", "MoveNoSuccess", errormsge);
                        deviceOperationResponses.Message = errormsge;
                        return deviceOperationResponses;
                    }
                }
                else
                {
                    var deviceOperationResponses = await InteractingDevice.defaultAgvChassis.MoveOperationLocal(InteractingDevice, request, (int)AgvOperationType.ToBottom);
                    var errormsge = string.Format($"Move：AGV移动到外点{item.AgvPosition}_{deviceOperationResponses.Code}_{deviceOperationResponses.Message}");
                    logger.LogDebug(errormsge);
                    InteractingDevice.ReportingProcess(errormsge);
                    if (deviceOperationResponses.Code != ErrorCodes.Sys.SUCCESS)
                    {
                        InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_MoveNoSuccess", "MoveNoSuccess", errormsge);
                        deviceOperationResponses.Message = errormsge;
                        return deviceOperationResponses;
                    }
                }

                var moveId = InteractingDevice.publicMoveId;

                //等待车辆到外点
                logger.LogDebug($"\r\n 车辆到达外点···{moveId} \r\n");
                InteractingDevice.ReportingProcess($"\r\n 等待车辆到达外点···{moveId} \r\n");

                var deviceOperationResponse = await InteractingDevice.defaultAgvChassis.ArrivedLocal(InteractingDevice, request, moveId);
                var errormsg = string.Format($"Arrived：外点{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_ArrivedNoSuccess", "ArrivedNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                logger.LogDebug("\r\n 等待车辆到外点告诉PLC已离开 5061\r\n");
                InteractingDevice.ReportingProcess("\r\n 等待车辆到外点告诉PLC已离开 5061\r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5061, 1);

                var response4061 = await InteractingDevice.MonitoringSignal(4061, "下料仓流程结束：4061");
                if (!response4061.Item1)
                {
                    InteractingDevice.ReportingProcess("下料仓流程结束：4061", AlarmLevel.Severe, "AEP20009");
                    return await InteractingDevice.ResponseFail(response4061.Item2);
                }
                InteractingDevice.ReportingProcess("\r\n 收到 下料仓流程结束：4061\r\n");
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

        private static void SetUnloadingPanel(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapPanel item)
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
            logger.LogDebug("\r\n 重置允许AGV离开料架\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4060, 0);
            logger.LogDebug("\r\n 重置下料仓动作结束\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4064, 0);
            logger.LogDebug("\r\n 重置允许AGV进入料架\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4062, 0);
            logger.LogDebug("\r\n 重置上料仓动作结束4061\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4061, 0);
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

                for (int i = 0; i < operationEntity.PanelList.Count; i++)
                {
                    var status = operationEntity.PanelList[i].ProductStatus == ProductStatus.Finished_PRE_BUFFER ? ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1 : operationEntity.PanelList[i].ProductStatus;
                    operationEntity.PanelList[i].ProductStatus = status;
                    operationEntity.PanelList[i].Layer = i;
                    operationEntity.PanelList[i].Position = 1;
                    operationEntity.PanelList[i].LocationCode = DeviceDescriptor.DeviceId;
                }
                PayloadPanels.Clear();
                PayloadPanels.AddRange(operationEntity.PanelList);
                logger.LogDebug($"\r\n 料架下料时整体料仓信息PayloadPanels：{JsonSerializer.Serialize(PayloadPanels)}\r\n ");
                await PayloadPanels.RaiseCollectionChangedEvent(InteractingDevice.DeviceId);
            }
            catch (Exception ex)
            {
                InteractingDevice.ReportingProcess("下料仓更新板料信息异常UpdateSiloInfoForLoad", AlarmLevel.Severe, "AES10027");
                logger.LogError(ex, "下料仓更新板料信息异常UpdateSiloInfoForLoad：" + ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_UpdateUnloadingSiloInfo_Exception", "Exception", ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }
    }
}
