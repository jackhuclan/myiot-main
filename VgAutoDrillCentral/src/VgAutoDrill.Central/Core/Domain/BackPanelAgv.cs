using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VgAutoDrill.Central.Core.Mysql;

namespace VgAutoDrill.Central.Core.Domain;

public class BackPanelAgv : PanelAgv
{
    private readonly MysqlTaskSchedulerOptions _taskSchedulerOptions;

    [ActivatorUtilitiesConstructor]
    public BackPanelAgv(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _taskSchedulerOptions = serviceProvider.GetRequiredService<IOptions<MysqlTaskSchedulerOptions>>().Value;
    }
}
