using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Schedule.Deliver;

internal class ScheduleTaskDeliverPolicyFactory : IScheduleTaskDeliverPolicyFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ScheduleTaskDeliverPolicyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IScheduleTaskDeliverPolicy Create(DeviceProxy callerDevice, MaterialKind materialKind)
    {
        Type t = typeof(IScheduleTaskDeliverPolicy);
        switch (materialKind)
        {
            case MaterialKind.Panel:
                if (DeviceKindExtensions.IsDrill(callerDevice.Descriptor.DeviceKind))
                {
                    t = typeof(PanelOfDrillScheduleTaskDeliverPolicy);
                    break;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            case MaterialKind.PanelSilo:
                if (callerDevice.Descriptor.DeviceKind == DeviceKind.PublicPanelSiloWIP
                    || callerDevice.Descriptor.DeviceKind == DeviceKind.PanelSiloFork)
                {
                    t = typeof(PanelSiloScheduleTaskDeliverPolicy);
                    break;
                }
                else if (callerDevice.Descriptor.DeviceKind == DeviceKind.Pin)
                {
                    t = typeof(PinScheduleTaskDeliverPolicy);
                    break;
                }
                else if (callerDevice.Descriptor.DeviceKind == DeviceKind.UnPin)
                {
                    t = typeof(UnPinScheduleTaskDeliverPolicy);
                    break;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            case MaterialKind.CutterSilo:
                t = typeof(CutterSiloShelfScheduleTaskDeliverPolicy);
                break;
            case MaterialKind.Cutter:
                if (DeviceKindExtensions.IsDrill(callerDevice.Descriptor.DeviceKind))
                {
                    t = typeof(CutterOfDrillScheduleTaskDeliverPolicy);
                    break;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            default:
                throw new InvalidOperationException();
        }

        return (IScheduleTaskDeliverPolicy)ActivatorUtilities.CreateInstance(_serviceProvider, t);
    }

}
