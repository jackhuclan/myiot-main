using VgAutoDrill.Central.Core.Domain;

namespace VgAutoDrill.Central.Core.Manager;

internal class TransferJobListener : ITransferJobListener
{
    public Task OnStatusChanged(TransferJob transferJob) => Task.CompletedTask;
}
