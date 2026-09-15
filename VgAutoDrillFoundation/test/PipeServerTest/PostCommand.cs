using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace PipeServerTest;

internal class PostCommand : SimpleCommand<MockDevice>
{
    private readonly ILogger<TestCommand> _logger;

    public PostCommand(IServiceProvider serviceProvider,
        MockDevice device,
        CommandDescriptor commandDescriptor)
        : base(serviceProvider, device, commandDescriptor)
    {
        _logger = device.LoggerFactory.CreateLogger<TestCommand>();
    }

    public override Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        _logger.LogInformation("PostCommand invoked...");
        return Task.FromResult(new DeviceServiceInvokeResponse { Code = ErrorCodes.Sys.SUCCESS });
    }
}
