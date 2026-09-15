using Microsoft.Extensions.Logging;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Infrastructure;

namespace VgAutoDrill.Central.Core.Calculator.ForkOutside;

internal class ForkOutsideAutoSiloTransferStrategy : IAutoSiloTransferStrategy
{
    private readonly ILogger<ForkOutsideAutoSiloTransferStrategy> _logger;
    private readonly IObjectFactory _objectFactory;
    private readonly PartitionAutoSiloTransferOptions _transferOptions;

    public ForkOutsideAutoSiloTransferStrategy(ILoggerFactory loggerFactory,
        IObjectFactory objectFactory,
        PartitionAutoSiloTransferOptions transferOptions)
    {
        _objectFactory = objectFactory;
        _transferOptions = transferOptions;
        _logger = loggerFactory.CreateLogger<ForkOutsideAutoSiloTransferStrategy>();
    }

    public void ReassignSiloTransferJob()
    {
        if (_transferOptions.EnableDrilledTrackOut)
            StartDrilledTrackOutThread();//熟料:中转位->外部线边仓

        if (_transferOptions.EnableUndrilledTrackIn)
            StartRawTrackInThread();//生料:外部线边仓->中转位

        if (_transferOptions.EnableEmptyBoxTrackIn)
            StartEmptyBoxTrackInThread();//空料仓:外部线边仓->中转位

        if (_transferOptions.EnableEmptyBoxTrackOut)
            StartEmptyBoxTrackOutThread();//空料仓:中转位->外部线边仓

        if (_transferOptions.EnableFirstTrackOut)
            StartFirstTrackOutThread();//首件:中转位->外部退pin线

        if (_transferOptions.EnableUndrilledTrackOut)
            StartUndrilledTrackOutThread();//生料:中转位->外部线边仓
    }

    public Task StartDrilledTrackInThread() => Task.CompletedTask;

    public Task StartDrilledTrackOutThread()
    {
        _logger.LogInformation($"StartDrilledTrackOutThread,partCode:{_transferOptions.PartCode}==========================\r\n");
        var drilledTrackOutCalculator = _objectFactory.GetOrCreate<DrilledFromForkToOutsideUnPinCalculator>();
        return drilledTrackOutCalculator.TryGenerateTransferJob(_transferOptions.PartCode);
    }

    public Task StartEmptyBoxTrackInThread()
    {
        _logger.LogInformation($"StartEmptyBoxTrackInThread,partCode:{_transferOptions.PartCode}==========================\r\n");
        var emptyBoxTrackInCalculator = _objectFactory.GetOrCreate<EmptyBoxFromOutsideToForkCalculator>();
        return emptyBoxTrackInCalculator.TryGenerateTransferJob(_transferOptions.PartCode);
    }

    public Task StartEmptyBoxTrackOutThread()
    {
        _logger.LogInformation($"StartEmptyBoxTrackOutThread,partCode:{_transferOptions.PartCode}==========================\r\n");
        var emptyBoxTrackOutCalculator = _objectFactory.GetOrCreate<EmptyBoxFromForkToOutsideCalculator>();
        return emptyBoxTrackOutCalculator.TryGenerateTransferJob(_transferOptions.PartCode);
    }

    public Task StartFirstTrackOutThread()
    {
        _logger.LogInformation($"StartFirstTrackOutThread,partCode:{_transferOptions.PartCode}==========================\r\n");
        var firstTrackOutCalculator = _objectFactory.GetOrCreate<FirstFromForkToOutsideUnPinCalculator>();
        return firstTrackOutCalculator.TryGenerateTransferJob(_transferOptions.PartCode);
    }

    public Task StartRawTrackInThread()
    {
        _logger.LogInformation($"StartRawTrackInThread,partCode:{_transferOptions.PartCode}==========================\r\n");
        var rawTrackInCalculator = _objectFactory.GetOrCreate<UndrilledFromOutsideToForkCalculator>();
        return rawTrackInCalculator.TryGenerateTransferJob(_transferOptions.PartCode);
    }

    public Task StartUndrilledTrackOutThread()
    {
        _logger.LogInformation($"StartUndrilledTrackOutThread,partCode:{_transferOptions.PartCode}==========================\r\n");
        var rawTrackInCalculator = _objectFactory.GetOrCreate<UndrilledFromForkToOutsideCalculator>();
        return rawTrackInCalculator.TryGenerateTransferJob(_transferOptions.PartCode);
    }
}
