using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Fundation.Channel.Http;
using VgAutoDrill.Fundation.Channel.Mqtt;
using VgAutoDrill.Fundation.Iot.Configuration;

namespace VgAutoDrill.Fundation.Channel;

internal class MessageChannelFactory : IMessageChannelFactory
{
    private readonly IServiceProvider _provider;

    public MessageChannelFactory(IServiceProvider provider)
    {
        this._provider = provider;
    }

    public IMessageChannel Create(DeviceDescriptor deviceDescriptor, string channel)
    {
        if (deviceDescriptor.SoloMode)
            return (IMessageChannel)ActivatorUtilities.CreateInstance(_provider, typeof(LocalMessageChannel));

        switch (channel.ToLowerInvariant())
        {
            case "mqttv2":
                return (IMessageChannel)ActivatorUtilities.CreateInstance(_provider, typeof(MqttMessageChannelV2));
            case "mqtt":
                return (IMessageChannel)ActivatorUtilities.CreateInstance(_provider, typeof(MqttMessageChannel));
            default:
                return (IMessageChannel)ActivatorUtilities.CreateInstance(_provider, typeof(HttpMessageChannel));
        }
    }
}
