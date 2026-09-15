using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class MaterialStockOverviewRepository : BaseRepository<MaterialStockOverview>, IMaterialStockOverviewRepository
    {
        public MaterialStockOverviewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
