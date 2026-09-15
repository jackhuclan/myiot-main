using Microsoft.Extensions.Logging;
using Opc.Ua;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgDeviceGateway.Devices.Shelf;

namespace VegaIot.External.Std.Shelf;

public class StdShelf : SiloShelf
{
    private readonly ILogger<StdShelf> _logger;
    private volatile bool _isBusy = false;

    public StdShelf(DeviceDescriptor DeviceDescriptor,
        IServiceProvider serviceProvider,
        IDeviceEngine deviceEngine,
        IObjectFactory objectFactory)
        : base(DeviceDescriptor, serviceProvider, deviceEngine)
    {
        _logger = LoggerFactory.CreateLogger<StdShelf>();
    }

    public new Dictionary<string, LocationStd> Locations { get; set; } = new();

    protected override void CreateShelfHandler()
    {
        PropertyContainer.AddHandler<StdPropertyHandler, StdShelf>(this);
        AlarmContainer.AddHandler<StdAlarmHandler, StdShelf>(this);
        EventContainer.AddHandler<StdEventHandler, StdShelf>(this);
        StateContainer.AddHandler<StdStateHandler, StdShelf>(this);
        AGVToShelfSiloShelfLoadPolicy = ObjectFactory.CreateObject<AGVToSiloShelf_LoadMaterial_SiloShelf_InteractionPolicy>(this);
        AGVToShelfSiloShelfUnloadPolicy = ObjectFactory.CreateObject<AGVToSiloShelf_UnloadMaterial_SiloShelf_InteractionPolicy>(this);
    }

    protected override void InitLocationData()
    {
        if (_fromLocal)
        {
            Locations = new Dictionary<string, LocationStd>(spindleNum);
            for (int i = 1; i <= spindleNum; i++)
            {
                Locations[$"{i}"] = ObjectFactory.CreateObject<LocationStd>(this);
                Locations[$"{i}"].Panels.CollectionChanged += OnPanelsChanged;
                Locations[$"{i}"].Position = i.ToStr();
                Locations[$"{i}"].LocationCode = $"{DeviceId}{i.ToStr().PadLeft(3, '0')}";
                Locations[$"{i}"].CurrentContext.ShelfIndex = i;
                Locations[$"{i}"].CurrentContext.RackIndex = i - 1;
                Locations[$"{i}"].CurrentContext.LocationCode = Locations[$"{i}"].LocationCode;
                Locations[$"{i}"].CurrentContext.SiloCode = string.Empty;
                Locations[$"{i}"].LoadLocalPointConfig();
            }
        }
        else
        {
            if (RackInfos.Count == 0)
            {
                _logger.LogError("未能找到对应的库位，程序已退出");
                return;
            }

            Locations = new Dictionary<string, LocationStd>(RackInfos.Count);
            var locations = RackInfos.OrderBy(x => x.Code).ToList();
            for (int i = 1; i <= locations.Count; i++)
            {
                var rack = locations[i - 1];
                Locations[$"{i}"] = ObjectFactory.CreateObject<LocationStd>(this);
                Locations[$"{i}"].Panels.CollectionChanged += OnPanelsChanged;
                Locations[$"{i}"].LocationCode = rack.Code ?? string.Empty;
                Locations[$"{i}"].CurrentContext.SiloCode = rack.SiloCode ?? string.Empty;
                Locations[$"{i}"].Position = i.ToStr();
                Locations[$"{i}"].CurrentContext.ShelfIndex = i;
                Locations[$"{i}"].CurrentContext.RackIndex = i - 1;
                Locations[$"{i}"].CurrentContext.LocationCode = rack.Code.ToStr();
                Locations[$"{i}"].FeedAGVOutputPoint = rack.FeedAGVOutputPoint;
                Locations[$"{i}"].FeedAGVInnerPoint = rack.FeedAGVInnerPoint;
                Locations[$"{i}"].FeedAGVRestPoint = rack.FeedAGVRestPoint;
                Locations[$"{i}"].TransAGVOutputPoint = rack.TransAGVOutputPoint;
                Locations[$"{i}"].TransAGVInnerPoint = rack.TransAGVInnerPoint;
                Locations[$"{i}"].TransAGVRestPoint = rack.TransAGVRestPoint;
                Locations[$"{i}"].PositonCode = rack.PositionCode;
            }
        }
    }
}
