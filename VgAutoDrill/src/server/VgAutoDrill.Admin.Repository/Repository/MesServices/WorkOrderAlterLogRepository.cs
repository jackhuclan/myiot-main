using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class WorkOrderAlterLogRepository : BaseRepository<WorkOrderAlterLog>, IWorkOrderAlterLogRepository
    {
        public WorkOrderAlterLogRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
