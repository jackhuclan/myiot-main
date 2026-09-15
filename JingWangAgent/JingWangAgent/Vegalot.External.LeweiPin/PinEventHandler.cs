using System.Text;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Event;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;
using System.Text.Json;

namespace Vegalot.External.LeweiPin
{
    public class PinEventHandler : AbstractEventHandler<Pin>
    {
        private readonly ILogger<PinEventHandler> logger;
        private readonly int spindleNum;
        private readonly int layerLimit;
        private readonly int callAgvTimeInterval;
        private readonly int agvEndTimeInterval;
        private DateTime callAgvTime = DateTime.Now;
        private string[] spindleAgvPosition;
        private string[] spindleInnerAgvPosition;

        public PinEventHandler(ILogger<PinEventHandler> logger,
            IServiceProvider serviceProvider,
            Pin siloShelf)
            : base(serviceProvider, siloShelf)
        {
            this.logger = logger;

            spindleNum = siloShelf.DeviceDescriptor.SpindleNum.ToInt();
            layerLimit = siloShelf.DeviceDescriptor.LayerLimit.ToInt();
            callAgvTimeInterval = siloShelf.DeviceDescriptor.Extra["CallAgvTimeInterval"].ToInt();
            agvEndTimeInterval = siloShelf.DeviceDescriptor.Extra["AgvEndTimeInterval"].ToInt();
            spindleAgvPosition = siloShelf.spindleAgvPosition;
            spindleInnerAgvPosition = siloShelf.spindleInnerAgvPosition;
        }

