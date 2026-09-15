namespace VgAutoDrill.Central.Core.Calculator;

internal class NoneAutoSiloTransferStrategy : IAutoSiloTransferStrategy
{
    public void ReassignSiloTransferJob()
    { }

    public Task StartDrilledTrackInThread() => Task.CompletedTask;

    public Task StartDrilledTrackOutThread() => Task.CompletedTask;

    public Task StartEmptyBoxTrackInThread() => Task.CompletedTask;

    public Task StartEmptyBoxTrackOutThread() => Task.CompletedTask;

    public Task StartFirstTrackOutThread() => Task.CompletedTask;

    public Task StartRawTrackInThread() => Task.CompletedTask;

    public Task StartUndrilledTrackOutThread() => Task.CompletedTask;
}
