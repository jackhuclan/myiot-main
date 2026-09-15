using VgAutoDrill.Central.Core.Mysql;

namespace VgAutoDrill.Central.Core.Calculator;

public interface IAutoSiloTransferStrategyFactory
{
    IAutoSiloTransferStrategy Create(PartitionAutoSiloTransferOptions transferOptions);
}
