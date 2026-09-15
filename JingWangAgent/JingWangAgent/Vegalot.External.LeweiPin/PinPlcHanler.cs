using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;
using System.Text.Json;
using IoTClient.Clients.PLC;
using IoTClient.Enums;
using System;
using Newtonsoft.Json.Linq;
using System.Reflection.Metadata;

namespace Vegalot.External.LeweiPin
{
    public class PinPlcHandle : DeviceShare<Pin>, IPlcHandle
    {
        private readonly ILogger<PinPlcHandle> logger;
        private readonly List<LoopTask> plcTasks = new List<LoopTask>();
        private readonly int period = 500;

        public PinPlcHandle(ILogger<PinPlcHandle> logger, IServiceProvider serviceProvider, Pin device) : base(serviceProvider, device)
        {
            this.logger = logger;
            period = device.DeviceDescriptor.Extra["PlcLoopPeriod"].ToInt();
        }

        public bool InitTask()
        {
            plcTasks.Add(new LoopTask(HandleLeftAskUpload, period, logger));
            plcTasks.Add(new LoopTask(HandleLeftPanelFinish, period, logger));
            plcTasks.Add(new LoopTask(HandleLeftAskDownLoad, period, logger));
            plcTasks.Add(new LoopTask(HandleRightAskUpload, period, logger));
            plcTasks.Add(new LoopTask(HandleRightPanelFinish, period, logger));
            plcTasks.Add(new LoopTask(HandleRightAskDownLoad, period, logger));

            return true;
        }

        public bool Start()
        {
            InitTask();
            foreach (var task in plcTasks)
            {
                task.Start();
            }

            return true;
        }

        public bool Stop()
        {
            foreach (var task in plcTasks)
            {
                task.Stop();
            }

            Thread.Sleep(1000);
            plcTasks.Clear();
            return true;
        }

        /// <summary>
        /// 上PIN 左工位呼叫上料仓
        /// </summary>
        public void HandleLeftAskUpload()
        {
            //上PIN左料仓缺料箱（通知AGV送新的料箱）读取
            short askUploadSignal = InteractingDevice.mitsubishiClient.ReadInt16("W100").Value;
            if (askUploadSignal == 1)
            {
               
                // logger.LogDebug($"HandleLeftAskUpload: 执行  PinCallAgvUploadSilo=true");
                InteractingDevice.WatchShelfProperty["1"].PinCallAgvUploadSilo = true;
            }
            else
            {
                //AGV给上PIN左料仓装箱完成并已离开
                InteractingDevice.mitsubishiClient.Write("W106", 0);
                InteractingDevice.WatchShelfProperty["1"].PinCallAgvUploadSilo = false;
            }
        }


