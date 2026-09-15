using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot;
using VgDeviceGateway.Devices.Common;
using VgAutoDrill.Fundation.Utils;

namespace Vegalot.External.LeweiUnPin
{
    public class UnPinPlcHandler : DeviceShare<UnPin>, IPlcHandle
    {
        private readonly ILogger<UnPinPlcHandler> logger;
        private readonly List<LoopTask> plcTasks = new List<LoopTask>();
        private readonly int period = 500;
        private int index = 1;

        public UnPinPlcHandler(ILogger<UnPinPlcHandler> logger, IServiceProvider serviceProvider, UnPin device) : base(serviceProvider, device)
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
        /// 下PIN 左工位呼叫上料仓
        /// </summary>
        public void HandleLeftAskUpload()
        {
            var askUploadSignal = InteractingDevice.mitsubishiClient.ReadInt16("W100").Value;        
            if (askUploadSignal == 1)
            {
               
                //TODO 判断是否还有该料号的料仓可下Pin,没有则写信号通知
                //  logger.LogInformation($"HandleLeftAskUpload:  收到 1号工位请求上料信号");
                InteractingDevice.WatchShelfProperty["1"].UpinCallAgvUploadSilo = true;
            }
            else
            {
                InteractingDevice.mitsubishiClient.Write("W106", (short)0);
                InteractingDevice.WatchShelfProperty["1"].UpinCallAgvUploadSilo = false;
            }
        }

        public void HandleLeftPanelFinish()
        {
            try
            {
                var panelFinishSignal = InteractingDevice.mitsubishiClient.ReadInt16("W105").Value;
                
                if (panelFinishSignal == 1)
                {                 
                    var layer = InteractingDevice.mitsubishiClient.ReadInt32("D4202").Value;//范围值1-18
                    logger.LogDebug($"HandleLeftPanelFinish:  层号:{layer}");
                    if (layer < 1 && layer > 18)
                    {
                        return;
                    }
                    ///清空plc里面地址位板料状态信息
                    int address =  4300 + 2 * (layer - 1);
                    InteractingDevice.mitsubishiClient?.Write($"D{address}", 0);                    
                    PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                    {
                        var panel = PayloadPanels.FirstOrDefault(x => x.Layer == layer - 1 && x.Position == 1);
                        var signal = InteractingDevice.mitsubishiClient.ReadInt16("W105").Value;//再次确认信号，防呆
                        if (panel != null && signal == 1)
                        {
                            panel.ProductStatus = ProductStatus.EmptySiloBox;
                            panel.PanelCode = "";
                            panel.PanelWidth = 0;
                            panel.PanelLength = 0;
                            panel.Pcs = 0;
                            panel.PanelThickness = 0;
                            panel.PinOffset = 0;
                            panel.ItemCode = "";
                            panel.LotId = "";
                            panel.BatchCode = "";
                            panel.Barcode = "";
                            //  PayloadPanels.RaiseCollectionChangedEvent();
                        }
                    }));
                  
                }
                else
                {
                   // InteractingDevice.mitsubishiClient.Write("W105",0);
                }
            }
            catch (Exception ex)
            {
                InteractingDevice.Connector.IsConnected = false;
                throw ex;
            }
        }

        public void HandleLeftAskDownLoad()
        {
            var askDownLoadSignal = InteractingDevice.mitsubishiClient.ReadInt16("W101").Value;
            if (askDownLoadSignal == 1)
            {
                // logger.LogInformation($"HandleLeftAskDownLoad:  收到 1号工位请求下料信号");
                InteractingDevice.WatchShelfProperty["1"].UpinCallAgvDownLoadSilo = true;
            }
            else
            {
                InteractingDevice.mitsubishiClient.Write("W106", (short)0);
                InteractingDevice.WatchShelfProperty["1"].UpinCallAgvDownLoadSilo = false;
            }
        }

        public void HandleRightAskUpload()
        {
            var askUploadSignal = InteractingDevice.mitsubishiClient.ReadInt16("W110").Value;
            if (askUploadSignal == 1)
            {
                // logger.LogInformation($"HandleRightAskUpload:  收到 2号工位请求上料信号");
                InteractingDevice.WatchShelfProperty["2"].UpinCallAgvUploadSilo = true;
            }
            else
            {
                InteractingDevice.mitsubishiClient.Write("W116", (short)0);
                InteractingDevice.WatchShelfProperty["2"].UpinCallAgvUploadSilo = false;
            }
        }

        public void HandleRightPanelFinish()
        {
            var panelFinishSignal = InteractingDevice.mitsubishiClient.ReadInt16("W115").Value;
            if (panelFinishSignal == 1)
            {
                var layer  = InteractingDevice.mitsubishiClient.ReadInt32("D4252").Value; //范围值1-18
                logger.LogDebug($"HandleRightPanelFinish:  层号:{layer}");
                if (layer < 1 && layer > 18)
                {
                    return;
                }
                ///清空plc里面地址位板料状态信息
                int address = 4350 + 2 * (layer - 1);
                InteractingDevice.mitsubishiClient?.Write($"D{address}", 0);
                PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                {
                    var panel = PayloadPanels.FirstOrDefault(x => x.Layer == layer - 1 && x.Position == 2);
                    var signal = InteractingDevice.mitsubishiClient.ReadInt16("W115").Value;//再次确认信号，防呆
                    if (panel != null && signal == 1)
                    {
                        panel.ProductStatus = ProductStatus.EmptySiloBox;
                        panel.PanelCode = "";
                        panel.PanelWidth = 0;
                        panel.PanelLength = 0;
                        panel.Pcs = 0;
                        panel.PanelThickness = 0;
                        panel.PinOffset = 0;
                        panel.ItemCode = "";
                        panel.LotId = "";
                        panel.BatchCode = "";
                        panel.Barcode = "";
                        // PayloadPanels.RaiseCollectionChangedEvent();
                    }
                }));
            }
            else
            {
              //  InteractingDevice.mitsubishiClient.Write("W115", (short)0);
            }
        }

        public void HandleRightAskDownLoad()
        {
            var askDownLoadSignal = InteractingDevice.mitsubishiClient.ReadInt16("W111").Value;
            if (askDownLoadSignal == 1)
            {
                // logger.LogInformation($"HandleRightAskDownLoad:  收到 2号工位请求下料信号");
                InteractingDevice.WatchShelfProperty["2"].UpinCallAgvDownLoadSilo = true;
            }
            else
            {
                InteractingDevice.mitsubishiClient.Write("W116", (short)0);
                InteractingDevice.WatchShelfProperty["2"].UpinCallAgvDownLoadSilo = false;
            }
        }
    }
}
