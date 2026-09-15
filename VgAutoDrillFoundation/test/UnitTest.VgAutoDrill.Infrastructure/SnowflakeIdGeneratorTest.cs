using VgAutoDrill.Infrastructure;

namespace UnitTest.VgAutoDrill.Infrastructure;

public class SnowflakeIdGeneratorTest
{
    [Fact]
    public void NextIdShouldGreaterThanPreviousId()
    {
        for (int i = 0; i < 1000; i++)
        {
            var id1 = SnowflakeIdGenerator.nextId();
            var id2 = SnowflakeIdGenerator.nextId();
            Assert.True(id2 > id1);
        }
    }
}
