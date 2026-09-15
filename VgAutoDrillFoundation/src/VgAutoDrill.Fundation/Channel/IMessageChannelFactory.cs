using VgAutoDrill.Fundation.Iot.Configuration;

namespace VgAutoDrill.Fundation.Channel;

internal interface IMessageChannelFactory
{
    IMessageChannel Create(DeviceDescriptor deviceDescriptor, string channel);
}
