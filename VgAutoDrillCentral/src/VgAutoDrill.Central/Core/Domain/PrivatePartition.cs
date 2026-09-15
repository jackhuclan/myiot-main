using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Central.Core.Calculator;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Domain;

/// <summary>
/// 1私有分区，基于插齿位的分区
/// </summary>
internal class PrivatePartition : Partition
{
    private readonly ILogger<PrivatePartition> _logger;
    private readonly IServiceProvider _serviceProvider;

    public override IReadOnlyList<Location> RackLocations => _serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>()
        .PartitionLocations(this.PartCode)
        .ToList();

    public PrivatePartition(IServiceProvider serviceProvider,
        IOptions<MysqlTaskSchedulerOptions> options,
        IAutoSiloTransferStrategyFactory autoSiloTransferStrategyFactory,
        string partCode,
        PartitionKind partitionKind,
        ILoggerFactory loggerFactory)
        : base(options, autoSiloTransferStrategyFactory, partCode, partitionKind)
    {
        _logger = loggerFactory.CreateLogger<PrivatePartition>();
        _serviceProvider = serviceProvider;
    }
}
