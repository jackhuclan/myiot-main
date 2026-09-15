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
/// 2公共分区，基于公共缓存区
/// </summary>
internal class PublicPartition : Partition
{
    private readonly ILogger<PublicPartition> _logger;

    private readonly IServiceProvider _serviceProvider;

    public override IReadOnlyList<Location> RackLocations => _serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloShelf, ShelfScheduleTask>>()
        .PartitionLocations(this.PartCode)
        .ToList();

    public PublicPartition(IServiceProvider serviceProvider,
        IOptions<MysqlTaskSchedulerOptions> options,
        IAutoSiloTransferStrategyFactory autoSiloTransferStrategyFactory,
        string partCode,
        PartitionKind partitionKind,
        ILoggerFactory loggerFactory)
        : base(options, autoSiloTransferStrategyFactory, partCode, partitionKind)
    {
        _logger = loggerFactory.CreateLogger<PublicPartition>();
        _serviceProvider = serviceProvider;
    }
}
