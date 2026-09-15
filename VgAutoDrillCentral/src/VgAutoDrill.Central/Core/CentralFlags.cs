using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace VgAutoDrill.Central.Core;

public class CentralFlags
{
    public CentralFlags(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public static IServiceProvider Service => _serviceProvider;
    public static IMapper AutoMapper => _serviceProvider.GetRequiredService<IMapper>();
    /// <summary>
    /// 系统预加载数据完成
    /// </summary>
    public static volatile bool SystemPreloadCompleted = false;

    private static IServiceProvider _serviceProvider;
}
