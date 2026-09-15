using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Event;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.UnPin
{
    public class UnPinEventHandler : AbstractEventHandler<UnPin>
    {
        private readonly ILogger<UnPinEventHandler> logger;
        private readonly int spindleNum;
        private readonly int layerLimit;
        private readonly int callAgvTimeInterval;
        private readonly int agvEndTimeInterval;
        private DateTime callAgvTime = DateTime.Now;
        private string[] spindleAgvPosition;
        private string[] spindleInnerAgvPosition;

        public UnPinEventHandler(ILogger<UnPinEventHandler> logger,
            IServiceProvider serviceProvider,
            UnPin unPin)
            : base(serviceProvider, unPin)
        {
            this.logger = logger;

            spindleNum = unPin.DeviceDescriptor.SpindleNum.ToInt();
            layerLimit = unPin.DeviceDescriptor.LayerLimit.ToInt();
            callAgvTimeInterval = unPin.DeviceDescriptor.Extra["CallAgvTimeInterval"].ToInt();
            agvEndTimeInterval = unPin.DeviceDescriptor.Extra["AgvEndTimeInterval"].ToInt();
            spindleAgvPosition = unPin.spindleAgvPosition;
            spindleInnerAgvPosition = unPin.spindleInnerAgvPosition;
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
                WatchingProperties.Properties($"IsReady{location.Position}",
                                            $"IsCanCallAgv{location.Position}",
                                            $"IsAgvWorking{location.Position}",
                                            $"HandlAskUpload{location.Position}",
                                            $"HandleAskDownLoad{location.Position}")
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

            WatchingProperties.Property("PlcItemNo")
           .WhenValueChanged()
           .TriggerAlways(async () =>
           {
               try
               {
                   for (int i = 1; i <= InteractingDevice.spindleNum; i++)
                   {
                       var result = await InteractingDevice.CancelSchdule(i);
                       logger.LogInformation($"CancelSchdule: {i}号工位,Result:{JsonSerializer.Serialize(result)}");
                   }

                   //if (!string.IsNullOrEmpty(WatchingProperties.Property("PlcItemNo").NewValue.ToStr()))
                   //{
                   //    string itemNo = WatchingProperties.Property("PlcItemNo").NewValue.ToStr();

                   //    var panelInfo = await InteractingDevice.GetPanelItemInfoAsync(itemNo);
                   //    if (panelInfo == null) return;
                   //    await InteractingDevice.modbusIpMaster.WriteMultipleRegistersAsync(1, 80, panelInfo.panelLength.ToFloat().StringFloatToUshort());
                   //    await InteractingDevice.modbusIpMaster.WriteMultipleRegistersAsync(1, 82, panelInfo.panelWidth.ToFloat().StringFloatToUshort());
                   //}
               }
               catch (Exception e)
               {
                   logger.LogError($"获取 PlcItemNo 异常:{e.ToString()}");
               }
           });

            WatchingProperties.Property("HandleLeftAskUpload")
            .TriggerAlways(() =>
            {
                try
                {
                    if (WatchingProperties.Property("HandleLeftAskUpload").NewValue.ToInt() == 1)
                    {
                        // logger.LogDebug($"HandleLeftAskUpload: 执行  UpinCallAgvUploadSilo=true");
                        //TODO 判断是否还有该料号的料仓可下Pin,没有则写信号通知
                        //  logger.LogInformation($"HandleLeftAskUpload:  收到 1号工位请求上料信号");
                        InteractingDevice.WatchShelfProperty["1"].UpinCallAgvUploadSilo = true;
                    }
                    else
                    {
                        InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 46, 0);
                        InteractingDevice.WatchShelfProperty["1"].UpinCallAgvUploadSilo = false;
                    }
                }
                catch (Exception e)
                {
                    logger.LogError($"HandleLeftAskUpload 异常:{e.ToString()}");
                }
            });

            WatchingProperties.Property("HandleLeftPanelFinish")
             .TriggerAlways(() =>
             {
                 try
                 {
                     if (WatchingProperties.Property("HandleLeftPanelFinish").NewValue.ToInt() == 1)
                     {
                         var Layer = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 43, 1)[0];
                         logger.LogDebug($"HandleLeftPanelFinish:  层号:{Layer}");

                         //PayloadPanels.ChangeListSafely(Task.Run(() =>
                         //{
                         var panels = PayloadPanels.Where(x => x.Layer <= Layer - 1 && x.Position == 1).ToList();
                         if (panels != null && panels.Any())
                         {
                             panels.ForEach(panel =>
                             {
                                 panel.ProductStatus = ProductStatus.EmptySiloBox;
                                 panel.PanelCode = "";
                                 panel.PanelWidth = 0;
                                 panel.PanelLength = 0;
                                 panel.PinOffset = 0;
                                 panel.ItemCode = "";
                                 panel.LotId = "";
                                 panel.BatchCode = "";
                                 panel.Barcode = "";
                                 panel.Pcs = 0;
                                 panel.PanelThickness = 0;
                             });
                         }
                         // }));

                         PayloadPanels.RaiseCollectionChangedEvent(InteractingDevice.GetLocationCode(1));
                         InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 45, 1);
                     }
                     else
                     {
                         InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 45, 0);
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
                         InteractingDevice.WatchShelfProperty["1"].UpinCallAgvDownLoadSilo = true;
                     }
                     else
                     {
                         InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 47, 0);
                         InteractingDevice.WatchShelfProperty["1"].UpinCallAgvDownLoadSilo = false;
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
                       InteractingDevice.WatchShelfProperty["2"].UpinCallAgvUploadSilo = true;
                   }
                   else
                   {
                       InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 56, 0);
                       InteractingDevice.WatchShelfProperty["2"].UpinCallAgvUploadSilo = false;
                   }
               }
               catch (Exception e)
               {
                   logger.LogError($"HandleRightAskUpload 异常:{e.ToString()}");
               }
           });

            WatchingProperties.Property("HandleRightPanelFinish")
           .TriggerAlways(() =>
           {
               try
               {
                   if (WatchingProperties.Property("HandleRightPanelFinish").NewValue.ToInt() == 1)
                   {
                       var Layer = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 53, 1)[0];
                       logger.LogDebug($"HandleRightPanelFinish:  层号:{Layer}");

                       //PayloadPanels.ChangeListSafely(Task.Run(() =>
                       //{
                       var panels = PayloadPanels.Where(x => x.Layer <= Layer - 1 && x.Position == 2).ToList();
                       if (panels != null && panels.Any())
                       {
                           panels.ForEach(panel =>
                           {
                               panel.ProductStatus = ProductStatus.EmptySiloBox;
                               panel.PanelCode = "";
                               panel.PanelWidth = 0;
                               panel.PanelLength = 0;
                               panel.PinOffset = 0;
                               panel.ItemCode = "";
                               panel.LotId = "";
                               panel.BatchCode = "";
                               panel.Barcode = "";
                               panel.Pcs = 0;
                               panel.PanelThickness = 0;
                           });
                       }
                       //}));
                       PayloadPanels.RaiseCollectionChangedEvent(InteractingDevice.GetLocationCode(2));
                       InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 55, 1);
                   }
                   else
                   {
                       InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 55, 0);
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
                       InteractingDevice.WatchShelfProperty["2"].UpinCallAgvDownLoadSilo = true;
                   }
                   else
                   {
                       InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 57, 0);
                       InteractingDevice.WatchShelfProperty["2"].UpinCallAgvDownLoadSilo = false;
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
                int[] status = new int[spindleNum];// -1 无料仓  1-空仓，可下料， 0--无动作

                for (int i = 0; i < spindleNum; i++)
                {
                    var sub = InteractingDevice.PayloadPanels.GetRange(i * layerLimit, layerLimit);

                    if (InteractingDevice.SiloIsNotExistByPosition(i + 1))
                    {
                        //料仓不存在 可上料
                        status[i] = -1;
                    }
                    else if (InteractingDevice.SiloIsEmptytByPosition(i + 1))
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
                var unpinItemNo = DeviceDescriptor.Extra["NeedPlc"].ToBool() ? WatchingProperties.Property("PlcItemNo").NewValue.ToStr() : InteractingDevice.WatchShelfProperty[$"{SpindleNum}"].CurrentContext.ItemCode;
                logger.LogInformation($"上报 信息：  位置{callAgvSplinesStatus + 1}  动作 {status[callAgvSplinesStatus]} （-1 无料仓   0 无生料  1 有生料）  ");
                if (status[callAgvSplinesStatus] == -1)  //无料仓呼叫 上料
                {
                    var req = new DeviceEventReportRequest()
                    {
                        ProductId = DeviceDescriptor.ProductId,
                        DeviceId = DeviceDescriptor.DeviceId,
                        ClientId = InteractingDevice.ClientId,
                        EventId = Events.REQUEST_AGV_LOAD_SILO_ONLY + "#" + SpindleNum,
                        RequestInputProductStatus = ProductStatus.PRE_UNPIN_TRANSFER_AGV_OUTPUT_1,
                        RequestInteractionBehavior = InteractionBehavior.Make(this.DeviceDescriptor.DeviceKind, InteractionBehavior.FRONT_LOAD_SILO_ONLY),
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
                                { "SiloCode",""},
                                { "UnpinItemCode",unpinItemNo},
                            },

                        PayloadPanels = PanelList.FromList(InteractingDevice.PayloadPanels.GetRange((SpindleNum - 1) * layerLimit, layerLimit))
                    };

                    logger.LogInformation($"{SpindleNum}号工位 上报 上料 请求参数：  {System.Text.Json.JsonSerializer.Serialize(req)}  ");

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
                        RequestOutputProductStatus = ProductStatus.EmptySiloBox,
                        RequestInteractionBehavior = InteractionBehavior.Make(this.DeviceDescriptor.DeviceKind, InteractionBehavior.FRONT_UNLOAD_SILO_ONLY),
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
