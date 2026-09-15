namespace UnitTest.VgAutoDrill.Infrastructure;

internal class FakePluginSetupOptions
{
    public string MyProperty { get; set; }

    public TestKind TestKind { get; set; } = TestKind.One;
}

internal enum TestKind
{
    One, Two, Three
}
