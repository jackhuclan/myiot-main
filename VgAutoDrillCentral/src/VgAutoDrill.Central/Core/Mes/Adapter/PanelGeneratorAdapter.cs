using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Mes.Adapter;

public class PanelGeneratorAdapter : IPanelGeneratorAdapter
{
    private readonly IPanelService _panelService;
    private readonly IMapper _mapper;

    public PanelGeneratorAdapter(IPanelService panelService,
        IMapper mapper,
        ISysConfigManager sysConfigManager)
    {
        _panelService = panelService;
        _mapper = mapper;
    }

    public async Task<List<Panel>> GetNextPanelNumber(GetNextPanelRequest request)
    {
        var condition = _mapper.Map<BatchInsertPanelReq>(request);
        var panelDtos = await _panelService.GeneratePanels(condition);
        var panels = _mapper.Map<List<Panel>>(panelDtos);
        var beginLayer = request.BeginLayer;
        panels.ForEach(panel =>
        {
            panel.Layer = beginLayer++;
            panel.Position = request.Position;
            panel.SiloCode = request.SiloCode;
        });

        return panels;
    }

    public async Task<List<Panel>> GetDrillNextPanelNumber(GetDrillNextPanelRequest request)
    {
        BatchInsertPanelReq condition = new BatchInsertPanelReq
        {
            BeginLayer = request.BeginPosition,
            Count = request.Count,
            ItemCode = request.ItemCode,
            ProductStatus = request.ProductStatus,
            PanelWidth = request.PanelWidth,
            PanelLength = request.PanelLength,
            Pcs = request.Pcs,
            PinOffset = request.PinOffset,
            BatchCode = request.BatchCode,
        };
        var panelDtos = await _panelService.GeneratePanels(condition);

        var panels = _mapper.Map<List<Panel>>(panelDtos);
        var beginPosition = request.BeginPosition;
        panels.ForEach(panel =>
        {
            panel.Layer = request.Layer;
            panel.Position = beginPosition++;
            panel.SiloCode = request.SiloCode;
        });

        return panels;
    }

    public async Task<LoadSiloPanelResponse> LoadSiloPanelsInfo(GetNextPanelRequest request)
    {
        var condition = _mapper.Map<BatchInsertPanelReq>(request);
        var panelDtos = await _panelService.GeneratePanels(condition);
        var panels = _mapper.Map<List<Panel>>(panelDtos);
        var beginLayer = request.BeginLayer;
        panels.ForEach(panel =>
        {
            panel.Layer = beginLayer++;
            panel.Position = request.Position;
            panel.SiloCode = request.SiloCode;
        });
        return new LoadSiloPanelResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = "保存成功！",
            Data = panels
        };
    }
}
