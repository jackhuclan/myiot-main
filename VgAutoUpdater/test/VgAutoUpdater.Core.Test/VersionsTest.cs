using System.Text.Json;
using VgAutoUpdater.Core.Models;

namespace VgAutoUpdater.Core.Test;

public class VersionsTest
{
    [Fact]
    public void SerializeVersionJsonWorks()
    {
        var versions = new Versions
        {
            Mode = UpdateMode.Upgrade,
            Enable = true,
            CommonScripts = new List<string> {
            "install.bat",
            "uninstall.bat",
            "common/aa.bat"
        },
            AppList = new List<App> {
            new App
            {
                AppName = "test",
                ZipName = "v1.1.0.0.zip",
                ServiceName = "VgAutoUpdaterHub",
                AppVersion = Version.Parse("1.1.0.0"),
                PreScripts = new List<string>{".\\uninstall.bat test VgAutoUpdaterHub v1.1.0.0}"},
                PostScripts = new List<string>{ ".\\install.bat test\\v1.1.0.0\\VgAutoUpdaterHub.exe VgAutoUpdaterHub 5258" }
            },
            new App
            {
                AppName = "test",
                ZipName = "v1.2.0.0.zip",
                ServiceName = "VgAutoUpdaterHub",
                AppVersion = Version.Parse("1.2.0.0"),
                PreScripts = new List<string>{".\\uninstall.bat test VgAutoUpdaterHub v1.1.0.0}"},
                PostScripts = new List<string>{ ".\\install.bat test\\v1.1.0.0\\VgAutoUpdaterHub.exe VgAutoUpdaterHub 5258" }
            },
            new App
            {
                AppName = "test",
                ZipName = "v1.3.0.0.zip",
                ServiceName = "VgAutoUpdaterHub",
                AppVersion = Version.Parse("1.3.0.0"),
                PreScripts = new List<string>{".\\uninstall.bat test VgAutoUpdaterHub v1.3.0.0}"},
                PostScripts = new List<string>{ ".\\install.bat test\\v1.3.0.0\\VgAutoUpdaterHub.exe VgAutoUpdaterHub 5258" }
            },
        }
        };

        string json = JsonSerializer.Serialize(versions, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText("versions.json", json);
        Assert.NotNull(json);
    }

    [Fact]
    public void DeserializeVersionJsonWorks()
    {
        var json = File.ReadAllText("versions.json");
        var versions = JsonSerializer.Deserialize<Versions>(json);
        Assert.NotNull(versions);
        Assert.Equal(3, versions.AppList.Count);
    }

    [Fact]
    public void AppShouldEqual_WhenAppHasSameAppNameAndAppVersion()
    {
        var app1 = new App
        {
            AppName = "test",
            AppVersion = Version.Parse("1.0.0.0")
        };

        var app2 = new App
        {
            AppName = "test",
            AppVersion = Version.Parse("1.0.0.0")
        };

        var list = new List<App>();
        list.Add(app1);

        Assert.Equal(app2, app1);
        Assert.True(list.Contains(app2));
    }
}
