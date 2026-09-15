namespace AppsettingsUpdater.Test;

public class JsonConfigurationFileReplacerTest
{
    [Fact]
    public void ReplaceValuesShouldWork()
    {
        var valuesStream = new FileStream(".\\app\\values.json", FileMode.Open);
        var data = JsonConfigurationFileReader.Parse(valuesStream);

        var parser = new JsonConfigurationFileReplacer();
        var replacedJson = parser.Replace(File.ReadAllText(".\\app\\appsettings.Development.json"), data);

        Assert.Contains("myHost22", replacedJson);
        Assert.Contains("myProductId", replacedJson);
        Assert.Contains("myDeviceId", replacedJson);
        Assert.Contains("myInputCapabilities", replacedJson);
        Assert.Contains("1381", replacedJson);
        Assert.Contains("F:\\\\W\\\\L\\\\", replacedJson);
        Assert.Contains("myKeepingDeviceConnectionJob", replacedJson);
    }

    [Fact]
    public void ReplaceShouldWork()
    {
        var data = new Dictionary<string, string?>();
        data.Add("MqttServerOptions:Host", "myHost22");
        data.Add("DeviceListOptions:Devices[0]:ProductId", "myProductId22");
        data.Add("DeviceListOptions:Devices[0]:DeviceId", "myDeviceId");
        data.Add("DeviceListOptions:Devices[0]:InputCapabilities[1]", "myInputCapabilities");
        data.Add("DeviceListOptions:Devices[0]:Extra:ReadCodeLength", "1381");
        data.Add("$['Microsoft.AspNetCore']", "muti");
        data.Add("CentralWebOptions:DeviceConfig", "muti");
        data.Add("Logging:LogLevel['Microsoft.AspNetCore']", "fffff");
        data.Add("DeviceListOptions:Devices[0]:Extra:AgvToken", "F:\\W\\L\\");
        data.Add("Logging:Console:LogLevel['VgAutoDrill.Fundation.Iot.Jobs.KeepingDeviceConnectionJob']", "myKeepingDeviceConnectionJob");

        var parser = new JsonConfigurationFileReplacer();
        var replacedJson = parser.Replace(File.ReadAllText(".\\app\\appsettings.Development.json"), data);
        File.WriteAllText(".\\app\\appsettings.Development.json", replacedJson);
        Assert.Contains("myHost22", replacedJson);
        Assert.Contains("myProductId", replacedJson);
        Assert.Contains("myDeviceId", replacedJson);
        Assert.Contains("myInputCapabilities", replacedJson);
        Assert.Contains("1381", replacedJson);
        Assert.Contains("F:\\\\W\\\\L\\\\", replacedJson);
        Assert.Contains("myKeepingDeviceConnectionJob", replacedJson);
    }

    [Fact]
    public void ReplaceOneLevelWithNoArray()
    {
        var data = new Dictionary<string, string?>();
        data.Add("PageMasterDeviceId", "master009");
        var parser = new JsonConfigurationFileReplacer();
        var replacedJson = parser.Replace(File.ReadAllText(".\\app\\appsettings.Development.json"), data);
        Assert.Contains("master009", replacedJson);
    }

    [Fact]
    public void ReplaceThreeLevelWithNoArray()
    {
        var data = new Dictionary<string, string?>();
        data.Add("Logging:Console:LogLevel:Default", "WarnDD");
        var parser = new JsonConfigurationFileReplacer();
        var replacedJson = parser.Replace(File.ReadAllText(".\\app\\appsettings.Development.json"), data);
        Assert.Contains("WarnDD", replacedJson);
    }
    [Fact]
    public void ReplaceThreeLevelWithPoint()
    {
        var data = new Dictionary<string, string?>();
        data.Add("Logging:Console:LogLevel['Microsoft.AspNetCore']", "Error11");
        data.Add("Logging:Console:LogLevel['VgAutoDrill.Fundation.Iot.Jobs.KeepingDeviceConnectionJob']", "Error12");
        var parser = new JsonConfigurationFileReplacer();
        var replacedJson = parser.Replace(File.ReadAllText(".\\app\\appsettings.Development.json"), data);
        File.WriteAllText(".\\app\\appsettings.Development.json", replacedJson);
        Assert.Contains("Error11", replacedJson);
        Assert.Contains("Error12", replacedJson);
    }

    [Fact]
    public void ReplaceValuesByFilePath()
    {
        var valuesStream = new FileStream(".\\app\\values.json", FileMode.Open);
        var data = JsonConfigurationFileReader.Parse(valuesStream);

        var parser = new JsonConfigurationFileReplacer();
        var replacedJson = parser.Replace(File.ReadAllText(".\\app\\appsettings.Development.json"), data);
        File.WriteAllText(".\\app\\appsettings.Development.json", replacedJson);
        Assert.Contains("myHost22", replacedJson);
        Assert.Contains("myProductId", replacedJson);
        Assert.Contains("myDeviceId", replacedJson);
        Assert.Contains("myInputCapabilities", replacedJson);
        Assert.Contains("1381", replacedJson);
        Assert.Contains("F:\\\\W\\\\L\\\\", replacedJson);
        Assert.Contains("myKeepingDeviceConnectionJob", replacedJson);
    }

    [Fact]
    public void ReplaceValuesByAbsolutePath()
    {
        var valuesStream = new FileStream(".\\app\\values.json", FileMode.Open);
        var data = JsonConfigurationFileReader.Parse(valuesStream);

        var parser = new JsonConfigurationFileReplacer();
        var replacedJson = parser.Replace(File.ReadAllText(".\\app\\appsettings.Development.json"), data);
        File.WriteAllText(".\\app\\appsettings.Development.json", replacedJson);
        Assert.Contains("myHost22", replacedJson);
        Assert.Contains("myProductId", replacedJson);
        Assert.Contains("myDeviceId", replacedJson);
        Assert.Contains("myInputCapabilities", replacedJson);
        Assert.Contains("1381", replacedJson);
        Assert.Contains("F:\\\\W\\\\L\\\\", replacedJson);
        Assert.Contains("myKeepingDeviceConnectionJob", replacedJson);
    }
}
