using Microsoft.Extensions.DependencyInjection;

namespace VgAutoUpdater.Core;

public class ScriptRunnerFactory : IScriptRunnerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ScriptRunnerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ScriptRunner Create(params object[] parameters)
    {
        return ActivatorUtilities.CreateInstance<ScriptRunner>(_serviceProvider, parameters);
    }
}
