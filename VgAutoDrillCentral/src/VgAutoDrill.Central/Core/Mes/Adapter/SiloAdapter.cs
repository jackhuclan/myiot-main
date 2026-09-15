using AutoMapper;
using SqlSugar;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Silo;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Mes.Adapter;

public class SiloAdapter : ISiloAdapter
{
    private readonly ISiloService _siloService;
    private readonly IMapper _mapper;
    private readonly ISiloDetailDomainService _siloDetailDomainService;
    private readonly IDrillPanelDetailDomainService _drillPanelDetailDomainService;
    private readonly ILocationDetailService _locationDetailService;
    private readonly IRackDomainService _rackDomainService;

    public SiloAdapter(ISiloService siloService,
        IMapper mapper,
        ISiloDetailDomainService siloDetailDomainService,
        IDrillPanelDetailDomainService drillPanelDetailDomainService,
        IRackDomainService rackDomainService,
        ILocationDetailService locationDetailService)
    {
        _siloService = siloService;
        _mapper = mapper;
        _siloDetailDomainService = siloDetailDomainService;
        _drillPanelDetailDomainService = drillPanelDetailDomainService;
        _rackDomainService = rackDomainService;
        _locationDetailService = locationDetailService;
    }

    public async Task<List<Fundation.Iot.Models.Panel>> GetSiloPanels(string siloCode)
    {
        if (string.IsNullOrEmpty(siloCode)) ThrowHelper.ThrowArgumentNullException(nameof(siloCode));
        var siloQuery = await _siloService.GetList(new Admin.Model.ViewModels.Mes.Silo.SiloQueryReq { Code = siloCode });
        var floorNums = siloQuery.Data.List.FirstOrDefault().FloorCount;

        var where = PredicateBuilder.True<SiloDetail>();
        where = where.And(p => p.SiloCode.ToLower().Equals(siloCode.ToLower()));

        var siloDetails = await _siloDetailDomainService.QueryAsync(where, p => new { p.FloorNum }, OrderByType.Asc);
        var panels = Fundation.Iot.Models.Panel.HasSilo.NoPanelForSingleLayer(siloCode, 1, floorNums.Value);
        foreach (var detail in siloDetails)
        {
            var panel = panels.FirstOrDefault(p => p.Layer == detail.FloorNum);
            if (panel != null)
            {
                panel.ItemCode = detail.ItemCode ?? string.Empty;
                panel.LotId = string.Empty;
                panel.BatchCode = string.Empty;
                panel.SiloCode = detail.SiloCode;
                panel.PanelCode = detail.PanelCode;
                panel.ProductStatus = (VgAutoDrill.Fundation.Iot.Models.ProductStatus)detail.ProductStatus;
            }
        }

        return panels;
    }

    public async Task<SiloStatus> GetSiloStatus(string siloCode)
    {
        return (VgAutoDrill.Fundation.Iot.Models.SiloStatus)await _siloService.GetSiloStatus(siloCode);
    }

    public async Task<List<Fundation.Iot.Models.Panel>> GetSiloPanelsByDevice(string locationCode)
    {
        if (string.IsNullOrEmpty(locationCode)) ThrowHelper.ThrowArgumentNullException(nameof(locationCode));

        var rackData = await _rackDomainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower() == locationCode.ToLower());
        if (rackData == null)
        {
            return new List<Fundation.Iot.Models.Panel>();
        }
        else
        {
            var response = await _locationDetailService.GetByCode(locationCode);
            var panels = response.Data.Select(detail => new Fundation.Iot.Models.Panel
            {
                PanelCode = detail.PanelCode ?? string.Empty,
                SiloCode = rackData.SiloCode ?? string.Empty,
                ItemCode = detail.ItemCode ?? string.Empty,
                LotId = string.Empty,
                BatchCode = string.Empty,
                Layer = detail.FloorNum.ToInt(),
                Position = rackData.PositionCode.ToInt(),
                ProductStatus = detail.ProductStatus == null ? ProductStatus.EmptyPayload : (ProductStatus)detail.ProductStatus,
                LocationCode = detail.Code,
                PanelLength = (float)(detail.PanelLength ?? 0m),
                PinOffset = (float)(detail.PinOffset ?? 0m),
                PanelWidth = (float)(detail.PanelWidth ?? 0m)
            }).ToList();

            return panels;
        }
    }

    public async Task<List<Fundation.Iot.Models.Panel>> GetSiloPanelsByDrillDevice(string deviceId, List<int> layers)
    {
        if (string.IsNullOrEmpty(deviceId)) ThrowHelper.ThrowArgumentNullException(nameof(deviceId));

        var drillPanelDetails = await _drillPanelDetailDomainService.QueryAsync(p => !string.IsNullOrEmpty(p.DeviceCode)
            && p.DeviceCode.ToLower() == deviceId.ToLower() && layers.Contains(p.Layer), p => p.Layer, OrderByType.Asc);
        if (drillPanelDetails == null || drillPanelDetails.Count == 0)
        {
            return new List<Fundation.Iot.Models.Panel>();
        }

        int spindleNum = drillPanelDetails.DistinctBy(p => p.SplindleIndex).Count();
        List<Fundation.Iot.Models.Panel> returnPanels = new List<Fundation.Iot.Models.Panel>();
        foreach (var layer in layers)
        {
            var panels = Fundation.Iot.Models.Panel.HasSilo.NoPanelForSingleLayer(string.Empty, spindleNum, layer);
            foreach (var detail in drillPanelDetails)
            {
                var panel = panels.FirstOrDefault(p => p.Layer == detail.Layer && p.Position == detail.SplindleIndex);
                if (panel != null)
                {
                    panel.ItemCode = detail.ItemCode ?? string.Empty;
                    panel.LotId = string.Empty;
                    panel.BatchCode = string.Empty;
                    panel.SiloCode = detail.DeviceCode;
                    panel.PanelCode = detail.PanelCode;
                    panel.ProductStatus = detail.ProductStatus == null ? ProductStatus.EmptyPayload : (ProductStatus)detail.ProductStatus;
                    panel.LocationCode = deviceId;
                }
            }
            returnPanels.AddRange(panels);
        }

        return returnPanels;
    }

    public async Task<List<SiloDto>> GetSilos()
    {
        var siloQuery = await _siloService.GetList(new SiloQueryReq { PageSize = int.MaxValue });
        return siloQuery.Data.List;
    }
}
