using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;

namespace VgAutoDrill.Iot.Mock
{
    public class MockPlcConnector : IDeviceConnector
    {
        public bool IsConnected { get; } = false;
        private readonly DeviceDescriptor deviceDescriptor;
        private readonly ILogger<MockPlcConnector> logger;

        public event EventHandler<PlcConnectedChangedEventArgs> OnPlcConnectedChanged;

        public MockPlcConnector(DeviceDescriptor deviceDescriptor,
            ILoggerFactory loggerFactory)
        {
            this.deviceDescriptor = deviceDescriptor;
            logger = loggerFactory.CreateLogger<MockPlcConnector>();
        }

        //todo connect plc
        public Task Connect(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }

        public Task Connect(Func<DeviceDescriptor, Task<bool>> connectFunc, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
