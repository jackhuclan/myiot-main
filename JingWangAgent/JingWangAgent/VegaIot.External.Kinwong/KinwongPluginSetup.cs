// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Fundation.Drill;
using VgAutoDrill.Infrastructure.Plugin;

namespace VegaIot.External.Kinwong;

public class KinwongPluginSetup : IPluginSetupItem
{
    public KinwongPluginSetup(PluginSetupDescriptor pluginSetupDescriptor)
    {
        PluginDescriptor = pluginSetupDescriptor;
    }

    public PluginSetupDescriptor PluginDescriptor { get; }

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration is IConfigurationBuilder configurationBuilder)
        {
            var ajwJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.kinwong.json");
            configurationBuilder.AddJsonFile(ajwJsonPath, false);
        }

        services.Configure<DrillFilePathOptions>(configuration.GetSection(nameof(DrillFilePathOptions)));
        services.Configure<KinwongDatabaseItemcode>(configuration.GetSection(nameof(KinwongDatabaseItemcode)));
        services.AddSingleton<IDrillFilePathLocator, KinwongDrillFileLocator>();
        services.AddSingleton<IDrillFileLoadResult, KinwongLoadFileResult>();
        services.AddSingleton<IDrillLoadPanelSNToDB, KinwongInsertQRCodeToDB>();
    }
}
