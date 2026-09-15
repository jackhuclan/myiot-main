namespace VgAutoUpdater.Core;

public interface IScriptRunnerFactory
{
    ScriptRunner Create(params object[] parameters);
}