        public override void AddWatchingEvents()
        {
            WatchingProperties.Property("TranscationChange")
           .When(p => p.NewValue.ToBool() == true)
           .TriggerAlways(() =>
           {
               StringBuilder sbl = new StringBuilder();
               foreach (var transId in InteractingDevice.TransactionMessage)
               {
                   sbl.Append($"{transId.Key}号工位:{transId.Value} \r\n");
               }

               InteractingDevice.TransIds = sbl.ToString();
           });

            foreach (var location in InteractingDevice.WatchShelfProperty.Values)
            {
                WatchingProperties.Properties($"IsReady{location.Position}", $"IsCanCallAgv{location.Position}", $"HandlAskUpload{location.Position}", $"HandleAskDownLoad{location.Position}")
                    .When(proteryties =>
                    {
                        return proteryties.Property($"IsReady{location.Position}").NewValue.ToBool()
                               && proteryties.Property($"IsCanCallAgv{location.Position}").NewValue.ToBool()
                               && !proteryties.Property($"IsAgvWorking{location.Position}").NewValue.ToBool()
                               && (proteryties.Property($"HandlAskUpload{location.Position}").NewValue.ToBool()
                               || proteryties.Property($"HandleAskDownLoad{location.Position}").NewValue.ToBool());
                    })
                    .TriggerAlways(async () =>
                    {
                        try
                        {
                            logger.LogInformation($"{location.Position} 号料架 自动呼叫AGV 开始");
                            var result = await InteractingDevice.CallAgv(location.Position.ToInt(), false);
                            InteractingDevice.TransactionMessage[$"{location.Position.ToInt()}"] = result?.Message!;
                        }
                        catch (Exception ee)
                        {
                            logger.LogError($"{location.Position} 号料架 呼叫AGV异常 ： {ee.Message} ");
                            InteractingDevice.TransactionMessage[$"{location.Position.ToInt()}"] = $"呼叫AGV异常 ： {ee.Message}";
                        }

                        WatchingProperties.Property("TranscationChange").SetValue(true);
                    });
            }

            WatchingProperties.Property("MqttConnected")
               .PostCondition(p => !p.NewValue.ToBool() && p.IsValueChanged)
               .TriggerAlways(() =>
               {
                   logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  mqtt连接断开重置呼叫按钮 ");
                   foreach (var shelf in InteractingDevice.WatchShelfProperty)
                   {
                       shelf.Value.IsCanCallAgv = true;
                   }
               });

            WatchingProperties.Property("MqttConnected")
           .PostCondition(p => p.NewValue.ToBool() && p.IsValueChanged)
           .TriggerAlways(async () =>
           {
               logger.LogInformation($"{DateTime.Now.ToShortTimeString()}  监听到mqtt连接成功!，触发事件! ");
               foreach (var location in InteractingDevice.WatchShelfProperty)
               {
                   await InteractingDevice.PayloadPanels.RaiseCollectionChangedEvent(location.Value.LocationCode);
               }
           });

            WatchingProperties.Property("HandleLeftAskUpload")
            .TriggerAlways(() =>
            {
                try
                {
                    if (WatchingProperties.Property("HandleLeftAskUpload").NewValue.ToInt() == 1)
                    {
                        logger.LogDebug($"HandleLeftAskUpload: 执行  PinCallAgvUploadSilo=true");
                        InteractingDevice.WatchShelfProperty["1"].PinCallAgvUploadSilo = true;
                    }
                    else
                    {
                        // InteractingDevice.mitsubishiClient.Write( "W105", 0);
                        InteractingDevice.WatchShelfProperty["1"].PinCallAgvUploadSilo = false;
                    }
                }
                catch (Exception e)
                {
                    logger.LogError($"HandleLeftAskUpload 异常:{e.ToString()}");
                }
            });

            WatchingProperties.Property("HandleLeftPanelFinish")
             .TriggerAlways(async () =>
             {
                 try
                 {
                     if (WatchingProperties.Property("HandleLeftPanelFinish").NewValue.ToInt() == 1)
                     {

                         //var Layer = InteractingDevice.mitsubishiClient.ReadUInt16("23").Value;
                         //var barcode = InteractingDevice.mitsubishiClient.ReadString("1").Value.Replace("\0", "").Replace("\r", "");
                         //var itemCode = InteractingDevice.mitsubishiClient.ReadString("60").Value.Replace("\0", "").Replace("\r", "");
                         //var panelLength = InteractingDevice.mitsubishiClient.ReadFloat("80").Value;
                         //var panelWidth = InteractingDevice.mitsubishiClient.ReadFloat("82").Value;
                         //var PinOffset = InteractingDevice.mitsubishiClient.ReadFloat("84").Value;
                         //var stackCount = InteractingDevice.mitsubishiClient.ReadFloat("86").Value;
                         var Layer = 18;
                         var barcode = "12345";
                         var itemCode = "VgTest";
                         var panelLength = (float)30;
                         var panelWidth = (float)25;
                         var PinOffset = (float)2;
                         var stackCount = (float)5;
                         //logger.LogInformation($"HandleLeftPanelFinish: 原始值: panelLength:{JsonSerializer.Serialize(InteractingDevice.mitsubishiClient.ReadString("80").Value)},panelWidth:{JsonSerializer.Serialize(InteractingDevice.mitsubishiClient.ReadString("82").Value)}, PinOffset:{JsonSerializer.Serialize(InteractingDevice.mitsubishiClient.ReadString("84"))},StackCount:{InteractingDevice.mitsubishiClient.ReadString("86").Value}");
                         //logger.LogDebug($"HandleLeftPanelFinish: 条码: {barcode},层号:{Layer}," +
                         //    $"料号:{itemCode},panelLength:{panelLength},PanelWidth:{panelWidth},PinOffset:{PinOffset},stackCount:{stackCount}");

                         GetNextPanelRequest request = new GetNextPanelRequest()
                         {
                             BeginLayer = Layer - 1,
                             Count = stackCount.ToInt(),
                             ItemCode = itemCode,
                             PanelWidth = panelWidth,
                             PinOffset = PinOffset,
                             ProductStatus = ProductStatus.Finished_PIN,
                             SiloCode = barcode,
                             Position = 1,
                             BatchCode = "",
                             LotId = "",
                         };

                         var response = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<GetNextPanelRequest, List<Panel>>(InteractingDevice.DeviceDescriptor.Extra["GetNextPanelNumberV2"].ToStr(), request);

                         if (response != null && response.Any())
                         {
                             var newPanel = response.FirstOrDefault();
                             await PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                                          {
                                              var panel = PayloadPanels.FirstOrDefault(x => x.Layer == Layer - 1 && x.Position == 1);
                                              if (panel != null)
                                              {
                                                  panel.ProductStatus = ProductStatus.Finished_PIN;
                                                  panel.ItemCode = itemCode;
                                                  panel.PanelWidth = newPanel.PanelWidth;
                                                  panel.PanelLength = newPanel.PanelLength;
                                                  panel.Pcs = newPanel.Pcs;
                                                  panel.PinOffset = PinOffset;
                                                  panel.PanelCode = newPanel.PanelCode;
                                              }
                                          }));
                         }

                         //PayloadPanels.RaiseCollectionChangedEvent();
                         // InteractingDevice.mitsubishiClient.Write("W105", 1);
                     }
                     else
                     {
                         //  InteractingDevice.mitsubishiClient.Write("W105", 0);
                         // InteractingDevice.mitsubishiClient.Write("25", 0);
                         logger.LogDebug($"HandleLeftPanelFinish: 信号清0");
                     }
                 }
                 catch (Exception e)
                 {
                     logger.LogError($"HandleLeftPanelFinish 异常:{e.ToString()}");
                 }
             });

