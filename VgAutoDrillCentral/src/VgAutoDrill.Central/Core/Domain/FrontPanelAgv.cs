using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VgAutoDrill.Central.Core.Mysql;

namespace VgAutoDrill.Central.Core.Domain;

public class FrontPanelAgv : PanelAgv
{
    private readonly MysqlTaskSchedulerOptions _mysqlTaskSchedulerOptions;

    [ActivatorUtilitiesConstructor]
    public FrontPanelAgv(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _mysqlTaskSchedulerOptions = serviceProvider.GetRequiredService<IOptions<MysqlTaskSchedulerOptions>>().Value;
    }
}
