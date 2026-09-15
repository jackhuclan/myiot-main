using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VgAutoDrill.Fundation.Pipe;

public static class PipeServiceCollectionExtensions
{
    /// <summary>
    /// 添加命名管道服务端
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddPipeServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<PipeServer>();

        services.AddSingleton<IPacketDecoder, StandardJsonDecoder>();
        services.AddSingleton<IPacketEncoder, StandardJsonEncoder>();
        services.Configure<PipeServerOptions>(configuration.GetSection(nameof(PipeServerOptions)));
    }

    /// <summary>
    /// 添加命名管道客户端
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddPipeClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IPipeClient, PipeClient>();

        services.AddSingleton<IPacketDecoder, StandardJsonDecoder>();
        services.AddSingleton<IPacketEncoder, StandardJsonEncoder>();
        services.Configure<PipeClientOptions>(configuration.GetSection(nameof(PipeClientOptions)));
    }
}