            WatchingProperties.Property("HandleLeftAskDownLoad")
             .TriggerAlways(() =>
             {
                 try
                 {
                     if (WatchingProperties.Property("HandleLeftAskDownLoad").NewValue.ToInt() == 1)
                     {
                         InteractingDevice.WatchShelfProperty["1"].PinCallAgvDownLoadSilo = true;
                     }
                     else
                     {
                         InteractingDevice.mitsubishiClient.Write("W106", 0);
                         InteractingDevice.WatchShelfProperty["1"].PinCallAgvDownLoadSilo = false;
                     }
                 }
                 catch (Exception e)
                 {
                     logger.LogError($"HandleLeftAskDownLoad 异常:{e.ToString()}");
                 }
             });

            WatchingProperties.Property("HandleRightAskUpload")
           .TriggerAlways(() =>
           {
               try
               {
                   if (WatchingProperties.Property("HandleRightAskUpload").NewValue.ToInt() == 1)
                   {
                       InteractingDevice.WatchShelfProperty["2"].PinCallAgvUploadSilo = true;
                   }
                   else
                   {
                       //InteractingDevice.mitsubishiClient.Write("W115", 0);
                       InteractingDevice.WatchShelfProperty["2"].PinCallAgvUploadSilo = false;
                   }
               }
               catch (Exception e)
               {
                   logger.LogError($"HandleRightAskUpload 异常:{e.ToString()}");
               }
           });

