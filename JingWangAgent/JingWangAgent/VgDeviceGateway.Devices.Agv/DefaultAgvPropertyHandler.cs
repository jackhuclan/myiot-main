using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Property;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Agv.PropertyHandler;

namespace VgDeviceGateway.Devices.Agv
{
    public class DefaultAgvPropertyHandler : AbstractPropertyHandler<DefaultAgv>
    {
        private IAgvPropertyHandler agvPropertyHandler;

        public DefaultAgvPropertyHandler(
            IServiceProvider serviceProvider,
            DefaultAgv defaultagv)
            : base(serviceProvider, defaultagv)
        {
            agvPropertyHandler = InteractionAgvFactory.CreateAgvPropertyHandler(InteractingDevice);

            PeriodicTimers["20s"]!.OnTick += async () =>
            {
                var mergeInf = WatchingProperties.GetValues();
                var request = new DevicePropertiesReportRequest()
                {
                    DeviceId = DeviceDescriptor.DeviceId,
                    ProductId = DeviceDescriptor.ProductId,
                    Params = mergeInf,
                };
                await DataExporter.DevicePropertiesReport(request);
            };
        }

        public override void AddWatchingProperties()
        {
            agvPropertyHandler.AddWatchingProperties();
        }

        public override async void CollectPropertyValues()
        {
            try
            {
                var realProperties = await CollectRealProperties();
                WatchingProperties.SetValues(realProperties);
                //var allstr = "";
                //foreach (var item in realProperties)
                //{
                //    allstr += $"{item.Key}:{item.Value}";
                //}
            }
            catch (Exception)
            {
                InteractingDevice.logger.LogError("CollectPropertyValues 异常");
            }
        }

