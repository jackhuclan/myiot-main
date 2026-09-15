using MQTTnet.Server;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Reporter;

public interface IDeviceLoginReporter
{
    Task<OnlineResponse> Report(OnlineRequest? request, InterceptingPacketEventArgs arg);
}
