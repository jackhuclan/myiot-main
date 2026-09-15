using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;

namespace PipeServerTest;

internal class MockDevice : Device
{
    public MockDevice(DeviceDescriptor deviceDescriptor,
        IDeviceEngine deviceEngine,
        IServiceProvider serviceProvider)
        : base(deviceDescriptor, deviceEngine, serviceProvider)
    {
    }

    protected override void ConfigureServiceCapabilities()
    {
        base.ConfigureServiceCapabilities();
        var testCommand = ObjectFactory.GetOrCreate<TestCommand>(this, new CommandDescriptor(
            "testCommand",
            "a/b/testCommand",
            CommandUsageKind.Pipe
            ));
        var postCommand = ObjectFactory.GetOrCreate<PostCommand>(this, new CommandDescriptor(
            "postCommand",
            "a/b/postCommand",
            CommandUsageKind.Pipe
            ));

        AddCommand(testCommand);
        AddCommand(postCommand);
    }
}
