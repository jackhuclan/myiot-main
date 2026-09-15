using Microsoft.Extensions.Logging;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Infrastructure;

namespace VgAutoDrill.Central.Core.Calculator.WipOutside;

internal class WipOutsideAutoSiloTransferStrategy : IAutoSiloTransferStrategy
{
    private readonly ILogger<WipOutsideAutoSiloTransferStrategy> _logger;
    private readonly IObjectFactory _objectFactory;
    private readonly PartitionAutoSiloTransferOptions _transferOptions;

    public WipOutsideAutoSiloTransferStrategy(ILoggerFactory loggerFactory,
        IObjectFactory objectFactory,
        PartitionAutoSiloTransferOptions transferOptions)
    {
        _objectFactory = objectFactory;
        _transferOptions = transferOptions;
        _logger = loggerFactory.CreateLogger<WipOutsideAutoSiloTransferStrategy>();
    }

    public void ReassignSiloTransferJob()
    {
        if (_transferOptions.EnableDrilledTrackOut)
            StartDrilledTrackOutThread();//熟料:线边仓->外部线边仓

        if (_transferOptions.EnableUndrilledTrackIn)
            StartRawTrackInThread();//生料:外部线边仓->线边仓

        if (_transferOptions.EnableEmptyBoxTrackIn)
            StartEmptyBoxTrackInThread();//空料仓:外部线边仓->线边仓

        if (_transferOptions.EnableEmptyBoxTrackOut)
            StartEmptyBoxTrackOutThread();//空料仓:线边仓->外部线边仓

        if (_transferOptions.EnableFirstTrackOut)
            StartFirstTrackOutThread();//首件:线边仓->外部退pin线
    }

    public Task StartDrilledTrackInThread() => Task.CompletedTask;

    public Task StartDrilledTrackOutThread()
    {
        _logger.LogInformation($"StartDrilledTrackOutThread,partCode:{_transferOptions.PartCode}==========================\r\n");
        var drilledTrackOutCalculator = _objectFactory.GetOrCreate<DrilledFromWipToOutsideUnPinCalculator>();
        return drilledTrackOutCalculator.TryGenerateTransferJob(_transferOptions.PartCode);
    }

    public Task StartEmptyBoxTrackInThread()
    {
        _logger.LogInformation($"StartEmptyBoxTrackInThread,partCode:{_transferOptions.PartCode}==========================\r\n");
        var emptyBoxTrackInCalculator = _objectFactory.GetOrCreate<EmptyBoxFromOutsideToWipCalculator>();
        return emptyBoxTrackInCalculator.TryGenerateTransferJob(_transferOptions.PartCode);
    }

    public Task StartEmptyBoxTrackOutThread()
    {
        _logger.LogInformation($"StartEmptyBoxTrackOutThread,partCode:{_transferOptions.PartCode}==========================\r\n");
        var emptyBoxTrackOutCalculator = _objectFactory.GetOrCreate<EmptyBoxFromWipToOutsideCalculator>();
        return emptyBoxTrackOutCalculator.TryGenerateTransferJob(_transferOptions.PartCode);
    }

    public Task StartFirstTrackOutThread()
    {
        _logger.LogInformation($"StartFirstTrackOutThread,partCode:{_transferOptions.PartCode}==========================\r\n");
        var firstTrackOutCalculator = _objectFactory.GetOrCreate<FirstFromWipToOutsideUnPinCalculator>();
        return firstTrackOutCalculator.TryGenerateTransferJob(_transferOptions.PartCode);
    }

    public Task StartRawTrackInThread()
    {
        _logger.LogInformation($"StartRawTrackInThread,partCode:{_transferOptions.PartCode}==========================\r\n");
        var rawTrackInCalculator = _objectFactory.GetOrCreate<UndrilledFromOutsideToWipCalculator>();
        return rawTrackInCalculator.TryGenerateTransferJob(_transferOptions.PartCode);
    }

    public Task StartUndrilledTrackOutThread() => Task.CompletedTask;
}
