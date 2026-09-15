// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.Infrastructure.Plugin;

namespace VgEAPClient.Socket;

public class EAPClientSocketSetup : IPluginSetupItem
{
    public EAPClientSocketSetup(PluginSetupDescriptor pluginSetupDescriptor)
    {
        PluginDescriptor = pluginSetupDescriptor;
    }

    public PluginSetupDescriptor PluginDescriptor { get; }

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(typeof(IAsyncTaskWaiter<>), typeof(AsyncTaskWaiter<>));
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IEQPSocketDataReporter, SocketDataReporter>());
    }
}
