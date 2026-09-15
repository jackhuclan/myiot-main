using VgAutoDrill.Fundation.Utils;

namespace UnitTest.VgAutoDrill.Fundation.Utils;

public class StringExtensionsTest
{
    [Theory]
    [InlineData("helloworld", "hello", "world")]
    public void StringTrimStartShouldWork(string source, string trim, string target)
    {
        Assert.Equal(target, source.TrimStart(trim));
    }

    [Theory]
    [InlineData("aaaaaaaaaaaaaaaahelloworld", "a", "helloworld")]
    public void StringRecursiveTrimStartShouldWork(string source, string trim, string target)
    {
        Assert.Equal(target, source.TrimStart(trim, true));
    }

    [Theory]
    [InlineData("helloworld", "world", "helloworld")]
    public void StringTrimEndShouldWork(string source, string trim, string target)
    {
        Assert.Equal(target, source.TrimEnd(trim));
    }

    [Theory]
    [InlineData("helloworldaaaaaaaaaaaaaaaaaaa", "a", "helloworld")]
    public void StringRecursiveTrimEndShouldWork(string source, string trim, string target)
    {
        Assert.Equal(target, source.TrimEnd(trim, true));
    }
}
