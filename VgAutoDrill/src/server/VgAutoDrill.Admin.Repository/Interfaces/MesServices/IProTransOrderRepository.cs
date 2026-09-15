using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTransOrder;

namespace VgAutoDrill.Admin.Repository.Interfaces.Equipment
{

    public interface IProTransOrderRepository : IBaseRepository<TransOrder>
    {
        Task<IPageList<TransOrder>> GetList(GetTransOrderListReq req);
    }
}
