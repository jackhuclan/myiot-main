using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VgAutoDrill.OpenAPI;

public static class ConsulServiceCollectionExtensions
{
    public static void AddConsulServiceConsumer(this IServiceCollection services, IConfiguration configuration)
    {
        var consulOptions = configuration.GetSection(nameof(ConsulServiceConsumerOptions)).Get<ConsulServiceConsumerOptions>();
        if (consulOptions == null)
        {
            throw new ArgumentNullException($"{nameof(ConsulServiceConsumerOptions)} is null");
        }

        services.Configure<ConsulServiceConsumerOptions>(configuration.GetSection(nameof(ConsulServiceConsumerOptions)));
        services.AddHttpClient();
        services.AddSingleton<IHttpRequestInvoker, HttpRequestInvoker>();
        services.AddSingleton<IConsulServiceConsumer, ConsulServiceConsumer>();
        services.AddSingleton<ConsulClient>(sp => new ConsulClient(c =>
        {
            c.Address = new Uri(consulOptions.ConsulAddress);
            c.Namespace = consulOptions.Namespace;
            c.Datacenter = consulOptions.Datacenter;
            c.Token = consulOptions.Token;
        }));
    }

    public static void AddConsulServiceProvider(this IServiceCollection services, IConfiguration configuration)
    {
        var consulOptions = configuration.GetSection(nameof(ConsulServiceProviderOptions)).Get<ConsulServiceProviderOptions>();
        if (consulOptions == null)
        {
            throw new ArgumentNullException($"{nameof(ConsulServiceProviderOptions)} is null");
        }

        services.Configure<ConsulServiceProviderOptions>(configuration.GetSection(nameof(ConsulServiceProviderOptions)));
        services.AddHostedService<AgentServiceRegistrationHostedService>();
    }

    public static void UseHealthCheck(this IApplicationBuilder app, string checkPath = "/healthcheck")
    {
        app.Map(checkPath, builder => builder.Run(async context =>
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("ok");
        }));
    }
}
