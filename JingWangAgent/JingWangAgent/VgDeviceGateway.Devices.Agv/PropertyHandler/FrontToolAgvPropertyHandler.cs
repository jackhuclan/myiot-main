using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common.Agv;

namespace VgDeviceGateway.Devices.Agv.PropertyHandler
{
    public class FrontToolAgvPropertyHandler : DeviceShare<DefaultAgv>, IAgvPropertyHandler
    {
        private readonly ILogger<FrontToolAgvPropertyHandler> logger;

        public FrontToolAgvPropertyHandler(ILogger<FrontToolAgvPropertyHandler> logger, IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
        {
            this.logger = logger;
        }

        public void AddWatchingProperties()
        {
            WatchingProperties
             .AddProperty("MqttConnected", false)
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
             .AddProperty("AgvReturnTaskId", "0")//这个很重要
             .AddProperty("CarCurrentPos", "")
             .AddProperty("CarTargetPos", "")
             .AddProperty("MoveTargetPos", "")
             .AddProperty("IsMoving", false)
             .AddProperty("IsArrived", false)
             .AddProperty("AgvScanResult", true)
             .AddProperty("Offset_X", 0)
             .AddProperty("Offset_Y", 0)
             .AddProperty("Offset_Z", 0)
             .AddProperty("WarningCode", 0)
             .AddProperty("PrepareLoadOk", false)
             .AddProperty("InvokeLoadOk", false)
             .AddProperty("CompleteLoadOk", false)
             .AddProperty("PrepareUnloadOk", false)
             .AddProperty("InvokeUnloadOk", false)
             .AddProperty("CompleteUnloadOk", false)
             .AddProperty("AgvChassisErrorMessage", false)
             .AddProperty("PlcRealTime4200", "")
             .AddProperty("PlcRealTime4300", "")
             .AddProperty("PlcRealTime4000", "")
             .AddProperty("PlcRealTime5000", "")
             .AddProperty("IsExistSilo", true)
             .AddProperty("CheckOnline", true)
             .AddProperty("CheckReady", true)
             .AddProperty("CheckWorking", true);

            //IsWorkingProperty = WatchingProperties.Property("IsWorking");
            //IsWorkingProperty.SetValue(true);
            //IsWorkingProperty.NewValue.ToBool();
        }

        public Dictionary<string, object?> PlcInformation()
        {
            return GetPLCInfo();
        }

        public async Task<Dictionary<string, object?>> AgvChassisInformation()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            var getinfoResponse = await InteractingDevice.defaultAgvChassis.GetAgvInfoLocal(InteractingDevice);
            var carInfo = getinfoResponse.Params["CarInfoResponse"] as CarModel;
            if (carInfo != null)
            {
                dictionary["CarCurrentPos"] = carInfo.CurrentStation.ToStr();
                dictionary["CarTargetPos"] = carInfo.TargetStation.ToStr();
                dictionary["Battery"] = carInfo.Battery.ToFloat();
                dictionary["IsAgvLowBattery"] = carInfo.IsLowBattery.ToBool();
                dictionary["IsLowBattery"] = InteractingDevice.defaultAgvChassis.IsLowBatteryLocal(InteractingDevice).Result;
                dictionary["AgvStatus"] = carInfo.Status.ToStr();
                dictionary["CanDispatch"] = InteractingDevice.defaultAgvChassis.CanDispatchLocal(InteractingDevice).Result;
            }
            else
            {
                dictionary["CarCurrentPos"] = "";
                dictionary["CarTargetPos"] = "";
                dictionary["Battery"] = "";
                dictionary["IsAgvLowBattery"] = false;
                dictionary["IsLowBattery"] = false;
                dictionary["AgvStatus"] = "";
                dictionary["CanDispatch"] = false;
                logger.LogDebug($"CollectRealProperties_ArrivedInfo_GetCarInfoNull");
                logger.LogError($"CollectRealProperties_ArrivedInfo_GetCarInfoNull");
            }
            return dictionary;
        }

        private Dictionary<string, object> GetPLCInfo()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            try
            {
                ushort[] status = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(InteractingDevice.slaveId, 3501, 4);
                dictionary["PlcIsReady"] = status[0] == 1;
                dictionary["IsAuto"] = status[1] == 2;
                dictionary["IsError"] = status[2] == 1;
                dictionary["IsHalt"] = status[3] == 1;
                ushort[] ushorts = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(InteractingDevice.slaveId, 3514, 1);
                dictionary["WarningCode"] = ushorts[0];
                dictionary["IsExistSilo"] = true;
                dictionary["PlcRealTime4000"] = "";
                dictionary["PlcRealTime5000"] = "";

                ushort[] step3601 = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(InteractingDevice.slaveId, 3601, 4);
                dictionary["PlcRealTime4200"] = $"第{InteractingDevice.spindlePosition}轴上料步骤:{step3601[0]},是否完成：{step3601[1]}";
                dictionary["PlcRealTime4300"] = $"第{InteractingDevice.spindlePosition}轴下料步骤:{step3601[2]},是否完成：{step3601[3]}";
                return dictionary;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"PLC异常：{ex.Message}");
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_GetPLCInfo_Exception", "Exception", ex.Message);
                return dictionary;
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }
    }
}
