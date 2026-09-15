// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Fundation.Drill;
using VgAutoDrill.Infrastructure.Plugin;

namespace VegaIot.External.Bomin
{
    public class BominPluginSetup : IPluginSetupItem
    {
        public PluginSetupDescriptor PluginDescriptor { get; }

        public BominPluginSetup(PluginSetupDescriptor pluginSetupDescriptor)
        {
            PluginDescriptor = pluginSetupDescriptor;
        }

        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is IConfigurationBuilder configurationBuilder)
            {
                var ajwJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.bomin.json");
                configurationBuilder.AddJsonFile(ajwJsonPath, false);
            }

            services.Configure<FtpDrillFilePathOptions>(configuration.GetSection(nameof(FtpDrillFilePathOptions)));
            services.AddSingleton<IDrillFilePathLocator, BominDrillFileLocator>();
        }
    }
}
