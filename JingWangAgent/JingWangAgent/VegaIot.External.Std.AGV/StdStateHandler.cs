using Microsoft.Extensions.Logging;
using VgDeviceGateway.Devices.Shelf;

namespace VegaIot.External.Std.Shelf;

public class StdStateHandler : SiloShelfStateHandler
{
    public StdStateHandler(ILogger<SiloShelfStateHandler> logger, IServiceProvider serviceProvider, StdShelf siloShelf) : base(logger, serviceProvider, siloShelf)
    {
    }
}
