using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Fundation.Iot.Configuration;

namespace VgAutoDrill.Fundation.CNC;

public class CNCCommandWrapper : ICNCCommandWrapper
{
    private readonly DeviceDescriptor deviceDescriptor;
    private readonly IServiceProvider serviceProvider;

    public CNCCommandWrapper(DeviceDescriptor deviceDescriptor, IServiceProvider serviceProvider)
    {
        this.deviceDescriptor = deviceDescriptor;
        this.serviceProvider = serviceProvider;
    }

    public ICNCCommand? CNCCommand { get; private set; }

    public ICNCCommand CreateCNCCommand()
    {
        var type = this.deviceDescriptor.DeviceKind == Iot.Models.DeviceKind.CNC84Drill ? typeof(CNC84Command) : typeof(CNC95Command);
        CNCCommand = (ICNCCommand)ActivatorUtilities.CreateInstance(serviceProvider, type);
        return CNCCommand;
    }
}
