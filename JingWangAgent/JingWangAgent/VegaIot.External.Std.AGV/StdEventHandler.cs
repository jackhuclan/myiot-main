using Microsoft.Extensions.Logging;
using VgDeviceGateway.Devices.Shelf;

namespace VegaIot.External.Std.Shelf;

public class StdEventHandler : SiloShelfEventHandler
{
    public StdEventHandler(ILogger<SiloShelfEventHandler> logger, IServiceProvider serviceProvider, StdShelf siloShelf) : base(logger, serviceProvider, siloShelf)
    {
    }
}
