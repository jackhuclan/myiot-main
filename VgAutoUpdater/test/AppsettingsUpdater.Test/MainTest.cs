namespace AppsettingsUpdater.Test;

public class MainTest
{
    [Fact]
    public async Task TestCaseSame()
    {
        await Program.Main(new[] { "--file=.\\app\\appsettings.Development.json",
            "--source=http://192.168.102.253:5258/conf/appsettings.Production.json",
            "--data={\"DeviceListOptions:Devices[0]:DeviceId\":\"09333\",\"DeviceListOptions:Devices[0]:Extra:LowBattery\":\"7822\",\"PageMasterDeviceId\":\"master0001\"}" });

        Assert.DoesNotContain("{{", File.ReadAllText(".\\app\\appsettings.Development.json"));
    }

    [Fact]
    public async Task TestCaseSame2()
    {
        await Program.Main(new[] { "--file=E:\\AutoUpdate\\drill\\v1.4.0.0\\conf\\appsettings.Production.json",
            "--source=http://192.168.102.253:5258/conf/appsettings.Production.json",
            "--data={\"PageMasterDeviceId\":\"HSHJ001\",\"DeviceListOptions:Devices[0]:DeviceId\":\"RT-EE0-EE\",\"DeviceListOptions:Devices[0]:HostAddress\":\"http://192.168.2.2:8004\"}" });

        Assert.DoesNotContain("{{", File.ReadAllText("D:\\Vega\\drill\\v1.4.0.0\\conf\\appsettings.Production.json"));
    }

    [Fact]
    public async Task TestCaseSame3()
    {
        await Program.Main(new[] { "--file=E:\\AutoUpdate\\drill\\v1.4.0.0\\conf\\appsettings.Production.json",
            "--source=http://192.168.102.253:5258/conf/appsettings.Production.json",
            "--data=eyJQYWdlTWFzdGVyRGV2aWNlSWQiOiJIU0hKMDAyIiwiRGV2aWNlTGlzdE9wdGlvbnM6RGV2aWNlc1swXTpEZXZpY2VJZCI6IlJULUVFMC1FRSIsIkRldmljZUxpc3RPcHRpb25zOkRldmljZXNbMF06SG9zdEFkZHJlc3MiOiJodHRwOi8vMTkyLjE2OC4yLjI6ODAwNCJ9"});

        Assert.DoesNotContain("{{", File.ReadAllText("D:\\Vega\\drill\\v1.4.0.0\\conf\\appsettings.Production.json"));
    }
    // .\\AppsettingsUpdater\\AppsettingsUpdater.exe --data={\"PageMasterDeviceId\":\"HSHJ001\",\"DeviceListOptions:Devices[0]:DeviceId\":\"RT-EE0-EE\",\"DeviceListOptions:Devices[0]:HostAddress\":\"http://192.168.2.2:8004\"} --file=D:\\Vega\\drill\\v1.4.0.0\\conf\\appsettings.Production.json --source=http://192.168.102.253:5258/conf/appsettings.Production.json
    /// <summary>
    /// case 1: .\\AppsettingsUpdater.exe --file=.\\app\\appsettings.Development.json --section=MqttServerOptions:Host --value=192.168.102.115
    /// </summary>
    [Fact]
    public async Task TestCase1()
    {
        await Program.Main(new[] { "--file=.\\app\\appsettings.Development.json",
            "--section=MqttServerOptions:Host",
            "--value=192.168.102.116" });

        Assert.DoesNotContain("{{", File.ReadAllText(".\\app\\appsettings.Development.json"));
    }

    /// <summary>
    /// case 2: .\\AppsettingsUpdater.exe --file=.\\app\\appsettings.Development.json --source=http://192.168.102.253:5258/conf/agv/appsettings.Development.json
    /// </summary>
    [Fact]
    public async Task TestCase2()
    {
        await Program.Main(new[] { "--file=.\\app\\appsettings.Development.json",
            "--source=http://192.168.102.253:5258/conf/appsettings.Production.json"});

        Assert.DoesNotContain("{{", File.ReadAllText(".\\app\\appsettings.Development.json"));
    }

    /// <summary>
    /// case 3: .\\AppsettingsUpdater.exe --file=.\\app\\appsettings.Development.json --source=http://192.168.102.253:5258/conf/agv/appsettings.Development.json --values=http://192.168.102.253:5258/conf/drill/values.json
    /// </summary>
    [Fact]
    public async Task TestCase3()
    {
        await Program.Main(new[] { "--file=.\\app\\appsettings.Development.json",
            "--source=http://192.168.102.253:5258/conf/appsettings.Production.json"
            ,"--values=http://192.168.102.253:5258/conf/values.json"});

        Assert.DoesNotContain("{{", File.ReadAllText(".\\app\\appsettings.Development.json"));
    }

    [Fact]
    public async Task TestCase3withPlaceholder()
    {
        await Program.Main(new[] { "--file=.\\app\\appsettings.Development.json",
            "--source=http://192.168.102.253:5258/conf/appsettings.Production.json"
            ,"--values=http://192.168.102.253:5258/conf/values.json"
            ,"--PageMasterDeviceId=77777"
            ,"--CentralWebOptions:DeviceConfig=iwuwhwj"});

        Assert.DoesNotContain("{{", File.ReadAllText(".\\app\\appsettings.Development.json"));
    }
    /// <summary>
    /// case 4: .\\AppsettingsUpdater.exe --file=.\\app\\appsettings.Development.json -source=http://192.168.102.253:5258/conf/agv/appsettings.Development.json --placeholder1=value1  --placeholder2=value2
    /// </summary>
    [Fact]
    public async Task TestCase4()
    {
        await Program.Main(new[] { "--file=.\\app\\appsettings.Development.json",
            "--source=http://192.168.102.253:5258/conf/appsettings.Production.json"
            ,"--PageMasterDeviceId=matsr003"});

        Assert.DoesNotContain("{{", File.ReadAllText(".\\app\\appsettings.Development.json"));
        Assert.Contains("matsr003", File.ReadAllText(".\\app\\appsettings.Development.json"));
    }

    [Fact]
    public async Task TestCase5()
    {
        await Program.Main(new[] { "--file=E:\\AutoUpdate\\drill\\v1.1.0.0\\appsettings.Development.json",
            "--source=http://192.168.102.253:5258/conf/appsettings.Production.json"
            ,"--PageMasterDeviceId=GSHJW99090001",
          "--DeviceListOptions:Devices[0]:Extra:Spindles=200",
        "--values=http://192.168.102.253:5258/conf/values.json"});

        Assert.DoesNotContain("{{", File.ReadAllText(".\\app\\appsettings.Development.json"));
        Assert.Contains("GSHJW99090001", File.ReadAllText(".\\app\\appsettings.Development.json"));
    }
}
