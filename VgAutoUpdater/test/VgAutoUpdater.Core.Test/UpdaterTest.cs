using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using VgAutoUpdater.Core.Configuration;

namespace VgAutoUpdater.Core.Test;

public class UpdaterTest
{
    [Fact]
    public async void CheckPackagesWorks()
    {
        Mock<ILogger> mockLogger = new Mock<ILogger>();
        Mock<ILoggerFactory> mockLoggerFactory = new Mock<ILoggerFactory>();
        mockLoggerFactory.Setup(f => f.CreateLogger(It.IsAny<string>())).Returns(mockLogger.Object);

        Mock<IHttpClientFactory> mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

        var services = new ServiceCollection();
        services.AddSingleton<IScriptRunnerFactory, ScriptRunnerFactory>();
        services.AddSingleton<IUpdater, Updater>();
        services.AddSingleton(mockHttpClientFactory.Object);
        services.AddSingleton(mockLoggerFactory.Object);
        services.AddSingleton(new MockOptions<VgAutoUpdaterOptions>().Use(new VgAutoUpdaterOptions
        {
            VersionUrl = "http://192.168.102.253:5258",
            UpdatesLocation = "D:\\work\\packages",
            InstallationPolicy = UpdatesInstallationPolicy.AgentReadyOrCentralForce
        }).Object);

        var updater = services.BuildServiceProvider().GetService<IUpdater>();
        Assert.NotNull(updater);

        var updateResponse = await updater.CheckPackages("test");
        Assert.Equal("200", updateResponse.Code);

        var notexistappResponse = await updater.CheckPackages("notexistapp");
        Assert.Equal("500", notexistappResponse.Code);
    }
}
