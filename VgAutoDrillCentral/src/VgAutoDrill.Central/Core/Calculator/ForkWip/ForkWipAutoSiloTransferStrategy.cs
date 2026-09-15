using Microsoft.Extensions.Logging;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Infrastructure;

namespace VgAutoDrill.Central.Core.Calculator.ForkWip;

internal class ForkWipAutoSiloTransferStrategy : IAutoSiloTransferStrategy
{
    private readonly ILogger<ForkWipAutoSiloTransferStrategy> _logger;
    private readonly IObjectFactory _objectFactory;
    private readonly PartitionAutoSiloTransferOptions _transferOptions;

    public ForkWipAutoSiloTransferStrategy(ILoggerFactory loggerFactory,
        IObjectFactory objectFactory,
        PartitionAutoSiloTransferOptions transferOptions)
    {
        _objectFactory = objectFactory;
        _transferOptions = transferOptions;
        _logger = loggerFactory.CreateLogger<ForkWipAutoSiloTransferStrategy>();
    }

    public void ReassignSiloTransferJob()
    {
        if (!CentralFlags.SystemPreloadCompleted)
        {
            _logger.LogWarning($"系统已启用预加载模式，但是尚未加载完成，请等待.");
            return;
        }

        if (_transferOptions.EnableDrilledTrackOut)
            StartDrilledTrackOutThread();//熟料:中转位->线边仓

        if (_transferOptions.EnableDrilledTrackIn)
            StartDrilledTrackInThread();//熟料:线边仓->中转位

        if (_transferOptions.EnableUndrilledTrackOut)
            StartUndrilledTrackOutThread();//生料:中转位->线边仓

        if (_transferOptions.EnableUndrilledTrackIn)
            StartRawTrackInThread();//生料:线边仓->中转位

        if (_transferOptions.EnableEmptyBoxTrackIn)
            StartEmptyBoxTrackInThread();//空料仓:线边仓->中转位

        if (_transferOptions.EnableEmptyBoxTrackOut)
            StartEmptyBoxTrackOutThread();//空料仓:中转位->线边仓

        if (_transferOptions.EnableFirstTrackOut)
            StartFirstTrackOutThread();//首件:中转位->线边仓
    }

    public Task StartDrilledTrackInThread()
    {
        _logger.LogInformation($"StartDrilledTrackInThread,PartCode:{_transferOptions.PartCode}==========================\r\n");
        var drilledTrackInCalculator = _objectFactory.GetOrCreate<DrilledFromWipToForkCalculator>();
        return drilledTrackInCalculator.TryGenerateTransferJob(_transferOptions.PartCode);
    }

    public Task StartDrilledTrackOutThread()
    {
        _logger.LogInformation($"StartDrilledTrackOutThread,PartCode:{_transferOptions.PartCode}==========================\r\n");
        var drilledTrackOutCalculator = _objectFactory.GetOrCreate<DrilledFromForkToWipCalculator>();
        return drilledTrackOutCalculator.TryGenerateTransferJob(_transferOptions.PartCode);
    }

    public Task StartUndrilledTrackOutThread()
    {
        _logger.LogInformation($"StartUndrilledTrackOutThread,PartCode:{_transferOptions.PartCode}==========================\r\n");
        var drilledTrackOutCalculator = _objectFactory.GetOrCreate<UndrilledFromForkToWipCalculator>();
        return drilledTrackOutCalculator.TryGenerateTransferJob(_transferOptions.PartCode);
    }

    public Task StartEmptyBoxTrackInThread()
    {
        _logger.LogInformation($"StartEmptyBoxTrackInThread,PartCode:{_transferOptions.PartCode}==========================\r\n");
        var emptyBoxTrackInCalculator = _objectFactory.GetOrCreate<EmptyBoxFromWipToForkCalculator>();
        return emptyBoxTrackInCalculator.TryGenerateTransferJob(_transferOptions.PartCode);
    }

    public Task StartEmptyBoxTrackOutThread()
    {
        _logger.LogInformation($"StartEmptyBoxTrackOutThread,PartCode:{_transferOptions.PartCode}==========================\r\n");
        var emptyBoxTrackOutCalculator = _objectFactory.GetOrCreate<EmptyBoxFromForkToWipCalculator>();
        return emptyBoxTrackOutCalculator.TryGenerateTransferJob(_transferOptions.PartCode);
    }

    public Task StartFirstTrackOutThread()
    {
        _logger.LogInformation($"StartFirstTrackOutThread,PartCode:{_transferOptions.PartCode}==========================\r\n");
        var firstTrackOutCalculator = _objectFactory.GetOrCreate<FirstFromForkToWipCalculator>();
        return firstTrackOutCalculator.TryGenerateTransferJob(_transferOptions.PartCode);
    }

    public Task StartRawTrackInThread()
    {
        _logger.LogInformation($"StartRawTrackInThread,PartCode:{_transferOptions.PartCode}==========================\r\n");
        var rawTrackInCalculator = _objectFactory.GetOrCreate<UndrilledFromWipToForkCalculator>();
        return rawTrackInCalculator.TryGenerateTransferJob(_transferOptions.PartCode);
    }
}
