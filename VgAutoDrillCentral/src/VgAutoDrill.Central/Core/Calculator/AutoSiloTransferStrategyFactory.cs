using VgAutoDrill.Central.Core.Calculator.ForkOutside;
using VgAutoDrill.Central.Core.Calculator.ForkWip;
using VgAutoDrill.Central.Core.Calculator.WipOutside;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Infrastructure;

namespace VgAutoDrill.Central.Core.Calculator;

internal class AutoSiloTransferStrategyFactory : IAutoSiloTransferStrategyFactory
{
    private readonly IObjectFactory _objectFactory;

    public AutoSiloTransferStrategyFactory(IObjectFactory objectFactory)
    {
        _objectFactory = objectFactory;
    }

    public IAutoSiloTransferStrategy Create(PartitionAutoSiloTransferOptions transferOptions)
    {
        switch (transferOptions.TransferMode)
        {
            case AutoSiloTransferMode.ForkOutside:
                return _objectFactory.CreateObject<ForkOutsideAutoSiloTransferStrategy>(transferOptions);

            case AutoSiloTransferMode.WipOutside:
                return _objectFactory.CreateObject<WipOutsideAutoSiloTransferStrategy>(transferOptions);

            case AutoSiloTransferMode.ForkWip:
                return _objectFactory.CreateObject<ForkWipAutoSiloTransferStrategy>(transferOptions);

            default:
                return _objectFactory.CreateObject<NoneAutoSiloTransferStrategy>();
        }
    }
}
