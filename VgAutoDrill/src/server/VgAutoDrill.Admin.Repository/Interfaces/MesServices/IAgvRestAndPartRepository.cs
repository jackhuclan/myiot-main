using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.AgvRestAndPart;

namespace VgAutoDrill.Admin.Repository.Interfaces.Equipment
{
    public interface IAgvRestAndPartRepository : IBaseRepository<AgvRestAndPart>
    {
        Task<IPageList<RestAndPartDto>> GetList(GetRestAndPartListReq req);

        Task<RestAndPartDto> QueryByID(long Id);
    }
}
