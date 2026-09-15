using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Infrastructure;

namespace VgAutoDrill.Central.Core.Handler.Alarm;

internal class DeviceAlarmDispatcher : IDeviceAlarmDispatcher
{
    private Dictionary<string, DeviceAlarmDispatchPolicy> _policies = new();

    public DeviceAlarmDispatcher(IObjectFactory objectFactory)
    {
        AddPolicy(new DeviceAlarmDispatchPolicy()
        {
            PolicyName = "DrillInteruptedAlarmTask",
            DispatchPredicate = (request) => DeviceKindExtensions.IsDrill(request.RequestDeviceKind),
            ExcuteHandler = objectFactory.CreateObject<DrillInteruptedTaskAlarmHandler>(),
        });
    }

    public void AddPolicy(DeviceAlarmDispatchPolicy policy)
    {
        if (_policies.ContainsKey(policy.PolicyName)) { _policies.Remove(policy.PolicyName); }
        _policies.Add(policy.PolicyName, policy);
    }

    /// <summary>
    /// 根据报警数据选择匹配的处理策略
    /// </summary>
    /// <param name="deviceAlarmReportRequest"></param>
    /// <returns></returns>
    public List<DeviceAlarmDispatchPolicy> SelectMatchedPolicies(DeviceAlarmReportRequest deviceAlarmReportRequest)
    {
        var matchedPolices = new List<DeviceAlarmDispatchPolicy>();
        foreach (var policy in _policies.Values)
        {
            if (policy.DispatchPredicate(deviceAlarmReportRequest))
            {
                matchedPolices.Add(policy);
            }
        }

        return matchedPolices;
    }
}
