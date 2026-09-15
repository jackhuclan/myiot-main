using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;
using VgDeviceGateway.Devices.Common.Agv;

namespace VgDeviceGateway.Devices.Agv.InteractionLoad
{
    public class BackPanelAgvLoadSilo : DeviceShare<DefaultAgv>, IAgvLoadInteraction
    {
        private readonly ILogger<BackPanelAgvLoadSilo> logger;

        public BackPanelAgvLoadSilo(ILogger<BackPanelAgvLoadSilo> logger, IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
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
                logger.LogDebug("\r\n 开始 PrepareLoadForShelf \r\n");
                var startTime = DateTime.Now;
                logger.LogDebug("\r\n 发送上料请求5062\r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5062, 1);

                var response = await InteractingDevice.MonitoringSignal(4062, $"允许AGV进入插齿区");
                if (!response.Item1)
                {
                    InteractingDevice.ReportingProcess("允许AGV进入插齿区4062超时", AlarmLevel.Severe, "AEP20003");
                    return await InteractingDevice.ResponseFail(response.Item2);
                }

                logger.LogDebug("\r\n 通知PLC小车需要移动5055 \r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5055, 1);//通知PLC小车需要移动
                var agvMoveCan = await InteractingDevice.MonitoringSignal(4055, "底盘允许移动");
                if (!agvMoveCan.Item1)
                {
                    InteractingDevice.ReportingProcess("底盘允许移动4055超时", AlarmLevel.Severe, "AEP20004");
                    return await Response(ErrorCodes.Sys.FAIL, agvMoveCan.Item2);
                }
                logger.LogDebug("\r\n 重置4055信号 \r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4055, 0);

                logger.LogDebug("\r\n 收到允许AGV进入插齿区 重置4055信号 ActionStatus  改为Done \r\n");
                InteractingDevice.ReportingProcess("\r\n 收到允许AGV进入插齿区 重置4055信号 ActionStatus  改为Done \r\n");
                InteractingDevice.MoveActionStatus = AgvMoveActionStatus.Done;
                InteractingDevice.AgvCanLeave = true;

                if (InteractingDevice.configExtra["AgvChassisSupplier"].ToStr() == "StdRobot")
                {
                    if (!await InteractingDevice.CheckCanExcuteNextTask())
                    {
                        InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_MoveNoSuccess", "MoveNoSuccess", "PrepareLoadForSilo Std AskLeave 接口超时...");
                        return await InteractingDevice.ResponseFail("PrepareLoadForSilo Std AskLeave 接口超时...");
                    }
                }

                var innerPos = request.Params["ShelfInnerPos"].ToStr().Trim();
                logger.LogDebug($"AGV移动到插齿区内点:{innerPos}");

                request.Params["MoveTargetPos"] = innerPos;

                if (InteractingDevice.configExtra["AgvChassisSupplier"].ToStr() == "HikRobot")
                {
                    request.Params["method"] = "end2";
                    request.Params["count"] = "1";
                    request.Params["reqCode"] = "ReqCode" + InteractingDevice.DeviceId + DateTime.Now.ToString("yyyyMMddHHmmss") + "2";
                }

                InteractingDevice.AgvCanLeave = false;
                var deviceOperationResponse = await InteractingDevice.defaultAgvChassis.MoveLocal(InteractingDevice, request);
                var errormsg = string.Format($"Move：AGV移动到插齿区内点{innerPos}_{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                logger.LogDebug(errormsg);
                InteractingDevice.ReportingProcess(errormsg, deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS ? AlarmLevel.Severe : AlarmLevel.Information,
                    deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS ? "AES10008" : "");
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_MoveNoSuccess", "MoveNoSuccess", errormsg);
                    deviceOperationResponse.Message = errormsg;
                    return deviceOperationResponse;
                }

                var moveId = InteractingDevice.publicMoveId;

                logger.LogDebug($"\r\n 等待车辆到达插齿内点···:{moveId} \r\n");
                deviceOperationResponse = await InteractingDevice.defaultAgvChassis.ArrivedLocal(InteractingDevice, request, moveId);
                errormsg = string.Format($"Arrived：{moveId}_{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    deviceOperationResponse.Message = errormsg;
                    return deviceOperationResponse;
                }

                logger.LogDebug($"\r\n 车辆到达插齿内点···:{moveId} \r\n");

                logger.LogDebug("\r\n AGV到达叉齿区告知PLC\r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5063, 1);
                InteractingDevice.MoveActionStatus = AgvMoveActionStatus.Doing;
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

                var response = await InteractingDevice.MonitoringSignal(4060, $"AGV可以离开插齿：4060");
                if (!response.Item1)
                {
                    InteractingDevice.ReportingProcess("底盘允许移动：4055 超时", AlarmLevel.Severe, "AEP20004");
                    return await InteractingDevice.ResponseFail(response.Item2);
                }

                logger.LogDebug("\r\n 通知PLC小车需要移动5055 \r\n");
                InteractingDevice.ReportingProcess("通知PLC小车需要移动5055，等待底盘允许移动4055");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5055, 1);//通知PLC小车需要移动
                var agvMoveCan = await InteractingDevice.MonitoringSignal(4055, "底盘允许移动");
                if (!agvMoveCan.Item1)
                {
                    InteractingDevice.ReportingProcess("底盘允许移动4055超时", AlarmLevel.Severe, "AEP20004");
                    return await Response(ErrorCodes.Sys.FAIL, agvMoveCan.Item2);
                }
                logger.LogDebug("\r\n 收到底盘允许移动 重置4055，并重置信号 \r\n");
                InteractingDevice.ReportingProcess("收到底盘允许移动 重置4055，并重置信号");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4055, 0);

                //收到可以离开
                logger.LogDebug("\r\n 收到AGV可以离开插齿 ActionStatus  改为Done \r\n");
                InteractingDevice.MoveActionStatus = AgvMoveActionStatus.Done;
                InteractingDevice.AgvCanLeave = true;

                if (InteractingDevice.configExtra["AgvChassisSupplier"].ToStr() == "StdRobot")
                {
                    if (!await InteractingDevice.CheckCanExcuteNextTask())
                    {
                        InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_MoveNoSuccess", "MoveNoSuccess", "InvokeLoadForSilo Std AskLeave 接口超时...");
                        return await InteractingDevice.ResponseFail("InvokeLoadForSilo Std AskLeave 接口超时...");
                    }
                }

                //if (!request.Params.ContainsKey("ShelfAgvIdlePos"))
                //{
                //    return await InteractingDevice.ResponseFail("参数传值不正确：没有 ShelfAgvIdlePos");
                //}
                //var idlePos = request.Params["ShelfAgvIdlePos"].ToStr().Trim();
                //logger.LogDebug($"AGV移动到空闲点:{idlePos}");
                //request.Params["MoveTargetPos"] = idlePos;//休息点
                logger.LogDebug($"AGV移动到外点:{item.AgvPosition}");
                request.Params["MoveTargetPos"] = item.AgvPosition;

                InteractingDevice.AgvCanLeave = false;

                if (InteractingDevice.configExtra["AgvChassisSupplier"].ToString() == "HikRobot")
                {
                    request.Params["count"] = "1";
                    request.Params["method"] = "end3";
                    request.Params["reqCode"] = "ReqCode" + InteractingDevice.DeviceId + DateTime.Now.ToString("yyyyMMddHHmmss") + "3";
                    var deviceOperationResponses = await InteractingDevice.defaultAgvChassis.MoveLocal(InteractingDevice, request);
                    var errormsge = string.Format($"Move：AGV移动到外点{item.AgvPosition}_{deviceOperationResponses.Code}_{deviceOperationResponses.Message}");
                    logger.LogDebug(errormsge);

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
                logger.LogDebug($"\r\n 等待车辆到达外点···{moveId} \r\n");
                var deviceOperationResponse = await InteractingDevice.defaultAgvChassis.ArrivedLocal(InteractingDevice, request, moveId);
                var errormsg = string.Format($"Arrived：外点{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_ArrivedNoSuccess", "ArrivedNoSuccess", errormsg);
                    deviceOperationResponse.Message = errormsg;
                    return deviceOperationResponse;
                }

                logger.LogDebug("\r\n 车辆到外点告诉PLC已离开\r\n");
                InteractingDevice.ReportingProcess("\r\n 车辆到外点告诉PLC已离开\r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5061, 1);

                var response4064 = await InteractingDevice.MonitoringSignal(4064, $"上料仓4064流程结束：4064");
                if (!response4064.Item1)
                {
                    return await InteractingDevice.ResponseFail(response4064.Item2);
                }

                logger.LogDebug("\r\n 设置上料的板料信息\r\n");
                InteractingDevice.ReportingProcess("设置上料的板料信息");
                var result = SetLoadingPanel(request, item);
                if (!result)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_SetLoadingPanel_SetLoadingPanelError", "SetLoadingPanelError", "SetLoadingPanel false");
                    InteractingDevice.ReportingProcess("设置上料的板料信息_异常", AlarmLevel.Severe, "AES10018");
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

        private bool SetLoadingPanel(DeviceServiceInvokeRequest request, SwapPanel item)
        {
            bool result = true;

            var loadingPanel = new SwapPanel()
            {
                AgvPosition = item.AgvPosition,
                SpindleId = item.SpindleId,

                PanelList = PayloadPanels
            };
            request.Params["LoadingPanel"] = JsonSerializer.Serialize(loadingPanel);
            request.Params["IsFullSilo"] = WatchingProperties.Property("IsFullSilo").NewValue.ToBool();
            logger.LogDebug($"AGV上料仓LoadingPanel：{JsonSerializer.Serialize(loadingPanel)}");
            return result;
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
            logger.LogDebug("\r\n 重置允许AGV离开料架4060\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4060, 0);
            logger.LogDebug("\r\n 重置上料仓动作结束4061\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4061, 0);
            logger.LogDebug("\r\n 重置允许AGV进入料架4062\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4062, 0);
            logger.LogDebug("\r\n 重置下料仓动作结束 4064\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4064, 0);
        }

        public void UpdateSiloInfoForLoad(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogDebug($"BackPanelAgvLoadSilo_UpdateSiloInfoForLoad_更新{InteractingDevice.loadAndUnLoadLayer}层信息");
                for (int i = 0; i < PayloadPanels.Count; i++)
                {
                    PayloadPanels[i].SetEmpty();
                    PayloadPanels[i].Position = 1;
                    PayloadPanels[i].ProductStatus = ProductStatus.EmptyPayload;
                    PayloadPanels[i].LocationCode = DeviceDescriptor.DeviceId;
                    PayloadPanels[i].SiloCode = string.Empty;
                }

                PayloadPanels.RaiseCollectionChangedEvent(InteractingDevice.DeviceId);
            }
            catch (Exception ex)
            {
                InteractingDevice.ReportingProcess("上料仓更新板料信息异常UpdateSiloInfoForLoad", AlarmLevel.Severe, "AES10026");
                logger.LogError(ex, "上料仓更新板料信息异常UpdateSiloInfoForLoad：" + ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_UpdateLoadingSiloInfo_Exception", "Exception", ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }
    }
}
