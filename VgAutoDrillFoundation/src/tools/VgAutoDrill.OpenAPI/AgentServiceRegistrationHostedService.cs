using Consul;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace VgAutoDrill.OpenAPI;

internal class AgentServiceRegistrationHostedService : IHostedService
{
    private readonly ConsulServiceProviderOptions _consulServiceProviderOptions;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    public AgentServiceRegistrationHostedService(IOptions<ConsulServiceProviderOptions> options,
        IHostApplicationLifetime hostApplicationLifetime)
    {
        _consulServiceProviderOptions = options.Value;
        _hostApplicationLifetime = hostApplicationLifetime;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var consulClient = new ConsulClient(c =>
        {
            c.Address = new Uri(_consulServiceProviderOptions.ConsulAddress);
            c.Namespace = _consulServiceProviderOptions.Namespace;
            c.Datacenter = _consulServiceProviderOptions.Datacenter;
            c.Token = _consulServiceProviderOptions.Token;
        });

        var registration = new AgentServiceRegistration()
        {
            ID = Guid.NewGuid().ToString(),
            Name = _consulServiceProviderOptions.ServiceName,
            Address = _consulServiceProviderOptions.ServiceIP,
            Port = _consulServiceProviderOptions.ServicePort,
            Meta = new Dictionary<string, string>() { { "Weight", _consulServiceProviderOptions.Weight.ToString() } },
            Tags = _consulServiceProviderOptions.Tags,
            Check = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(_consulServiceProviderOptions.ServiceCheckInterval),
                HTTP = $"http://{_consulServiceProviderOptions.ServiceIP}:{_consulServiceProviderOptions.ServicePort}{_consulServiceProviderOptions.ServiceHealthCheck}",
                Timeout = TimeSpan.FromSeconds(_consulServiceProviderOptions.ServiceCheckTimeout)
            }
        };

        _hostApplicationLifetime.ApplicationStopping.Register(async () =>
        {
            await consulClient.Agent.ServiceDeregister(registration.ID);
        });

        await consulClient.Agent.ServiceRegister(registration);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
