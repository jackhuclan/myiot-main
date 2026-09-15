using VgAutoDrill.Fundation.Utils;

namespace UnitTest.VgAutoDrill.Fundation.Utils;

public class DictionaryExtensionsTest
{
    [Fact]
    public void TestDictionaryExtensionsAdd()
    {
        var dic = new Dictionary<string, object>();
        dic.AddMore("a", 1)
            .AddMore("b", 2);

        Assert.Equal(2, dic.Count);
        Assert.Equal(1, dic["a"]);
        Assert.Equal(2, dic["b"]);
    }
}
