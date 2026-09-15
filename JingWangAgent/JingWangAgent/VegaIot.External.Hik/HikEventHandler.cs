using Microsoft.Extensions.Logging;
using VgDeviceGateway.Devices.Shelf;

namespace VegaIot.External.Hik;

public class HikEventHandler : SiloShelfEventHandler
{
    public HikEventHandler(ILogger<SiloShelfEventHandler> logger, IServiceProvider serviceProvider, HikTransfer siloShelf) : base(logger, serviceProvider, siloShelf)
    {
    }
}
