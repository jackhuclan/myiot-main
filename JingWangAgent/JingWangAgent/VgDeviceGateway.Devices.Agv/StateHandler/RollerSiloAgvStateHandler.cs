using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Agv.StateHandler
{
    public class RollerSiloAgvStateHandler : DeviceShare<DefaultAgv>, IAgvStateHandler
    {
        private readonly ILogger<RollerSiloAgvStateHandler> logger;

        public RollerSiloAgvStateHandler(ILogger<RollerSiloAgvStateHandler> logger, IServiceProvider serviceProvider, DefaultAgv device) : base(serviceProvider, device)
        {
            this.logger = logger;
        }

        public List<string> SetPropertiesOnline()
        {
            return new List<string>()
            {
               "IsConnected",
                "IsAuto",
                "PlcIsReady",
                "IsError",
                "IsHalt",
                "IsAgvWorkFail",
                "CanDispatch",
                "IsLowBattery",
                "IsCharging",
                "IsWorking",
                "AgvIsReady",
                "CheckOnline"
            };
        }

        public Predicate<WatchableProperties> StateOnlineCondition { get; } = properties =>
            properties.Property("IsConnected").NewValue.ToBool()
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
                && properties.Property("CheckOnline").NewValue.ToBool();

        public List<string> SetPropertiesReady()
        {
            return new List<string>()
            {
                "IsConnected",
                "IsAuto",
                "PlcIsReady",
                "IsError",
                "IsHalt",
                "IsAgvWorkFail",
                "CanDispatch",
                "IsLowBattery",
                "IsCharging",
                "IsWorking",
                "AgvIsReady",
                "CheckReady" };
        }

        public bool SetStateConditionReady(WatchableProperties properties)
        {
            return properties.Property("IsConnected").NewValue.ToBool()
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
                  && properties.Property("CheckReady").NewValue.ToBool();
        }

        public List<string> SetPropertiesWorking()
        {
            return new List<string>()
            {
                "IsConnected",
                "IsAuto",
                "PlcIsReady",
                "IsError",
                "IsHalt",
                "IsAgvWorkFail",
                "CanDispatch",
                "IsLowBattery",
                "IsCharging",
                "IsWorking",
                "AgvIsReady",
                "CheckWorking"
            };
        }

        public bool SetStateConditionWorking(WatchableProperties properties)
        {
            return properties.Property("IsConnected").NewValue.ToBool()
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
                   && properties.Property("CheckWorking").NewValue.ToBool();
        }

        public List<string> SetPropertiesLowBattery()
        {
            return new List<string>()
            {
                "IsConnected",
                "IsAuto",
                "PlcIsReady",
                "IsError",
                "IsHalt",
                "CanDispatch",
                "IsAgvWorkFail",
                "IsLowBattery",
                "IsCharging"
            };
        }

        public bool SetStateConditionLowBattery(WatchableProperties properties)
        {
            return properties.Property("IsConnected").NewValue.ToBool()
                   && properties.Property("IsAuto").NewValue.ToBool()
                   && properties.Property("PlcIsReady").NewValue.ToBool()
                   && !properties.Property("IsError").NewValue.ToBool()
                   && !properties.Property("IsHalt").NewValue.ToBool()
                   && properties.Property("CanDispatch").NewValue.ToBool()
                   && !properties.Property("IsAgvWorkFail").NewValue.ToBool()
                   && properties.Property("IsLowBattery").NewValue.ToBool()
                   && !properties.Property("IsCharging").NewValue.ToBool();
        }

        public List<string> SetPropertiesCharging()
        {
            return new List<string>()
            {
               "IsConnected", "IsAuto", "PlcIsReady", "IsError", "IsHalt", "IsAgvWorkFail", "CanDispatch", "IsCharging"
            };
        }

        public bool SetStateConditionCharging(WatchableProperties properties)
        {
            return properties.Property("IsConnected").NewValue.ToBool()
                   && properties.Property("IsAuto").NewValue.ToBool()
                   && properties.Property("PlcIsReady").NewValue.ToBool()
                   && !properties.Property("IsError").NewValue.ToBool()
                   && !properties.Property("IsHalt").NewValue.ToBool()
                   && properties.Property("CanDispatch").NewValue.ToBool()
                   && !properties.Property("IsAgvWorkFail").NewValue.ToBool()
                   && properties.Property("IsCharging").NewValue.ToBool();
        }

        public List<string> SetPropertiesException()
        {
            return new List<string>()
            {
              "IsConnected", "PlcIsReady", "IsAuto", "IsError", "IsHalt", "CanDispatch", "IsAgvWorkFail"
            };
        }

        public bool SetStateConditionException(WatchableProperties properties)
        {
            return !properties.Property("IsConnected").NewValue.ToBool()
                    || !properties.Property("PlcIsReady").NewValue.ToBool()
                    || !properties.Property("IsAuto").NewValue.ToBool()
                    || properties.Property("IsError").NewValue.ToBool()
                    || properties.Property("IsHalt").NewValue.ToBool()
                    || !properties.Property("CanDispatch").NewValue.ToBool()
                    || properties.Property("IsAgvWorkFail").NewValue.ToBool();
        }

        public void UpdatePLcSiloInfo()
        {
            try
            {
                ushort[] valuedata = new ushort[InteractingDevice.PayloadPanels.Count];
                for (int i = 0; i < InteractingDevice.PayloadPanels.Count; i++)
                {
                    //PLC料仓信息
                    var panelType = 0;
                    var panelinfo = InteractingDevice.PayloadPanels[i];
                    if (panelinfo != null && panelinfo.ProductStatus == ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1)
                    {
                        panelType = 1;
                    }
                    else if (panelinfo != null && panelinfo.ProductStatus == ProductStatus.PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1)
                    {
                        panelType = 2;
                    }
                    else
                    {
                        panelType = 3;
                    }
                    valuedata[i] = panelType.ToUshort();
                }
                InteractingDevice.modbusIpMaster.WriteMultipleRegisters(InteractingDevice.slaveId, 5030, valuedata);
            }
            catch (Exception ee)
            {
                logger.LogError($"PLC断线_UpdatePLcSiloInfo_：{ee.Message}");
            }
        }

        public void UpdatePLcError()
        {
            logger.LogDebug($"UpdatePLcError：方法未实现");
        }

        public void UpdatePLcAgvStatus(DeviceStatus newStatus)
        {
            logger.LogDebug($"UpdatePLcAgvStatus：方法未实现");
        }
    }
}
