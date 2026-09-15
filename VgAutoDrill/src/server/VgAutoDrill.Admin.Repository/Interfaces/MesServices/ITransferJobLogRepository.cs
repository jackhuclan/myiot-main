using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJobLog;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface ITransferJobLogRepository : IBaseRepository<TransferJobLog>
    {
        Task<IPageList<TransferJobLog>> GetList(GetTransferJobLogListReq req);
    }
}
