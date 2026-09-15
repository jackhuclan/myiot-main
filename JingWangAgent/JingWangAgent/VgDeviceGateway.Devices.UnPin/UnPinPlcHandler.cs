using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.UnPin.Plc
{
    public class UnPinPlcHandler : DeviceShare<UnPin>, IPlcHandle
    {
        private readonly ILogger<UnPinPlcHandler> logger;
        private readonly List<LoopTask> plcTasks = new List<LoopTask>();
        private readonly int period = 500;

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
            var askUploadSignal = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 40, 1);
            InteractingDevice.ItemNo = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 60, 20).UshortToStrings().Replace("\0", "").Replace("\r", "");
            if (askUploadSignal?[0] == 1)
            {
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

        public void HandleLeftPanelFinish()
        {
            try
            {
                var panelFinishSignal = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 44, 1);
                if (panelFinishSignal[0] == 1)
                {
                    var Layer = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 43, 1)[0];
                    logger.LogDebug($"HandleLeftPanelFinish:  层号:{Layer}");

                    PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                    {
                        var panel = PayloadPanels.FirstOrDefault(x => x.Layer == Layer - 1 && x.Position == 1);
                        if (panel != null)
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

                    InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 45, 1);
                }
                else
                {
                    InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 45, 0);
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
            var askDownLoadSignal = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 42, 1);
            if (askDownLoadSignal?[0] == 1)
            {
                // logger.LogInformation($"HandleLeftAskDownLoad:  收到 1号工位请求下料信号");
                InteractingDevice.WatchShelfProperty["1"].UpinCallAgvDownLoadSilo = true;
            }
            else
            {
                InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 47, 0);
                InteractingDevice.WatchShelfProperty["1"].UpinCallAgvDownLoadSilo = false;
            }
        }

        public void HandleRightAskUpload()
        {
            var askUploadSignal = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 50, 1);
            if (askUploadSignal?[0] == 1)
            {
                // logger.LogInformation($"HandleRightAskUpload:  收到 2号工位请求上料信号");
                InteractingDevice.WatchShelfProperty["2"].UpinCallAgvUploadSilo = true;
            }
            else
            {
                InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 56, 0);
                InteractingDevice.WatchShelfProperty["2"].UpinCallAgvUploadSilo = false;
            }
        }

        public void HandleRightPanelFinish()
        {
            var panelFinishSignal = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 54, 1);
            if (panelFinishSignal[0] == 1)
            {
                var Layer = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 53, 1)[0];
                logger.LogDebug($"HandleRightPanelFinish:  层号:{Layer}");

                PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                {
                    var panel = PayloadPanels.FirstOrDefault(x => x.Layer == Layer - 1 && x.Position == 2);
                    if (panel != null)
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

                InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 55, 1);
            }
            else
            {
                InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 55, 0);
            }
        }

        public void HandleRightAskDownLoad()
        {
            var askDownLoadSignal = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(1, 52, 1);
            if (askDownLoadSignal?[0] == 1)
            {
                // logger.LogInformation($"HandleRightAskDownLoad:  收到 2号工位请求下料信号");
                InteractingDevice.WatchShelfProperty["2"].UpinCallAgvDownLoadSilo = true;
            }
            else
            {
                InteractingDevice.modbusIpMaster.WriteSingleRegister(1, 57, 0);
                InteractingDevice.WatchShelfProperty["2"].UpinCallAgvDownLoadSilo = false;
            }
        }
    }
}
