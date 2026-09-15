// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Interaction;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.UnPin
{
    public class UnPin_UnloadMaterial_InteractionPolicy : AbstractUnloadMaterialInteractionPolicy<UnPin>
    {
        private readonly ILogger<UnPin_UnloadMaterial_InteractionPolicy> logger;

        public UnPin_UnloadMaterial_InteractionPolicy(
            ILogger<UnPin_UnloadMaterial_InteractionPolicy> logger,
            IServiceProvider serviceProvider,
            UnPin siloShelf)
            : base(serviceProvider, siloShelf)
        {
            this.logger = logger;
        }

        public override async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            //var spli = deviceServiceInvokeRequest.Params["ShelfIndex"].ToUshort();
            //InteractingDevice.modbusIpMaster.WriteSingleRegister(1, spli == 1 ? (ushort)47 : (ushort)57, 1);
            //return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "");
            return await DoService(deviceServiceInvokeRequest, async (request, position) =>
            {
                var subPanels = InteractingDevice.PayloadPanels.GetRange((position - 1) * InteractingDevice.layerLimit, InteractingDevice.layerLimit);

                deviceServiceInvokeRequest!.Params["UnloadingPanel"] = new SwapPanel() { SpindleId = position, PanelList = subPanels };

                logger.LogDebug($"CompleteUnloadMaterial {position}号工位 下料完成 UnloadingPanel 赋值：{JsonSerializer.Serialize(deviceServiceInvokeRequest!.Params["UnloadingPanel"])}");

                InteractingDevice.UpdateSiloInfoByPosition(position, Panel.NoSilo.PanelForSingleSpindle(position, 0, InteractingDevice.layerLimit));

                logger.LogDebug($"CompleteUnloadMaterial {position}号工位 下料完成后 PayloadPanels {JsonSerializer.Serialize(InteractingDevice.PayloadPanels.GetRange((position - 1) * InteractingDevice.layerLimit, InteractingDevice.layerLimit))}");
                InteractingDevice.WatchShelfProperty[$"{position}"].IsAgvWorking = false;
                InteractingDevice.WatchShelfProperty[$"{position}"].CurrentContext.IsExistSilo = false;
                InteractingDevice.modbusIpMaster?.WriteSingleRegister(1, position == 1 ? (ushort)47 : (ushort)57, 1);
                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, JsonSerializer.Serialize(InteractingDevice.PayloadPanels.GetRange((position - 1) * InteractingDevice.layerLimit, InteractingDevice.layerLimit)), deviceServiceInvokeRequest.Params);
            }, "CompleteUnloadMaterial");
        }

        public override async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} InvokeUnloadMaterial!");
            InteractingDevice.WatchShelfProperty[$"{deviceServiceInvokeRequest.Params["ShelfIndex"].ToUshort()}"].IsAgvWorking = true;
            return await InteractingDevice.ResponseSuccess(deviceServiceInvokeRequest.ReplyTopic);
        }

        public override async Task<DeviceServiceInvokeResponse> PrepareUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            return await DoService(deviceServiceInvokeRequest, async (request, position) =>
            {
                InteractingDevice.WatchShelfProperty[$"{position}"].IsAgvWorking = true;
                // 判断是否有料仓信息
                if (InteractingDevice.SiloIsNotExistByPosition(position))
                {
                    logger.LogError($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}  {position} 不存在料仓不能下料!");
                    return await InteractingDevice.ResponseFail($"{position}号工位 上无料仓，不能下料\r\n{JsonSerializer.Serialize(InteractingDevice.PayloadPanels.GetRange((position - 1) * InteractingDevice.layerLimit, InteractingDevice.layerLimit))}", deviceServiceInvokeRequest.Params);
                }
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}  {position} 存在 料仓 可以下料!");
                return await InteractingDevice.ResponseSuccess(JsonSerializer.Serialize(InteractingDevice.PayloadPanels.GetRange((position - 1) * InteractingDevice.layerLimit, InteractingDevice.layerLimit)), deviceServiceInvokeRequest.Params);
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
