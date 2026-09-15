using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Interaction;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Shelf
{
    public class AGVToSiloShelf_UnloadMaterial_SiloShelf_InteractionPolicy : AbstractUnloadMaterialInteractionPolicy<SiloShelf>
    {
        private readonly ILogger<AGVToSiloShelf_UnloadMaterial_SiloShelf_InteractionPolicy> logger;

        public AGVToSiloShelf_UnloadMaterial_SiloShelf_InteractionPolicy(
            ILogger<AGVToSiloShelf_UnloadMaterial_SiloShelf_InteractionPolicy> logger,
            IServiceProvider serviceProvider,
            SiloShelf siloShelf)
            : base(serviceProvider, siloShelf)
        {
            this.logger = logger;
        }

        public override async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            return await DoService(deviceServiceInvokeRequest, async (request, position) =>
            {
                return await InteractingDevice.Locations[$"{position}"].CompleteUnloadMaterial(deviceServiceInvokeRequest);
            }, "CompleteUnloadMaterial");
        }

        public override async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            return await DoService(deviceServiceInvokeRequest, async (request, position) =>
            {
                return await InteractingDevice.Locations[$"{position}"].InvokeUnloadMaterial(deviceServiceInvokeRequest);
            }, "InvokeUnloadMaterial");
        }

        public override async Task<DeviceServiceInvokeResponse> PrepareUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            return await DoService(deviceServiceInvokeRequest, async (request, position) =>
            {
                return await InteractingDevice.Locations[$"{position}"].PrepareUnloadMaterial(deviceServiceInvokeRequest);
            }, "PrepareUnloadMaterial");
        }

        private async Task<DeviceServiceInvokeResponse> DoService(DeviceServiceInvokeRequest deviceServiceInvokeRequest,
                        Func<DeviceServiceInvokeRequest, int, Task<DeviceServiceInvokeResponse>> action, string methodName = "")
        {
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to {methodName}!");
            var eventId = "";
            var eventName = "";
            var eventMessage = "";
            try
            {
                if (deviceServiceInvokeRequest.Params == null || !deviceServiceInvokeRequest.Params.ContainsKey("ShelfIndex"))
                {
                    eventId = Events.Drill.DRILL_PARAMETER_NOT_CONTAIN_POSITION_EVENT;
                    eventName = Events.Drill.DRILL_PARAMETER_NOT_CONTAIN_POSITION_EVENT_NAME;
                    eventMessage = $"{methodName} 传入的参数不包含 ShelfIndex 信息";
                    return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"{methodName} 传入的参数不包含 ShelfIndex 信息");
                }

                ushort position = deviceServiceInvokeRequest.Params["ShelfIndex"].ToUshort();
                if (!(position > 0 && position <= InteractingDevice.spindleNum))
                {
                    eventId = Events.Drill.DRILL_PARAMETER_POSITION_FALT_EVENT;
                    eventName = Events.Drill.DRILL_PARAMETER_POSITION_FALT_EVENT_NAME;
                    eventMessage = $"{methodName} 下发ShelfIndex参数值{position}不在1和{InteractingDevice.spindleNum}之间，请下发正确的上下料轴信息";
                    logger.LogError(eventMessage);
                    return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, eventMessage);
                }

                var actionResult = await action.Invoke(deviceServiceInvokeRequest, position);
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
                return await InteractingDevice.Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message);
            }
            finally
            {
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} finish {methodName}!");
            }
        }
    }
}
