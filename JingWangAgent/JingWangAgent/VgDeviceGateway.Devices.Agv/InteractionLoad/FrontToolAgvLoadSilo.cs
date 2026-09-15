using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Agv.InteractionLoad
{
    public class FrontToolAgvLoadSilo : DeviceShare<DefaultAgv>, IAgvLoadInteraction
    {
        private readonly ILogger<FrontToolAgvLoadSilo> logger;

        public FrontToolAgvLoadSilo(IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
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

                var response = await InteractingDevice.MonitoringSignal(4062, $"允许AGV进入插齿区：4062");
                if (!response.Item1)
                {
                    return await InteractingDevice.ResponseFail(response.Item2);
                }

                var innerPos = request.Params["ShelfInnerPos"].ToStr().Trim();
                logger.LogDebug($"AGV移动到插齿区内点:{innerPos}");//move 到插齿区内点
                request.Params["MoveTargetPos"] = innerPos;
                var deviceOperationResponse = await InteractingDevice.defaultAgvChassis.MoveLocal(InteractingDevice, request);
                var errormsg = string.Format($"Move：AGV移动到插齿区内点{innerPos}_{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                logger.LogDebug(errormsg);
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_MoveNoSuccess", "MoveNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                var moveId = "";
                if (deviceOperationResponse.Params.ContainsKey("AgvReturnTaskId"))
                {
                    moveId = deviceOperationResponse.Params["AgvReturnTaskId"].ToStr();
                }
                logger.LogDebug($"\r\n 等待车辆到达插齿内点···:{moveId} \r\n");
                deviceOperationResponse = await InteractingDevice.defaultAgvChassis.ArrivedLocal(InteractingDevice, request, moveId);
                errormsg = string.Format($"Arrived：{moveId}_{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_ArrivedNoSuccess", "ArrivedNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                logger.LogDebug("\r\n AGV到达叉齿区告知PLC\r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5063, 1);

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
                var item = JsonSerializer.Deserialize<SwapTray>(request.Params["OperationListItem"].ToStr());

                var response = await InteractingDevice.MonitoringSignal(4060, $"AGV可以离开插齿：4060");
                if (!response.Item1)
                {
                    return await InteractingDevice.ResponseFail(response.Item2);
                }

                //收到可以离开
                logger.LogDebug("\r\n 收到AGV可以离开插齿 ActionStatus  改为Done \r\n");
                InteractingDevice.MoveActionStatus = AgvMoveActionStatus.Done;

                logger.LogDebug($"AGV移动到插齿区外点:{item.AgvPosition}");
                request.Params["MoveTargetPos"] = item.AgvPosition;
                var deviceOperationResponse = await InteractingDevice.defaultAgvChassis.MoveLocal(InteractingDevice, request);
                var errormsg = string.Format($"Move：AGV移动到插齿区外点{item.AgvPosition}_{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                logger.LogDebug(errormsg);
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_MoveNoSuccess", "MoveNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                var moveId = "";
                if (deviceOperationResponse.Params.ContainsKey("AgvReturnTaskId"))
                {
                    moveId = deviceOperationResponse.Params["AgvReturnTaskId"].ToStr();
                }

                //等待车辆到外点
                logger.LogDebug($"\r\n 等待车辆到达插齿外点···{moveId} \r\n");
                deviceOperationResponse = await InteractingDevice.defaultAgvChassis.ArrivedLocal(InteractingDevice, request, moveId);
                errormsg = string.Format($"Arrived：外点{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Work_ArrivedNoSuccess", "ArrivedNoSuccess", errormsg);
                    return deviceOperationResponse;
                }
                logger.LogDebug("\r\n 等待车辆到外点告诉PLC已离开\r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5061, 1);

                var response4064 = await InteractingDevice.MonitoringSignal(4064, $"上料仓4064流程结束：4064");
                if (!response4064.Item1)
                {
                    return await InteractingDevice.ResponseFail(response4064.Item2);
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
            logger.LogDebug("\r\n 重置允许AGV离开料架4060\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4060, 0);
            logger.LogDebug("\r\n 重置下料仓动作结束4061\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4061, 0);
            logger.LogDebug("\r\n 重置允许AGV进入料架4062\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4062, 0);
        }

        public void UpdateSiloInfoForLoad(DeviceServiceInvokeRequest request)
        {
            try
            {
                for (int i = 0; i < PayloadCutterTrays.Count; i++)
                {
                    PayloadCutterTrays[i].Status = CutterTrayStatus.EmptyTray;
                }
                PayloadCutterTrays.RaiseCollectionChangedEvent(InteractingDevice.DeviceId);
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
    }
}