        public void HandleLeftPanelFinish()
        {
            try
            {
                var panelFinishSignal = InteractingDevice.mitsubishiClient.ReadInt16("W105").Value;
                if (panelFinishSignal == 1)
                {
                    var layer = InteractingDevice.mitsubishiClient.ReadInt32("D4202").Value;//料层号 1-18
                    logger.LogDebug($"HandleLeftPanelFinish:  层号:{layer}");
                    if (layer < 1 && layer > 18)
                    {
                        return;
                    }
                    var barcode = "";
                    var itemCode = InteractingDevice.mitsubishiClient.ReadStringExtensions("D4000").Trim();//料号
                    var panelLength = (float)Math.Round(InteractingDevice.mitsubishiClient.ReadInt32("D4206").Value/(float)10000,2, MidpointRounding.AwayFromZero);//板长
                    var panelWidth = (float)Math.Round(InteractingDevice.mitsubishiClient.ReadInt32("D4208").Value / (float)10000, 2, MidpointRounding.AwayFromZero); ;//板宽
                    var PinOffset = (float)0.0;
                    var stackCount = (float)InteractingDevice.mitsubishiClient.ReadInt32("D4210").Value;//叠板层数
                    var thickness = (float)Math.Round(InteractingDevice.mitsubishiClient.ReadInt32("D4212").Value / (float)10000, 2, MidpointRounding.AwayFromZero); //板厚
                    var siloCode = PayloadPanels.FirstOrDefault(s => !string.IsNullOrWhiteSpace(s.SiloCode))?.SiloCode;
                    logger.LogInformation(@$"HandleLeftPanelFinish: 原始值: layer:{layer}, panelLength:{panelLength},panelWidth:{panelWidth},PinOffset:{PinOffset},
                                           StackCount:{stackCount}, PanelThickness:{thickness}");

                    GetNextPanelRequest request = new GetNextPanelRequest()
                    {
                        BeginLayer = layer - 1,
                        Count = 1,
                        ItemCode = itemCode,
                        PanelWidth = panelWidth,
                        PanelLength = panelLength,
                        PinOffset = PinOffset,
                        ProductStatus = ProductStatus.Finished_PIN,
                        SiloCode = siloCode,
                        Position = 1,
                        BatchCode = barcode,
                        LotId = "",
                    };

                    logger.LogInformation($"GetLeftPanel Request:{JsonSerializer.Serialize(request)}");
                    var response = InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<GetNextPanelRequest, List<Panel>>(InteractingDevice.DeviceDescriptor.Extra["GetNextPanelNumberV2"].ToStr(), request).Result;
                    logger.LogInformation($"GetLeftPanel response:{JsonSerializer.Serialize(response)}");

                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));

                    Task.Run(async () =>
                    {
                        try
                        {
                            Item item = new Item()
                            {
                                Code = itemCode,
                                IncodeNumber = barcode,
                                PanelLength = panelLength,
                                PanelWidth = panelWidth,
                                PanelCount = (decimal)stackCount,
                            };
                            logger.LogInformation($"ProduceLeftItem Request:{JsonSerializer.Serialize(item)}");
                            var response = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<Item, bool>(InteractingDevice.DeviceDescriptor.Extra["GenerateItem"].ToStr(), item);
                            logger.LogInformation($"ProduceLeftItem response: {response}");
                        }
                        catch (Exception e)
                        {
                            logger.LogError(e, $"ProduceLeftItem Error:{e.Message}");
                            logger.LogInformation($"ProduceLeftItem Error: {e.ToStr()}");
                        }

                    }, cancellationTokenSource.Token);

                    var panel = PayloadPanels.FirstOrDefault(x => x.Layer == layer - 1 && x.Position == 1);
                    var signal = InteractingDevice.mitsubishiClient.ReadInt16("W105").Value;//再次确认信号，防呆
                    if (panel != null && signal==1)
                    {
                        panel.ProductStatus = ProductStatus.Finished_PIN;
                        panel.ItemCode = itemCode;
                        panel.PanelWidth = panelWidth;
                        panel.PinOffset = PinOffset;
                        panel.PanelLength = panelLength;
                        panel.Barcode = barcode;
                        panel.Pcs = stackCount.ToInt();
                        panel.PanelThickness = thickness;
                        panel.PanelCode = response?.FirstOrDefault()?.PanelCode ?? "";
                        logger.LogInformation(@$"HandleLeftPanelFinish: 条码: {panel.Barcode},层号:{layer},
                         PanelCode:{panel.PanelCode} itemCode:{panel.ItemCode},panelLength:{panel.PanelLength},PanelWidth:{panel.PanelWidth},PinOffset:{panel.PinOffset},stackCount:{panel.Pcs}");

                        PayloadPanels.RaiseCollectionChangedEvent(InteractingDevice.GetLocationCode(1));
                        logger.LogInformation(JsonSerializer.Serialize($"1号位置,PayloadPanels集合变化后{layer}层的值{PayloadPanels.FirstOrDefault(x => x.Layer == layer - 1 && x.Position == 1)}"));
                    }                    
                }
                else
                {
                   // InteractingDevice.mitsubishiClient.Write("W105", 0);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"HandleLeftPanelFinish: Error:{ex}");
                InteractingDevice.Connector.IsConnected = false;
                throw ex;
            }
        }

        public void HandleLeftAskDownLoad()
        {
            var askDownLoadSignal = InteractingDevice.mitsubishiClient.ReadInt16("W101").Value;
            if (askDownLoadSignal== 1)
            {
                InteractingDevice.WatchShelfProperty["1"].PinCallAgvDownLoadSilo = true;
            }
            else
            {
                InteractingDevice.mitsubishiClient.Write("W106", 0);
                InteractingDevice.WatchShelfProperty["1"].PinCallAgvDownLoadSilo = false;
            }
        }

        public void HandleRightAskUpload()
        {
            var askUploadSignal = InteractingDevice.mitsubishiClient.ReadInt16("W110").Value;
            if (askUploadSignal == 1)
            {
                InteractingDevice.WatchShelfProperty["2"].PinCallAgvUploadSilo = true;
            }
            else
            {
                InteractingDevice.mitsubishiClient.Write("W116", 0);
                InteractingDevice.WatchShelfProperty["2"].PinCallAgvUploadSilo = false;
            }
        }


       
        public void HandleRightPanelFinish()
        {
            try
            {
                var panelFinishSignal = InteractingDevice.mitsubishiClient.ReadUInt16("W115").Value;
                if (panelFinishSignal == 1)
                {                    
                    var layer = InteractingDevice.mitsubishiClient.ReadInt32("D4252").Value;//料层号
                    logger.LogDebug($"HandleRightPanelFinish:  层号:{layer}");
                    if (layer < 1 && layer > 18)
                    {
                        return;
                    }
                    var barcode = ""; 
                    var itemCode = InteractingDevice.mitsubishiClient.ReadStringExtensions("D4050");//料号
                    var panelLength = (float)Math.Round(InteractingDevice.mitsubishiClient.ReadInt32("D4256").Value/(float)10000, 2, MidpointRounding.AwayFromZero);//板长
                    var panelWidth = (float)Math.Round(InteractingDevice.mitsubishiClient.ReadInt32("D4258").Value/ (float)10000, 2, MidpointRounding.AwayFromZero);//板宽
                    var PinOffset = (float)0.0;
                    var stackCount = (float)InteractingDevice.mitsubishiClient.ReadInt32("D4260").Value;//叠板层数
                    var thickness = (float)Math.Round(InteractingDevice.mitsubishiClient.ReadInt32("D4262").Value / (float)10000,2, MidpointRounding.AwayFromZero); //板厚
                    var siloCode =  PayloadPanels.FirstOrDefault(s => !string.IsNullOrWhiteSpace(s.SiloCode))?.SiloCode;
                    logger.LogInformation(@$"HandleRightPanelFinish: 原始值:
                        layer:{layer},
                        panelLength:{panelLength},
                        panelWidth:{panelWidth}, 
                        PinOffset:{PinOffset},
                        StackCount:{stackCount},
                        PanelThickness:{thickness}");

                    GetNextPanelRequest request = new GetNextPanelRequest()
                    {
                        BeginLayer = layer - 1,
                        Count = 1,
                        ItemCode = itemCode,
                        PanelWidth = panelWidth,
                        PinOffset = PinOffset,
                        ProductStatus = ProductStatus.Finished_PIN,
                        SiloCode = siloCode,
                        Position = 2,
                        BatchCode = barcode,
                        LotId = "",
                    };

                    logger.LogInformation($"GetRightPanel Request:{JsonSerializer.Serialize(request)}");
                    var response = InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<GetNextPanelRequest, List<Panel>>(InteractingDevice.DeviceDescriptor.Extra["GetNextPanelNumberV2"].ToStr(), request).Result;
                    logger.LogInformation($"GetRightPanel Resonse:{JsonSerializer.Serialize(response)}");

                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));

                    Task.Run(async () =>
                    {
                        try
                        {
                            Item item = new Item()
                            {
                                Code = itemCode,
                                IncodeNumber = barcode,
                                PanelLength = panelLength,
                                PanelWidth = panelWidth,
                                PanelCount = (decimal)stackCount,
                            };
                            logger.LogInformation($"ProduceRightItem Request:{JsonSerializer.Serialize(item)}");
                            var response = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<Item, bool>(InteractingDevice.DeviceDescriptor.Extra["GenerateItem"].ToStr(), item);
                            logger.LogInformation($"ProduceRightItem response: {response}");
                        }
                        catch (Exception e)
                        {
                            logger.LogError(e, $"ProduceRightItem Error:{e.Message}");
                            logger.LogInformation($"ProduceRightItem Error: {e.ToStr()}");
                        }

                    }, cancellationTokenSource.Token);

                    var panel = PayloadPanels.FirstOrDefault(x => x.Layer == layer - 1 && x.Position == 2);
                    var signal = InteractingDevice.mitsubishiClient.ReadInt16("W115").Value;//再次确认信号，防呆
                    if (panel != null && signal==1)
                    {
                        panel.ProductStatus = ProductStatus.Finished_PIN;
                        panel.ItemCode = itemCode;
                        panel.PanelWidth = panelWidth;
                        panel.PinOffset = PinOffset;
                        panel.PanelLength = panelLength;
                        panel.Barcode = barcode;
                        panel.Pcs = stackCount.ToInt();
                        panel.PanelThickness = thickness;
                      
                        panel.PanelCode = response.FirstOrDefault()?.PanelCode;
                        logger.LogInformation(@$"HandleRightPanelFinish: 条码: {barcode},层号:{layer},
                        PanelCode:{panel.PanelCode} itemCode:{itemCode},panelLength:{panelLength},PanelWidth:{panelWidth},PinOffset:{PinOffset},stackCount:{stackCount}");

                        PayloadPanels.RaiseCollectionChangedEvent(InteractingDevice.GetLocationCode(2));
                        logger.LogInformation(JsonSerializer.Serialize($"2号位置,PayloadPanels集合变化后{layer}层的值{PayloadPanels.FirstOrDefault(x => x.Layer == layer - 1 && x.Position == 2)}"));
                    }
                }
                else
                {
                    //InteractingDevice.mitsubishiClient.Write("W115", 0);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"HandleRightPanelFinish: Error:{ex}");
                InteractingDevice.Connector.IsConnected = false;
                throw ex;
            }
        }

        public void HandleRightAskDownLoad()
        {
            var askDownLoadSignal = InteractingDevice.mitsubishiClient.ReadInt16("W111").Value;
            if (askDownLoadSignal == 1)
            {
                InteractingDevice.WatchShelfProperty["2"].PinCallAgvDownLoadSilo = true;
            }
            else
            {
                InteractingDevice.mitsubishiClient.Write("W116", 0);
                InteractingDevice.WatchShelfProperty["2"].PinCallAgvDownLoadSilo = false;
            }
        }
    }
}
