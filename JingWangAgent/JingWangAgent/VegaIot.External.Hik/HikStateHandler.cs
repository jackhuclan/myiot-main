using Microsoft.Extensions.Logging;
using VgDeviceGateway.Devices.Shelf;

namespace VegaIot.External.Hik;

public class HikStateHandler : SiloShelfStateHandler
{
    public HikStateHandler(ILogger<SiloShelfStateHandler> logger, IServiceProvider serviceProvider, HikTransfer siloShelf) : base(logger, serviceProvider, siloShelf)
    {
    }
}
