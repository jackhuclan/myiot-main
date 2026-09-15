using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJobLog;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface ITransferJobLogDomainService : IBaseDomainService<TransferJobLog>
    {
        Task<PageDto<TransferJobLogDto>> GetList(GetTransferJobLogListReq req);
    }
}