        private async Task<Dictionary<string, object?>> CollectRealProperties()
        {
            var realProperties = new Dictionary<string, object?>();
            var carProperties = await agvPropertyHandler.AgvChassisInformation();
            if (carProperties != null)
            {
                realProperties["CarCurrentPos"] = carProperties["CarCurrentPos"];
                realProperties["CarTargetPos"] = carProperties["CarTargetPos"];
                realProperties["Battery"] = carProperties["Battery"];
                realProperties["IsAgvLowBattery"] = carProperties["IsAgvLowBattery"];
                realProperties["IsLowBattery"] = carProperties["IsLowBattery"];
                realProperties["AgvStatus"] = carProperties["AgvStatus"];
                realProperties["CanDispatch"] = carProperties["CanDispatch"];
            }
            else
            {
                realProperties["CarCurrentPos"] = "";
                realProperties["CarTargetPos"] = "";
                realProperties["Battery"] = "";
                realProperties["IsAgvLowBattery"] = false;
                realProperties["IsLowBattery"] = false;
                realProperties["AgvStatus"] = "";
                realProperties["CanDispatch"] = false;
            }

            realProperties["AgvScanResult"] = InteractingDevice.AgvScanResult;
            realProperties["IsCharging"] = InteractingDevice.isCharging;
            realProperties["IsAgvWorkFail"] = InteractingDevice.isAgvWorkFail;
            realProperties["IsConnected"] = InteractingDevice.Engine.DeviceConnector.IsConnected;
            realProperties["IsMoving"] = InteractingDevice.isMoving;
            realProperties["IsWorking"] = InteractingDevice.isWorking;
            realProperties["AgvIsReady"] = InteractingDevice.agvIsReady;
            realProperties["IsFullSilo"] = InteractingDevice.isFullSilo ? InteractingDevice.isFullSilo : checFullSilo();
            realProperties["MqttConnected"] = MqttClientWrapper?.IsConnected == null ? false : MqttClientWrapper?.IsConnected;
            realProperties["CheckOnline"] = true;
            realProperties["CheckReady"] = true;
            realProperties["CheckWorking"] = true;
            realProperties["AgvTaskId"] = InteractingDevice.AgvTaskId; ;
            realProperties["AgvMoveStartTime"] = InteractingDevice.AgvMoveStartTime;
            realProperties["AgvMoveArrivedTime"] = InteractingDevice.AgvMoveArrivedTime;

            var arrivedinfo = await InteractingDevice.defaultAgvChassis.ArrivedInfoLocal(InteractingDevice);
            if (arrivedinfo != null)
            {
                realProperties.TryAdd("Offset_X", (arrivedinfo.X_Error.ToFloat() * 1000).ToString("0.00"));
                realProperties.TryAdd("Offset_Y", (arrivedinfo.X_Error.ToFloat() * 1000).ToString("0.00"));
                realProperties.TryAdd("Offset_Z", (arrivedinfo.Z_Error.ToFloat() * 1000).ToString("0.00"));
                realProperties.TryAdd("AgvTaskId", arrivedinfo.TaskId);
                realProperties.TryAdd("IsArrived", InteractingDevice.defaultAgvChassis.CheckIsArrivedLocal(InteractingDevice).Result);
            }
            else
            {
                realProperties.TryAdd("Offset_X", 0);
                realProperties.TryAdd("Offset_Y", 0);
                realProperties.TryAdd("Offset_Z", 0);
                realProperties.TryAdd("AgvTaskId", "");
                realProperties.TryAdd("IsArrived", false);
                InteractingDevice.logger.LogDebug($"CollectRealProperties_ArrivedInfo_Null");
                InteractingDevice.logger.LogError($"CollectRealProperties_ArrivedInfo_Null");
            }
            if (InteractingDevice.Engine.DeviceConnector.IsConnected)
            {
                var plcInfo = agvPropertyHandler.PlcInformation();
                if (plcInfo != null)
                {
                    realProperties["PlcIsReady"] = plcInfo["PlcIsReady"];
                    realProperties["IsAuto"] = plcInfo["IsAuto"];
                    realProperties["IsError"] = plcInfo["IsError"];
                    realProperties["IsHalt"] = plcInfo["IsHalt"];
                    realProperties["WarningCode"] = plcInfo["WarningCode"];
                    realProperties["PlcRealTime4000"] = plcInfo["PlcRealTime4000"];
                    realProperties["PlcRealTime5000"] = plcInfo["PlcRealTime5000"];
                    realProperties["PlcRealTime4200"] = plcInfo["PlcRealTime4200"];
                    realProperties["PlcRealTime4300"] = plcInfo["PlcRealTime4300"];
                    realProperties["IsExistSilo"] = plcInfo["IsExistSilo"];
                }
                else
                {
                    realProperties["PlcIsReady"] = false;
                    realProperties["IsAuto"] = false;
                    realProperties["IsError"] = false;
                    realProperties["IsHalt"] = false;
                    realProperties["WarningCode"] = "";
                    realProperties["PlcRealTime4000"] = "";
                    realProperties["PlcRealTime5000"] = "";
                    realProperties["PlcRealTime4200"] = "";
                    realProperties["PlcRealTime4300"] = "";
                    realProperties["IsExistSilo "] = false;
                    InteractingDevice.logger.LogDebug($"CollectRealProperties_GetPLCInfo_NULL");
                    InteractingDevice.logger.LogError($"CollectRealProperties_GetPLCInfo_NULL");
                }
            }
            else
            {
                realProperties["PlcIsReady"] = false;
                realProperties["IsAuto"] = false;
                realProperties["IsError"] = false;
                realProperties["IsHalt"] = false;
                realProperties["WarningCode"] = "";
                realProperties["PlcRealTime4000"] = "";
                realProperties["PlcRealTime5000"] = "";
                realProperties["PlcRealTime4200"] = "";
                realProperties["PlcRealTime4300"] = "";
                realProperties["IsExistSilo"] = false;
                InteractingDevice.logger.LogDebug($"CollectRealProperties_PLC_UnConnected");
                InteractingDevice.logger.LogError($"CollectRealProperties_PLC_UnConnected");
            }
            return realProperties;
        }

        private bool checFullSilo()
        {
            var full = true;
            if (InteractingDevice.PayloadPanels == null) { return false; }
            foreach (var item in InteractingDevice.PayloadPanels)
            {
                if (item.ProductStatus == ProductStatus.EmptyPayload || item.ProductStatus == ProductStatus.EmptySiloBox)
                {
                    full = false;
                    break;
                }
            }
            return full;
        }
    }
}