            WatchingProperties.Property("HandleRightPanelFinish")
           .TriggerAlways(async () =>
           {
               try
               {
                   if (WatchingProperties.Property("HandleRightPanelFinish").NewValue.ToInt() == 1)
                   {
                       //var Layer = InteractingDevice.mitsubishiClient.ReadUInt16("33").Value;
                       //var barcode = InteractingDevice.mitsubishiClient.ReadString("1").Value.Replace("\0", "").Replace("\r", "");
                       //var itemCode = InteractingDevice.mitsubishiClient.ReadString("60").Value.Replace("\0", "").Replace("\r", "");
                       //var panelLength = InteractingDevice.mitsubishiClient.ReadFloat("80").Value;
                       //var panelWidth = InteractingDevice.mitsubishiClient.ReadFloat("82").Value;
                       //var PinOffset = InteractingDevice.mitsubishiClient.ReadFloat("84").Value;
                       //var stackCount = InteractingDevice.mitsubishiClient.ReadFloat("86").Value;

                       var Layer = 18;
                       var barcode = "12345";
                       var itemCode = "VgTest";
                       var panelLength = (float)30;
                       var panelWidth = (float)25;
                       var PinOffset = (float)2;
                       var stackCount = (float)5;
                       logger.LogDebug($"HandleRightPanelFinish: 条码: {barcode},层号:{Layer}," +
                           $"料号:{itemCode},panelLength:{panelLength},PanelWidth:{panelWidth},PinOffset:{PinOffset},stackCount:{stackCount}");

                       GetNextPanelRequest request = new GetNextPanelRequest()
                       {
                           BeginLayer = Layer - 1,
                           Count = stackCount.ToInt(),
                           ItemCode = itemCode,
                           PanelWidth = panelWidth,
                           PinOffset = PinOffset,
                           ProductStatus = ProductStatus.Finished_PIN,
                           SiloCode = barcode,
                           Position = 1,
                           BatchCode = "",
                           LotId = "",
                       };

                       var response = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<GetNextPanelRequest, List<Panel>>(InteractingDevice.DeviceDescriptor.Extra["GetNextPanelNumberV2"].ToStr(), request);
                       if (response != null && response.Any())
                       {
                           var newPanel = response.FirstOrDefault();

                           await PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                           {
                               var panel = PayloadPanels.FirstOrDefault(x => x.Layer == Layer - 1 && x.Position == 2);
                               panel.ProductStatus = ProductStatus.Finished_PIN;
                               panel.ItemCode = itemCode;
                               panel.PanelWidth = newPanel.PanelWidth;
                               panel.PanelLength = newPanel.PanelLength;
                               panel.Pcs = newPanel.Pcs;
                               panel.PinOffset = PinOffset;
                               panel.PanelCode = newPanel.PanelCode;
                           }));
                       }                           

                       // PayloadPanels.RaiseCollectionChangedEvent();
                       // InteractingDevice.mitsubishiClient.Write( "W115", 1);
                   }
                   else
                   {
                       // InteractingDevice.mitsubishiClient.Write("W115", 0);
                   }
               }
               catch (Exception e)
               {
                   logger.LogError($"HandleRightPanelFinish 异常:{e.ToString()}");
               }
           });

            WatchingProperties.Property("HandleRightAskDownLoad")
           .TriggerAlways(() =>
           {
               try
               {
                   if (WatchingProperties.Property("HandleRightAskDownLoad").NewValue.ToInt() == 1)
                   {
                       InteractingDevice.WatchShelfProperty["2"].PinCallAgvDownLoadSilo = true;
                   }
                   else
                   {
                       InteractingDevice.mitsubishiClient.Write("W116", 0);
                       InteractingDevice.WatchShelfProperty["2"].PinCallAgvDownLoadSilo = false;
                   }
               }
               catch (Exception e)
               {
                   logger.LogError($"HandleRightAskDownLoad 异常:{e.ToString()}");
               }
           });
        }

        public async Task<BaseResponse> CallAgv(int SpindleNum = 1)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                TimeSpan timeSpan = DateTime.Now.Subtract(InteractingDevice.callAgvTimes[SpindleNum]);
                if (timeSpan.TotalSeconds < callAgvTimeInterval)
                {
                    logger.LogDebug($"{SpindleNum}号料架 小于呼叫间隔不能呼叫");
                    response.Message = $" 小于呼叫间隔不能呼叫";
                    return response;
                }

                InteractingDevice.callAgvTimes[SpindleNum] = DateTime.Now;

                if (!InteractingDevice.WatchShelfProperty[$"{SpindleNum}"].IsCanCallAgv)
                {
                    response.Message = $" 呼叫agv信号为 false 不能呼叫";
                    return response;
                }

                if (!InteractingDevice.MqttClientWrapper.IsConnected)
                {
                    response.Message = "呼叫AGV Mqtt 连接断开 不上报消息";
                    return response;
                }

                var agvPosition = spindleAgvPosition;
                var agvInnerPosition = spindleInnerAgvPosition;
                var spindleBehavior = new int[spindleNum];
                int[] status = new int[spindleNum];// -1 无料仓   0 无生料  1 有生料

                for (int i = 0; i < spindleNum; i++)
                {
                    var sub = InteractingDevice.PayloadPanels.GetRange(i * layerLimit, layerLimit);

                    if (InteractingDevice.SiloIsNotExistByPosition(i + 1))
                    {
                        //料仓不存在 可上料
                        status[i] = -1;
                    }
                    else if (InteractingDevice.SiloIsHasRawtByPosition(i + 1))
                    {
                        status[i] = 1;
                    }
                    else
                    {
                        status[i] = 0;
                    }
                }

                for (int i = 0; i < status.Count(); i++)
                {
                    switch (status[i])
                    {
                        case -1:
                            spindleBehavior[i] = 0;
                            break;

                        case 0:
                            spindleBehavior[i] = -1;
                            break;

                        case 1:
                            spindleBehavior[i] = 1;
                            break;
                    }
                }

                int callAgvSplinesStatus = SpindleNum - 1;

                logger.LogInformation($"上报 信息：  位置{callAgvSplinesStatus + 1}  动作 {status[callAgvSplinesStatus]} （-1 无料仓   0 无生料  1 有生料）  ");
                if (status[callAgvSplinesStatus] == -1)  //无料仓呼叫 上料
                {
                    var req = new DeviceEventReportRequest()
                    {
                        ProductId = DeviceDescriptor.ProductId,
                        DeviceId = DeviceDescriptor.DeviceId,
                        ClientId = InteractingDevice.ClientId,
                        EventId = Events.REQUEST_AGV_LOAD_SILO_ONLY + "#" + SpindleNum,
                        RequestInputProductStatus = ProductStatus.EmptySiloBox,
                        RequestInteractionBehavior = InteractionBehavior.Make(DeviceDescriptor.DeviceKind, InteractionBehavior.FRONT_LOAD_SILO_ONLY),
                        RequestDeviceKind = DeviceDescriptor.DeviceKind,
                        RequestMaterialKind = MaterialKind.PanelSilo,
                        RequestInteractionDirection = InteractionPosition.Front,
                        Params = new Dictionary<string, object?>()
                        {
                            { "SpindleNum", 1 },
                            { "ShelfIndex", SpindleNum },
                            { "Spindles", $"{InteractingDevice.spindleAgvPosition[callAgvSplinesStatus]}" },
                            { "ShelfInnerPos", $"{InteractingDevice.spindleInnerAgvPosition[callAgvSplinesStatus]}" },
                            { "TransSpindles", $"{InteractingDevice.transSpindleAgvPosition[callAgvSplinesStatus]}" },
                            { "TransShelfInnerPos", $"{InteractingDevice.transSpindleInnerAgvPosition[callAgvSplinesStatus]}" },
                            { "TransActionPos", $"{InteractingDevice.transSpindMiddleAgvPosition[callAgvSplinesStatus]}" },

                            { "ShelfAgvIdlePos", $"{InteractingDevice.spindIdePosition[callAgvSplinesStatus]}" },
                            { "TransShelfAgvIdlePos", $"{InteractingDevice.transSpindIdeAgvPosition[callAgvSplinesStatus]}" },
                            { "SpindleBehavior", spindleBehavior[callAgvSplinesStatus].ToStr() },
                            { "DeviceCode", DeviceDescriptor.DeviceId + SpindleNum.ToString().PadLeft(3, '0') },
                            { "SiloCode", "" }
                        },

                        PayloadPanels = PanelList.FromList(InteractingDevice.PayloadPanels.GetRange((SpindleNum - 1) * layerLimit, layerLimit))
                    };

                    logger.LogInformation($"{SpindleNum}号工位 上报 上料 请求参数：  {JsonSerializer.Serialize(req)}  ");

                    var result = await DataExporter.DeviceEventReport(req);

                    if (result == null)
                    {
                        response.Message = $"{DateTime.Now.ToString()}  ----  和服务器断开连接，呼叫失败";
                    }
                    else
                    {
                        if (result.Code == ErrorCodes.Sys.SUCCESS)
                        {
                            InteractingDevice.WatchShelfProperty[$"{SpindleNum}"].IsCanCallAgv = false;
                            InteractingDevice.TransactionIds[$"{SpindleNum}"] = result.TraceId;
                            response.Code = 0;
                            response.Message = $"{DateTime.Now.ToString()}  ----呼叫成功   {result.TraceId}";
                        }
                        else if (result.Code == ErrorCodes.Sys.FAIL)
                        {
                            response.Message = $"{DateTime.Now.ToString()}  ----呼叫失败  {result.Message}";
                        }
                    }

                    logger.LogInformation($"{SpindleNum}号工位 上报 上料 信息结果：  {JsonSerializer.Serialize(result)}  ");

                    return response;
                }
                else if (status[callAgvSplinesStatus] == 1)  //有料仓呼叫 下料
                {
                    var req = new DeviceEventReportRequest()
                    {
                        ProductId = DeviceDescriptor.ProductId,
                        DeviceId = DeviceDescriptor.DeviceId,
                        ClientId = InteractingDevice.ClientId,
                        EventId = Events.REQUEST_AGV_UNLOAD_SILO_ONLY + "#" + SpindleNum,
                        RequestOutputProductStatus = ProductStatus.Finished_PIN,
                        RequestInteractionBehavior = InteractionBehavior.Make(DeviceDescriptor.DeviceKind, InteractionBehavior.FRONT_UNLOAD_SILO_ONLY),
                        RequestDeviceKind = DeviceDescriptor.DeviceKind,
                        RequestMaterialKind = MaterialKind.PanelSilo,
                        RequestInteractionDirection = InteractionPosition.Front,
                        Params = new Dictionary<string, object?>()
                          {
                                { "SpindleNum", 1 },
                                { "ShelfIndex",SpindleNum },
                                { "Spindles",  $"{InteractingDevice.spindleAgvPosition[callAgvSplinesStatus]}"},
                                { "ShelfInnerPos",  $"{InteractingDevice.spindleInnerAgvPosition[callAgvSplinesStatus]}"},
                                { "TransSpindles",  $"{InteractingDevice.transSpindleAgvPosition[callAgvSplinesStatus]}"},
                                { "TransShelfInnerPos",  $"{InteractingDevice.transSpindleInnerAgvPosition[callAgvSplinesStatus]}"},
                                { "TransActionPos",  $"{InteractingDevice.transSpindMiddleAgvPosition[callAgvSplinesStatus]}"},
                                { "ShelfAgvIdlePos",  $"{InteractingDevice.spindIdePosition[callAgvSplinesStatus]}"},
                                { "TransShelfAgvIdlePos",  $"{InteractingDevice.transSpindIdeAgvPosition[callAgvSplinesStatus]}"},
                                { "SpindleBehavior", spindleBehavior[callAgvSplinesStatus].ToStr() },
                                { "DeviceCode",DeviceDescriptor.DeviceId+SpindleNum.ToString().PadLeft(3,'0')},
                                { "SiloCode",InteractingDevice.PayloadPanels.GetRange((SpindleNum - 1) * layerLimit, layerLimit).FirstOrDefault()?.SiloCode }
                          },
                        PayloadPanels = PanelList.FromList(InteractingDevice.PayloadPanels.GetRange((SpindleNum - 1) * layerLimit, layerLimit))
                    };
                    logger.LogInformation($"{SpindleNum}号工位 上报 下料 请求参数：  {JsonSerializer.Serialize(req)}  ");
                    var result = await DataExporter.DeviceEventReport(req);
                    if (result == null)
                    {
                        response.Message = $"{DateTime.Now.ToString()}  ----  和服务器断开连接，呼叫失败";
                    }
                    else
                    {
                        if (result.Code == ErrorCodes.Sys.SUCCESS)
                        {
                            InteractingDevice.WatchShelfProperty[$"{SpindleNum}"].IsCanCallAgv = false;
                            InteractingDevice.TransactionIds[$"{SpindleNum}"] = result.TraceId;
                            response.Code = 0;
                            response.Message = $"{DateTime.Now.ToString()}  ----呼叫成功   {result.TraceId}";
                        }
                        else if (result.Code == ErrorCodes.Sys.FAIL)
                        {
                            response.Message = $"{DateTime.Now.ToString()}  ----呼叫失败  {result.Message}";
                        }
                    }

                    logger.LogInformation($" {SpindleNum}号工位 上报 下料 信息结果：  {JsonSerializer.Serialize(result)}  ");

                    return response;
                }
                else
                {
                    response.Message = $"不呼叫AGV ,请检查料仓信息是否与实际相符";
                    return response;
                }
            }
            catch (Exception ee)
            {
                logger.LogError($"{SpindleNum}号工位 呼叫AGV异常 ： {ee.Message} ");
                response.Message = $"呼叫AGV异常 ： {ee.Message} ";
                return response;
            }
        }
    }
}
