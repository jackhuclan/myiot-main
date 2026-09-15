using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Pipe;

namespace PipeServerTest;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
             .ConfigureHostConfiguration(configurationBuilder =>
             {
                 configurationBuilder.AddJsonFile("appsettings.json");
             })
             .ConfigureServices((context, services) =>
             {
                 services.AddDevicesCore(context.Configuration);
                 services.AddHttpRequest(context.Configuration);
                 services.AddPeriodicTimers(context.Configuration);
                 services.AddPipeServer(context.Configuration);
             }).Build();

        await host.RunAsync();

        Console.ReadLine();
    }
}
