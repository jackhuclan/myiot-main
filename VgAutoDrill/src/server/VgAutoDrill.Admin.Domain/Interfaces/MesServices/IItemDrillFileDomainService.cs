using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemDrillFile;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IItemDrillFileDomainService : IBaseDomainService<ItemDrillFile>
    {
        Task<PageDto<ItemDrillFileDto>> GetList(GetItemDrillFileListReq req);
    }
}
