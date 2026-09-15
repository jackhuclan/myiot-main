using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Model.ViewModels.Mes.AgvRest;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Central.Core.Mes.Model;

namespace VgAutoDrill.Central.Core.Mes.Adapter;

public class RestPointAdapter : IRestPointAdapter
{
    private readonly IAgvRestAndPartService _agvRestAndPartService;
    private readonly IAgvRestService _agvRestService;
    private readonly IMapper _mapper;

    public RestPointAdapter(IAgvRestService agvRestService,
        IAgvRestAndPartService agvRestAndPartService,
        IMapper mapper)
    {
        _agvRestService = agvRestService;
        _agvRestAndPartService = agvRestAndPartService;
        _mapper = mapper;
    }

    public async Task<List<RestPoint>> GetRestPoints()
    {
        var response = await _agvRestService.GetList(new GetRestCodeListReq { PageSize = int.MaxValue });
        return _mapper.Map<List<RestCodeDto>, List<RestPoint>>(response.Data.List);
    }

}
