using VgAutoDrill.Central.Core.Domain;

namespace VgAutoDrill.Central.Core.Manager;

internal interface ITransferJobListener
{
    Task OnStatusChanged(TransferJob transferJob);
}
