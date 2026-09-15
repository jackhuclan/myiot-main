using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class CutterPlanRepository : BaseRepository<CutterPlan>, ICutterPlanRepository
    {
        public CutterPlanRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
