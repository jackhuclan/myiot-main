using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Interaction;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot;
using VgDeviceGateway.Devices.Common;
using VgAutoDrill.Fundation.Utils;

namespace Vegalot.External.LeweiUnPin
{
    public class UnPin_LoadMaterial_InteractionPolicy : AbstractLoadMaterialInteractionPolicy<UnPin>
    {
        private readonly ILogger<UnPin_LoadMaterial_InteractionPolicy> logger;

        public UnPin_LoadMaterial_InteractionPolicy(ILogger<UnPin_LoadMaterial_InteractionPolicy> logger,
            IServiceProvider serviceProvider,
            UnPin siloShelf)
            : base(serviceProvider, siloShelf)
        {
            this.logger = logger;
        }

        public override async Task<DeviceServiceInvokeResponse> PrepareLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            return await DoService(deviceServiceInvokeRequest, async (request, position) =>
            {
                InteractingDevice.WatchShelfProperty[$"{position}"].IsAgvWorking = true;
                // 判断是否有料仓信息
                if (!InteractingDevice.SiloIsNotExistByPosition(position))
                {
                    logger.LogError($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}---{position}号工位 存在料仓不能上料!");
                    return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"{position} 号工位上有料仓，不能上料 \r\n {JsonSerializer.Serialize(InteractingDevice.PayloadPanels.GetRange((position - 1) * InteractingDevice.layerLimit, InteractingDevice.layerLimit))}", deviceServiceInvokeRequest.Params);
                }
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}---{position}号工位 不存在料仓 可以上料!");
                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, JsonSerializer.Serialize(InteractingDevice.PayloadPanels.GetRange((position - 1) * InteractingDevice.layerLimit, InteractingDevice.layerLimit)), deviceServiceInvokeRequest.Params);
            }, "PrepareLoadMaterial");
        }

        public override async Task<DeviceServiceInvokeResponse> InvokeLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} InvokeLoadMaterial!");
            InteractingDevice.WatchShelfProperty[$"{deviceServiceInvokeRequest.Params["ShelfIndex"].ToUshort()}"].IsAgvWorking = true;
            return await ResponseSuccess(deviceServiceInvokeRequest.ReplyTopic);
        }

        public override async Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            return await DoService(deviceServiceInvokeRequest, async (request, position) =>
            {
                if (deviceServiceInvokeRequest.Params.ContainsKey("IsLastStep") && deviceServiceInvokeRequest.Params["IsLastStep"].ToBool())
                {
                    return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, JsonSerializer.Serialize(InteractingDevice.PayloadPanels.GetRange((position - 1) * InteractingDevice.layerLimit, InteractingDevice.layerLimit)));
                }

                if (!deviceServiceInvokeRequest.Params.ContainsKey("LoadingPanel"))
                {
                    logger.LogError($"CompleteLoadMaterial {position}号工位 上料仓获取的LoadingPanel的信息 为空");
                    InteractingDevice.UpdateSiloInfoByPosition(position, Panel.HasSilo.NoPanelForSingleSpindle("", position, 0, InteractingDevice.layerLimit));
                }
                else
                {
                    logger.LogInformation($"CompleteLoadMaterial {position}号工位 上料获取的LoadingPanel的信息：{deviceServiceInvokeRequest.Params["LoadingPanel"].ToStr()}");

                    var loadSiloInfo = JsonSerializer.Deserialize<SwapPanel>(deviceServiceInvokeRequest.Params["LoadingPanel"]?.ToStr()
                  , new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    var operationEntity = loadSiloInfo?.PanelList;
                    logger.LogDebug($"CompleteLoadMaterial {position}号工位 上料完成未修改前 PayloadPanels {JsonSerializer.Serialize(InteractingDevice.PayloadPanels.GetRange((position - 1) * InteractingDevice.layerLimit, InteractingDevice.layerLimit))}");
                    if (operationEntity != null)
                    {
                        if (operationEntity == null || operationEntity.Count == 0)
                        {
                            logger.LogError($"{position} 号工位获取的板材信息为null");
                            InteractingDevice.UpdateSiloInfoByPosition(position, Panel.HasSilo.NoPanelForSingleSpindle("", position, 0, InteractingDevice.layerLimit));
                        }
                        else
                        {
                            var lotNo = operationEntity.FirstOrDefault(s=>!string.IsNullOrWhiteSpace(s.ItemCode))?.ItemCode.Trim();
                            if (!string.IsNullOrWhiteSpace(lotNo))
                            {
                                logger.LogError($"{position} 号工位获取料号（工单）是{lotNo}");
                                InteractingDevice.mitsubishiClient.WriteStringExtensions(position == 1 ? "D4000" : "D4050", lotNo);
                                InteractingDevice.ItemNo = lotNo;
                            }
                           
                            for (int i = 0; i < operationEntity.Count; i++)
                            {
                                operationEntity[i].Position = position;
                                operationEntity[i].ProductStatus = operationEntity[i].ProductStatus == ProductStatus.EmptySiloBox ? ProductStatus.EmptySiloBox : ProductStatus.Finished_UNPIN;
                            }

                            InteractingDevice.UpdateSiloInfoByPosition(position, operationEntity);
                        }
                    }
                    else
                    {
                        logger.LogDebug($"上生料获取的板材信息为null");
                        InteractingDevice.UpdateSiloInfoByPosition(position, Panel.HasSilo.NoPanelForSingleSpindle("", position, 0, InteractingDevice.layerLimit));
                    }

                    logger.LogDebug($"CompleteLoadMaterial {position} 号工位料架 上料完成后 PayloadPanels {JsonSerializer.Serialize(InteractingDevice.PayloadPanels.GetRange((position - 1) * InteractingDevice.layerLimit, InteractingDevice.layerLimit))}");
                }

                InteractingDevice.WatchShelfProperty[$"{position}"].IsAgvWorking = false;
                InteractingDevice.WatchShelfProperty[$"{position}"].CurrentContext.IsExistSilo = true;
                var singlePayloadPanels = PayloadPanels.Skip(InteractingDevice.layerLimit * (position - 1)).Take(InteractingDevice.layerLimit).ToList();
                var beginWorkLayer = singlePayloadPanels.Where(x => !string.IsNullOrWhiteSpace(x.ItemCode))
                                                                    .OrderBy(x => x.Layer).FirstOrDefault()?.Layer ?? 0;
                //写入板料数量
                var num = singlePayloadPanels.Count(s => s.ProductStatus == ProductStatus.Finished_UNPIN);              
                InteractingDevice.mitsubishiClient?.Write(position == 1 ? "D4200" : "D4250", num);
                logger.LogInformation($"{position}位置上{num}块板料");
                //写入开始工作的料层号
                InteractingDevice.mitsubishiClient?.Write(position == 1 ? "D4204" : "D4254", beginWorkLayer + 1);
                logger.LogInformation($"{position}位置开始作业的料层号{beginWorkLayer + 1}");

                for (int i = 0; i < singlePayloadPanels.Count(); i++)
                {
                    //左门4300--4336 右门4350--4386   每2个地址位为1层,共18层,1表示有板料，0表示无板料
                    int address = (position == 1 ? 4300 : 4350) + 2 * i;
                    InteractingDevice.mitsubishiClient?.Write($"D{address}", singlePayloadPanels[i].ProductStatus == ProductStatus.Finished_UNPIN ? 1 : 0);
                }

                //写入agv运送料箱已达到退PIN，并已离开信号
                InteractingDevice.mitsubishiClient?.Write(position == 1 ? "W105" : "W115", (short)1);
                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, JsonSerializer.Serialize(InteractingDevice.PayloadPanels));
            }, "CompleteLoadMaterial");
        }

        private async Task<DeviceServiceInvokeResponse> DoService(DeviceServiceInvokeRequest deviceServiceInvokeRequest,
            Func<DeviceServiceInvokeRequest, int, Task<DeviceServiceInvokeResponse>> action,
            string methodName = "")
        {
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to {methodName}!");
            var eventId = "";
            var eventName = "";
            var eventMessage = "";
            ushort position = 0;
            try
            {
                if (deviceServiceInvokeRequest.Params == null || !deviceServiceInvokeRequest.Params.ContainsKey("ShelfIndex"))
                {
                    eventId = Events.Drill.DRILL_PARAMETER_NOT_CONTAIN_POSITION_EVENT;
                    eventName = Events.Drill.DRILL_PARAMETER_NOT_CONTAIN_POSITION_EVENT_NAME;
                    eventMessage = $"{methodName} 传入的参数不包含 ShelfIndex 信息";
                    logger.LogError(eventMessage);
                    return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"{methodName} 传入的参数不包含 ShelfIndex 信息");
                }

                position = deviceServiceInvokeRequest.Params["ShelfIndex"].ToUshort();
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
                logger.LogError(ex, $"{position}号工位上料异常：{ex.Message}");
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
