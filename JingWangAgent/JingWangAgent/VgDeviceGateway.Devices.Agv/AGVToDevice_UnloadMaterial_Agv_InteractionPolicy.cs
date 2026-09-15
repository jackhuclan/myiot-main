using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Interaction;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Agv.InteractionUnload;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Agv
{
    public class AGVToDevice_UnloadMaterial_Agv_InteractionPolicy : AbstractUnloadMaterialInteractionPolicy<DefaultAgv>
    {
        private ILogger<AGVToDevice_UnloadMaterial_Agv_InteractionPolicy> logger;
        private Dictionary<string, IAgvUnloadInteraction> interactionUnloadAgv;

        public AGVToDevice_UnloadMaterial_Agv_InteractionPolicy(
            ILogger<AGVToDevice_UnloadMaterial_Agv_InteractionPolicy> logger,
            IServiceProvider serviceProvider,
            DefaultAgv device)
            : base(serviceProvider, device)
        {
            this.logger = logger;
            InitInteractionAgvs();
        }

        public void InitInteractionAgvs()
        {
            try
            {
                interactionUnloadAgv = InteractionAgvFactory.CreateDrillInteractiveUnloadAllObject(InteractingDevice);
            }
            catch (Exception ex)
            {
                var message = $"设备类型：{InteractingDevice.DeviceDescriptor.DeviceKind}，交互类型：request.CallerRequestMaterialKind不正确：{ex.Message}";
                logger.LogDebug(message);
                logger.LogError(message);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to CompleteUnloadMaterial!");

            try
            {
                var serviceCallResponse = await DoServiceCallDeviceFirstComplete(deviceServiceInvokeRequest, DeviceOperationType.CompleteUnloadMaterial,
                async (deviceOperationType, directionAndType) =>
                {
                    return await interactionUnloadAgv[directionAndType].CheckDeviceStatusLocal(deviceServiceInvokeRequest, deviceOperationType);
                },
                async (deviceServiceInvokeRequest, deviceOperationType, directionAndType) =>
                {
                    return await interactionUnloadAgv[directionAndType].TargetDeviceOperationLocal(deviceServiceInvokeRequest, deviceOperationType);
                },
                async (deviceServiceInvokeRequest, deviceOperationType, directionAndType) =>
                {
                    return await interactionUnloadAgv[directionAndType].AgvUnloadMaterialSelfLocal(deviceServiceInvokeRequest, deviceOperationType);
                }
                );
                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} end to CompleteUnloadMaterial!");

                return serviceCallResponse;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_CompleteUnloadMaterial_Exception", "Exception", ex.Message);
                return await InteractingDevice.Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to InvokeUnloadMaterial!");

            InteractingDevice.WatchingProperties.Property("InvokeUnloadOk").SetValue(false);
            try
            {
                var serviceCallResponse = await DoServiceCallDeviceFirstComplete(deviceServiceInvokeRequest, DeviceOperationType.InvokeUnloadMaterial,
                async (deviceOperationType, directionAndType) =>
                {
                    return await interactionUnloadAgv[directionAndType].CheckDeviceStatusLocal(deviceServiceInvokeRequest, deviceOperationType);
                },
                async (deviceServiceInvokeRequest, deviceOperationType, directionAndType) =>
                {
                    return await interactionUnloadAgv[directionAndType].TargetDeviceOperationLocal(deviceServiceInvokeRequest, deviceOperationType);
                },
                async (deviceServiceInvokeRequest, deviceOperationType, directionAndType) =>
                {
                    return await interactionUnloadAgv[directionAndType].AgvUnloadMaterialSelfLocal(deviceServiceInvokeRequest, deviceOperationType);
                }
                );
                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} end to InvokeUnloadMaterial!");
                return serviceCallResponse;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_InvokeUnloadMaterial_Exception", "Exception", ex.Message);
                return await InteractingDevice.Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> PrepareUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to PrepareUnloadMaterial!");

            InteractingDevice.WatchingProperties.Property("PrepareUnloadOk").SetValue(false);
            try
            {
                var serviceCallResponse = await DoServiceCallDeviceFirstComplete(deviceServiceInvokeRequest, DeviceOperationType.PrepareUnloadMaterial,
                async (deviceOperationType, directionAndType) =>
                {
                    return await interactionUnloadAgv[directionAndType].CheckDeviceStatusLocal(deviceServiceInvokeRequest, deviceOperationType);
                },
                async (deviceServiceInvokeRequest, deviceOperationType, directionAndType) =>
                {
                    if (deviceServiceInvokeRequest.Params.ContainsKey("IsPassPre"))
                    {
                        if (deviceServiceInvokeRequest.Params["IsPassPre"].ToInt() == 1)
                        {
                            logger.LogDebug("执行跳过Preunload");
                            InteractingDevice.ReportingProcess("执行跳过Preunload");
                            return await Response(ErrorCodes.Sys.SUCCESS, "执行跳过unPreload");
                        }
                    }
                    logger.LogDebug("没有执行跳过unPreload");
                    InteractingDevice.ReportingProcess("没有执行跳过unPreload");
                    return await interactionUnloadAgv[directionAndType].TargetDeviceOperationLocal(deviceServiceInvokeRequest, deviceOperationType);
                },
                async (deviceServiceInvokeRequest, deviceOperationType, directionAndType) =>
                {
                    return await interactionUnloadAgv[directionAndType].AgvUnloadMaterialSelfLocal(deviceServiceInvokeRequest, deviceOperationType);
                }
                );
                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} end to PrepareUnloadMaterial!");
                return serviceCallResponse;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_PrepareUnloadMaterial_Exception", "Exception", ex.Message);
                return await InteractingDevice.Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        private async Task<DeviceServiceInvokeResponse> DoServiceCallDeviceFirstComplete(DeviceServiceInvokeRequest deviceServiceInvokeRequest,
        DeviceOperationType deviceOperationType,
        Func<DeviceOperationType, string, Task<DeviceServiceInvokeResponse>> checkDeviceStatusAction,
        Func<DeviceServiceInvokeRequest, DeviceOperationType, string, Task<DeviceServiceInvokeResponse>> deviceAction,
        Func<DeviceServiceInvokeRequest, DeviceOperationType, string, Task<DeviceServiceInvokeResponse>> selfAction)
        {
            try
            {
                var materialKind = deviceServiceInvokeRequest.CallerRequestMaterialKind;
                string directionAndType = $"{InteractingDevice.DeviceDescriptor.DeviceKind}Unload{materialKind}";

                logger.LogDebug($"\r\n 开始DoServiceCallDeviceFirstComplete_deviceOperationType:{deviceOperationType}_directionAndType:{directionAndType}_checkDeviceStatusAction \r\n");
                var checkStatusResponse = await checkDeviceStatusAction.Invoke(deviceOperationType, directionAndType);
                var message = $"DoServiceCallDeviceFirstComplete_deviceOperationType:{deviceOperationType}_directionAndType:{directionAndType}_checkDeviceStatusAction:{checkStatusResponse.Code}_{checkStatusResponse.Message}_Request:{JsonSerializer.Serialize(deviceServiceInvokeRequest)}";
                logger.LogDebug($"\r\n {message} \r\n");
                if (checkStatusResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_DoServiceCall_CheckDeviceStatusActionError", "CheckDeviceStatusActionError", checkStatusResponse.Message);
                    return await InteractingDevice.ResponseFail(checkStatusResponse.Message);
                }

                logger.LogDebug($"\r\n 开始DoServiceCallDeviceFirstComplete_deviceOperationType:{deviceOperationType}_directionAndType:{directionAndType}_deviceAction \r\n");
                var deviceResult = await deviceAction.Invoke(deviceServiceInvokeRequest, deviceOperationType, directionAndType);
                var deviceResultmessage = $"DoServiceCallDeviceFirstComplete_deviceOperationType:{deviceOperationType}_directionAndType:{directionAndType}_deviceAction:{deviceResult.Code}_{deviceResult.Message}_Request:{JsonSerializer.Serialize(deviceServiceInvokeRequest)}";
                logger.LogDebug($"\r\n {deviceResultmessage} \r\n");
                if (deviceResult.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_DoServiceCall_DeviceActionError", "DeviceActionError", deviceResult.Message);
                    return await InteractingDevice.ResponseFail(deviceResult.Message);
                }

                logger.LogDebug($"\r\n 开始DoServiceCallDeviceFirstComplete_deviceOperationType:{deviceOperationType}_directionAndType:{directionAndType}_selfAction \r\n");
                var selfResult = await selfAction.Invoke(deviceServiceInvokeRequest, deviceOperationType, directionAndType);
                var selfResultmessage = $"DoServiceCallDeviceFirstComplete_deviceOperationType:{deviceOperationType}_directionAndType:{directionAndType}_selfAction:{selfResult.Code}_{selfResult.Message}_Request:{JsonSerializer.Serialize(deviceServiceInvokeRequest)}";
                logger.LogDebug($"\r\n {selfResultmessage} \r\n");
                if (selfResult.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_DoServiceCall_SelfActionError", "SelfActionError", selfResult.Message);
                    return await InteractingDevice.ResponseFail(selfResult.Message);
                }

                return await InteractingDevice.ResponseSuccess();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_DoServiceCall_Exception", "Exception", ex.Message);
                return await InteractingDevice.Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        private async Task<DeviceServiceInvokeResponse> DoServiceCallSelfFirstComplete(DeviceServiceInvokeRequest deviceServiceInvokeRequest,
            DeviceOperationType deviceOperationType,
            Func<DeviceOperationType, Task<DeviceServiceInvokeResponse>> checkDeviceStatusAction,
            Func<DeviceServiceInvokeRequest, Task<DeviceServiceInvokeResponse>> selfAction,
                Func<DeviceServiceInvokeRequest, DeviceOperationType, Task<DeviceServiceInvokeResponse>> deviceAction)
        {
            try
            {
                var checkStatusResponse = await checkDeviceStatusAction.Invoke(deviceOperationType);
                var message = $"DoServiceCallSelfFirstComplete_{deviceOperationType}_checkDeviceStatusAction:{checkStatusResponse.Code}_{checkStatusResponse.Message}_Request:{JsonSerializer.Serialize(deviceServiceInvokeRequest)}";
                logger.LogDebug(message);
                if (checkStatusResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_DoServiceCallSelfFirstComplete_checkDeviceStatusActionError", "checkDeviceStatusActionError", checkStatusResponse.Message);
                    return await InteractingDevice.ResponseFail(checkStatusResponse.Message);
                }

                var selfresult = await selfAction.Invoke(deviceServiceInvokeRequest);
                var selfresultmessage = $"DoServiceCallSelfFirstComplete_{deviceOperationType}_selfAction:{selfresult.Code}_{selfresult.Message}_Request:{JsonSerializer.Serialize(deviceServiceInvokeRequest)}";
                logger.LogDebug(selfresultmessage);
                if (selfresult.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_DoServiceCallSelfFirstComplete_SelfActionNoSuccess", "SelfActionNoSuccess", selfresult.Message);
                    return await InteractingDevice.ResponseFail(selfresult.Message);
                }

                var result = await deviceAction.Invoke(deviceServiceInvokeRequest, deviceOperationType);
                var resultmessage = $"DoServiceCallSelfFirstComplete_{deviceOperationType}_deviceAction:{selfresult.Code}_{selfresult.Message}_Request:{JsonSerializer.Serialize(deviceServiceInvokeRequest)}";
                logger.LogDebug(resultmessage);
                if (result.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_DoServiceCallSelfFirstComplete_DeviceActionNoSuccess", "DeviceActionNoSuccess", result.Message);
                    return await InteractingDevice.ResponseFail(result.Message);
                }

                return await InteractingDevice.ResponseSuccess();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_DoServiceCall_Exception", "Exception", ex.Message);
                return await InteractingDevice.Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        public async Task<DeviceServiceInvokeResponse> UnloadMaterialBehavior(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            try
            {
                var errormsg = "";
                if (InteractingDevice.CanProceedNextStep())
                {
                    logger.LogDebug($"\r\n 结束任务：开始 PrepareUnloadMaterial 之前 \r\n");
                    return await ResponseFail("结束任务：开始 PrepareUnloadMaterial 之前");
                }
                await InteractingDevice.ReportingProcess("开始PrepareUnloadMaterial");
                logger.LogDebug($"\r\n 开始PrepareUnloadMaterial \r\n");
                //deviceServiceInvokeRequest.Params["SwapPanelItem"] = JsonSerializer.Serialize(item);
                var deviceOperationResponse = await PrepareUnloadMaterial(deviceServiceInvokeRequest);
                errormsg = string.Format($"PrepareUnloadMaterial：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                logger.LogDebug($"\r\n {errormsg} \r\n");
                await InteractingDevice.ReportingProcess(errormsg);
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_UnloadMaterialBehavior_PrepareUnloadMaterialNoSuccess", "PrepareUnloadMaterialNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                if (InteractingDevice.CanProceedNextStep())
                {
                    return await ResponseFail("结束任务：开始 InvokeUnloadMaterial 之前");
                }
                logger.LogDebug($"\r\n 开始InvokeUnloadMaterial \r\n");
                await InteractingDevice.ReportingProcess("开始InvokeUnloadMaterial");
                //deviceServiceInvokeRequest.Params["SwapPanelItem"] = JsonSerializer.Serialize(item);
                deviceOperationResponse = await InvokeUnloadMaterial(deviceServiceInvokeRequest);
                errormsg = string.Format($"InvokeUnloadMaterial：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                logger.LogDebug($"\r\n {errormsg} \r\n");
                await InteractingDevice.ReportingProcess(errormsg);
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_UnloadMaterialBehavior_InvokeUnloadMaterialNoSuccess", "InvokeUnloadMaterialNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                if (InteractingDevice.CanProceedNextStep())
                {
                    return await ResponseFail("结束任务：开始CompleteLoadMaterial 之前");
                }

                logger.LogDebug($"\r\n 开始CompleteUnloadMaterial \r\n");
                await InteractingDevice.ReportingProcess("开始CompleteUnloadMaterial");
                //deviceServiceInvokeRequest.Params["SwapPanelItem"] = JsonSerializer.Serialize(item);
                deviceOperationResponse = await CompleteUnloadMaterial(deviceServiceInvokeRequest);
                errormsg = string.Format($"CompleteUnloadMaterial：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                logger.LogDebug($"\r\n {errormsg} \r\n");
                await InteractingDevice.ReportingProcess(errormsg);
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_UnloadMaterialBehavior_CompleteUnloadMaterialNoSuccess", "CompleteUnloadMaterialNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                if (deviceOperationResponse.Params.ContainsKey("UnloadingPanel"))
                {
                    logger.LogDebug($"\r\n 料仓信息更改_下料,钻机返回的交互板料：{JsonSerializer.Serialize(deviceOperationResponse.Params["UnloadingPanel"])} \r\n");
                }
                else
                {
                    logger.LogDebug($"\r\n 料仓信息更改_下料,钻机返回的交互板料：无无无无无无无无无无无无无无无无无无无无 \r\n");
                }
                var materialKind = deviceServiceInvokeRequest.CallerRequestMaterialKind;
                string directionAndType = $"{InteractingDevice.DeviceDescriptor.DeviceKind}Unload{materialKind}";
                logger.LogDebug($"\r\n 料仓信息更改_下料 \r\n");
                await InteractingDevice.ReportingProcess("料仓信息更改_下料");
                interactionUnloadAgv[directionAndType].UpdateSiloInfoForUnload(deviceServiceInvokeRequest);
                InteractingDevice.currentUnloadedCount += 1;
                interactionUnloadAgv[directionAndType].ResetSingleFinished(deviceServiceInvokeRequest);

                return deviceOperationResponse;
            }
            catch (Exception ex)
            {
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_UnloadMaterialBehavior_Exception", "Exception", "异常：" + ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ErrorCodes.Sys.EXCEPTION_MESSAGE);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }
    }
}
