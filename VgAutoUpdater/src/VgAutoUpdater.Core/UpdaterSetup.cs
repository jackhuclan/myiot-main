using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoUpdater.Core.Configuration;

namespace VgAutoUpdater.Core;

public static class UpdaterSetup
{
    public static void AddUpdater(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        VgAutoUpdaterOptions? updaterOptions = configuration.GetSection(nameof(VgAutoUpdaterOptions)).Get<VgAutoUpdaterOptions>();
        if (updaterOptions == null)
        {
            ThrowHelper.ThrowArgumentNullException(nameof(updaterOptions));
        }

        services.Configure<VgAutoUpdaterOptions>(configuration.GetSection(nameof(VgAutoUpdaterOptions)));
        services.AddSingleton<IScriptRunnerFactory, ScriptRunnerFactory>();
        services.AddSingleton<IUpdater, Updater>();
        services.AddHostedService(sp => sp.GetRequiredService<IUpdater>());
    }
}
