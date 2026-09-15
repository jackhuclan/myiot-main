using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Extensions;

public static class DeviceServiceInvokeRequestExtensions
{
    public static string GetRequestTopic(this DeviceServiceInvokeRequest? request)
    {
        if (request == null) return string.Empty;
        return $"{Topics.Downstream.ServiceInvokeTopic(request.TargetProductId, request.TargetDeviceId, request.ServiceId, Topics.Downstream.ServiceInvokeTopicTemplate)}";
    }

    public static string GetReplyTopic(this DeviceServiceInvokeRequest? request)
    {
        if (request == null) return string.Empty;
        return string.Format("{0}/reply/{1}", GetRequestTopic(request), Guid.NewGuid().ToString("N")); ;
    }
}
