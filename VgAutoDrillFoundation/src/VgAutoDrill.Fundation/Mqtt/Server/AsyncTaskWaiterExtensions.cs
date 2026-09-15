using MQTTnet.Internal;
using MQTTnet.Packets;
using MQTTnet.Server;
using VgAutoDrill.Infrastructure;

namespace VgAutoDrill.Fundation.Mqtt.Server;

public static class AsyncTaskWaiterExtensions
{
    public static Task HandleInterceptingInboundPacketAsync(this IAsyncTaskWaiter taskWaiter,
        InterceptingPacketEventArgs eventArgs)
    {
        if (eventArgs.Packet is MqttPublishPacket)
        {
            MqttPublishPacket packet = (MqttPublishPacket)eventArgs.Packet;
            if (!taskWaiter.TryGetValue(packet.Topic, out var awaitable))
            {
                return CompletedTask.Instance;
            }

            awaitable.TrySetResult(packet.PayloadSegment.ToArray());
            eventArgs.ProcessPacket = true;
        }
        return CompletedTask.Instance;
    }

    public static async Task<byte[]> ExecuteTaskAsWaiter(this IAsyncTaskWaiter taskWaiter,
        string waitingKey,
        Task waitableTask,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var awaitable = new Infrastructure.AsyncTaskCompletionSource<byte[]>();

            if (!taskWaiter.TryAdd(waitingKey, awaitable))
            {
                throw new InvalidOperationException();
            }

            await waitableTask.ConfigureAwait(false);

            using (cancellationToken.Register(() => awaitable.TrySetCanceled()))
            {
                return await awaitable.Task.ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            taskWaiter.TryRemove(waitingKey);
        }
    }
}
