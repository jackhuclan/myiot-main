using VgAutoDrill.Central.Core.Domain;

namespace VegaIot.External.Agv.Executor;

public interface ITransferSiloExecutor
{
    Task<bool> Execute(TransferJob job);
}
