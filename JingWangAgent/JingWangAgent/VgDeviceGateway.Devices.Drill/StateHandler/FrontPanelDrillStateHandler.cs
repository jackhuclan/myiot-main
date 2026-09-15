using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Drill.StateHandler
{
    public class FrontPanelDrillStateHandler : DeviceShare<DefaultDrill>, IDrillStateHandler
    {
        private readonly ILogger<FrontPanelDrillStateHandler> logger;

        public FrontPanelDrillStateHandler(ILogger<FrontPanelDrillStateHandler> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
        }

        public bool SetExceptionStateCondition(WatchableProperties properties)
        {
            var isNoErrorOnCnc84 = properties.Property("Drill_NoError").NewValue.ToBool();
            return !isNoErrorOnCnc84;
        }

        public bool SetWorkingStateCondition(WatchableProperties properties)
        {
            var isOnWork = properties.Property("Drill_WorkStart").NewValue.ToBool();
            return isOnWork;
        }

        public bool SetReadyStateCondition(WatchableProperties properties)
        {
            var isOnNoWork = properties.Property("Drill_WorkEnd").NewValue.ToBool();
            return !isOnNoWork;
        }

        public List<string> SetReadyProperties()
        {
            return new List<string>()
            {
               "Drill_WorkEnd"
            };
        }

        public List<string> SetWorkProperties()
        {
            return new List<string>()
            {
                "Drill_WorkStart"
            };
        }

        public List<string> SetExceptionProperties()
        {
            return new List<string>()
            {
               "Drill_NoError"
            };
        }
    }
}
