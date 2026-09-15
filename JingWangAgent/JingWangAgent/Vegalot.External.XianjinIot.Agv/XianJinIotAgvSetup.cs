// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vegalot.External.XianjinIot.Agv.Command;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Infrastructure.Plugin;

namespace Vegalot.External.XianjinIot.Agv
{
    internal class XianJinIotAgvSetup : IPluginSetupItem
    {
        public XianJinIotAgvSetup(PluginSetupDescriptor pluginSetupDescriptor)
        {
            PluginDescriptor = pluginSetupDescriptor;
        }

        public PluginSetupDescriptor PluginDescriptor { get; }

        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is IConfigurationBuilder configurationBuilder)
            {
                var agvJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.XianJinIotAgv.json");
                configurationBuilder.AddJsonFile(agvJsonPath, false);
            }

            services.Configure<XianJinIotAgvOptions>(configuration.GetSection(nameof(XianJinIotAgvOptions)));
            services.AddSingleton<IClientMqttListener, XianJinIotAgvClientMqttListener>();
            services.AddSingleton<IClientMqttApplicationMessageListener, XianJinIotAgvClientMqttApplicationMessageListener>();
        }
    }
}
