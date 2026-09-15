using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroupDetail;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface ICutterCroupDetailDomainService : IBaseDomainService<Model.Entites.Mes.CutterGroupDetail>
    {
        Task<CutterGroupDetailDto> GetDetailByTaskCode(string taskCode);

        Task<List<CutterGroupDetailDto>> GetDetailByTaskCode(List<string> taskCodes);
    }
}
