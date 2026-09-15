// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.UnitTest
{
    public class AjwBackAgvTest
    {
        [Fact]
        public void TestAjwBackAgvWatchableProperties()
        {
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties
            .AddProperty("IsConnected", false)
            .AddProperty("IsAuto", false)
            .AddProperty("PlcIsReady", false)
            .AddProperty("IsError", false)
            .AddProperty("IsHalt", false)
            .AddProperty("IsAgvWorkFail", false)
            .AddProperty("CanDispatch", false)
            .AddProperty("IsLowBattery", false)
            .AddProperty("IsCharging", false)
            .AddProperty("IsWorking", false)
            .AddProperty("AgvIsReady", false)
            .AddProperty("IsAgvLowBattery", false)
            .AddProperty("IsFullSilo", false)
            .AddProperty("Battery", 30)
            .AddProperty("AgvStatus", "")
            .AddProperty("AgvTaskId", "")
            .AddProperty("AgvReturnTaskId", "0")//�������Ҫ
            .AddProperty("CarCurrentPos", "")
            .AddProperty("CarTargetPos", "")
            .AddProperty("MoveTargetPos", "")
            .AddProperty("IsMoving", false)
            .AddProperty("IsArrived", false)
            .AddProperty("AgvScanResult", true)
            .AddProperty("Offset_X", 0)
            .AddProperty("Offset_Z", 0)
            .AddProperty("WaringCode", 0)
            .AddProperty("PrepareLoadOk", false)
            .AddProperty("InvokeLoadOk", false)
            .AddProperty("CompleteLoadOk", false)
            .AddProperty("PrepareUnloadOk", false)
            .AddProperty("InvokeUnloadOk", false)
            .AddProperty("CompleteUnloadOk", false)
            .AddProperty("CheckOnline", true)
            .AddProperty("CheckReady", true)
            .AddProperty("CheckWorking", true);

            AddWatchingStates(watchableProperties);

            watchableProperties.SetValues(new Dictionary<string, object?>
            {
                //{"ExceptionEventId", ""},
                //{"ExceptionEventName", ""},
                //{"ExceptionEventMessage", ""},
                {"IsConnected", true},
                {"IsAuto", true},
                {"PlcIsReady", true},
                {"IsError", true},
                {"IsHalt", false},
                {"IsAgvWorkFail", false},
                {"CanDispatch", true},
                {"IsLowBattery", false},
                {"IsCharging", false},
                {"IsWorking", false},
                {"AgvIsReady", false},
                {"IsAgvLowBattery", false},
                {"IsFullSilo", false},
                {"Battery", 82},
                {"AgvStatus", "Connected"},
                {"AgvTaskId", ""},
                //{"AgvReturnTaskId", "0"},
                {"CarCurrentPos", "00022"},
                {"CarTargetPos", "00022"},
                //{"MoveTargetPos", ""},
                {"IsMoving", false},
                {"IsArrived", false},
                {"AgvScanResult", true},
                {"Offset_X", "0.00"},
                {"Offset_Z", "0.00"},
                {"WaringCode", 24},
                //{"PrepareLoadOk", false},
                //{"InvokeLoadOk", false},
                //{"CompleteLoadOk", false},
                //{"PrepareUnloadOk", false},
                //{"InvokeUnloadOk", false},
                //{"CompleteUnloadOk", false},
                {"CheckOnline", true},
                {"CheckReady", true},
                {"CheckWorking", true}
            });

            Thread.Sleep(5000);

            watchableProperties.SetValues(new Dictionary<string, object?>
                {
                //{"ExceptionEventId", ""},
                //{"ExceptionEventName", ""},
                //{"ExceptionEventMessage", ""},
                {"IsConnected", true},
                {"IsAuto", true},
                {"PlcIsReady", true},
                {"IsError", false},
                {"IsHalt", false},
                {"IsAgvWorkFail", false},
                {"CanDispatch", true},
                {"IsLowBattery", false},
                {"IsCharging", false},
                {"IsWorking", false},
                {"AgvIsReady", false},
                {"IsAgvLowBattery", false},
                {"IsFullSilo", false},
                {"Battery", 78.8},
                {"AgvStatus", "Connected"},
                {"AgvTaskId", ""},
                {"/*AgvReturnTaskId*/", "0"},
                {"CarCurrentPos", "00022"},
                {"CarTargetPos", "00022"},
                //{"MoveTargetPos", ""},
                {"IsMoving", false},
                {"IsArrived", false},
                {"AgvScanResult", true},
                {"Offset_X", "0.00"},
                {"Offset_Z", "0.00"},
                {"WaringCode", 0},
                //{"PrepareLoadOk", false},
                //{"InvokeLoadOk", false},
                //{"CompleteLoadOk", false},
                //{"PrepareUnloadOk", false},
                //{"InvokeUnloadOk", false},
                //{"CompleteUnloadOk", false},
                {"CheckOnline", true},
                {"CheckReady", true},
                {"CheckWorking", true}
            });

            //watchableProperties.SetValues(new Dictionary<string, object?>
            //{
            //});
        }

        private void AddWatchingStates(WatchableProperties watchableProperties)
        {
            watchableProperties.Properties("IsConnected", "IsAuto", "PlcIsReady", "IsError", "IsHalt", "IsAgvWorkFail", "CanDispatch", "IsLowBattery", "IsCharging", "IsWorking", "AgvIsReady", "CheckOnline")
                 .When(properties => properties.Property("IsConnected").NewValue.ToBool()
                 && properties.Property("IsAuto").NewValue.ToBool()
                 && properties.Property("PlcIsReady").NewValue.ToBool()
                 && !properties.Property("IsError").NewValue.ToBool()
                 && !properties.Property("IsHalt").NewValue.ToBool()
                 && !properties.Property("IsAgvWorkFail").NewValue.ToBool()
                 && properties.Property("CanDispatch").NewValue.ToBool()
                 && !properties.Property("IsLowBattery").NewValue.ToBool()
                 && !properties.Property("IsCharging").NewValue.ToBool()
                 && !properties.Property("IsWorking").NewValue.ToBool()
                 && !properties.Property("AgvIsReady").NewValue.ToBool()
                 && properties.Property("CheckOnline").NewValue.ToBool())
                 .TriggerAlways(async () =>
                 {
                     System.Diagnostics.Debug.WriteLine("DeviceStatus.Online");
                     //var request = GetStatusRequest(DeviceStatus.Online);
                     //await DataExporter.DeviceStatusReport(request);
                     //InteractingDevice.Status = DeviceStatus.Online;
                 });

            watchableProperties.Properties("IsConnected", "IsAuto", "PlcIsReady", "IsError", "IsHalt", "IsAgvWorkFail", "CanDispatch", "IsLowBattery", "IsCharging", "IsWorking", "AgvIsReady", "CheckReady")
                  .When(properties => properties.Property("IsConnected").NewValue.ToBool()
                  && properties.Property("IsAuto").NewValue.ToBool()
                  && properties.Property("PlcIsReady").NewValue.ToBool()
                  && !properties.Property("IsError").NewValue.ToBool()
                  && !properties.Property("IsHalt").NewValue.ToBool()
                  && !properties.Property("IsAgvWorkFail").NewValue.ToBool()
                  && properties.Property("CanDispatch").NewValue.ToBool()
                  && !properties.Property("IsLowBattery").NewValue.ToBool()
                  && !properties.Property("IsCharging").NewValue.ToBool()
                  && !properties.Property("IsWorking").NewValue.ToBool()
                  && properties.Property("AgvIsReady").NewValue.ToBool()
                  && properties.Property("CheckReady").NewValue.ToBool())
                  .TriggerAlways(() =>
                  {
                      System.Diagnostics.Debug.WriteLine("DeviceStatus.Ready");
                      //InteractingDevice.Status = DeviceStatus.Ready;
                      //var request = GetStatusRequest(DeviceStatus.Ready);
                      //_ = DataExporter.DeviceStatusReport(request);
                      ////����PLC���ϲ���Ϣ
                      //UpdatePLcSiloInfo();
                  });

            watchableProperties.Properties("IsConnected", "IsAuto", "PlcIsReady", "IsError", "IsHalt", "IsAgvWorkFail", "CanDispatch", "IsLowBattery", "IsCharging", "IsWorking", "AgvIsReady", "CheckWorking")
                  .When(properties => properties.Property("IsConnected").NewValue.ToBool()
                  && properties.Property("IsAuto").NewValue.ToBool()
                  && properties.Property("PlcIsReady").NewValue.ToBool()
                  && !properties.Property("IsError").NewValue.ToBool()
                  && !properties.Property("IsHalt").NewValue.ToBool()
                  && !properties.Property("IsAgvWorkFail").NewValue.ToBool()
                  && properties.Property("CanDispatch").NewValue.ToBool()
                  && !properties.Property("IsLowBattery").NewValue.ToBool()
                  && !properties.Property("IsCharging").NewValue.ToBool()
                  && properties.Property("IsWorking").NewValue.ToBool()
                  && properties.Property("AgvIsReady").NewValue.ToBool()
                  && properties.Property("CheckWorking").NewValue.ToBool())
                  .TriggerAlways(() =>
                  {
                      System.Diagnostics.Debug.WriteLine("DeviceStatus.Working");
                      //InteractingDevice.Status = DeviceStatus.Working;
                      //var request = GetStatusRequest(DeviceStatus.Working);
                      //_ = DataExporter.DeviceStatusReport(request);
                      ////����PLC���ϲ���Ϣ
                      //UpdatePLcSiloInfo();
                  });

            watchableProperties.Properties("IsConnected", "IsAuto", "PlcIsReady", "IsError", "IsHalt", "CanDispatch", "IsAgvWorkFail", "IsLowBattery", "IsCharging")
                   .When(properties => properties.Property("IsConnected").NewValue.ToBool()
                   && properties.Property("IsAuto").NewValue.ToBool()
                   && properties.Property("PlcIsReady").NewValue.ToBool()
                   && !properties.Property("IsError").NewValue.ToBool()
                   && !properties.Property("IsHalt").NewValue.ToBool()
                   && properties.Property("CanDispatch").NewValue.ToBool()
                   && !properties.Property("IsAgvWorkFail").NewValue.ToBool()
                   && properties.Property("IsLowBattery").NewValue.ToBool()
                   && !properties.Property("IsCharging").NewValue.ToBool())
                   .TriggerAlways(() =>
                   {
                       System.Diagnostics.Debug.WriteLine("DeviceStatus.LowBattery");
                       //InteractingDevice.Status = DeviceStatus.LowBattery;
                       //var request = GetStatusRequest(DeviceStatus.LowBattery);
                       //_ = DataExporter.DeviceStatusReport(request);
                       ////����PLC���ϲ���Ϣ
                       //UpdatePLcSiloInfo();
                   });

            watchableProperties.Properties("IsConnected", "IsAuto", "PlcIsReady", "IsError", "IsHalt", "IsAgvWorkFail", "CanDispatch", "IsCharging")
                  .When(properties => properties.Property("IsConnected").NewValue.ToBool()
                  && properties.Property("IsAuto").NewValue.ToBool()
                  && properties.Property("PlcIsReady").NewValue.ToBool()
                  && !properties.Property("IsError").NewValue.ToBool()
                  && !properties.Property("IsHalt").NewValue.ToBool()
                  && properties.Property("CanDispatch").NewValue.ToBool()
                  && !properties.Property("IsAgvWorkFail").NewValue.ToBool()
                  && properties.Property("IsCharging").NewValue.ToBool())
                  .TriggerAlways(() =>
                  {
                      System.Diagnostics.Debug.WriteLine("DeviceStatus.Charging");
                      //InteractingDevice.Status = DeviceStatus.Charging;
                      //var request = GetStatusRequest(DeviceStatus.Charging);
                      //_ = DataExporter.DeviceStatusReport(request);
                      ////����PLC���ϲ���Ϣ
                      //UpdatePLcSiloInfo();
                  });

            watchableProperties.Properties("IsConnected", "PlcIsReady", "IsAuto", "IsError", "IsHalt", "CanDispatch", "IsAgvWorkFail")
                  .When(properties => !properties.Property("IsConnected").NewValue.ToBool()
                  || !properties.Property("PlcIsReady").NewValue.ToBool()
                  || !properties.Property("IsAuto").NewValue.ToBool()
                  || properties.Property("IsError").NewValue.ToBool()
                  || properties.Property("IsHalt").NewValue.ToBool()
                  || !properties.Property("CanDispatch").NewValue.ToBool()
                  || properties.Property("IsAgvWorkFail").NewValue.ToBool())
                 .TriggerAlways(() =>
                 {
                     System.Diagnostics.Debug.WriteLine("DeviceStatus.Exception");
                     //logger.LogDebug($"\r\n ��ص��쳣��\r\n" +
                     //    $"CurrentEventTraceId��{InteractingDevice.CurrentEventTraceId}\r\n" +
                     //    $"IsConnected��{WatchingProperties.Property("IsConnected").NewValue.ToBool()}\r\n" +
                     //    $"PlcIsReady��{WatchingProperties.Property("PlcIsReady").NewValue.ToBool()}\r\n" +
                     //    $"IsAuto��{WatchingProperties.Property("IsAuto").NewValue.ToBool()}\r\n" +
                     //    $"IsError��{WatchingProperties.Property("IsError").NewValue.ToBool()}\r\n" +
                     //    $"WaringCode��{WatchingProperties.Property("WaringCode").NewValue.ToStr()}\r\n" +
                     //    $"IsHalt��{WatchingProperties.Property("IsHalt").NewValue.ToBool()}\r\n" +
                     //    $"CanDispatch��{WatchingProperties.Property("CanDispatch").NewValue.ToBool()}\r\n" +
                     //    $"IsAgvWorkFail��{WatchingProperties.Property("IsAgvWorkFail").NewValue.ToBool()} \r\n");
                     //InteractingDevice.Status = DeviceStatus.Exception;
                     //var request = GetStatusRequest(DeviceStatus.Exception);
                     //_ = DataExporter.DeviceStatusReport(request);
                     ////����PLC���ϲ���Ϣ
                     //UpdatePLcSiloInfo();
                 });
        }
    }
}
