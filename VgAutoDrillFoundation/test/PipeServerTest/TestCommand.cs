using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace PipeServerTest;

internal class TestCommand : SimpleCommand<MockDevice>
{
    private readonly ILogger<TestCommand> _logger;

    public TestCommand(IServiceProvider serviceProvider,
        MockDevice device,
        CommandDescriptor commandDescriptor)
        : base(serviceProvider, device, commandDescriptor)
    {
        _logger = device.LoggerFactory.CreateLogger<TestCommand>();
    }

    public override Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        _logger.LogInformation("TestCommand invoked...");
        return Task.FromResult(new DeviceServiceInvokeResponse { Code = ErrorCodes.Sys.SUCCESS });
    }
}
