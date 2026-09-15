using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Agv.InteractionUnload
{
    public class BackPanelAgvUnloadPanel : DeviceShare<DefaultAgv>, IAgvUnloadInteraction
    {
        private readonly ILogger<BackPanelAgvUnloadPanel> logger;

        public BackPanelAgvUnloadPanel(ILogger<BackPanelAgvUnloadPanel> logger, IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
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
            return await PrepareUnloadForPanel(request);
        }

        private async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            return await InvokeUnloadForPanel(request);
        }

        private async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            return await CompleteUnloadForPanel(request);
        }

        private async Task<DeviceServiceInvokeResponse> PrepareUnloadForPanel(DeviceServiceInvokeRequest request)
        {
            try
            {
                var item = JsonSerializer.Deserialize<SwapPanel>(request.Params["OperationListItem"].ToStr());
                if (item == null)
                {
                    logger.LogDebug($"\r\n PrepareUnloadForPanel OperationListItem 是null \r\n");
                    return await InteractingDevice.ResponseFail("PrepareUnloadForPanel OperationListItem 是null ");
                }
                var startTime = DateTime.Now;

                InteractingDevice.ReportingProcess($"验证是否有下料板料");
                logger.LogDebug($"\r\n 验证是否有下料板料 \r\n");
                if (item.PanelList.Count <= 0)
                {
                    logger.LogDebug($"\r\n 没有下料板料 \r\n");
                    InteractingDevice.ReportingProcess($"没有下料板料数据", AlarmLevel.Severe, "AES10021");
                    return await InteractingDevice.ResponseFail("没有下料板料");
                }

                var carCurrentPos = "";

                if (InteractingDevice.configExtra["AgvChassisSupplier"].ToString() == "AjwRobot")
                {
                    logger.LogDebug("\r\n  等待_车辆当前位置和下料位置相匹配\r\n");
                    InteractingDevice.ReportingProcess("等待_车辆当前位置和下料位置相匹配");
                    var isTimeOut = false;
                    var isFinishedWork = false;
                    while (true)
                    {
                        carCurrentPos = InteractingDevice.WatchingProperties.Property("CarCurrentPos").NewValue.ToStr();
                        logger.LogDebug($"\r\n 车辆当前位置：{carCurrentPos},上料位置：{item.AgvPosition} \r\n");
                        InteractingDevice.ReportingProcess($"车辆当前位置：{carCurrentPos},上料位置：{item.AgvPosition}");
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
                        var pipeimsg = $"等待_车辆当前位置{carCurrentPos}和下料位置{item.AgvPosition}相匹配_超时，或异常_{InteractingDevice.Status}";
                        InteractingDevice.ReportingProcess(pipeimsg, AlarmLevel.Severe, "AES10019");
                        return await InteractingDevice.ResponseFail(pipeimsg);
                    }
                }
                logger.LogDebug("\r\n  收到_车辆当前位置和下料位置相匹配\r\n");
                InteractingDevice.ReportingProcess("收到_车辆当前位置和下料位置相匹配");

                var offsetX = InteractingDevice.WatchingProperties.Property("Offset_X").NewValue.ToFloat();
                logger.LogDebug($"\r\n 当前位{carCurrentPos}—{item.AgvPosition} 给PLC赋值 X轴偏差值：{offsetX}\r\n");
                InteractingDevice.ReportingProcess($" 当前位{carCurrentPos}—{item.AgvPosition} 给PLC赋值 X轴偏差值：{offsetX}");
                InteractingDevice.modbusIpMaster.WriteMultipleRegisters(InteractingDevice.slaveId, 5019, offsetX.FloatToReal());

                logger.LogDebug("\r\n PLC开始读取偏差值5066\r\n");
                InteractingDevice.ReportingProcess("PLC开始读取偏差值5066");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5066, 1);

                logger.LogDebug("\r\n 给PLC写入AGV到位5056\r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5056, 1);

                InteractingDevice.ReportingProcess($"等待{item.SpindleId}轴已准备好接收熟料信号：4023");
                var response = await InteractingDevice.MonitoringSignal(4023, $"{item.SpindleId}轴已准备好接收熟料信号：4023");
                if (!response.Item1)
                {
                    InteractingDevice.ReportingProcess($"等待{item.SpindleId}轴已准备好接收熟料信号：4023超时", AlarmLevel.Severe, "AEP20005");
                    return await InteractingDevice.ResponseFail(response.Item2);
                }
                InteractingDevice.ReportingProcess($"收到{item.SpindleId}轴已准备好接收熟料信号：4023");
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

        private async Task<DeviceServiceInvokeResponse> InvokeUnloadForPanel(DeviceServiceInvokeRequest request)
        {
            var startTime = DateTime.Now;
            try
            {
                var item = JsonSerializer.Deserialize<SwapPanel>(request.Params["OperationListItem"].ToStr());
                if (item == null)
                {
                    logger.LogDebug($"\r\n InvokeUnloadForPanel OperationListItem Item 是null \r\n");
                    return await InteractingDevice.ResponseFail("InvokeUnloadForDrill OperationListItem Item 是null");
                }

                //通知PLC板长数据
                await InteractingDevice.ReportingProcess($"\r\n 同步给PLC板料板长信息：{item.PanelList[0].PanelLength}\r\n");
                await InteractingDevice.modbusIpMaster.WriteSingleRegisterAsync(InteractingDevice.slaveId, 5015, Math.Round(item.PanelList[0].PanelLength).ToUshort());
                logger.LogDebug($"\r\n 同步给PLC板料板长信息：{item.PanelList[0].PanelWidth}\r\n");

                logger.LogDebug("\r\n 开始下板料\r\n");
                await InteractingDevice.ReportingProcess($"{item.SpindleId}轴，开始下板料：5014");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5014, 1);

                var EnabledReceiveScannig = InteractingDevice.configExtra["EnabledReceiveScannig"].ToBool();
                logger.LogDebug($"\r\n 下是否启用读取二维码：{EnabledReceiveScannig}\r\n");
                InteractingDevice.ReportingProcess($"上料是否启用读取二维码：{EnabledReceiveScannig}");
                if (EnabledReceiveScannig)
                {
                    InteractingDevice.panelBarCode = "";
                    var responseScan = await InteractingDevice.MonitoringSignal(4028, "PLC通知可以读码4028");

                    if (!responseScan.Item1)
                    {
                        InteractingDevice.ReportingProcess(responseScan.Item2);
                        return await InteractingDevice.ResponseFail(responseScan.Item2);
                    }
                    var coderesult = InteractingDevice.ReadMaterialCode(4350);
                    logger.LogDebug($"\r\n 板料二维码信息4350：{coderesult.Item2}\r\n");
                    InteractingDevice.ReportingProcess($"板料二维码信息4350：{coderesult.Item2}");
                    InteractingDevice.panelBarCode = InteractingDevice.ScannigConvert(coderesult.Item2);
                }
                else
                {
                    logger.LogDebug($"\r\n 上料是否启用读取二维码 \r\n");
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5065, 2);
                }

                SetUnloadingPanel(request, item);
                InteractingDevice.ReportingProcess($"{item.SpindleId}轴，设置下料的板信息");
                logger.LogDebug($"\r\n 设置下料的板信息:{JsonSerializer.Serialize(item)} \r\n");

                await InteractingDevice.ReportingProcess($"等到已收到熟料：4024");
                var response = await InteractingDevice.MonitoringSignal(4024, $"已收到熟料：4024");
                if (!response.Item1)
                {
                    InteractingDevice.ReportingProcess($"等到已收到熟料：4024超时", AlarmLevel.Severe, "AEP20006");
                    return await InteractingDevice.ResponseFail(response.Item2);
                }
                await InteractingDevice.ReportingProcess($"收到已收到熟料：4024");

                await InteractingDevice.ReportingProcess($"等待下熟料流程结束：4025");
                var response4025 = await InteractingDevice.MonitoringSignal(4025, $"下熟料4025流程结束：4025");
                if (!response4025.Item1)
                {
                    InteractingDevice.ReportingProcess($"下熟料流程结束：4025超时", AlarmLevel.Severe, "AEP20007");
                    return await InteractingDevice.ResponseFail(response4025.Item2);
                }
                await InteractingDevice.ReportingProcess($"收到下熟料流程结束：4025");
                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_InvokeUnloadForPanel_Exception", "Exception", ex.Message);
                return await InteractingDevice.ResponseFail($"AGV_InvokeUnloadForPanel_Exception:{ex.Message}");
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        private async Task<DeviceServiceInvokeResponse> CompleteUnloadForPanel(DeviceServiceInvokeRequest request)
        {
            try
            {
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

        private void SetUnloadingPanel(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapPanel item)
        {
            item.PanelList[0].Barcode = InteractingDevice.panelBarCode;
            deviceServiceInvokeRequest.Params["UnloadingPanel"] = item;
        }

        public async Task<DeviceServiceInvokeResponse> TargetDeviceOperationLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
        {
            return await InteractingDevice.TargetDeviceOperation(deviceServiceInvokeRequest, deviceOperation);
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
            logger.LogDebug("\r\n 重置 收到熟料4023\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4023, 0);
            logger.LogDebug("\r\n 重置 收到熟料4024\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4024, 0);
            logger.LogDebug("\r\n 重置下熟料流程结束4025\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4025, 0);
            logger.LogDebug("\r\n 重置 允许读码 4028\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4028, 0);
        }

        public void UpdateSiloInfoForUnload(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            try
            {
                var item = JsonSerializer.Deserialize<SwapPanel>(deviceServiceInvokeRequest.Params["OperationListItem"].ToStr());
                if (item == null)
                {
                    logger.LogDebug($"\r\n OperationListItem：为 NULL \r\n ");
                    return;
                }
                var panelsilo = PayloadPanels.FirstOrDefault(x => x.SiloCode != "" && x.SiloCode.ToLower() != InteractingDevice.DeviceId.ToLower());
                if (panelsilo == null)
                {
                    var str = " 下料时 UpdateSiloInfoForUnload：内未找到料仓号";
                    InteractingDevice.ReportingProcess(str, AlarmLevel.Severe, "AES10022");
                    logger.LogDebug(str);
                    return;
                }

                var operationEntity = deviceServiceInvokeRequest.Params["UnloadingPanel"] as SwapPanel;
                logger.LogDebug($"\r\n UpdateUnloadingSiloInfo_operationEntity：{JsonSerializer.Serialize(operationEntity)}\r\n ");
                if (operationEntity == null || operationEntity.PanelList == null)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_UpdateUnloadingSiloInfo_UnExistUnloadingPanel", "UnExistUnloadingPanel", "operationEntity:null");
                    return;
                }

                logger.LogDebug($"\r\n UpdateUnloadingSiloInfo_item.InteractionBehavior：{item.InteractionSequence}\r\n ");

                if (item.InteractionSequence == InteractionSequence.UnloadOnly
                    || item.InteractionSequence == InteractionSequence.LoadThenUnload
                    || item.InteractionSequence == InteractionSequence.UnloadThenLoad)
                {
                    var panelSilo = PayloadPanels.FirstOrDefault(x => x.SiloCode != "" && x.SiloCode.ToLower() != InteractingDevice.DeviceId.ToLower());
                    var siloCode = panelSilo == null ? $"AGVnull生成" : panelSilo.SiloCode;

                    operationEntity.PanelList[0].SiloCode = siloCode;
                    operationEntity.PanelList[0].ProductStatus = ProductStatus.PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1;
                    operationEntity.PanelList[0].Layer = InteractingDevice.loadAndUnLoadLayer;
                    operationEntity.PanelList[0].Position = 1;
                    operationEntity.PanelList[0].LocationCode = DeviceDescriptor.DeviceId;
                    PayloadPanels[InteractingDevice.loadAndUnLoadLayer] = operationEntity.PanelList[0];
                    logger.LogDebug($"更新{InteractingDevice.loadAndUnLoadLayer + 1}层下料信息：{JsonSerializer.Serialize(operationEntity.PanelList[0])}\r\n ");
                }
                else
                {
                    for (int i = 0; i < operationEntity.PanelList.Count; i++)
                    {
                        var panel = operationEntity.PanelList[i];
                        panel.ProductStatus = ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1;
                        panel.Layer = i + 1;
                        panel.Position = 1;
                        panel.LocationCode = DeviceDescriptor.DeviceId;
                    }
                    PayloadPanels.Clear();
                    PayloadPanels.AddRange(operationEntity.PanelList);
                }
                PayloadPanels.RaiseCollectionChangedEvent(InteractingDevice.DeviceId);
            }
            catch (Exception ex)
            {
                InteractingDevice.ReportingProcess("下料更新板料信息异常UpdateSiloInfoForLoad", AlarmLevel.Severe, "AES10025");
                logger.LogError(ex, "下料更新板料信息异常UpdateSiloInfoForLoad：" + ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_UpdateUnloadingSiloInfo_Exception", "Exception", ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }
    }
}
