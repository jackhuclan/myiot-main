using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTransOrder;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IProTransOrderDomainService : IBaseDomainService<TransOrder>
    {
        Task<PageDto<TransOrderDto>> GetList(GetTransOrderListReq req);
    }
}
