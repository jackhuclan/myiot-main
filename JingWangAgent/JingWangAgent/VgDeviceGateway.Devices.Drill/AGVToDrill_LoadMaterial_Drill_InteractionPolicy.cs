using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Interaction;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgDeviceGateway.Devices.Drill.InteractionLoad;

namespace VgDeviceGateway.Devices.Drill
{
    public class AGVToDrill_LoadMaterial_Drill_InteractionPolicy : AbstractLoadMaterialInteractionPolicy<DefaultDrill>
    {
        private readonly ILogger<AGVToDrill_LoadMaterial_Drill_InteractionPolicy> logger;
        private readonly IObjectFactory factory;
        private Dictionary<string, IDrillLoadInteraction> interactionLoadDrills;

        public AGVToDrill_LoadMaterial_Drill_InteractionPolicy(
              ILogger<AGVToDrill_LoadMaterial_Drill_InteractionPolicy> logger,
              IServiceProvider serviceProvider,
              IObjectFactory factory,
              DefaultDrill device)
              : base(serviceProvider, device)
        {
            this.logger = logger;
            this.factory = factory;
            interactionLoadDrills = InteractionFactory.CreateDrillInteractiveLoadAllObject(factory, InteractingDevice);
        }

        public override async Task<DeviceServiceInvokeResponse> PrepareLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            return await DoService(deviceServiceInvokeRequest, async (request, position, directionAndType) =>
            {
                return await interactionLoadDrills[directionAndType].PrepareLoadMaterialLocal(request, position);
            }, "PrepareLoadMaterial");
        }

        public override async Task<DeviceServiceInvokeResponse> InvokeLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            return await DoService(deviceServiceInvokeRequest, async (request, position, directionAndType) =>
            {
                return await interactionLoadDrills[directionAndType].InvokeLoadMaterialLocal(position);
            }, "InvokeLoadMaterial");
        }

        public override async Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            return await DoService(deviceServiceInvokeRequest, async (request, position, directionAndType) =>
            {
                return await interactionLoadDrills[directionAndType].CompleteLoadMaterialLocal(request, position);
            }, "CompleteLoadMaterial");
        }

        private async Task<DeviceServiceInvokeResponse> DoService(DeviceServiceInvokeRequest deviceServiceInvokeRequest,
                                                                  Func<DeviceServiceInvokeRequest, int, string, Task<DeviceServiceInvokeResponse>> action,
                                                                  string methodName = "")
        {
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to {methodName}!");
            logger.LogDebug($"上料 begin to  {methodName} 上下料过程中传入的参数 {JsonSerializer.Serialize(deviceServiceInvokeRequest)}");
            var eventId = "";
            var eventName = "";
            var eventMessage = "";
            try
            {
                var direction = deviceServiceInvokeRequest.CallerRequestInteractionDirection;
                var materialKind = deviceServiceInvokeRequest.CallerRequestMaterialKind;
                string directionAndType = $"{direction}Load{materialKind}";
                if (DeviceDescriptor.DeviceKind == DeviceKind.CNC95Drill)
                {
                    directionAndType = $"Rear95Load{materialKind}";
                }
                if (DeviceDescriptor.Extra["AgvOperationTypes"].ToInt() == 8)
                {
                    directionAndType = $"Half{directionAndType}";
                    directionAndType = $"HalfRearLoad{materialKind}";
                }
                var preCondition = await interactionLoadDrills[directionAndType].CanExecuteDeviceServiceInvokeLocal(deviceServiceInvokeRequest, InteractingDevice, eventId, eventName, eventMessage, methodName);
                if (preCondition.Code != ErrorCodes.Sys.SUCCESS)
                {
                    return preCondition;
                }
                ushort position = deviceServiceInvokeRequest.Params["Position"].ToUshort();

                var actionResult = await action.Invoke(deviceServiceInvokeRequest, position, directionAndType);
                if (actionResult.Code != ErrorCodes.Sys.SUCCESS)
                {
                    eventId = actionResult.Code;
                    eventName = $"方法：{methodName} 未正常执行";
                    eventMessage = actionResult.Message;
                }
                return actionResult;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                eventId = $"{methodName}_Exception";
                eventName = "Exception";
                eventMessage = ex.Message;
                return await InteractingDevice.Response("钻机代理异常："+ErrorCodes.Sys.EXCEPTION_CODE, ex.Message);
            }
            finally
            {
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} finish {methodName}!");
               
            }
        }
    }
}
