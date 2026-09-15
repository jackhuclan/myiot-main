using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Agv.InteractionLoad
{
    public class FrontToolAgvLoadCutter : DeviceShare<DefaultAgv>, IAgvLoadInteraction
    {
        private readonly ILogger<FrontToolAgvLoadCutter> logger;

        public FrontToolAgvLoadCutter(ILogger<FrontToolAgvLoadCutter> logger, IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
        {
            this.logger = logger;
        }

        public async Task<DeviceServiceInvokeResponse> CheckDeviceStatusLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperationType)
        {
            var checkresult = CheckStatusTray(deviceServiceInvokeRequest, deviceOperationType);
            if (!checkresult.Item1)
            {
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_PrepareLoadMaterial_CheckStatusError", "CheckStatusError", checkresult.Item3);
            }
            return await InteractingDevice.Response(checkresult.Item1 ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL, checkresult.Item3);
        }

        private Tuple<bool, string, string> CheckStatusTray(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
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
            return await PrepareLoadForCutter(deviceServiceInvokeRequest);
        }

        private async Task<DeviceServiceInvokeResponse> InvokeLoadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            return await InvokeLoadForCutter(request);
        }

        public async Task<DeviceServiceInvokeResponse> CompleteLoadMaterialSelf(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            return await CompleteLoadForCutter(deviceServiceInvokeRequest);
        }

        private async Task<DeviceServiceInvokeResponse> PrepareLoadForCutter(DeviceServiceInvokeRequest request)
        {
            try
            {
                //var item = request.Params["OperationListItem"] as SwapTray;
                var item = JsonSerializer.Deserialize<SwapTray>(request.Params["OperationListItem"].ToStr());
                if (item == null)
                {
                    logger.LogDebug("\r\n PrepareLoadForTray_OperationListItem_Null\r\n");
                    return await InteractingDevice.ResponseFail("PrepareLoadForTray_OperationListItem_Null ");
                }

                if (item.InteractionSequence == InteractionSequence.LoadOnly)
                {
                    logger.LogDebug($"{item.InteractionSequence}过了pre，success默认插齿上无刀盒 3006");
                    await InteractingDevice.ReportingProcess($"{item.InteractionSequence}过了pre，success默认插齿上无刀盒 3006");
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 3006, 2);
                }
                else
                {
                    logger.LogDebug($"{item.InteractionSequence}不是只上生料，不赋值3006");
                }
                var startTime = DateTime.Now;
                var carCurrentPos = "";

                logger.LogDebug("\r\n  等待_车辆当前位置和上料位置相匹配\r\n");
                await InteractingDevice.ReportingProcess("等待_车辆当前位置和上料位置相匹配");
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
                    await InteractingDevice.ReportingProcess(pipeimsg);
                    return await InteractingDevice.ResponseFail(pipeimsg);
                }
                logger.LogDebug("\r\n 收到_车辆当前位置和上料位置相匹配\r\n");
                await InteractingDevice.ReportingProcess("收到_车辆当前位置和上料位置相匹配");

                var offsetX = InteractingDevice.WatchingProperties.Property("Offset_X").NewValue.ToFloat();
                var offsetY = InteractingDevice.WatchingProperties.Property("Offset_Y").NewValue.ToFloat();
                var offsetZ = InteractingDevice.WatchingProperties.Property("Offset_Z").NewValue.ToFloat();
                logger.LogDebug($"\r\n 当前位{carCurrentPos}—{item.AgvPosition} 给PLC赋值 X轴偏差值：{offsetX}，Y轴偏差值：{offsetY}，角度偏差值：{offsetZ}\r\n");
                await InteractingDevice.ReportingProcess($" 当前位{carCurrentPos}—{item.AgvPosition} 给PLC赋值 X轴偏差值：{offsetX}，Y轴偏差值：{offsetY}，角度偏差值：{offsetZ}");
                InteractingDevice.modbusIpMaster.WriteMultipleRegisters(InteractingDevice.slaveId, 3020, offsetX.FloatToReal());//偏移量
                InteractingDevice.modbusIpMaster.WriteMultipleRegisters(InteractingDevice.slaveId, 3022, offsetY.FloatToReal());//偏移量
                InteractingDevice.modbusIpMaster.WriteMultipleRegisters(InteractingDevice.slaveId, 3024, offsetZ.FloatToReal());//偏移量

                var isMang = await InteractingDevice.MonitoringSignal(3505, "机械不在忙碌");
                if (!isMang.Item1)
                {
                    logger.LogDebug($"\r\n 机械不在忙碌：{isMang.Item1} \r\n");
                    return await Response(ErrorCodes.Sys.FAIL, isMang.Item2);
                }

                logger.LogDebug("\r\n 开始上刀盒 3001\r\n");
                await InteractingDevice.ReportingProcess("开始上刀盒 3001");
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 3001, 1);

                logger.LogDebug("\r\n 设置上料的刀盒信息\r\n");
                await InteractingDevice.ReportingProcess("设置上料的刀盒信息");
                var result = SetLoadingTray(request, item);
                if (!result)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_SetLoadingTray_SetLoadingTrayError", "SetLoadingTrayError", "SetLoadingTray false");
                    await InteractingDevice.ReportingProcess("设置上料的刀盒信息_异常");
                    return await InteractingDevice.ResponseFail("设置上料的刀盒信息 异常");
                }

                var enabledScannig = InteractingDevice.configExtra["EnabledScannig"].ToBool();
                logger.LogDebug($"\r\n 是否启用二维码：{enabledScannig}\r\n");
                await InteractingDevice.ReportingProcess($"是否启用二维码：{enabledScannig}");
                if (enabledScannig)
                {
                    logger.LogDebug($"\r\n 告知PLC启用扫码功能 3027 \r\n");
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 3027, 1);

                    var response = await InteractingDevice.MonitoringSignal(3515, "告知上位机可以对比二维码：3515");
                    if (!response.Item1)
                    {
                        return await InteractingDevice.ResponseFail(response.Item2);
                    }
                    var code = InteractingDevice.ReadMaterialCode(4350);
                    var swapTray = request.Params["LoadingTray"] as SwapTray;
                    var Trayxxx = swapTray.TrayList.FirstOrDefault();
                    var istrue = 2;
                    var aaa = Trayxxx.TrayCode;
                    //todo 未统一 暂不测
                    //if (code.Item2 == Trayxxx.TrayCode)
                    //{
                    //    istrue = 1;
                    //}
                    istrue = 1;
                    logger.LogDebug($"\r\n 二维码比对结果 3028：{istrue} \r\n");
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 3028, istrue.ToUshort());
                }
                else
                {
                    logger.LogDebug($"\r\n 告知PLC 不启用扫码功能 3027：2 \r\n");
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 3027, 2);
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

        private async Task<DeviceServiceInvokeResponse> InvokeLoadForCutter(DeviceServiceInvokeRequest request)
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

        private async Task<DeviceServiceInvokeResponse> CompleteLoadForCutter(DeviceServiceInvokeRequest request)
        {
            try
            {
                var item = JsonSerializer.Deserialize<SwapTray>(request.Params["OperationListItem"].ToStr());
                if (item == null)
                {
                    logger.LogDebug("\r\n PrepareLoadForTray_OperationListItem_Null\r\n");
                    return await InteractingDevice.ResponseFail("PrepareLoadForTray_OperationListItem_Null ");
                }

                InteractingDevice.ReportingProcess("等待 上生料流程结束：3506");
                var response = await InteractingDevice.MonitoringSignal(3506, "上生料流程结束：3506");
                if (!response.Item1)
                {
                    return await InteractingDevice.ResponseFail(response.Item2);
                }
                InteractingDevice.ReportingProcess("收到 上生料流程结束：3506");
                InteractingDevice.ReportingProcess("等待 单轴任务完成：3509");
                var response2 = await InteractingDevice.MonitoringSignal(3509, "单轴任务完成：3509");
                if (!response2.Item1)
                {
                    return await InteractingDevice.ResponseFail(response2.Item2);
                }
                InteractingDevice.ReportingProcess("单轴任务完成：3509");

                if (item.Region > 3)
                {
                    InteractingDevice.ReportingProcess("等待 单点任务完成：3510");
                    var response3 = await InteractingDevice.MonitoringSignal(3510, $"单点位任务完成:{item.Region}：3510");
                    if (!response3.Item1)
                    {
                        return await InteractingDevice.ResponseFail(response2.Item2);
                    }
                    InteractingDevice.ReportingProcess("收到 单点任务完成：3510");
                }
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

        private bool SetLoadingTray(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapTray item)
        {
            bool result = true;
            var layer = (int)(InteractingDevice.loadAndUnLoadLayer / 10);
            var position = (int)(InteractingDevice.loadAndUnLoadLayer % 10);

            var oneTray = InteractingDevice.PayloadCutterTrays.FirstOrDefault(x => x != null && x.Z == layer - 1 && x.IndexOnLayer == position);
            if (oneTray != null)
            {
                logger.LogDebug($"SetLoadingTray oneTray 1111");
                oneTray.SetEmpty();
            }
            else
            {
                logger.LogDebug($"SetLoadingTray oneTray null");
                result = false;
                return result;
            }

            logger.LogDebug($"SetLoadingTray oneTray 2222:" + JsonSerializer.Serialize(oneTray));
            var loadingTray = new SwapTray()
            {
                AgvPosition = item.AgvPosition,
                SpindleId = item.SpindleId,

                TrayList = new List<CutterTray>() { oneTray }
            };
            deviceServiceInvokeRequest.Params["LoadingTray"] = loadingTray;
            logger.LogDebug($"SetLoadingTray oneTray 3333");
            return result;
        }

        public async Task<DeviceServiceInvokeResponse> TargetDeviceOperationLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
        {
            return await InteractingDevice.TargetDeviceOperation(deviceServiceInvokeRequest, deviceOperation);
        }

        public void ResetSingleStart(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            LoadTrayResetSingleStart();
        }

        public void ResetSingleFinished(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            LoadTrayResetSingleFinished();
        }

        private void LoadTrayResetSingleStart()
        {
        }

        private void LoadTrayResetSingleFinished()
        {
            logger.LogDebug("\r\n 重置 上生料流程结束3506\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 3506, 0);
            logger.LogDebug("\r\n 重置 预准备信号3508\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 3508, 0);
            logger.LogDebug("\r\n 重置 单轴任务完成3509\r\n");
            InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 3509, 0);
        }

        public void UpdateSiloInfoForLoad(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            try
            {
                PayloadCutterTrays[InteractingDevice.loadAndUnLoadLayer].Status = CutterTrayStatus.EmptyTray;
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
