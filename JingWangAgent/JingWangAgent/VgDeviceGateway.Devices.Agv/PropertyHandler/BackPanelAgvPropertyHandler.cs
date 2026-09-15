using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common.Agv;

namespace VgDeviceGateway.Devices.Agv.PropertyHandler
{
    public class BackPanelAgvPropertyHandler : DeviceShare<DefaultAgv>, IAgvPropertyHandler
    {
        private readonly ILogger<BackPanelAgvPropertyHandler> logger;

        public BackPanelAgvPropertyHandler(ILogger<BackPanelAgvPropertyHandler> logger, IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
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
             .AddProperty("IsExistSilo", false)
             .AddProperty("CheckOnline", true)
             .AddProperty("CheckReady", true)
             .AddProperty("CheckWorking", true)
             .AddProperty("AgvTaskId", "")
             .AddProperty("AgvMoveStartTime", "")
             .AddProperty("AgvMoveArrivedTime", "");
            //IsWorkingProperty = WatchingProperties.Property("IsWorking");
            //IsWorkingProperty.SetValue(true);
            //IsWorkingProperty.NewValue.ToBool();
        }

        public Dictionary<string, object?> PlcInformation()
        {
            return GetPLCInfo();
        }

        private DateTime dt = DateTime.Now;
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
                dictionary["IsLowBattery"] = await InteractingDevice.defaultAgvChassis.IsLowBatteryLocal(InteractingDevice);// CheckLowBattery(carInfo);
                dictionary["AgvStatus"] = carInfo.Status.ToStr();
                dictionary["CanDispatch"] = await InteractingDevice.defaultAgvChassis.CanDispatchLocal(InteractingDevice);// CheckCanDispatch(carInfo);

                dt = DateTime.Now;
            }
            else
            {
                if (dt.AddSeconds(30) < DateTime.Now)
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
            }
             
            return dictionary;
        }

        private object CheckCanDispatch(CarModel carInfo)
        {
            var canDispatch = carInfo.CanDispatch.ToBool();
            if (canDispatch)
            {
                return true;
            }
            return false;
        }

        private object CheckLowBattery(CarModel carInfo)
        {
            var configLowBattery = InteractingDevice.DeviceDescriptor.Extra["LowBattery"].ToFloat();
            var battery = carInfo.Battery.ToFloat();
            if (battery < configLowBattery)
            {
                return true;
            }
            return false;
        }

        private Dictionary<string, object> GetPLCInfo()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            try
            {
                ushort[] status = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(InteractingDevice.slaveId, 4001, 4);
                dictionary["PlcIsReady"] = status[0] == 1;
                dictionary["IsAuto"] = status[1] == 2;
                dictionary["IsError"] = status[2] == 1;
                dictionary["IsHalt"] = status[3] == 1;
                ushort[] ushorts = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(InteractingDevice.slaveId, 4026, 1);
                dictionary["WarningCode"] = ushorts[0];

                ushort[] ushortExist = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(InteractingDevice.slaveId, 4066, 1);
                dictionary["IsExistSilo"] = ushortExist[0] == 1 ? true : false;

                ushort[] ushorts4000 = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(InteractingDevice.slaveId, 4000, 100);
                ushort[] ushorts5000 = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(InteractingDevice.slaveId, 5000, 100);
                var str4000 = "4000开始的信号：";
                var str5000 = "5000开始的信号：";
                dictionary["PlcRealTime4000"] = str4000 + string.Join('_', ushorts4000);
                dictionary["PlcRealTime5000"] = str5000 + string.Join('_', ushorts5000);

                var str4200 = $"第{InteractingDevice.spindlePosition}轴上料步骤完成情况监控：";
                dictionary["PlcRealTime4200"] = string.Join('_', str4200);

                var str4300 = $"第{InteractingDevice.spindlePosition}轴下料步骤完成情况监控：";
                dictionary["PlcRealTime4300"] = string.Join('_', str4300);
                return dictionary;
            }
            catch (Exception ex)
            {
                dictionary["PlcIsReady"] = false;
                dictionary["IsAuto"] = false;
                dictionary["IsError"] = false;
                dictionary["IsHalt"] = false;
                dictionary["WarningCode"] = "";
                dictionary["PlcRealTime4000"] = "";
                dictionary["PlcRealTime5000"] = "";
                dictionary["PlcRealTime4200"] = "";
                dictionary["PlcRealTime4300"] = "";
                dictionary["IsExistSilo"] = false;
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
