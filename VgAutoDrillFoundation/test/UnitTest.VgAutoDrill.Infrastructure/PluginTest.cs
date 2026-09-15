using Microsoft.Extensions.Options;

namespace UnitTest.VgAutoDrill.Infrastructure;

internal class PluginTest : IPluginTest
{
    private readonly FakePluginSetupOptions _fakePluginSetupOptions;

    public PluginTest(IOptions<FakePluginSetupOptions> options)
    {
        _fakePluginSetupOptions = options.Value;
    }

    public string DoSomething()
    {
        return "PluginTest";
    }
}
