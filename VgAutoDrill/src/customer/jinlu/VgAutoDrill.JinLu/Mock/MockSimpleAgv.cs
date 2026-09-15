using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Mqtt.Client;

namespace VgAutoDrill.JinLu.Mock
{
    public class MockSimpleAgv : MockBaseVehicle<MockSimpleAgv>
    {
        private readonly IHttpRequestInvoker? httpRequestInvoker;
        private readonly DeviceDescriptor deviceDescriptor;
        private readonly IDeviceProvider deviceProvider;
        private readonly ILogger<MockSimpleAgv> logger;
        private readonly ILoggerFactory loggerFactory;
        private readonly IMqttClientWrapper mqttClientWrapper;
        private readonly int postAndGetTimeout;
        private CentralWebOptions centralWebOptions;
        private Uri Uri { get; set; }

        private byte SlaveId { get; set; }

        private bool IsConnected;

        private bool IsUseable;

        //private ModbusIpMaster modbusIpMaster;

        private Dictionary<string, object> configExtra;

        private Dictionary<string, object> AxisMap = new Dictionary<string, object>() {
            { "00007",1},
            { "00008",2},
            { "00009",3},
            { "00010",4},
            { "00011",5},
            { "00012",6}
        };

        public MockSimpleAgv(IHttpRequestInvoker httpRequestInvoker,
            IDeviceEngine deviceEngine,
            IMqttClientWrapper mqttClientWrapper,
            DeviceDescriptor deviceDescriptor,
            IOptions<CentralWebOptions> options,
            ILoggerFactory loggerFactory)
            : base(httpRequestInvoker, deviceEngine, mqttClientWrapper, deviceDescriptor, options, loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            centralWebOptions = options.Value;
            httpRequestInvoker = httpRequestInvoker;
            this.deviceDescriptor = deviceDescriptor;
            this.deviceProvider = deviceProvider;
            logger = loggerFactory.CreateLogger<MockSimpleAgv>();


            this.Connector.ConnectFunc = async (deviceDescriptor) =>
            {
                await Task.Run(() =>
                {
                    return true;
                });
                return true;
            };

            CollectDataFunc = (device) =>
            {

            };
        }
    }
}