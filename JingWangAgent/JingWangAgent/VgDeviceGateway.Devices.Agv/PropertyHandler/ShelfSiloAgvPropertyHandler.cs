using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common.Agv;

namespace VgDeviceGateway.Devices.Agv.PropertyHandler
{
    public class ShelfSiloAgvPropertyHandler : DeviceShare<DefaultAgv>, IAgvPropertyHandler
    {
        private readonly ILogger<ShelfSiloAgvPropertyHandler> logger;

        public ShelfSiloAgvPropertyHandler(ILogger<ShelfSiloAgvPropertyHandler> logger, IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
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
                dictionary["IsLowBattery"] = CheckLowBattery(carInfo);
                dictionary["AgvStatus"] = carInfo.Status.ToStr();
                dictionary["CanDispatch"] = CheckCanDispatch(carInfo);
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
                var isError = false;
                var carinfo = getCarInfoResponse().Result;
                if (carinfo.Alarms != null)
                {
                    foreach (var item in carinfo.Alarms)
                    {
                        switch (item)
                        {
                            case 11002: // 车体急停故障
                                isError = true;
                                break;

                            case 11003: // 车体触边故障
                                isError = true;
                                break;

                            case 12002: // 车体急停故障
                                isError = true;
                                break;

                            case 12003: // 车体触边故障
                                isError = true;
                                break;
                        }
                    }
                }
                else
                {
                    isError = true;
                }

                //var isAuto = false;
                //var carSelfStatus = carinfo.CarSelfStatus;
                //if (carSelfStatus == "Run") {
                //    isAuto = true;
                //}
                //logger.LogDebug("监控到小车状态不是RUN，carSelfStatus：" + carinfo.CarSelfStatus);
                //dictionary["IsAuto"] = isAuto;
                dictionary["IsAuto"] = true;
                dictionary["IsError"] = isError;

                dictionary["PlcIsReady"] = true;
                dictionary["IsHalt"] = false;
                dictionary["WarningCode"] = 0;
                dictionary["PlcRealTime4000"] = "";
                dictionary["PlcRealTime5000"] = "";
                dictionary["PlcRealTime4200"] = "";
                dictionary["PlcRealTime4300"] = "";
                dictionary["IsExistSilo"] = true;
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

        public async Task<CarModel> getCarInfoResponse()
        {
            var getinfoResponse = await InteractingDevice.defaultAgvChassis.GetAgvInfoLocal(InteractingDevice);
            var carInfo = getinfoResponse.Params["CarInfoResponse"] as CarModel;

            if (carInfo == null)
            {
                logger.LogError("ShelfSiloAgvPropertyHandler_getCarInfoResponse:没有查询到车辆信息");
                return new CarModel();
            }
            return carInfo;
        }
    }
}
