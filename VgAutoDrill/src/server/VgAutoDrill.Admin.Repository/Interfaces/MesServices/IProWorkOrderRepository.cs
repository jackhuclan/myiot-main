using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder;

namespace VgAutoDrill.Admin.Repository.Interfaces.Equipment
{

    public interface IProWorkOrderRepository : IBaseRepository<WorkOrder>
    {
        Task<IPageList<WorkOrder>> GetList(GetWorkOrderListReq req);
    }
}
