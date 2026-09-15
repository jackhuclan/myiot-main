// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Fundation.Drill;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Infrastructure.Plugin;

namespace Vegalot.External.XianjinIot.Drill
{
    internal class XianjinIotDrillSetup : IPluginSetupItem
    {
        public XianjinIotDrillSetup(PluginSetupDescriptor pluginSetupDescriptor)
        {
            PluginDescriptor = pluginSetupDescriptor;
        }

        public PluginSetupDescriptor PluginDescriptor { get; }

        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is IConfigurationBuilder configurationBuilder)
            {
                var ajwJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.xianjiniotdrill.json");
                configurationBuilder.AddJsonFile(ajwJsonPath, false);
            }

            services.Configure<XianJinIotDrillOptions>(configuration.GetSection(nameof(XianJinIotDrillOptions)));
            services.AddSingleton<IClientMqttListener, XianJinIotDrillClientMqttListener>();
            services.AddSingleton<IClientMqttApplicationMessageListener, XianjinIotDrillClientMqttApplicationMessageListener>();
            services.AddSingleton<IDrillLoadPanelToDrillComplete, XianjinDrillLoadPanelToDrillComplete>();
        }
    }
}
