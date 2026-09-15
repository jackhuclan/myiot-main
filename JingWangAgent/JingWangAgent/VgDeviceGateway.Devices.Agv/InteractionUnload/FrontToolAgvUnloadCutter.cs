using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Agv.InteractionUnload
{
    public class FrontToolAgvUnloadCutter : DeviceShare<DefaultAgv>, IAgvUnloadInteraction
    {
        private readonly ILogger<FrontToolAgvUnloadCutter> logger;

        public FrontToolAgvUnloadCutter(ILogger<FrontToolAgvUnloadCutter> logger, IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
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
            var checkresult = CheckStatusPanel(deviceServiceInvokeRequest, deviceOperation);
            if (!checkresult.Item1)
            {
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_PrepareLoadMaterial_CheckStatusError", "CheckStatusError", checkresult.Item3);
            }
            return await InteractingDevice.Response(checkresult.Item1 ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL, checkresult.Item3);
        }

        private Tuple<bool, string, string> CheckStatusPanel(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
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
            return await PrepareUnloadForCutter(request);
        }

        private async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            return await InvokeUnloadForCutter(request);
        }

        private async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            return await CompleteUnloadForCutter(request);
        }

        private async Task<DeviceServiceInvokeResponse> PrepareUnloadForCutter(DeviceServiceInvokeRequest request)
        {
            try
            {
                var item = JsonSerializer.Deserialize<SwapTray>(request.Params["OperationListItem"].ToStr());
                if (item == null)
                {
                    logger.LogDebug($"\r\n PrepareUnloadForCutter OperationListItem 是null \r\n");
                    return await InteractingDevice.ResponseFail("PrepareUnloadForCutter OperationListItem 是null");
                }
                var startTime = DateTime.Now;

                //InteractingDevice.ReportingProcess($"验证是否有下料刀盒");
                //logger.LogDebug($"\r\n 验证是否有下料刀盒 \r\n");
                //if (item.TrayList.Count <= 0)
                //{
                //    logger.LogDebug($"\r\n 没有下料刀盒 \r\n");
                //    InteractingDevice.ReportingProcess($"没有下料刀盒");
                //    return await InteractingDevice.ResponseFail("没有下料刀盒");
                //}

                var carCurrentPos = "";

                logger.LogDebug("\r\n  等待_车辆当前位置和下料位置相匹配\r\n");
                await InteractingDevice.ReportingProcess("等待_车辆当前位置和下料位置相匹配");
                var isTimeOut = false;
                var isFinishedWork = false;
                while (true)
                {
                    carCurrentPos = InteractingDevice.WatchingProperties.Property("CarCurrentPos").NewValue.ToStr();
                    if (carCurrentPos == item.AgvPosition)
                    {
                        break;
                    }
                    isTimeOut = InteractingDevice.IsTimeout(startTime, InteractingDevice.waitPlcSignalTimeout);
                    if (isTimeOut)
                    {
                        InteractingDevice.Status = DeviceStatus.Exception;
                        logger.LogDebug($"\r\n 等待 车辆当前位置和下料位置相匹配 超时\r\n");
                        break;
                    }
                    isFinishedWork = InteractingDevice.CanProceedNextStep();
                    if (isFinishedWork)
                    {
                        logger.LogDebug($"\r\n 程序异常跳出循环 车辆当前位置和下料位置相匹配 \r\n");
                        break;
                    }
                    await Task.Delay(50);
                }
                if (isTimeOut || isFinishedWork)
                {
                    InteractingDevice.Status = DeviceStatus.Exception;
                    var errmsgg = $"等待_车辆当前位置{carCurrentPos}和下料位置相匹配{item.AgvPosition}_超时，或异常_{InteractingDevice.Status}";
                  await  InteractingDevice.ReportingProcess(errmsgg);
                    return await InteractingDevice.ResponseFail(errmsgg);
                }
                await InteractingDevice.ReportingProcess($"收到_车辆当前位置{carCurrentPos}和下料位置{item.AgvPosition}相匹配");

                var offsetX = InteractingDevice.WatchingProperties.Property("Offset_X").NewValue.ToFloat();
                var offsetY = InteractingDevice.WatchingProperties.Property("Offset_Y").NewValue.ToFloat();
                var offsetZ = InteractingDevice.WatchingProperties.Property("Offset_Z").NewValue.ToFloat();
                var offsetStr = $"\r\n 当前位{carCurrentPos}—{item.AgvPosition} 给PLC赋值 X轴偏差值：{offsetX}， Y轴偏差值：{offsetY}，Z轴偏差值：{offsetZ}\r\n";
                logger.LogDebug(offsetStr);
                await InteractingDevice.ReportingProcess(offsetStr);
                InteractingDevice.modbusIpMaster.WriteMultipleRegisters(InteractingDevice.slaveId, 3020, offsetX.FloatToReal());
                InteractingDevice.modbusIpMaster.WriteMultipleRegisters(InteractingDevice.slaveId, 3022, offsetY.FloatToReal());
                InteractingDevice.modbusIpMaster.WriteMultipleRegisters(InteractingDevice.slaveId, 3024, offsetZ.FloatToReal());

                logger.LogDebug("\r\n 通知PLC开始读取偏差值3026\r\n");
                await InteractingDevice.ReportingProcess("通知PLC开始读取偏差值3026");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 3026, 1);

                await InteractingDevice.ReportingProcess($"等待{item.SpindleId}轴已准备好接收熟料信号：3512");
                var response = await InteractingDevice.MonitoringSignal(3512, $"{item.SpindleId}轴已准备好接收熟料信号：3512");
                if (!response.Item1)
                {
                    return await InteractingDevice.ResponseFail(response.Item2);
                }
                await InteractingDevice.ReportingProcess($"收到{item.SpindleId}轴已准备好接收熟料信号：3512");
                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_PrepareUnloadForDrill_Exception", "Exception", ex.Message);
                return await InteractingDevice.ResponseFail(ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        private async Task<DeviceServiceInvokeResponse> InvokeUnloadForCutter(DeviceServiceInvokeRequest request)
        {
            var startTime = DateTime.Now;
            try
            {
                var item = JsonSerializer.Deserialize<SwapTray>(request.Params["OperationListItem"].ToStr());
                if (item == null)
                {
                    logger.LogDebug($"\r\n InvokeUnloadForDrill OperationListItem Item 是null \r\n");
                    return await InteractingDevice.ResponseFail("InvokeUnloadForDrill OperationListItem Item 是null");
                }

                var isMang = await InteractingDevice.MonitoringSignal(3505, "机械不在忙碌");
                if (!isMang.Item1)
                {
                    logger.LogDebug($"\r\n 机械不在忙碌：{isMang.Item1} \r\n");
                    return await Response(ErrorCodes.Sys.FAIL, isMang.Item2);
                }

                logger.LogDebug("\r\n 开始下刀盒\r\n");
                await InteractingDevice.ReportingProcess($"{item.SpindleId}轴，开始下刀盒：3001");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 3001, 1);

                var isExit = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(InteractingDevice.slaveId, 3516, 1);
                var noemptylist = InteractingDevice.PayloadCutterTrays.Where(x => x.Status == CutterTrayStatus.NoTray).ToList();
                if (noemptylist.Count <= 1 && isExit[0] == 1)
                {
                    var aaa = $"\r\n 暂存位已存在物料！，并且料仓无空位 \r\n";
                    await InteractingDevice.ReportingProcess(aaa);
                    logger.LogDebug(aaa);
                    return await Response(ErrorCodes.Sys.FAIL, aaa);
                }
                if (isExit[0] == 0)
                {
                    var aaa = $"\r\n 暂存位没有物料！可以暂存 \r\n";
                    await InteractingDevice.ReportingProcess(aaa);
                    logger.LogDebug(aaa);

                    var tsSignal = await InteractingDevice.MonitoringSignal(3517, "移动刀盒到暂存位3517");
                    if (!tsSignal.Item1)
                    {
                        return await Response(ErrorCodes.Sys.FAIL, tsSignal.Item2);
                    }
                }
                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_CompleteUnloadForDrill_Exception", "Exception", ex.Message);
                return await InteractingDevice.ResponseFail($"AGV_CompleteUnloadForDrill_Exception:{ex.Message}");
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        private async Task<DeviceServiceInvokeResponse> CompleteUnloadForCutter(DeviceServiceInvokeRequest request)
        {
            try
            {
                var item = JsonSerializer.Deserialize<SwapTray>(request.Params["OperationListItem"].ToStr());
                if (item == null)
                {
                    logger.LogDebug("\r\n PrepareLoadForTray_OperationListItem_Null\r\n");
                    return await InteractingDevice.ResponseFail("PrepareLoadForTray_OperationListItem_Null ");
                }

                await InteractingDevice.ReportingProcess("等待 下熟料流程结束：3507");
                var response = await InteractingDevice.MonitoringSignal(3507, "上生料流程结束：3507");
                if (!response.Item1)
                {
                    return await InteractingDevice.ResponseFail(response.Item2);
                }
                await InteractingDevice.ReportingProcess("收到 下熟料流程结束：3507");

                await InteractingDevice.ReportingProcess("等待 单轴任务完成：3509");
                var response2 = await InteractingDevice.MonitoringSignal(3509, "单轴任务完成：3509");
                if (!response2.Item1)
                {
                    return await InteractingDevice.ResponseFail(response2.Item2);
                }
                await InteractingDevice.ReportingProcess("单轴任务完成：3509");

                //if (!request.Params.ContainsKey("UnloadingTray"))
                //{
                //    InteractingDevice.ReportingProcess("钻机参数中没有 UnloadingTray");
                //    return await InteractingDevice.ResponseFail("钻机参数中没有 UnloadingTray");
                //}
                //request.Params["TemporaryStorage"] = request.Params["UnloadingTray"];

                if (item.Region > 3)
                {
                    await InteractingDevice.ReportingProcess("等待 单点任务完成：3510");
                    var response3 = await InteractingDevice.MonitoringSignal(3510, $"单点位任务完成:{item.Region}：3510");
                    if (!response3.Item1)
                    {
                        return await InteractingDevice.ResponseFail(response2.Item2);
                    }
                    await InteractingDevice.ReportingProcess("收到 单点任务完成：3510");
                }

                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_CompleteUnloadForDrill_Exception", "Exception", ex.Message);
                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        private static void SetUnloadingPanel(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapPanel item)
        {
            deviceServiceInvokeRequest.Params["UnloadingTrays"] = item;
        }

        public async Task<DeviceServiceInvokeResponse> TargetDeviceOperationLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
        {
            var response = await InteractingDevice.TargetDeviceOperation(deviceServiceInvokeRequest, deviceOperation);
            if (deviceOperation == DeviceOperationType.CompleteUnloadMaterial)
            {
                if (!response.Params.ContainsKey("UnloadingTray"))
                {
                    await InteractingDevice.ReportingProcess("钻机参数中没有 UnloadingTray");
                    return await InteractingDevice.ResponseFail("钻机参数中没有 UnloadingTray");
                }
                deviceServiceInvokeRequest.Params["TemporaryStorage"] = response.Params["UnloadingTray"];
            }

            return response;
        }

        public void ResetSingleStart(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            UnloadPanelResetSingleStart();
        }

        public void ResetSingleFinished(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            UnloadPanelResetSingleFinished();
        }

        private void UnloadPanelResetSingleStart()
        {
        }

        private void UnloadPanelResetSingleFinished()
        {
            logger.LogDebug($"\r\n 重置:3007-3010 \r\n");
            var splinedOperation3002_3012 = new ushort[] { 0, 0, 0, 0 };
            InteractingDevice.modbusIpMaster.WriteMultipleRegisters(InteractingDevice.slaveId, 3007, splinedOperation3002_3012);

            logger.LogDebug("\r\n 重置 准备收熟料3512\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 3512, 0);
            logger.LogDebug("\r\n 重置 收到熟料3513\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 3513, 0);
        }

        public void UpdateSiloInfoForUnload(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            try
            {
                var item = JsonSerializer.Deserialize<SwapTray>(deviceServiceInvokeRequest.Params["OperationListItem"].ToStr());
                if (item == null)
                {
                    logger.LogDebug($"\r\n OperationListItem：为 NULL \r\n ");
                    return;
                }

                var operationEntity = JsonSerializer.Deserialize<SwapTray>(deviceServiceInvokeRequest.Params["UnloadingTray"].ToStr());
                logger.LogDebug($"\r\n UpdateUnloadingSiloInfo_operationEntity：{JsonSerializer.Serialize(operationEntity)}\r\n ");
                if (operationEntity == null || operationEntity.TrayList == null)
                {
                    logger.LogDebug($"\r\n UnloadingTray：为 NULL \r\n ");
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_UpdateUnloadingSiloInfo_UnExistUnloadingPanel", "UnExistUnloadingPanel", "operationEntity:null");
                    return;
                }

                logger.LogDebug($"\r\n UpdateUnloadingSiloInfo_item.InteractionBehavior：{item.InteractionSequence}\r\n ");
                if (item.InteractionSequence == InteractionSequence.UnloadOnly
                    || item.InteractionSequence == InteractionSequence.LoadThenUnload
                    || item.InteractionSequence == InteractionSequence.UnloadThenLoad)
                {
                    var layer = InteractingDevice.loadAndUnLoadLayer / 10;
                    var index = InteractingDevice.loadAndUnLoadLayer % 10;
                    var allindex = (layer - 1) * 6 + index;

                    PayloadCutterTrays[allindex].Status = CutterTrayStatus.Old;
                    PayloadCutterTrays[allindex].TrayCode = operationEntity.TrayList[0].TrayCode;
                    PayloadCutterTrays[allindex].ItemCode = operationEntity.TrayList[0].ItemCode;
                    logger.LogDebug($"更新{allindex}层下料信息：{JsonSerializer.Serialize(operationEntity.TrayList[0])}\r\n ");
                    logger.LogDebug($"\r\n 下料时整体料仓信息：{JsonSerializer.Serialize(PayloadCutterTrays)}\r\n ");
                }
                else
                {
                    for (int i = 0; i < operationEntity.TrayList.Count; i++)
                    {
                        var tray = operationEntity.TrayList[i];
                        tray.Status = CutterTrayStatus.Old;
                    }
                    PayloadCutterTrays.Clear();
                    PayloadCutterTrays.AddRange(operationEntity.TrayList);
                }
                PayloadCutterTrays.RaiseCollectionChangedEvent(InteractingDevice.DeviceId);
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
