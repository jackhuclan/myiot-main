using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemAtpFile;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IItemAtpFileDomainService : IBaseDomainService<ItemAtpFile>
    {
        Task<PageDto<ItemAtpFileDto>> GetList(GetItemAtpFileListReq req);
    }
}
