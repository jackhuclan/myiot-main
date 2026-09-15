using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class UnitMeasureRepository : BaseRepository<UnitMeasure>, IUnitMeasureRepository
    {
        public UnitMeasureRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
