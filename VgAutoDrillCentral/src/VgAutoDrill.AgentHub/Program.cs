using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VgAutoDrill.Admin.Common.Configuration;
using VgAutoDrill.Central.Configurations;

namespace VgAutoDrill.AgentHub;

internal class Program
{
    static void Main(string[] args)
    {
        var hostBuilder = Host.CreateDefaultBuilder(args)
             .ConfigureServices((hostBuilderContext, services) =>
             {
                 services.AddSingleton(new AppSettingsHelper(hostBuilderContext.Configuration));
                 services.AddHub(hostBuilderContext.Configuration);
                 //add support for drilladmin
                 services.AddDatabaseConfiguration(hostBuilderContext.Configuration);
                 // AutoMapper
                 services.AddAutoMapperConfiguration();
             })
             .ConfigureLogging((hostBuilderContext, loggingBuilder) =>
             {
                 // NLog: Setup NLog for Dependency injection
                 //builder.Logging..ClearProviders();
                 //builder.Logging.AddConsole();
                 //loggingBuilder.AddNLog();
             })
             .UseConsoleLifetime();

        //autofac
        hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        hostBuilder.ConfigureContainer<ContainerBuilder>(builder =>
        {
            builder.RegisterModule<AutofacModule>();
        });

        var host = hostBuilder.Build();

        host.Run();
    }
}
