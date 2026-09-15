using Microsoft.Extensions.Logging;
using VgDeviceGateway.Devices.Shelf;

namespace VegaIot.External.Hik;

public class HikPropertyHandler : SiloShelfPropertyHandler
{
    public HikPropertyHandler(ILogger<HikPropertyHandler> logger, IServiceProvider serviceProvider, HikTransfer siloShelf) : base(logger, serviceProvider, siloShelf)
    {
    }
}
