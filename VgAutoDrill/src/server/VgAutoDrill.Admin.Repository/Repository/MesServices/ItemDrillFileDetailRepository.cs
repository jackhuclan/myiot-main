using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class ItemDrillFileDetailRepository : BaseRepository<ItemDrillFileDetail>, IItemDrillFileDetailRepository
    {
        public ItemDrillFileDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
