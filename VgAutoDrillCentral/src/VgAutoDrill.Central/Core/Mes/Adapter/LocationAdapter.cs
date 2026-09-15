using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Rack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SiloPanelTrace;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Central.Core.Mes.Adapter;

public class LocationAdapter : ILocationAdapter
{
    private readonly IRackService _rackService;
    private readonly IMapper _mapper;
    private readonly ISiloPanelTraceService _locationtraceService;

    public LocationAdapter(IServiceProvider serviceProvider)
    {
        _rackService = serviceProvider.GetRequiredService<IRackService>();
        _mapper = serviceProvider.GetRequiredService<IMapper>();
        _locationtraceService = serviceProvider.GetRequiredService<ISiloPanelTraceService>();
    }

    public async Task<Location> GetLocation(string locationCode)
    {
        var rackDto = await _rackService.FindSingle(locationCode);
        return _mapper.Map<Location>(rackDto);
    }

    public async Task<List<Location>> GetLocations()
    {
        var result = await _rackService.GetList(new RackQueryReq { PageSize = int.MaxValue });
        //var locationList = result.Data.List.Where(x => !string.IsNullOrEmpty(x.WareHouseCode)).ToList();
        //return _mapper.Map<List<RackDto>, List<Location>>(locationList);
        return _mapper.Map<List<RackDto>, List<Location>>(result.Data.List);
    }

    public async Task TraceLocationPanels(PanelList panelList, string changeReason, long scheduleId = 0, long transferSiloTaskId = 0)
    {
        AddOrUpdateSiloPanelTraceReq req = new()
        {
            Code = Guid.NewGuid().ToString(),
            SiloCode = panelList.SiloCode,
            Location = panelList.LocationCode,
            Subject = changeReason,
            SiloSummary = panelList.SummaryPanelInfo(),
            UndrilledItem = string.Join(",", panelList.UndrilledItemCodes),
            DrilledItem = string.Join(",", panelList.DrilledItemCodes),
            HasMultipleDrilled = panelList.DrilledItemCodes.Count > 1
        };

        if (scheduleId > 0) req.ScheduleId = scheduleId;
        if (transferSiloTaskId > 0) req.TransportationTaskId = transferSiloTaskId;

        await _locationtraceService.AddFromPanelList(panelList, changeReason, scheduleId, transferSiloTaskId);
    }
}
