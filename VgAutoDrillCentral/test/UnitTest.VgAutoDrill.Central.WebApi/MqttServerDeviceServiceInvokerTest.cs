using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;
using VgAutoDrill.Central.WebApi.Core;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Mqtt.Server;
using VgAutoDrill.Infrastructure;

namespace UnitTest.VgAutoDrill.Central.WebApi;

public class MqttServerDeviceServiceInvokerTest
{
    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(50)]
    [InlineData(100)]
    public async Task TestEachInvokeServiceCanGetResponse(int testCount)
    {
        var services = new ServiceCollection();
        services.AddSingleton<ILoggerFactory>(new LoggerFactory());
        services.AddSingleton<IAsyncTaskWaiter, AsyncTaskWaiter>();
        var serviceProvider = services.BuildServiceProvider();
        var asyncTaskWaiter = serviceProvider.GetRequiredService<IAsyncTaskWaiter>();

        using (var mqttServer = await StartMqttServer(serviceProvider))
        {
            var subscribeTopic = $"{Topics.Downstream.ServiceInvokeTopic("MockProductId", "MockDeviceId", "RemoteCommand", Topics.Downstream.ServiceInvokeTopicTemplate)}";
            using var mqttClient = await StartMqttClient(serviceProvider);
            var subscribeResult = await mqttClient.SubscribeAsync(subscribeTopic);

            var invoker = new MqttServerDeviceServiceInvoker(mqttServer,
                asyncTaskWaiter,
                serviceProvider.GetRequiredService<ILoggerFactory>());
            var requests = new ConcurrentDictionary<int, DeviceServiceInvokeRequest>();
            var responses = new List<DeviceServiceInvokeResponse>();
            var tasks = new List<Task>();

            for (int i = 0; i < testCount; i++)
            {
                var aTask = await Task.Factory.StartNew(async (obj) =>
                {
                    var request = new DeviceServiceInvokeRequest
                    {
                        ServiceId = "RemoteCommand",
                        TargetProductId = "MockProductId",
                        TargetDeviceId = "MockDeviceId",
                        TargetClientId = "MockClientId"
                    };

                    request.Params = new Dictionary<string, object?>
                       {
                           { "data",obj},
                           { "ManagedThreadId",Thread.CurrentThread.ManagedThreadId},
                       };

                    requests.TryAdd((int)obj, request);
                    var returnResponse = await invoker.InvokeService(request);
                    responses.Add(returnResponse);
                    Assert.Equal(ErrorCodes.Sys.SUCCESS, returnResponse.Code);
                }, i);

                tasks.Add(aTask);
            }
            ;
            Task.WaitAll(tasks.ToArray());

            Assert.Equal(testCount, responses.Count);
            for (int i = 0; i < testCount; i++)
            {
                var returnResponse = responses[i];
                var index = Convert.ToInt32(returnResponse.Params["data"].ToString());
                Assert.Equal(ErrorCodes.Sys.SUCCESS, returnResponse.Code);
                Assert.Equal(requests[index].ReplyTopic, returnResponse.Message);
            }
        }
    }

    static async Task<MqttServer> StartMqttServer(IServiceProvider serviceProvider)
    {
        var asyncTaskWaiter = serviceProvider.GetRequiredService<IAsyncTaskWaiter>();
        var mqttFactory = new MqttFactory();

        // Due to security reasons the "default" endpoint (which is unencrypted) is not enabled by default!
        var mqttServerOptions = mqttFactory.CreateServerOptionsBuilder().WithDefaultEndpoint().Build();
        var server = mqttFactory.CreateMqttServer(mqttServerOptions);
        server.InterceptingInboundPacketAsync += async (arg) =>
        {
            await asyncTaskWaiter.HandleInterceptingInboundPacketAsync(arg);
        };

        await server.StartAsync();
        return server;
    }

    static async Task<IMqttClient> StartMqttClient(IServiceProvider serviceProvider)
    {
        var asyncTaskWaiter = serviceProvider.GetRequiredService<IAsyncTaskWaiter>();
        var mqttFactory = new MqttFactory();
        IMqttClient mqttClient = mqttFactory.CreateMqttClient();
        var mqttClientOptions = new MqttClientOptionsBuilder()
        .WithTcpServer("localhost")
        .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
        .Build();

        mqttClient.ApplicationMessageReceivedAsync += async (MqttApplicationMessageReceivedEventArgs arg) =>
        {
            var request = JsonSerializer.Deserialize<DeviceServiceInvokeRequest>(arg.ApplicationMessage.PayloadSegment);
            Assert.Equal(request.ReplyTopic, arg.ApplicationMessage.ResponseTopic);
            await Task.Delay(Random.Shared.Next(2000));

            var response = new DeviceServiceInvokeResponse()
            {
                Code = ErrorCodes.Sys.SUCCESS,
                Message = request.ReplyTopic
            };
            response.Params = request.Params;
            response.Params.Add("ReplyTopic", request.ReplyTopic);
            await mqttClient.PublishBinaryAsync(arg.ApplicationMessage.ResponseTopic, System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response)));
        };

        var result = await mqttClient.ConnectAsync(mqttClientOptions);
        return mqttClient;
    }
}
