using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Exceptions;
using MQTTnet.Protocol;
using MQTTnet.Server;
using VgAutoDrill.Infrastructure;

namespace VgAutoDrill.Fundation.Mqtt.Server;

public static class MqttServerExtensions
{
    public static async Task<byte[]> InvokeServcieAsync(this MqttServer server,
        IAsyncTaskWaiter taskWaiter,
        string requestTopic,
        string responseTopic,
        byte[] payload,
        MqttQualityOfServiceLevel qualityOfServiceLevel,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(requestTopic))
        {
            throw new MqttProtocolViolationException("request topic is empty.");
        }

        if (string.IsNullOrWhiteSpace(responseTopic))
        {
            throw new MqttProtocolViolationException("response topic is empty.");
        }

        var requestMessage = new MqttApplicationMessageBuilder().WithTopic(requestTopic)
            .WithPayload(payload)
            .WithQualityOfServiceLevel(qualityOfServiceLevel)
            .WithResponseTopic(responseTopic)
            .Build();

        try
        {
            var awaitable = new AsyncTaskCompletionSource<byte[]>();

            if (!taskWaiter.TryAdd(responseTopic, awaitable))
            {
                throw new InvalidOperationException($"waiting reply of {responseTopic}...");
            }

            await server.InjectApplicationMessage(new InjectedMqttApplicationMessage(requestMessage), cancellationToken).ConfigureAwait(false);

            using (cancellationToken.Register(() => awaitable.TrySetCanceled()))
            {
                return await awaitable.Task.ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
        finally
        {
            taskWaiter.TryRemove(responseTopic);
        }
    }
}
