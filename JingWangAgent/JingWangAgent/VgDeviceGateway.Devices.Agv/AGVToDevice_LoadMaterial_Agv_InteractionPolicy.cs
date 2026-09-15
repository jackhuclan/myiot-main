using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Interaction;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgDeviceGateway.Devices.Agv.InteractionLoad;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Agv
{
    public class AGVToDevice_LoadMaterial_Agv_InteractionPolicy : AbstractLoadMaterialInteractionPolicy<DefaultAgv>
    {
        private readonly ILogger<AGVToDevice_LoadMaterial_Agv_InteractionPolicy> logger;
        private readonly IObjectFactory factory;
        private Dictionary<string, IAgvLoadInteraction> interactionLoadAgv;

        public AGVToDevice_LoadMaterial_Agv_InteractionPolicy(
            ILogger<AGVToDevice_LoadMaterial_Agv_InteractionPolicy> logger,
            IServiceProvider serviceProvider,
            IObjectFactory factory,
            DefaultAgv device)
            : base(serviceProvider, device)
        {
            this.logger = logger;
            this.factory = factory;
            InitInteractionAgvs();
        }

        public void InitInteractionAgvs()
        {
            try
            {
                interactionLoadAgv = InteractionAgvFactory.CreateDrillInteractiveLoadAllObject(InteractingDevice);
            }
            catch (Exception ex)
            {
                var message = $"设备类型：{InteractingDevice.DeviceDescriptor.DeviceKind}，交互类型：request.CallerRequestMaterialKind不正确：{ex.Message}";
                logger.LogDebug(message);
                logger.LogError(message);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to CompleteLoadMaterial!");

            try
            {
                var serviceCallResponse = await DoServiceCallDeviceFirstComplete(deviceServiceInvokeRequest, DeviceOperationType.CompleteLoadMaterial,
                  async (deviceOperationType, directionAndType) =>
                  {
                      return await interactionLoadAgv[directionAndType].CheckDeviceStatusLocal(deviceServiceInvokeRequest, deviceOperationType);
                  },
                  async (deviceServiceInvokeRequest, deviceOperationType, directionAndType) =>
                  {
                      return await interactionLoadAgv[directionAndType].TargetDeviceOperationLocal(deviceServiceInvokeRequest, deviceOperationType);
                  },
                  async (deviceServiceInvokeRequest, deviceOperationType, directionAndType) =>
                  {
                      return await interactionLoadAgv[directionAndType].AgvLoadMaterialSelfLocal(deviceServiceInvokeRequest, deviceOperationType);
                  }

                  );

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} end to CompleteLoadMaterial!");
                return serviceCallResponse;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_CompleteLoadMaterial_Exception", "Exception", ex.Message);
                return await InteractingDevice.Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> InvokeLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to InvokeLoadMaterial!");
            try
            {
                var serviceCallResponse = await DoServiceCallDeviceFirstComplete(deviceServiceInvokeRequest, DeviceOperationType.InvokeLoadMaterial,
                       async (deviceOperationType, directionAndType) =>
                       {
                           return await interactionLoadAgv[directionAndType].CheckDeviceStatusLocal(deviceServiceInvokeRequest, deviceOperationType);
                       },
                       async (deviceServiceInvokeRequest, deviceOperationType, directionAndType) =>
                       {
                           return await interactionLoadAgv[directionAndType].TargetDeviceOperationLocal(deviceServiceInvokeRequest, deviceOperationType);
                       },
                       async (deviceServiceInvokeRequest, deviceOperationType, directionAndType) =>
                       {
                           return await interactionLoadAgv[directionAndType].AgvLoadMaterialSelfLocal(deviceServiceInvokeRequest, deviceOperationType);
                       }
                       );

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} end to InvokeLoadMaterial!");

                return serviceCallResponse;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_InvokeLoadMaterial_Exception", "Exception", ex.Message);
                return await InteractingDevice.Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> PrepareLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to PrepareLoadMaterial!");
            try
            {
                var serviceCallResponse = await DoServiceCallDeviceFirstComplete(deviceServiceInvokeRequest, DeviceOperationType.PrepareLoadMaterial,
                          async (deviceOperationType, directionAndType) =>
                          {
                              return await interactionLoadAgv[directionAndType].CheckDeviceStatusLocal(deviceServiceInvokeRequest, deviceOperationType);
                          },
                          async (deviceServiceInvokeRequest, deviceOperationType, directionAndType) =>
                          {
                              if (deviceServiceInvokeRequest.Params.ContainsKey("IsPassPre"))
                              {
                                  if (deviceServiceInvokeRequest.Params["IsPassPre"].ToInt() == 1)
                                  {
                                      logger.LogDebug("执行跳过Preload");
                                      InteractingDevice.ReportingProcess("执行跳过Preload");
                                      return await Response(ErrorCodes.Sys.SUCCESS, "执行跳过Preload");
                                  }
                              }
                              logger.LogDebug("没有执行跳过Preload");
                              InteractingDevice.ReportingProcess("没有执行跳过Preload");
                              return await interactionLoadAgv[directionAndType].TargetDeviceOperationLocal(deviceServiceInvokeRequest, deviceOperationType);
                          },
                          async (deviceServiceInvokeRequest, deviceOperationType, directionAndType) =>
                          {
                              return await interactionLoadAgv[directionAndType].AgvLoadMaterialSelfLocal(deviceServiceInvokeRequest, deviceOperationType);
                          }
                          );

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} end to PrepareLoadMaterial!");
                return serviceCallResponse;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_PrepareLoadMaterial_Exception", "Exception", ex.Message);
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
                string directionAndType = $"{InteractingDevice.DeviceDescriptor.DeviceKind}Load{materialKind}";

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
                      Func<DeviceOperationType, string, Task<DeviceServiceInvokeResponse>> checkDeviceStatusAction,
                      Func<DeviceServiceInvokeRequest, DeviceOperationType, string, Task<DeviceServiceInvokeResponse>> deviceAction,
                      Func<DeviceServiceInvokeRequest, DeviceOperationType, string, Task<DeviceServiceInvokeResponse>> selfAction)
        {
            try
            {
                var materialKind = deviceServiceInvokeRequest.CallerRequestMaterialKind;
                string directionAndType = $"{InteractingDevice.DeviceDescriptor.DeviceKind}Load{materialKind}";

                logger.LogDebug($"\r\n 开始DoServiceCallDeviceFirstComplete_deviceOperationType:{deviceOperationType}_directionAndType:{directionAndType}_checkDeviceStatusAction \r\n");
                var checkStatusResponse = await checkDeviceStatusAction.Invoke(deviceOperationType, directionAndType);
                var message = $"DoServiceCallDeviceFirstComplete_deviceOperationType:{deviceOperationType}_directionAndType:{directionAndType}_checkDeviceStatusAction:{checkStatusResponse.Code}_{checkStatusResponse.Message}_Request:{JsonSerializer.Serialize(deviceServiceInvokeRequest)}";
                logger.LogDebug($"\r\n {message} \r\n");
                if (checkStatusResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_DoServiceCall_CheckDeviceStatusActionError", "CheckDeviceStatusActionError", checkStatusResponse.Message);
                    return await InteractingDevice.ResponseFail(checkStatusResponse.Message);
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

                logger.LogDebug($"\r\n 开始DoServiceCallDeviceFirstComplete_deviceOperationType:{deviceOperationType}_directionAndType:{directionAndType}_deviceAction \r\n");
                var deviceResult = await deviceAction.Invoke(deviceServiceInvokeRequest, deviceOperationType, directionAndType);
                var deviceResultmessage = $"DoServiceCallDeviceFirstComplete_deviceOperationType:{deviceOperationType}_directionAndType:{directionAndType}_deviceAction:{deviceResult.Code}_{deviceResult.Message}_Request:{JsonSerializer.Serialize(deviceServiceInvokeRequest)}";
                logger.LogDebug($"\r\n {deviceResultmessage} \r\n");
                if (deviceResult.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_DoServiceCall_DeviceActionError", "DeviceActionError", deviceResult.Message);
                    return await InteractingDevice.ResponseFail(deviceResult.Message);
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

        public async Task<DeviceServiceInvokeResponse> LoadMaterialBehavior(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            try
            {
                if (InteractingDevice.CanProceedNextStep())
                {
                    return await ResponseFail("结束任务：开始PrepareLoadMaterial 之前");
                }

                await InteractingDevice.ReportingProcess("开始PrepareLoadMaterial");
                logger.LogDebug($"\r\n 开始PrepareLoadMaterial \r\n");

                //deviceServiceInvokeRequest.Params["SwapPanelItem"] = JsonSerializer.Serialize(item);
                var deviceOperationResponse = await PrepareLoadMaterial(deviceServiceInvokeRequest);
                var errormsg = string.Format($"PrepareLoadMaterial：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                logger.LogDebug($"\r\n {errormsg} \r\n");
                await InteractingDevice.ReportingProcess(errormsg);
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_LoadMaterialBehavior_PrepareLoadMaterialNoSuccess", "PrepareLoadMaterialNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                if (InteractingDevice.CanProceedNextStep())
                {
                    return await ResponseFail("结束任务：开始InvokeLoadMaterial 之前");
                }

                await InteractingDevice.ReportingProcess("开始InvokeLoadMaterial");
                logger.LogDebug($"\r\n 开始InvokeLoadMaterial \r\n");
                //deviceServiceInvokeRequest.Params["SwapPanelItem"] = JsonSerializer.Serialize(item);
                deviceOperationResponse = await InvokeLoadMaterial(deviceServiceInvokeRequest);
                errormsg = string.Format($"InvokeLoadMaterial：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                logger.LogDebug($"\r\n {errormsg} \r\n");
                await InteractingDevice.ReportingProcess(errormsg);
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_LoadMaterialBehavior_InvokeLoadMaterialNoSuccess", "InvokeLoadMaterialNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                if (InteractingDevice.CanProceedNextStep())
                {
                    return await ResponseFail("结束任务：开始CompleteLoadMaterial 之前");
                }

                await InteractingDevice.ReportingProcess("开始CompleteLoadMaterial");
                logger.LogDebug($"\r\n 开始CompleteLoadMaterial \r\n");
                //deviceServiceInvokeRequest.Params["SwapPanelItem"] = JsonSerializer.Serialize(item);
                deviceOperationResponse = await CompleteLoadMaterial(deviceServiceInvokeRequest);
                errormsg = string.Format($"CompleteLoadMaterial：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                logger.LogDebug($"\r\n {errormsg} \r\n");
                await InteractingDevice.ReportingProcess(errormsg);
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_LoadMaterialBehavior_CompleteLoadMaterialNoSuccess", "CompleteLoadMaterialNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                var materialKind = deviceServiceInvokeRequest.CallerRequestMaterialKind;
                string directionAndType = $"{InteractingDevice.DeviceDescriptor.DeviceKind}Load{materialKind}";

                logger.LogDebug($"\r\n 料仓信息更改_上料 \r\n");
                await InteractingDevice.ReportingProcess("料仓信息更改_上料");
                interactionLoadAgv[directionAndType].UpdateSiloInfoForLoad(deviceServiceInvokeRequest);
                InteractingDevice.currentLoadedCount += 1;
                interactionLoadAgv[directionAndType].ResetSingleFinished(deviceServiceInvokeRequest);

                await Task.Delay(500);
                return deviceOperationResponse;
            }
            catch (Exception ex)
            {
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_LoadMaterialBehavior_Exception", "Exception", "异常：" + ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ErrorCodes.Sys.EXCEPTION_MESSAGE, null);
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }
    }
}
