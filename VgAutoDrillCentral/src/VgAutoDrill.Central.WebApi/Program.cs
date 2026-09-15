using Autofac;
using Autofac.Extensions.DependencyInjection;
using MQTTnet.AspNetCore;
using NLog.Web;
using VgAutoDrill.Central.Configurations;
using VgAutoDrill.Central.WebApi.Core;
using VgAutoDrill.Infrastructure.Plugin;

namespace VgAutoDrill.Central.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.AddJsonFile($"./conf/appsettings.{builder.Environment.EnvironmentName}.json", false);
        string portCentralInput = builder.Configuration["AppConfig:PortCentral"];
        if (!int.TryParse(portCentralInput, out var portCentral))
        {
            portCentral = 8001;
        }
        string portMqttInput = builder.Configuration["AppConfig:PortMqtt"];
        if (!int.TryParse(portMqttInput, out var portMqtt))
        {
            portMqtt = 1883;
        }

        int minThreads = builder.Configuration.GetValue("AppConfig:MinThreads", 200);
        int completionPortThreads = builder.Configuration.GetValue("AppConfig:CompletionPortThreads", 200);
        ThreadPool.SetMinThreads(minThreads, completionPortThreads);

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Limits.MinRequestBodyDataRate = null;
            options.ListenAnyIP(portMqtt, l => l.UseMqtt());
            options.ListenAnyIP(portCentral);
        });

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCentral(builder.Configuration);

        builder.Services.AddHostedMqttServer(optionsBuilder =>
        {
            optionsBuilder.WithConnectionBacklog(512);
            optionsBuilder.WithDefaultEndpointPort(portMqtt);
        });
        builder.Services.AddMqttConnectionHandler();
        builder.Services.AddSingleton(new Admin.Common.Configuration.AppSettingsHelper(builder.Configuration));

        // NLog: Setup NLog for Dependency injection
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Services.AddLogging(logging =>
        {
            logging.AddNLog("conf/NLog.config");
        });
        builder.Host.UseNLog();

        // Swagger Config
        builder.Services.AddSwaggerConfiguration();
        //add support for drilladmin
        builder.Services.AddDatabaseConfiguration(builder.Configuration);
        // AutoMapper
        builder.Services.AddAutoMapperConfiguration();
        //autofac
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
        {
            builder.RegisterModule<AutofacModule>();
        });
        builder.Services.AddHealthChecks();
        builder.Services.AddResponseCaching();
        builder.Services.AddPlugin(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName.ToLower() == "test")
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseResponseCaching();
        app.UseAuthorization();
        app.UseMqttServer(server =>
        {
            var mqttHandler = app.Services.GetRequiredService<MqttHandler>();
            server.ValidatingConnectionAsync += mqttHandler.ValidatingConnectionAsync;
            server.ClientConnectedAsync += mqttHandler.ClientConnectedAsync;
            server.ClientDisconnectedAsync += mqttHandler.ClientDisconnectedAsync;
            server.InterceptingInboundPacketAsync += mqttHandler.InterceptingInboundPacketAsync;
        });
        app.MapControllers();
        app.UseHealthChecks("/health");

        app.Run();
    }
}
