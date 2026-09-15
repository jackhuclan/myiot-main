using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Req;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Res;
using VgAutoDrill.Central.Core.Mes.Interface;

namespace VgAutoDrill.Central.Core.Mes.Adapter;


public class CutterGroupAdapter : ICutterGroupAdapter
{
    private readonly IMapper _mapper;
    private readonly ICutterGroupService _cutterGroupService;

    public CutterGroupAdapter(IMapper mapper, ICutterGroupService cutterGroupService)
    {
        _mapper = mapper;
        _cutterGroupService = cutterGroupService;


    }

    public async Task<ResponseDto<CutterGroupAptFileRes>> GetAtpFile(string deviceNo)
    {
        var data = await _cutterGroupService.GetCutterGroupAtpFile(new GetCutterGroupAtpFileReq()
        {
            DeviceCode = deviceNo,
        });
        return data;
    }
    public async Task<ResponseDto<bool>> UpdateGroupStatus(UpdateCutterGroupStatusReq req)
    {
        var data = await _cutterGroupService.UpdateCutterGroupStatus(new UpdateCutterGroupStatusReq()
        {
            GroupStatus = req.GroupStatus,
            GroupNo = req.GroupNo,
        });
        return data;
    }
}
