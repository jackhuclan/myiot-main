using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.State;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Agv.StateHandler;

namespace VgDeviceGateway.Devices.Agv
{
    public class DefaultAgvStateHandler : AbstractStateHandler<DefaultAgv>
    {
        private IAgvStateHandler abstartAgvStateHandler;

        public DefaultAgvStateHandler(
            IServiceProvider serviceProvider,
            DefaultAgv defaultAgv)
            : base(serviceProvider, defaultAgv)
        {
            abstartAgvStateHandler = InteractionAgvFactory.CreateAgvStateHandler(InteractingDevice);
        }

        public override void AddWatchingStates()
        {
            if (abstartAgvStateHandler == null)
            {
                return;
            }

            WatchingProperties.Properties(abstartAgvStateHandler.OnlineProperties.ToArray())
            .When(properties => abstartAgvStateHandler.StateOnlineCondition(properties))
            .TriggerAlways(async () =>
            {
                // InteractingDevice.logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to change status from {InteractingDevice.Status} to {DeviceStatus.Online} ...");
                var request = InteractingDevice.GetStatusRequest(DeviceStatus.Online);
                var reportonline = await DataExporter.DeviceStatusReport(request);
                if (InteractingDevice.configExtra.ContainsKey("ShowWhichLog")
                        && InteractingDevice.configExtra["ShowWhichLog"].ToStr() == "ShowReportOnlineLog")
                {
                    InteractingDevice.logger.LogDebug($"Online 状态上报结果：{reportonline.Code}_message:{reportonline.Message}_CurrentEventTraceId：{InteractingDevice.currentEventTraceId}\r\n");
                }
                abstartAgvStateHandler.UpdatePLcAgvStatus(DeviceStatus.Online);
                InteractingDevice.Status = DeviceStatus.Online;
                // InteractingDevice.logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} changed status to {DeviceStatus.Online}");
            });

            WatchingProperties.Properties(abstartAgvStateHandler.ReadyProperties.ToArray())
                  .When(properties => abstartAgvStateHandler.SetStateConditionReady(properties))
                  .TriggerAlways(async () =>
                  {
                      var request = InteractingDevice.GetStatusRequest(DeviceStatus.Ready);
                      var response = await DataExporter.DeviceStatusReport(request);
                      if (InteractingDevice.configExtra.ContainsKey("ShowWhichLog")
                          && InteractingDevice.configExtra["ShowWhichLog"].ToStr() == "ShowReportReadyLog")
                      {
                          InteractingDevice.logger.LogDebug($"Watch_Ready 状态上报结果：{response.Code}_message:{response.Message}_CurrentEventTraceId：{InteractingDevice.currentEventTraceId}\r\n");
                      }
                      abstartAgvStateHandler.UpdatePLcAgvStatus(DeviceStatus.Ready);

                      //1.25新增处理lowbattly待验证
                      //可以自动切回到ready的状态
                      if (InteractingDevice.Status == DeviceStatus.LowBattery)
                      {
                          InteractingDevice.Status = DeviceStatus.Ready;
                          InteractingDevice.logger.LogDebug($"属性判断为ready，Status状态为 LowBattery，Status更新成Ready");
                      }

                      if (InteractingDevice.Status == DeviceStatus.Exception)
                      {
                          InteractingDevice.isAgvWorkFail = true;
                          this.WatchingProperties.Property("IsAgvWorkFail").SetValue(true);
                          InteractingDevice.logger.LogDebug($"属性判断为ready和Status：{InteractingDevice.Status} 不一致，属性判断同步为不Ready");
                      }

                      //  InteractingDevice.Status = DeviceStatus.Ready;
                      abstartAgvStateHandler.UpdatePLcSiloInfo();
                  });

            WatchingProperties.Properties(abstartAgvStateHandler.WorkingProperties.ToArray())
             .When(properties => abstartAgvStateHandler.SetStateConditionWorking(properties))
             .TriggerAlways(async () =>
             {
                 // InteractingDevice.logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to change status from {InteractingDevice.Status} to {DeviceStatus.Working} ...");
                 //InteractingDevice.Status = DeviceStatus.Working;
                 var request = InteractingDevice.GetStatusRequest(DeviceStatus.Working);
                 var reportcharging = await DataExporter.DeviceStatusReport(request);
                 if (InteractingDevice.configExtra.ContainsKey("ShowWhichLog")
                     && InteractingDevice.configExtra["ShowWhichLog"].ToStr() == "ShowReportWorkingLog")
                 {
                     InteractingDevice.logger.LogDebug($"Working 状态上报结果：{reportcharging.Code}_message:{reportcharging.Message}_CurrentEventTraceId：{InteractingDevice.currentEventTraceId}\r\n");
                 }

               //  InteractingDevice.Status = DeviceStatus.Working;
                 abstartAgvStateHandler.UpdatePLcAgvStatus(DeviceStatus.Working);
                 abstartAgvStateHandler.UpdatePLcSiloInfo();
                 // InteractingDevice.logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} changed status to {DeviceStatus.Working}");
             });

            WatchingProperties.Properties(abstartAgvStateHandler.LowBatteryProperties.ToArray())
                   .When(properties => abstartAgvStateHandler.SetStateConditionLowBattery(properties))
                   .TriggerAlways(async () =>
                   {
                       if (InteractingDevice.Status == DeviceStatus.Working)
                       {
                           InteractingDevice.logger.LogDebug($"因为Working不能改为LowBattery！！！！");
                           return;
                       }
                       //  InteractingDevice.logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to change status from {InteractingDevice.Status} to {DeviceStatus.LowBattery} ...");
                       InteractingDevice.Status = DeviceStatus.LowBattery;
                       var request = InteractingDevice.GetStatusRequest(DeviceStatus.LowBattery);
                       var reportlow = await DataExporter.DeviceStatusReport(request);
                       if (InteractingDevice.configExtra.ContainsKey("ShowWhichLog")
                          && InteractingDevice.configExtra["ShowWhichLog"].ToStr() == "ShowReportLowBatteryLog")
                       {
                           InteractingDevice.logger.LogDebug($"LowBattery 状态上报结果：{reportlow.Code}_message:{reportlow.Message}_CurrentEventTraceId：{InteractingDevice.currentEventTraceId}\r\n");
                       }

                       abstartAgvStateHandler.UpdatePLcAgvStatus(DeviceStatus.LowBattery);
                       //更新PLC的料仓信息
                       abstartAgvStateHandler.UpdatePLcSiloInfo();
                       // InteractingDevice.logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} changed status to {DeviceStatus.LowBattery}");
                   });

            WatchingProperties.Properties(abstartAgvStateHandler.ChargingProperties.ToArray())
                  .When(properties => abstartAgvStateHandler.SetStateConditionCharging(properties))
                  .TriggerAlways(async () =>
                  {
                      //  InteractingDevice.logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to change status from {InteractingDevice.Status} to {DeviceStatus.Charging} ...");
                      InteractingDevice.Status = DeviceStatus.Charging;
                      var request = InteractingDevice.GetStatusRequest(DeviceStatus.Charging);
                      var reportcharging = await DataExporter.DeviceStatusReport(request);
                      if (InteractingDevice.configExtra.ContainsKey("ShowWhichLog")
                          && InteractingDevice.configExtra["ShowWhichLog"].ToStr() == "ShowReportChargingLog")
                      {
                          InteractingDevice.logger.LogDebug($"Charging 状态上报结果：{reportcharging.Code}_message:{reportcharging.Message}_CurrentEventTraceId：{InteractingDevice.currentEventTraceId}\r\n");
                      }

                      abstartAgvStateHandler.UpdatePLcAgvStatus(DeviceStatus.Charging);
                      //更新PLC的料仓信息
                      abstartAgvStateHandler.UpdatePLcSiloInfo();
                      //  InteractingDevice.logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} changed status to {DeviceStatus.Charging}");
                  });

            WatchingProperties.Properties(abstartAgvStateHandler.ExceptionProperties.ToArray())
                  .When(properties => abstartAgvStateHandler.SetStateConditionException(properties))
                 .TriggerAlways(async () =>
                 {
                     //InteractingDevice.logger.LogDebug($"\r\n 监控到异常：\r\n" +
                     //    $"CurrentEventTraceId：{InteractingDevice.currentEventTraceId}\r\n" +
                     //    $"IsConnected：{WatchingProperties.Property("IsConnected").NewValue.ToBool()}\r\n" +
                     //    $"PlcIsReady：{WatchingProperties.Property("PlcIsReady").NewValue.ToBool()}\r\n" +
                     //    $"IsAuto：{WatchingProperties.Property("IsAuto").NewValue.ToBool()}\r\n" +
                     //    $"IsError：{WatchingProperties.Property("IsError").NewValue.ToBool()}\r\n" +
                     //    $"WarningCode：{WatchingProperties.Property("WarningCode").NewValue.ToStr()}\r\n" +
                     //    $"ErrorMessage：{InteractingDevice.errorsMsg}\r\n" +
                     //    $"IsHalt：{WatchingProperties.Property("IsHalt").NewValue.ToBool()}\r\n" +
                     //    $"CanDispatch：{WatchingProperties.Property("CanDispatch").NewValue.ToBool()}\r\n" +
                     //    $"IsAgvWorkFail：{WatchingProperties.Property("IsAgvWorkFail").NewValue.ToBool()} \r\n");
                     InteractingDevice.Status = DeviceStatus.Exception;
                     var request = InteractingDevice.GetStatusRequest(DeviceStatus.Exception);
                     var errreport = await DataExporter.DeviceStatusReport(request);
                     if (InteractingDevice.configExtra.ContainsKey("ShowWhichLog")
                          && InteractingDevice.configExtra["ShowWhichLog"].ToStr() == "ShowReportExceptionLog")
                     {
                         InteractingDevice.logger.LogDebug($"Exception 状态上报结果：{errreport.Code}_message:{errreport.Message}_CurrentEventTraceId：{InteractingDevice.currentEventTraceId}\r\n");
                     }

                     abstartAgvStateHandler.UpdatePLcAgvStatus(DeviceStatus.Exception);
                     ////同步异常给PLC
                     abstartAgvStateHandler.UpdatePLcError();
                     //更新PLC的料仓信息
                     abstartAgvStateHandler.UpdatePLcSiloInfo();
                     //  InteractingDevice.logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} changed status to {DeviceStatus.Exception}");
                 });
        }

        //private DeviceStatusReportRequest GetStatusRequest(DeviceStatus newStatus)
        //{
        //    var request = new DeviceStatusReportRequest
        //    {
        //        DeviceId = DeviceDescriptor.DeviceId,
        //        ProductId = DeviceDescriptor.ProductId,
        //        NewStatus = newStatus,
        //        OldStatus = InteractingDevice.Status,
        //    };
        //    request.Params["CarCurrentPos"] = WatchingProperties.Property("CarCurrentPos").NewValue.ToStr();
        //    request.Params["IsFullSilo"] = WatchingProperties.Property("IsFullSilo").NewValue.ToBool();
        //    request.Params["WarningCode"] = WatchingProperties.Property("WarningCode").NewValue.ToStr();
        //    request.Params["WarningMessage"] = InteractingDevice.GetWarningMessage(WatchingProperties.Property("WarningCode").NewValue.ToStr());
        //    request.PayloadPanels = PayloadPanels;

        //    return request;
        //}
    }
}
