using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Schedule.Deliver;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Infrastructure;

namespace UnitTest.VgAutoDrill.Central.Mock;

internal class MockScheduleTaskDeliverPolicyFactory : IScheduleTaskDeliverPolicyFactory
{
    private readonly SiloTestContext _siloTestContext;
    private readonly IObjectFactory _objectFactory;

    public MockScheduleTaskDeliverPolicyFactory(SiloTestContext siloTestContext,
        IObjectFactory objectFactory)
    {
        _siloTestContext = siloTestContext;
        _objectFactory = objectFactory;
    }

    public IScheduleTaskDeliverPolicy Create(DeviceProxy callerDevice, MaterialKind materialKind)
    {
        return _objectFactory.CreateObject<MockScheduleTaskDeliverPolicy>();
    }
}
