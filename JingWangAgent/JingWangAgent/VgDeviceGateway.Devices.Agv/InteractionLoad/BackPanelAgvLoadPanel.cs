using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Agv.InteractionLoad
{
    public class BackPanelAgvLoadPanel : DeviceShare<DefaultAgv>, IAgvLoadInteraction
    {
        private readonly ILogger<BackPanelAgvLoadPanel> logger;

        public BackPanelAgvLoadPanel(ILogger<BackPanelAgvLoadPanel> logger, IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
        {
            this.logger = logger;
        }

        public async Task<DeviceServiceInvokeResponse> CheckDeviceStatusLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperationType)
        {
            var checkresult = CheckStatusPanel(deviceServiceInvokeRequest, deviceOperationType);
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
            return await PrepareLoadForPanel(deviceServiceInvokeRequest);
        }

        private async Task<DeviceServiceInvokeResponse> InvokeLoadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            return await InvokeLoadForPanel(request);
        }

        public async Task<DeviceServiceInvokeResponse> CompleteLoadMaterialSelf(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            return await CompleteLoadForPanel(deviceServiceInvokeRequest);
        }

        private async Task<DeviceServiceInvokeResponse> PrepareLoadForPanel(DeviceServiceInvokeRequest request)
        {
            try
            {
                var item = JsonSerializer.Deserialize<SwapPanel>(request.Params["OperationListItem"].ToStr());
                if (item == null)
                {
                    logger.LogDebug("\r\n PrepareLoadForPanel_OperationListItem_Null\r\n");
                    return await InteractingDevice.ResponseFail("PrepareLoadForPanel_OperationListItem_Null ");
                }

                var startTime = DateTime.Now;
                var carCurrentPos = "";

                logger.LogDebug("\r\n  等待_车辆当前位置和上料位置相匹配\r\n");
                InteractingDevice.ReportingProcess("等待_车辆当前位置和上料位置相匹配");
                var isTimeOut = false;
                var isFinishedWork = false;
                if (InteractingDevice.configExtra["AgvChassisSupplier"].ToString() == "AjwRobot")
                {
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
                            logger.LogDebug($"\r\n 等待 车辆当前位置和上料位置相匹配 超时\r\n");
                            break;
                        }
                        isFinishedWork = InteractingDevice.CanProceedNextStep();
                        if (isFinishedWork)
                        {
                            logger.LogDebug($"\r\n 程序异常跳出循环 车辆当前位置和上料位置相匹配 \r\n");
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

                logger.LogDebug("\r\n 收到_车辆当前位置和上料位置相匹配\r\n");
                InteractingDevice.ReportingProcess("收到_车辆当前位置和上料位置相匹配");

                var offsetX = InteractingDevice.WatchingProperties.Property("Offset_X").NewValue.ToFloat();
                logger.LogDebug($"\r\n 当前位{carCurrentPos}—{item.AgvPosition} 给PLC赋值 X轴偏差值：{offsetX}\r\n");
                InteractingDevice.ReportingProcess($" 当前位{carCurrentPos}—{item.AgvPosition} 给PLC赋值 X轴偏差值：{offsetX}");
                InteractingDevice.modbusIpMaster.WriteMultipleRegisters(InteractingDevice.slaveId, 5019, offsetX.FloatToReal());//偏移量

                logger.LogDebug("\r\n 给PLC写入AGV到位5056\r\n");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5056, 1);

                logger.LogDebug("\r\n 开始上板料 5014\r\n");
                InteractingDevice.ReportingProcess("开始上板料 5014");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5014, 1);

                var enabledScannig = InteractingDevice.configExtra["EnabledScannig"].ToBool();
                logger.LogDebug($"\r\n 上料是否启用读取二维码：{enabledScannig}\r\n");
                InteractingDevice.ReportingProcess($"上料是否启用读取二维码：{enabledScannig}");
                if (enabledScannig)
                {
                    InteractingDevice.panelBarCode = "";
                    var response = await InteractingDevice.MonitoringSignal(4028, "PLC通知可以读码4028");

                    StringBuilder resultaaa = new StringBuilder();
                    Tuple<bool, string>[] aaa = new Tuple<bool, string>[6];
                    string[] aaa2 = new string[6];

                    for (int i = 0; i < 6; i++)
                    {
                        aaa[i] = InteractingDevice.ReadMaterialCode(4350 + i);
                        aaa2[i] = InteractingDevice.ScannigConvert(aaa[i].Item2);
                        resultaaa.Append(aaa2[i]);
                    }
                    string fullResult = resultaaa.ToString();
                    string str = fullResult.Split(new[] { "\r\0" }, StringSplitOptions.None)[0];
                    // var coderesult = InteractingDevice.ReadMaterialCode(4350);
                    logger.LogDebug($"\r\n 板料二维码信息4350：{enabledScannig}\r\n");
                    InteractingDevice.ReportingProcess($"GET-TrasAGV读码：{"PLC：" + aaa[0].Item2 + aaa[1].Item2 + aaa[2].Item2 + aaa[3].Item2 + aaa[4].Item2 + aaa[5].Item2 + "转换后" + str}");
                    InteractingDevice.panelBarCode = str;//AGV读码赋给板料

                    #region 旧逻辑

                    //logger.LogDebug($"\r\n 告知PLC启用扫码功能 \r\n");
                    //InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5065, 1);

                    //var response = await InteractingDevice.MonitoringSignal(xxx, "比对二维码信号：xxx");//后期
                    //if (!response.Item1)
                    //{
                    //    return await InteractingDevice.ResponseFail(response.Item2);
                    //}
                    //var code = InteractingDevice.ReadMaterialCode();
                    //var swapPanel = request.Params["LoadingPanel"] as SwapPanel;
                    //var panelxxx = swapPanel.PanelList.FirstOrDefault();
                    //var istrue = 2;
                    //var aaa = panelxxx.PanelCode;
                    ////todo 未统一 暂不测
                    ////if (code.Item2 == panelxxx.PanelCode)
                    ////{
                    ////    istrue = 1;
                    ////}
                    //istrue = 1;
                    //logger.LogDebug($"\r\n 二维码比对结果5064：{istrue} \r\n");
                    //InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5064, istrue.ToUshort());

                    #endregion 旧逻辑
                }
                else
                {
                    logger.LogDebug($"\r\n 上料是否启用读取二维码 \r\n");
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5065, 2);
                }

                var result = SetLoadingPanel(request, item);
                logger.LogDebug($"\r\n 设置上料的板料信息，{result}\r\n");
                InteractingDevice.ReportingProcess($"设置上料的板料信息，{result}");
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
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_PrepareLoadForDrill_Exception", "Exception", ex.Message);
                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, $"PrepareLoadForDrill异常：{ex.Message}");
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        private async Task<DeviceServiceInvokeResponse> InvokeLoadForPanel(DeviceServiceInvokeRequest request)
        {
            try
            {
                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_InvokeLoadForDrill_Exception", "Exception", ex.Message);
                return await InteractingDevice.ResponseFail(ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        private async Task<DeviceServiceInvokeResponse> CompleteLoadForPanel(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            try
            {
                logger.LogDebug("\r\n默认 BUFFER已收到生料 5027\r\n");
                InteractingDevice.ReportingProcess($"默认 BUFFER已收到生料 5027");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5027, 1);

                InteractingDevice.ReportingProcess("等待 上生料流程结束：4022");
                var response = await InteractingDevice.MonitoringSignal(4022, "上生料流程结束：4022");
                if (!response.Item1)
                {
                    InteractingDevice.ReportingProcess("上生料流程结束4022超时", AlarmLevel.Severe, "AEP20002");
                    return await InteractingDevice.ResponseFail(response.Item2);
                }
                InteractingDevice.ReportingProcess("上生料流程结束：4022");
                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_CompleteLoadForDrill_Exception", "Exception", ex.Message);
                return await InteractingDevice.ResponseFail($"AGV_CompleteLoadForDrill_Exception:{ex.Message}");
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        private bool SetLoadingPanel(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapPanel item)
        {
            bool result = true;
            var onepanel = InteractingDevice.PayloadPanels.FirstOrDefault(x => x != null && x.Layer == InteractingDevice.loadAndUnLoadLayer);
            logger.LogDebug($"板料信息，loadAndUnLoadLayer：{InteractingDevice.loadAndUnLoadLayer}:" + JsonSerializer.Serialize(InteractingDevice.PayloadPanels));
            if (onepanel != null)
            {
                logger.LogDebug($"SetLoadingPanel onepanel 1111");
                onepanel.ProductStatus = ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1;
                onepanel.Position = item.SpindleId;
                onepanel.Barcode = InteractingDevice.panelBarCode;
            }
            else
            {
                logger.LogDebug($"SetLoadingPanel onepanel null");
                result = false;
                return result;
            }

            logger.LogDebug($"SetLoadingPanel onepanel 2222:" + JsonSerializer.Serialize(onepanel));
            var loadingPanel = new SwapPanel()
            {
                AgvPosition = item.AgvPosition,
                SpindleId = item.SpindleId,

                PanelList = new List<Panel>() { onepanel }
            };
            deviceServiceInvokeRequest.Params["LoadingPanel"] = loadingPanel;
            logger.LogDebug($"SetLoadingPanel onepanel 3333");
            return result;
        }

        public async Task<DeviceServiceInvokeResponse> TargetDeviceOperationLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
        {
            return await InteractingDevice.TargetDeviceOperation(deviceServiceInvokeRequest, deviceOperation);
        }

        public void ResetSingleStart(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            LoadPanelResetSingleStart();
        }

        public void ResetSingleFinished(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            LoadPanelResetSingleFinished();
        }

        private void LoadPanelResetSingleStart()
        {
        }

        private void LoadPanelResetSingleFinished()
        {
            logger.LogDebug("\r\n 重置 比对二维码信号4027\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4027, 0);
            logger.LogDebug("\r\n 重置 上生料流程结束4022\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4022, 0);
            logger.LogDebug("\r\n 重置 允许读码 4028\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 4028, 0);
        }

        public void UpdateSiloInfoForLoad(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            try
            {
                logger.LogDebug($"BackPanelAgvLoadPanel_UpdateSiloInfoForLoad_更新{InteractingDevice.loadAndUnLoadLayer}层信息");
                PayloadPanels[InteractingDevice.loadAndUnLoadLayer].SetEmpty(InteractingDevice.DeviceId, PayloadPanels.SiloCode);
                PayloadPanels[InteractingDevice.loadAndUnLoadLayer].Position = 1;
                PayloadPanels.RaiseCollectionChangedEvent(InteractingDevice.DeviceId);
            }
            catch (Exception ex)
            {
                InteractingDevice.ReportingProcess("上料更新板料信息异常UpdateSiloInfoForLoad", AlarmLevel.Severe, "AES10024");
                logger.LogError(ex, "上料更新板料信息异常UpdateSiloInfoForLoad：" + ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_UpdateLoadingSiloInfo_Exception", "Exception", ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }
    }
}
