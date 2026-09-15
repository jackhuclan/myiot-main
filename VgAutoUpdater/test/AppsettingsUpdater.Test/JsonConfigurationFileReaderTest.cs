namespace AppsettingsUpdater.Test;

public class JsonConfigurationFileReaderTest
{
    [Fact]
    public void ReadSteamShouldWork()
    {
        using var fs = new FileStream(".\\app\\appsettings.Development.json", FileMode.Open);
        var data = JsonConfigurationFileReader.Parse(fs);
        Assert.True(data.Count > 0);
    }
}
