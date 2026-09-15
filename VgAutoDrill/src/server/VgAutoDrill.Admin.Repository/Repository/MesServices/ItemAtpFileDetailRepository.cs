using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class ItemAtpFileDetailRepository : BaseRepository<ItemAtpFileDetail>, IItemAtpFileDetailRepository
    {
        public ItemAtpFileDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}