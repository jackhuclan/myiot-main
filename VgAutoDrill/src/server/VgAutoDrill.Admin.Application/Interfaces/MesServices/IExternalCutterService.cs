
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface IExternalCutterService
    {
        Task<ResponseDto<string>> Check(ExternalCutterGroupReq req);
        Task<ResponseDto<string>> Lock(ExternalCutterGroupReq req);
        Task<ResponseDto<string>> Unlock(ExternalCutterGroupReq req);

        Task<ResponseDto<List<ExternaCutterlTaskDto>>> GetPlanList(List<string> req);
    }
}
