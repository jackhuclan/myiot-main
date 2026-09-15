using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class MaterialStockStorageRepository : BaseRepository<MaterialStockStorage>, IMaterialStockStorageRepository
    {
        public MaterialStockStorageRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
