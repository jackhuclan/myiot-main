using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Repository.Repository.Department
{
    public class ItemTypeRepository : BaseRepository<ItemType>, IItemTypeRepository
    {
        public ItemTypeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
