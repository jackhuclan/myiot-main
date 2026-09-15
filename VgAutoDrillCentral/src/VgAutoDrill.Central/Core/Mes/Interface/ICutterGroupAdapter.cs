using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Req;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Res;

namespace VgAutoDrill.Central.Core.Mes.Interface;

public interface ICutterGroupAdapter
{
    Task<ResponseDto<bool>> UpdateGroupStatus(UpdateCutterGroupStatusReq req);

    Task<ResponseDto<CutterGroupAptFileRes>> GetAtpFile(string deviceNo);

}
