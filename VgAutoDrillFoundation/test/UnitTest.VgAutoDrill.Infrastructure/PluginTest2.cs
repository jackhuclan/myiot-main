using Microsoft.Extensions.Options;

namespace UnitTest.VgAutoDrill.Infrastructure;

internal class PluginTest2 : IPluginTest2
{
    private readonly FakePluginSetup2Options _fakePluginSetup2Options;

    public PluginTest2(IOptions<FakePluginSetup2Options> options)
    {
        _fakePluginSetup2Options = options.Value;
    }

    public string DoSomething() { return "PluginTest2"; }
}
