using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Pin.Plc
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
            var askUploadSignal = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 20, 1);
            if (askUploadSignal?[0] == 1)
            {
                // logger.LogDebug($"HandleLeftAskUpload: 执行  PinCallAgvUploadSilo=true");
                InteractingDevice.WatchShelfProperty["1"].PinCallAgvUploadSilo = true;
            }
            else
            {
                InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 26, 0);
                InteractingDevice.WatchShelfProperty["1"].PinCallAgvUploadSilo = false;
            }
        }

        public void HandleLeftPanelFinish()
        {
            try
            {
                var panelFinishSignal = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 24, 1);
                if (panelFinishSignal[0] == 1)
                {
                    var Layer = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 23, 1)[0];
                    var barcode = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 1, 19).UshortToStrings().Replace("\0", "").Replace("\r", "");
                    var itemCode = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 60, 20).UshortToStrings().Replace("\0", "").Replace("\r", "");
                    var panelLength = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 80, 2).UshortToFloat();
                    var panelWidth = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 82, 2).UshortToFloat();
                    var PinOffset = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 84, 2).UshortToFloat();
                    var stackCount = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 86, 2).UshortToFloat();
                    var thickness = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 88, 2).UshortToFloat();

                    logger.LogInformation($"HandleLeftPanelFinish: 原始值: panelLength:{JsonSerializer.Serialize(InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 80, 2))}," +
                  $"panelWidth:{JsonSerializer.Serialize(InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 82, 2))}, " +
                  $"PinOffset:{JsonSerializer.Serialize(InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 84, 2))}," +
                  $"StackCount:{JsonSerializer.Serialize(InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 86, 2))}," +
                  $"PanelThickness:{JsonSerializer.Serialize(InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 88, 2))}");

                    GetNextPanelRequest request = new GetNextPanelRequest()
                    {
                        BeginLayer = Layer - 1,
                        Count = 1,
                        ItemCode = itemCode,
                        PanelWidth = panelWidth,
                        PinOffset = PinOffset,
                        ProductStatus = ProductStatus.Finished_PIN,
                        SiloCode = PayloadPanels.FirstOrDefault(s => !string.IsNullOrWhiteSpace(s.SiloCode))?.SiloCode ?? "",
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
                                PanelLength = panelWidth,
                                PanelWidth = panelWidth,
                                PanelCount = (decimal)stackCount,
                            };
                            logger.LogInformation($"ProduceLeftItem Request:{JsonSerializer.Serialize(item)}");
                            var response = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<Item, bool>(InteractingDevice.DeviceDescriptor.Extra["GenerateItem"].ToStr(), item);
                            logger.LogInformation($"ProduceLeftItem response: {response}");
                        }
                        catch (Exception e)
                        {
                            logger.LogError(e,$"ProduceLeftItem Error:{e.Message}");
                            logger.LogInformation($"ProduceLeftItem Error: {e.ToStr()}");
                        }

                    }, cancellationTokenSource.Token);

                    var panel = PayloadPanels.FirstOrDefault(x => x.Layer == Layer - 1 && x.Position == 1);
                    panel.ProductStatus = ProductStatus.Finished_PIN;
                    panel.ItemCode = itemCode;
                    panel.PanelWidth = panelWidth;
                    panel.PinOffset = PinOffset;
                    panel.PanelLength = panelLength;
                    panel.Barcode = barcode;
                    panel.Pcs = stackCount.ToInt();
                    panel.PanelThickness = thickness;
                    panel.PanelCode = response.FirstOrDefault()?.PanelCode;
                    logger.LogInformation($"HandleLeftPanelFinish: 条码: {barcode},层号:{Layer}," +
                     $"PanelCode:{panel.PanelCode} itemCode:{itemCode},panelLength:{panelLength},PanelWidth:{panelWidth},PinOffset:{PinOffset},stackCount:{stackCount}");

                    PayloadPanels.RaiseCollectionChangedEvent(InteractingDevice.GetLocationCode(1));
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 25, 1);
                }
                else
                {
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 25, 0);
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
            var askDownLoadSignal = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 22, 1);
            if (askDownLoadSignal?[0] == 1)
            {
                InteractingDevice.WatchShelfProperty["1"].PinCallAgvDownLoadSilo = true;
            }
            else
            {
                InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 27, 0);
                InteractingDevice.WatchShelfProperty["1"].PinCallAgvDownLoadSilo = false;
            }
        }

        public void HandleRightAskUpload()
        {
            var askUploadSignal = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 30, 1);
            if (askUploadSignal?[0] == 1)
            {
                InteractingDevice.WatchShelfProperty["2"].PinCallAgvUploadSilo = true;
            }
            else
            {
                InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 36, 0);
                InteractingDevice.WatchShelfProperty["2"].PinCallAgvUploadSilo = false;
            }
        }

        public void HandleRightPanelFinish()
        {
            try
            {
                var panelFinishSignal = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 34, 1);
                if (panelFinishSignal[0] == 1)
                {
                    var Layer = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 33, 1)[0];
                    var barcode = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 1, 19).UshortToStrings().Replace("\0", "").Replace("\r", "");
                    var itemCode = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 60, 20).UshortToStrings().Replace("\0", "").Replace("\r", "");
                    var panelLength = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 80, 2).UshortToFloat();
                    var panelWidth = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 82, 2).UshortToFloat();
                    var PinOffset = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 84, 2).UshortToFloat();
                    var stackCount = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 86, 2).UshortToFloat();
                    var thickness = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 88, 2).UshortToFloat();
                    logger.LogInformation($"HandleRightPanelFinish: 原始值: panelLength:{JsonSerializer.Serialize(InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 80, 2))}," +
                        $"panelWidth:{JsonSerializer.Serialize(InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 82, 2))}, " +
                        $"PinOffset:{JsonSerializer.Serialize(InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 84, 2))}," +
                        $"StackCount:{JsonSerializer.Serialize(InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 86, 2))}," +
                        $"PanelThickness:{JsonSerializer.Serialize(InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 88, 2))}");

                    GetNextPanelRequest request = new GetNextPanelRequest()
                    {
                        BeginLayer = Layer - 1,
                        Count = 1,
                        ItemCode = itemCode,
                        PanelWidth = panelWidth,
                        PinOffset = PinOffset,
                        ProductStatus = ProductStatus.Finished_PIN,
                        SiloCode = PayloadPanels.FirstOrDefault()?.SiloCode,
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
                                PanelLength = panelWidth,
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

                    //await PayloadPanels.ChangeListSafely(Task.Run(() =>
                    //{
                    var panel = PayloadPanels.FirstOrDefault(x => x.Layer == Layer - 1 && x.Position == 2);
                    panel.ProductStatus = ProductStatus.Finished_PIN;
                    panel.ItemCode = itemCode;
                    panel.PanelWidth = panelWidth;
                    panel.PinOffset = PinOffset;
                    panel.PanelLength = panelLength;
                    panel.Barcode = barcode;
                    panel.Pcs = stackCount.ToInt();
                    panel.PanelThickness = thickness;
                    panel.PanelCode = response.FirstOrDefault()?.PanelCode;
                    logger.LogInformation($"HandleRightPanelFinish: 条码: {barcode},层号:{Layer}," +
                   $"PanelCode:{panel.PanelCode} itemCode:{itemCode},panelLength:{panelLength},PanelWidth:{panelWidth},PinOffset:{PinOffset},stackCount:{stackCount}");
                    //}));

                    PayloadPanels.RaiseCollectionChangedEvent(InteractingDevice.GetLocationCode(2));

                    InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 35, 1);
                }
                else
                {
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 35, 0);
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
            var askDownLoadSignal = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 32, 1);
            if (askDownLoadSignal?[0] == 1)
            {
                InteractingDevice.WatchShelfProperty["2"].PinCallAgvDownLoadSilo = true;
            }
            else
            {
                InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 37, 0);
                InteractingDevice.WatchShelfProperty["2"].PinCallAgvDownLoadSilo = false;
            }
        }
    }
}
