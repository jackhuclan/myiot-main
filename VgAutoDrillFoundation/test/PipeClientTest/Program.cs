using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Pipe;

namespace PipeClientTest;

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
                 services.AddPipeClient(context.Configuration);
             }).Build();

        //host.RunAsync();

        var pipeClient = host.Services.GetRequiredService<IPipeClient>();
        await pipeClient.ConnectAsync();

        Console.WriteLine("====================测试服务指令不存在==================");
        for (int i = 0; i < 5; i++)
        {
            var response = await pipeClient.Request(new VgAutoDrill.Fundation.Iot.Models.DeviceServiceInvokeRequest
            {
                ProductId = "MockDevice",
                DeviceId = "MockDevice",
                ServiceId = $"test{i}",
                EventId = $"event"
            });

            await Task.Delay(1000);
        }

        Console.WriteLine("====================测试内置服务指令，并等待回复==================");
        for (int i = 0; i < 5; i++)
        {
            var response = await pipeClient.Request(new VgAutoDrill.Fundation.Iot.Models.DeviceServiceInvokeRequest
            {
                ProductId = "MockDevice",
                DeviceId = "MockDevice",
                ServiceId = Topics.Services.INVOKE_LOAD_MATERIAL_SERVICE_ID,
                EventId = $"event"
            });

            await Task.Delay(1000);
        }

        Console.WriteLine("===================测试自定义服务指令，不等待回复==================");
        for (int i = 0; i < 5; i++)
        {
            await pipeClient.Post(new VgAutoDrill.Fundation.Iot.Models.DeviceServiceInvokeRequest
            {
                ProductId = "MockDevice",
                DeviceId = "MockDevice",
                ServiceId = $"a/b/postCommand",
                EventId = $"event"
            });

            await Task.Delay(1000);
        }

        Console.WriteLine("====================测试自定义服务指令存在，并等待回复==================");
        for (int i = 0; i < 5; i++)
        {
            var response = await pipeClient.Request(new VgAutoDrill.Fundation.Iot.Models.DeviceServiceInvokeRequest
            {
                ProductId = "MockDevice",
                DeviceId = "MockDevice",
                ServiceId = $"a/b/testCommand",
                EventId = $"event"
            });

            await Task.Delay(1000);
        }

        Console.WriteLine("====================测试结束==================");
        Console.ReadKey();
    }
}
