using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;

namespace VgDeviceGateway.Devices.Drill.StateHandler
{
    public class FrontToolDrillStateHandler : DeviceShare<DefaultDrill>, IDrillStateHandler
    {
        private readonly ILogger<FrontToolDrillStateHandler> logger;

        public FrontToolDrillStateHandler(ILogger<FrontToolDrillStateHandler> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
        }

        public bool SetExceptionStateCondition(WatchableProperties properties)
        {
            return false;
        }

        public bool SetWorkingStateCondition(WatchableProperties properties)
        {
            return false;
        }

        public bool SetReadyStateCondition(WatchableProperties properties)
        {
            return false;
        }

        public List<string> SetReadyProperties()
        {
            return new List<string>()
            {
            };
        }

        public List<string> SetWorkProperties()
        {
            return new List<string>()
            {
            };
        }

        public List<string> SetExceptionProperties()
        {
            return new List<string>()
            {
            };
        }
    }
}
