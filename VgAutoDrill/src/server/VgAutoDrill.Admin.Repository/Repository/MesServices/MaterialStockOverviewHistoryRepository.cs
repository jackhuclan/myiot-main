using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class MaterialStockOverviewHistoryRepository : BaseRepository<MaterialStockOverviewHistory>, IMaterialStockOverviewHistoryRepository
    {
        public MaterialStockOverviewHistoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
