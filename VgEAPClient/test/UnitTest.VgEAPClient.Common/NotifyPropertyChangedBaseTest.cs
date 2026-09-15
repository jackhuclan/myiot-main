using VgEAPClient.Common.CNC.Common;

namespace UnitTest.VgEAPClient.Common;

public class NotifyPropertyChangedBaseTest
{
    [Fact]
    public void Test()
    {
        var changedProperty = string.Empty;
        var commdataC = new DrillCommonDataC();
        commdataC.PropertyChanged += (obj, args) =>
        {
            changedProperty = args.PropertyName;
        };

        commdataC.PreDuty = "1";
        Assert.Equal("PreDuty", changedProperty);
    }
}
