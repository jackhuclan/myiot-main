using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class WorkOrderAndWorkStationRepository : BaseRepository<WorkOrderAndWorkStation>, IWorkOrderAndWorkStationRepository
    {
        public WorkOrderAndWorkStationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}

