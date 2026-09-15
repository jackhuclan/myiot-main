using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.AgvRestAndPart;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IAgvRestAndPartDomainService : IBaseDomainService<AgvRestAndPart>
    {
        public Task<PageDto<RestAndPartDto>> GetList(GetRestAndPartListReq req);

        public Task<RestAndPartDto> QueryByID(long id);
    }
}
